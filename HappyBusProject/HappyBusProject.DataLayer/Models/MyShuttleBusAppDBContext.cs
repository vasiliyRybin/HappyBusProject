using HappyBusProject.HappyBusProject.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

#nullable disable

namespace HappyBusProject
{
    public partial class MyShuttleBusAppNewDBContext : DbContext
    {
        public MyShuttleBusAppNewDBContext()
        {
        }

        public MyShuttleBusAppNewDBContext(DbContextOptions<MyShuttleBusAppNewDBContext> options) : base(options) { }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<DriversRatingHistory> DriversRatingHistories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<RouteStop> RouteStops { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UsersRatingHistory> UsersRatingHistories { get; set; }
        public virtual DbSet<CarsCurrentState> CarCurrentStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Car>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RegistrationNumPlate)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CarId).HasColumnName("CarID");

                entity.Property(e => e.MedicalExamPassDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Drivers)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Drivers_To_Cars");
            });

            modelBuilder.Entity<DriversRatingHistory>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("PK_DriversRatingHistory_1");

                entity.ToTable("DriversRatingHistory");

                entity.Property(e => e.RecordId)
                    .ValueGeneratedNever()
                    .HasColumnName("RecordID");

                entity.Property(e => e.Comment).HasMaxLength(1000);

                entity.Property(e => e.DriverId).HasColumnName("DriverID");

                entity.Property(e => e.RatedWhenDateTime).HasColumnType("datetime");

                entity.Property(e => e.RouteEndPointId).HasColumnName("RouteEndPointID");

                entity.Property(e => e.RouteStartPointId).HasColumnName("RouteStartPointID");

                entity.Property(e => e.WhoRatedId).HasColumnName("WhoRatedID");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.DriversRatingHistories)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DriversRatingHistory_Drivers");

                entity.HasOne(d => d.WhoRated)
                    .WithMany(p => p.DriversRatingHistories)
                    .HasForeignKey(d => d.WhoRatedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DriversRatingHistory_Users");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => new { e.Id });

                entity.Property(e => e.CarId).HasColumnName("CarID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.EndPointId).HasColumnName("EndPointID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OrderDateTime).HasColumnType("datetime");

                entity.Property(e => e.OrderType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartPointId).HasColumnName("StartPointID");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Cars");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Users");
            });

            modelBuilder.Entity<RouteStop>(entity =>
            {
                entity.HasKey(e => e.PointId);

                entity.Property(e => e.PointId).HasColumnName("PointID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UsersRatingHistory>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("UsersRatingHistory");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserID");

                entity.Property(e => e.Comment).HasMaxLength(1000);

                entity.Property(e => e.RatedWhenDateTime).HasColumnType("datetime");

                entity.Property(e => e.RecordId).HasColumnName("RecordID");

                entity.Property(e => e.RouteEndPointId).HasColumnName("RouteEndPointID");

                entity.Property(e => e.RouteStartPointId).HasColumnName("RouteStartPointID");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UsersRatingHistory)
                    .HasForeignKey<UsersRatingHistory>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersRatingHistory_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
