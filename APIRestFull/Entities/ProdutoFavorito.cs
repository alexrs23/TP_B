using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIRestFull.Entities
{
    public class ProdutoFavorito
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProdutoId { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public bool Efavorito { get; set; }
    }
}