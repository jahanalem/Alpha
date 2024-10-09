using Alpha.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

// https://stackoverflow.com/questions/45782446/unable-to-create-migrations-after-upgrading-to-asp-net-core-2-0
namespace Alpha.Web.App
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");//.GetConnectionString("DataAccessMySqlProvider");
            //string connectionString = configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;

            builder.UseSqlServer(connectionString);
            //builder.UseMySql(connectionString); // Pomelo.EntityFrameworkCore.MySql
            return new ApplicationDbContext(builder.Options);
        }
    }
}
