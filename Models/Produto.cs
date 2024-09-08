public class Produto
{
    public int ID_Produto { get; set; }

    // Marcar como opcional, removendo [Required] se estiver presente
    public string? Nome { get; set; }

    public string? Categoria { get; set; }

    public decimal Preco { get; set; }
}
