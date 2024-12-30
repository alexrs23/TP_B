using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace RCLAPI.DTO;
public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Ordem { get; set; }
    public string UrlImagem { get; set; }
    public string? Imagem { get; set; } // Alterado para string
    public string? ImageFile { get; set; }
}