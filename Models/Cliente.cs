namespace PIMWebAPI.Models
{
    public class Cliente
    {
        public int ID_Cliente { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }

        // Propriedade de navegação para pedidos
        public ICollection<Pedido> Pedidos { get; set; }
    }

}
