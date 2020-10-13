using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlitzkriegSoftware.Demo.Resiliancy.WebSvc.Ef
{
    /// <summary>
    /// EF Context: Bicycle
    /// </summary>
    public partial class BicycleContext : DbContext
    {

        #region "Properties: Custom"
        /// <summary>
        /// Sql Connection String
        /// </summary>
        public string SqlConnectionString { get; set; }
        /// <summary>
        /// Logger Factory
        /// </summary>
        private ILoggerFactory LoggerFactory { get; set; }
        #endregion

        #region "CTORs, do not remove"

        /// <summary>
        /// CTOR
        /// </summary>
        private BicycleContext()
        {
        }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="options">DbContextOptions</param>
#pragma warning disable IDE0051 // Required by EF
        private BicycleContext(DbContextOptions<BicycleContext> options)
#pragma warning restore IDE0051 // Remove unused private members
            : base(options)
        {
        }

        #endregion

        #region "CTORs: Custom e.g. use these instead"

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="loggerFactory">ILoggerFactory</param>
        /// <param name="connectionString">string</param>
        public BicycleContext(ILoggerFactory loggerFactory, string connectionString)
        {
            this.LoggerFactory = loggerFactory;
            this.SqlConnectionString = connectionString;
        }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="loggerFactory">ILoggerFactory</param>
        /// <param name="connectionString">string</param>
        /// <param name="options">DbContextOptions</param>
        public BicycleContext(ILoggerFactory loggerFactory, string connectionString, DbContextOptions<BicycleContext> options)
            : base(options)
        {
            this.LoggerFactory = loggerFactory;
            this.SqlConnectionString = connectionString;
        }

        #endregion

        #region "DbSets"

        /// <summary>
        /// DbSet: Configuration
        /// </summary>
        public virtual DbSet<Configuration> Configuration { get; set; }
        /// <summary>
        /// DbSet: Customer
        /// </summary>
        public virtual DbSet<Customer> Customer { get; set; }
        /// <summary>
        /// DbSet: ErrorLog
        /// </summary>
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        /// <summary>
        /// DbSet FilesToImport
        /// </summary>
        public virtual DbSet<FilesToImport> FilesToImport { get; set; }
        /// <summary>
        /// DbSet: Order
        /// </summary>
        public virtual DbSet<Order> Order { get; set; }
        /// <summary>
        /// DbSet: OrderDetail
        /// </summary>
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        /// <summary>
        /// DbSet: OrderDetail
        /// </summary>
        public virtual DbSet<OrdersRaw> OrdersRaw { get; set; }
        /// <summary>
        /// DbSet: Product
        /// </summary>
        public virtual DbSet<Product> Product { get; set; }

        #endregion

        #region "Events"

        /// <summary>
        /// Event: OnConfiguring
        /// </summary>
        /// <param name="optionsBuilder">DbContextOptionsBuilder</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null) throw new ArgumentNullException(nameof(optionsBuilder));

            if (!optionsBuilder.IsConfigured)
            {
                if (string.IsNullOrEmpty(this.SqlConnectionString)) throw new InvalidOperationException($"A Connections string must be specified using the CTOR or Property");

                // SEE: Notice that we are specifying the connection string, but also 
                // * a command timeout 
                // * a retry policy
                // Notice we can configure the logging to emit warnings about client 
                //   side evaluation, etc.
                // You can add other behaviors here
                optionsBuilder
                    .UseLoggerFactory(this.LoggerFactory)
                    .UseSqlServer(this.SqlConnectionString, o =>
                    {
                        o.CommandTimeout((int)TimeSpan.FromMinutes(1).TotalSeconds);
                        o.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorNumbersToAdd: null);
                    })
                    .ConfigureWarnings(warnings => warnings.Default(WarningBehavior.Log))
                    ;

                optionsBuilder.UseSqlServer(this.SqlConnectionString);
            }
        }

        /// <summary>
        /// Event: OnModelCreating
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.HasKey(e => e.Name);

                entity.ToTable("Configuration", "common");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer", "store");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.Address2).HasMaxLength(40);

                entity.Property(e => e.Address3).HasMaxLength(40);

                entity.Property(e => e.Address4).HasMaxLength(40);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.Company).HasMaxLength(255);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("EMail");

                entity.Property(e => e.NameFirst)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.NameLast)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PhonePrimary).HasMaxLength(15);

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.StateOrProvence).HasMaxLength(40);
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.ToTable("ErrorLog", "common");

                entity.Property(e => e.Comment).IsRequired();

                entity.Property(e => e.Step)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<FilesToImport>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("FilesToImport", "etl");

                entity.Property(e => e.FileNamePath).IsUnicode(false);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order", "store");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.OrderDate).HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Customer");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail", "store");

                entity.Property(e => e.CostEach).HasColumnType("money");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Product");
            });

            modelBuilder.Entity<OrdersRaw>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Orders-Raw", "etl");

                entity.Property(e => e.Email).HasColumnName("EMail");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product", "store");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((20.00))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        /// <summary>
        /// Event: OnModelCreatingPartial
        /// </summary>
        /// <param name="modelBuilder"></param>
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        #endregion
    }
}
