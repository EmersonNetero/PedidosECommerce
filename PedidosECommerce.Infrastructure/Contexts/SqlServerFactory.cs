using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PedidosECommerce.Infrastructure.Contexts;

public class SqlServerFactory : IDesignTimeDbContextFactory<SqlServer>
{
    public SqlServer CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SqlServer>();

        optionsBuilder.UseSqlServer(
            "Server=sql,1433;Database=PedidosDb;User Id=sa;Password=SenhaForte123!;TrustServerCertificate=True;"
        );

        return new SqlServer(optionsBuilder.Options);
    }
}