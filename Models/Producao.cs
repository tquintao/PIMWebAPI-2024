namespace PIMWebAPI.Models
{
    public class Producao
    {
        public int ID_Producao { get; set; }
        public int ID_Produto { get; set; }
        public DateTime Data_Producao { get; set; }
        public int Quantidade_Produzida { get; set; }

        // Propriedade de navegação opcional para Produto
        public Produto? Produto { get; set; }
    }

}
