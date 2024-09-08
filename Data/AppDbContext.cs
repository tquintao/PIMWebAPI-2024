namespace PIMWebAPI.Data
{
    using Models;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        // Definindo DbSets para as entidades
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<Producao> Producoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql
                ("Host=junction.proxy.rlwy.net;Port=54854;Database=railway;Username=postgres;Password=ceBlSRWMiXkclKmossHIDEQPaTaCGvoh");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ================================
            // Mapeamento da entidade Cliente
            // ================================
            modelBuilder.Entity<Cliente>()
                .ToTable("cliente")
                .HasKey(c => c.ID_Cliente);

            modelBuilder.Entity<Cliente>()
                .Property(c => c.ID_Cliente)
                .HasColumnName("id_cliente");

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Nome)
                .HasColumnName("nome");

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Email)
                .HasColumnName("email");

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Telefone)
                .HasColumnName("telefone");

            // ================================
            // Mapeamento da entidade Pedido
            // ================================
            modelBuilder.Entity<Pedido>()
                .ToTable("pedido")
                .HasKey(p => p.ID_Pedido);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.ID_Pedido)
                .HasColumnName("id_pedido");

            modelBuilder.Entity<Pedido>()
                .Property(p => p.ID_Cliente)
                .HasColumnName("id_cliente");

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Data_Pedido)
                .HasColumnName("data_pedido");

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Total_Pedido)
                .HasColumnName("total_pedido");

            // Relacionamento Pedido-Cliente
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ID_Cliente)
                .HasConstraintName("fk_pedido_cliente");

            // ================================
            // Mapeamento da entidade ItemPedido
            // ================================
            modelBuilder.Entity<ItemPedido>()
                .ToTable("itempedido")
                .HasKey(i => i.ID_ItemPedido);

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.ID_ItemPedido)
                .HasColumnName("id_itempedido");

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.ID_Pedido)
                .HasColumnName("id_pedido");

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.ID_Produto)
                .HasColumnName("id_produto");

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.Quantidade)
                .HasColumnName("quantidade");

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.Preco_Unitario)
                .HasColumnName("preco_unitario");

            // Relacionamento ItemPedido-Pedido
            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Pedido)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(i => i.ID_Pedido)
                .HasConstraintName("fk_itempedido_pedido");

            // Relacionamento ItemPedido-Produto
            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Produto)
                .WithMany()
                .HasForeignKey(i => i.ID_Produto)
                .HasConstraintName("fk_itempedido_produto");

            // ================================
            // Mapeamento da entidade Produto
            // ================================
            modelBuilder.Entity<Produto>()
                .ToTable("produto")
                .HasKey(p => p.ID_Produto);

            modelBuilder.Entity<Produto>()
                .Property(p => p.ID_Produto)
                .HasColumnName("id_produto");

            modelBuilder.Entity<Produto>()
                .Property(p => p.Nome)
                .HasColumnName("nome");

            modelBuilder.Entity<Produto>()
                .Property(p => p.Categoria)
                .HasColumnName("categoria");

            modelBuilder.Entity<Produto>()
                .Property(p => p.Preco)
                .HasColumnName("preco");

            // ================================
            // Mapeamento da entidade Producao
            // ================================
            modelBuilder.Entity<Producao>()
                .ToTable("producao")
                .HasKey(p => p.ID_Producao);

            modelBuilder.Entity<Producao>()
                .Property(p => p.ID_Producao)
                .HasColumnName("id_producao");

            modelBuilder.Entity<Producao>()
                .Property(p => p.ID_Produto)
                .HasColumnName("id_produto");

            modelBuilder.Entity<Producao>()
                .Property(p => p.Data_Producao)
                .HasColumnName("data_producao");

            modelBuilder.Entity<Producao>()
                .Property(p => p.Quantidade_Produzida)
                .HasColumnName("quantidade_produzida");

            // Relacionamento Producao-Produto
            modelBuilder.Entity<Producao>()
                .HasOne(p => p.Produto)
                .WithMany()
                .HasForeignKey(p => p.ID_Produto)
                .HasConstraintName("fk_producao_produto");
        }

    }
}
