using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIRestFull.Context;
using APIRestFull.Entities;
using APIRestFull.Repositories;

namespace APIRestFull.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutosController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProdutos(string tipoProduto, int? categoriaId = null)
        {
            IEnumerable<Produto> produtos;

            if (tipoProduto == "categoria" && categoriaId != null)
            {
                produtos = await _produtoRepository.ObterProdutosPorCategoriaAsync(categoriaId.Value);
            }
            else if (tipoProduto == "detalhe" && categoriaId != null)
            {
                Produto produto = await _produtoRepository.ObterDetalheProdutoAsync(categoriaId.Value);

                return Ok(produto);
            }
            else if (tipoProduto == "promocao")
            {
                var promocoes = await _produtoRepository.ObterProdutosPromocaoAsync();

                return Ok(promocoes);
            }
            else if (tipoProduto == "maisvendido")
            {
                produtos = await _produtoRepository.ObterProdutosMaisVendidosAsync();
            }
            else if (tipoProduto == "todos")
            {
                produtos = await _produtoRepository.ObterTodosProdutosAsync();
            }
            else
            {
                return BadRequest("Tipo de produto inválido");
            }

            return Ok(produtos);
        }

        [HttpGet("DetalheProduto/{id}")]
        public async Task<IActionResult> GetDetalheProduto(int id)
        {
            Produto produto = await _produtoRepository.ObterDetalheProdutoAsync(id);

            if (produto is null)
            {
                return NotFound($"Produto com id={id} não encontrado");
            }
            return Ok(produto);
        }
    }
}
