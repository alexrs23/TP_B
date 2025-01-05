using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class ItemCarrinhoCompra
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public string ClienteId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal ValorTotal { get; set; }
    public string imageURL { get; set; }
}