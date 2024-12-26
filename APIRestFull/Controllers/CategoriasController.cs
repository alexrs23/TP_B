using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using APIRestFull.Context;
using APIRestFull.Entities;
using Microsoft.AspNetCore.Mvc;
using APIRestFull.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace APIRestFull.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository categoriaRepository;

        public CategoriasController(ICategoriaRepository categoriaRepository)
        {
            this.categoriaRepository = categoriaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categorias = await categoriaRepository.GetCategorias();
            return Ok(categorias);
        }
    }
}
