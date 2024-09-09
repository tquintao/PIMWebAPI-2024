namespace PIMWebAPI.Data
{
    using Models; // Importação do namespace que contém os modelos
    using Microsoft.EntityFrameworkCore; // Necessário para trabalhar com Entity Framework Core

    public class AppDbContext : DbContext
    {
        // Construtor que recebe as opções do contexto (configurações de banco de dados)
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Definindo DbSets que representam as tabelas no banco de dados
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<Producao> Producoes { get; set; }

        // Método OnConfiguring define a string de conexão para o banco de dados PostgreSQL
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configurando o Npgsql para conexão com o banco PostgreSQL
            optionsBuilder.UseNpgsql
                ("Host=junction.proxy.rlwy.net;Port=54854;Database=railway;Username=postgres;Password=ceBlSRWMiXkclKmossHIDEQPaTaCGvoh");
        }

        // Mapeamento das entidades para o banco de dados, definindo tabelas, chaves e relacionamentos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ================================
            // Mapeamento da entidade Cliente
            // ================================
            modelBuilder.Entity<Cliente>()
                .ToTable("cliente")  // Mapeia a entidade para a tabela "cliente"
                .HasKey(c => c.ID_Cliente); // Define a chave primária

            modelBuilder.Entity<Cliente>()
                .Property(c => c.ID_Cliente)
                .HasColumnName("id_cliente"); // Define o nome da coluna no banco de dados

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
                .ToTable("pedido") // Mapeia a entidade para a tabela "pedido"
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

            // Relacionamento Pedido-Cliente (um cliente tem muitos pedidos)
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)  // Cliente pode ter vários pedidos
                .HasForeignKey(p => p.ID_Cliente) // Define a chave estrangeira
                .HasConstraintName("fk_pedido_cliente");

            // ================================
            // Mapeamento da entidade ItemPedido
            // ================================
            modelBuilder.Entity<ItemPedido>()
                .ToTable("itempedido") // Mapeia a entidade para a tabela "itempedido"
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

            // Relacionamento ItemPedido-Pedido (um pedido pode ter muitos itens)
            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Pedido)
                .WithMany(p => p.ItensPedido)  // Pedido pode ter vários itens
                .HasForeignKey(i => i.ID_Pedido)
                .HasConstraintName("fk_itempedido_pedido");

            // Relacionamento ItemPedido-Produto (um item está relacionado a um produto)
            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Produto)
                .WithMany() // Produto não precisa conhecer os itens
                .HasForeignKey(i => i.ID_Produto)
                .HasConstraintName("fk_itempedido_produto");

            // ================================
            // Mapeamento da entidade Produto
            // ================================
            modelBuilder.Entity<Produto>()
                .ToTable("produto") // Mapeia a entidade para a tabela "produto"
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
                .ToTable("producao") // Mapeia a entidade para a tabela "producao"
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

            // Relacionamento Producao-Produto (uma produção está relacionada a um produto)
            modelBuilder.Entity<Producao>()
                .HasOne(p => p.Produto)
                .WithMany() // Produto não precisa conhecer as produções
                .HasForeignKey(p => p.ID_Produto)
                .HasConstraintName("fk_producao_produto");
        }
    }
}
