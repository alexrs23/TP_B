using APIRestFull.Entities;
using APIRestFull.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APIRestFull.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrinhoCompraController : ControllerBase
    {
        private readonly ICarrinhoRepository _repository;
        public CarrinhoCompraController(ICarrinhoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetCarrinhoByCliente/{clienteId}")]
        public async Task<IActionResult> GetCarrinhoByCliente(string clienteId)
        {
            var carrinho = await _repository.GetCarrinhoByCliente(clienteId);
            if (carrinho is null)
                return NotFound();
            return Ok(carrinho);
        }

        [HttpDelete("DeleteItem/{itemCarrinhoId}")]
        public async Task<IActionResult> DeleteItem(int itemCarrinhoId)
        {
            var result = await _repository.DeleteItem(itemCarrinhoId);
            if (!result)
                return NotFound();
            return Ok();
        }

        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItem(ItemCarrinhoCompra itemCarrinho)
        {
            Console.WriteLine("BORA");
            var result = await _repository.AddItem(itemCarrinho);
            if (!result)
                return BadRequest();
            return Ok(result);
        }
    }
}