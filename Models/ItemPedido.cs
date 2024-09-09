using PIMWebAPI.Models;

public class ItemPedido
{
    // ================================
    // Propriedades Principais
    // ================================
    public int ID_ItemPedido { get; set; }  // Chave primária para o ItemPedido
    public int ID_Pedido { get; set; }  // Chave estrangeira que referencia a entidade Pedido
    public int ID_Produto { get; set; }  // Chave estrangeira que referencia a entidade Produto
    public int Quantidade { get; set; }  // Quantidade do produto no pedido
    public decimal Preco_Unitario { get; set; }  // Preço unitário do produto

    // ================================
    // Propriedades de Navegação Opcionais
    // ================================
    // A propriedade Pedido representa o relacionamento entre ItemPedido e Pedido.
    // Marcada como opcional (nullable) para casos em que o Pedido pode não ser necessário em alguns contextos.
    public Pedido? Pedido { get; set; }  // Propriedade de navegação opcional para Pedido

    // A propriedade Produto representa o relacionamento entre ItemPedido e Produto.
    // Marcada como opcional (nullable) para casos em que o Produto pode não ser necessário em alguns contextos.
    public Produto? Produto { get; set; }  // Propriedade de navegação opcional para Produto
}
