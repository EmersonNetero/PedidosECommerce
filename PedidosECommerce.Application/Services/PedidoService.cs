using MassTransit;
using Microsoft.Extensions.Logging;
using PedidosECommerce.Application.Abstractions;
using PedidosECommerce.Application.DTO;
using PedidosECommerce.Domain;
using PedidosECommerce.Domain.Entities;
using PedidosECommerce.Domain.Enums;
using PedidosECommerce.Domain.Exceptions;

namespace PedidosECommerce.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<PedidoService> _logger;
        private static readonly Random _random = new();
        private readonly IMongoAuditRepository _auditRepo;
        private readonly ICacheService _cacheService;


        public PedidoService(IPedidoRepository pedidoRepository, IPublishEndpoint publishEndpoint, ILogger<PedidoService> logger, IMongoAuditRepository auditRepo, ICacheService cacheService)
        {
            _pedidoRepository = pedidoRepository;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _auditRepo = auditRepo;
            _cacheService = cacheService;
        }

        public async Task<PagedResult<PedidoResponse>> GetAsync(PedidoFiltroRequest request)
        {
            var result = await _pedidoRepository.GetAsync(
                request.Status,
                request.Page,
                request.PageSize,
                request.Order);

            return new PagedResult<PedidoResponse>
            {
                Total = result.Total,
                Items = result.Items.Select(p => new PedidoResponse
                {
                    Id = p.Id,
                    NomeCliente = p.NomeCliente,
                    CriadoEm = p.DataCriacao,
                    Status = p.Status.ToString(),
                    DadosPedido = p.DadosPedido
                })
            };
        }

        public async Task<PedidoDetalheResponse> GetDetalhe(int id)
        {
            var cacheKey = $"pedido:{id}";
            var result = await _cacheService.GetAsync<Pedido>(cacheKey);
            if(result == null)
                result = await _pedidoRepository.GetOneAsync(id);
            if (result == null)
                throw new NotFoundException($"Pedido {id} não encontrado.");

            var pedidoDetalhe = new PedidoDetalheResponse
            {
                NomeCliente = result.NomeCliente,
                DadosPedido = result.DadosPedido,
                CriadoEm = result.DataCriacao,
                Status = result.Status.ToString(),
                Id = result.Id,
                CorrelationId = result.CorrelationId,
                Historico = result.Historico.Select(h => new PedidoHistoricoResponse
                {
                    Status = h.Status.ToString(),
                    DataProcessamento = h.DataProcessamento,
                    Erro = h.Erro
                }).ToList(),
            };

            await _cacheService.SetAsync(cacheKey, pedidoDetalhe, TimeSpan.FromMinutes(5));
            return pedidoDetalhe;
        }

        public async Task ProcessarPedido(int id, Guid correlationId)
        {
            _logger.LogInformation("Processando pedido id:{id}", id);
            await _auditRepo.RegistrarAsync(new PedidoAuditLog
            {
                PedidoId = id,
                CorrelationId = correlationId,
                Etapa = "Recebido",
                Status = "OK"
            });
            var pedido = await _pedidoRepository.GetOneAsync(id);
            try
            {
                if (pedido == null)
                    throw new NotFoundException($"Pedido {id} não encontrado.");
                pedido.Processar(PedidoStatus.EmProcessamento);

                await _auditRepo.RegistrarAsync(new PedidoAuditLog
                {
                    PedidoId = id,
                    CorrelationId = correlationId,
                    Etapa = "Processamento",
                    Status = "EmProcessamento"
                });

                await this.Processar(pedido);
                if(pedido.Status == PedidoStatus.Processado)
                {
                    await _auditRepo.RegistrarAsync(new PedidoAuditLog
                    {
                        PedidoId = id,
                        CorrelationId = correlationId,
                        Etapa = "Processamento",
                        Status = "Processado"
                    });
                    _logger.LogInformation("Pedido processado com sucesso {id}", id);
                }
                else
                {
                    await _auditRepo.RegistrarAsync(new PedidoAuditLog
                    {
                        PedidoId = id,
                        CorrelationId = correlationId,
                        Etapa = "Processamento",
                        Status = "Falha",
                        Erro = "Falha por simulação"
                    });
                    _logger.LogInformation("Erro em processar o pedido {id}", id);
                }
            }catch (Exception ex)
            {
                pedido.Processar(PedidoStatus.Falha, ex.Message);
                await _auditRepo.RegistrarAsync(new PedidoAuditLog
                {
                    PedidoId = id,
                    CorrelationId = correlationId,
                    Etapa = "Processamento",
                    Status = "Falha",
                    Erro = ex.Message
                });
                await _pedidoRepository.SaveChangesAsync();
                throw;
            }
                
        }

        private async Task Processar(Pedido pedido)
        {
            await Task.Delay(5000);
            var status = _random.Next(2) == 0 ? PedidoStatus.Processado : PedidoStatus.Falha;
            pedido.Processar(status);
            await _pedidoRepository.SaveChangesAsync();
            await _cacheService.RemoveAsync($"pedido{pedido.Id}");
        }

        public async Task<PedidoRecebido> ReceberPedido(ReceberPedidoDTO pedido)
        {
            var novoPedido = new Pedido(pedido.NomeCliente, pedido.DadosPedido);

            await _pedidoRepository.ReceberPedidoAsync(novoPedido);
            var pedidoRecebido = new PedidoRecebido()
            {
                DadosPedido = novoPedido.DadosPedido,
                NomeCliente = novoPedido.NomeCliente,
                Status = novoPedido.Status.ToString()
            };

            var evento = new PedidoCriadoEvent
            {
                Id = novoPedido.Id,
                CorrelationId = novoPedido.CorrelationId,
                NomeCliente = novoPedido.NomeCliente,
                DadosPedido = novoPedido.DadosPedido,
                Status = novoPedido.Status,
                DataCriacao = novoPedido.DataCriacao,
                //UltimaAtualizacao = novoPedido.DataAtualizacao
            };

            await _publishEndpoint.Publish(evento);

            return pedidoRecebido;
        }

        public async Task Reprocessar(int id)
        {
            var pedido = await _pedidoRepository.GetOneAsync(id);
            await _auditRepo.RegistrarAsync(new PedidoAuditLog
            {
                PedidoId = id,
                CorrelationId = Guid.NewGuid(),
                Etapa = "Reprocessamento",
                Status = "Falha",
                Erro = "Falha por simulação"
            });
            if (pedido == null) throw new NotFoundException("Pedido não encontrado.");

            if (pedido.Status != PedidoStatus.Falha) throw new ArgumentException("Esse pedido não pode ser reprocessado. Pedido com status diferente de Falha");

            var evento = new PedidoCriadoEvent
            {
                Id = pedido.Id,
                CorrelationId = pedido.CorrelationId,
                NomeCliente = pedido.NomeCliente,
                DadosPedido = pedido.DadosPedido,
                Status = pedido.Status,
                DataCriacao = pedido.DataCriacao,
                UltimaAtualizacao = pedido.DataAtualizacao
            };
            await _publishEndpoint.Publish(evento);

        }
    }
}
