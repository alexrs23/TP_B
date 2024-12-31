using GestaoLoja.Data;

namespace GestaoLoja.Entities;

public class Encomenda
{
    public int EncomendaId { get; set; }
    public DateTime DataEncomenda { get; set; }
    public string ClienteId { get; set; }  // Chave estrangeira do cliente (usuário)
    public ApplicationUser Cliente { get; set; }  // Referência ao cliente
    public int ModoEntregaId { get; set; }  // Tipo de entrega
    public ModoEntrega ModoEntrega { get; set; }  // Informações do modo de entrega
    public ICollection<ItemEncomenda> ItensEncomenda { get; set; }  // Itens comprados
    public decimal Total { get; set; }
}