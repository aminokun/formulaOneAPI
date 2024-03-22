using Microsoft.EntityFrameworkCore;
using FormulaOne.Entities.DbSet;


namespace FormulaOne.DataService.Data
{
    public class AppDbContext : DbContext
    {
        //Define db entities

        //virtual is used to modify a method, property, or indexer declaration, allowing it to be overridden.
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<Achievement> Achievements { get; set; }


        //these options are coming from the program.cs file
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //specified relationship between entities bij setting a FK constraint
            // this is called code first approach
            modelBuilder.Entity<Achievement>(entity =>
                {
                    entity.HasOne(d => d.Driver)
                    .WithMany(p => p.Achievements)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("FK_Achievements_Driver");
                }
            );
            modelBuilder.Entity<Driver>();

        }
    }
}
