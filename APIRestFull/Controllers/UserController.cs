using APIRestFull.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace APIRestFull.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("GetFavoritos/{username}")]
        public async Task<IActionResult> GetFavoritos(string username)
        {
            var favoritos = await _userRepository.GetFavoritos(username);
            if (favoritos is null) return NotFound();
            return Ok(favoritos);
        }

        [HttpPost("ActualizaFavorito/{acao}/{produtoId}")]
        public async Task<IActionResult> ActualizaFavorito(string acao, int produtoId)
        {
            var result = await _userRepository.ActualizaFavorito(acao, produtoId, "Jorge");
            if (!result)
                return BadRequest();
            return Ok(result);
        }
    }
}