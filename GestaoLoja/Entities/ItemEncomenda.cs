using GestaoLoja.Data;

namespace GestaoLoja.Entities;

public class ItemEncomenda
{
    public int ItemEncomendaId { get; set; }
    public int ProdutoId { get; set; }  // Chave estrangeira para o produto
    public Produto Produto { get; set; }  // Produto comprado
    public int Quantidade { get; set; }  // Quantidade comprada
    public decimal PrecoUnitario { get; set; }  // Preço do produto
    public int EncomendaId { get; set; }  // Chave estrangeira para a venda
    public Encomenda Encomenda { get; set; }  // Referência à venda
}
