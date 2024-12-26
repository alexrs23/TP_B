using Microsoft.AspNetCore.Mvc;

namespace APIRestFull.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private static readonly string[] strings = new[] {

        "Roupas","relogios","higiene"};
        
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetProducts")]
        public IEnumerable<Product> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Product
            {
                id = index,
                roupas = Random.Shared.Next(1, 100),
                relogios = Random.Shared.Next(1, 100),
                Summary = strings[Random.Shared.Next(strings.Length)]
            })
            .ToArray();
        }
    }
   
}
