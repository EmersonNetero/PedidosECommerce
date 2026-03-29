using Microsoft.AspNetCore.Mvc;
using PedidosECommerce.Application.DTO;
using PedidosECommerce.Application.Services;

namespace PedidosECommerce.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost]
        public async Task<IActionResult> Index(ReceberPedidoDTO novoPedido)
        {
            return Ok(await _pedidoService.ReceberPedido(novoPedido));
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<PedidoResponse>>> GetTodos([FromQuery] PedidoFiltroRequest request)
        {
            return Ok(await _pedidoService.GetAsync(request));
        }
    }
}
