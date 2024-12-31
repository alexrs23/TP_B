using System.ComponentModel.DataAnnotations.Schema;

namespace APIRestFull.Entities
{
    public class Categoria
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int? Ordem { get; set; }
        public string? UrlImagem { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
