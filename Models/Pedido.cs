using PIMWebAPI.Models;

public class Pedido
{
    public int ID_Pedido { get; set; }
    public int ID_Cliente { get; set; }
    public DateTime Data_Pedido { get; set; }
    public decimal Total_Pedido { get; set; }

    public Cliente? Cliente { get; set; }  // Propriedade de navegação opcional para Cliente
    public ICollection<ItemPedido>? ItensPedido { get; set; }  // Propriedade de navegação opcional para ItemPedido
}
