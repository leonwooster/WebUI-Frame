using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using AspCoreFrame.Entities;

namespace AspCoreFrame.Data
{
    public class DataContext: DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            options.EnableSensitiveDataLogging();
        }

        public DbSet<CommonDataBO> tbl_DKSH_TPI_KEYWORDS { get; set; }
    }
}
