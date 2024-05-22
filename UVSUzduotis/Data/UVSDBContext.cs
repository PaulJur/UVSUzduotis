using Microsoft.EntityFrameworkCore;
using UVSUzduotis.Model;

namespace UVSUzduotis.Data
{
    public class UVSDBContext : DbContext
    {
        public DbSet<UVSUzduotisModel> UVSThreadTable { get; set; }

        public DbSet<ThreadModelTest> ThreadTable { get; set; }
        //Localhost connection.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

                optionsBuilder.UseSqlServer("Server=localhost;Database=UVSUzduotisDB;Trusted_Connection=True;TrustServerCertificate=True;");

                base.OnConfiguring(optionsBuilder);
        }
    }
}
