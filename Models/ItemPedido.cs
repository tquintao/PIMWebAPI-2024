using PIMWebAPI.Models;

public class ItemPedido
{
    public int ID_ItemPedido { get; set; }
    public int ID_Pedido { get; set; }
    public int ID_Produto { get; set; }
    public int Quantidade { get; set; }
    public decimal Preco_Unitario { get; set; }

    /*public Pedido Pedido { get; set; }  // Propriedade de navegação para Pedido
    public Produto Produto { get; set; }  // Propriedade de navegação para Produto*/

    // Propriedades de navegação opcionais
    public Pedido? Pedido { get; set; }  // Propriedade de navegação opcional para Pedido
    public Produto? Produto { get; set; }  // Propriedade de navegação opcional para Produto
}
