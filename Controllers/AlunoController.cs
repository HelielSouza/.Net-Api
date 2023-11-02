using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoDotnet_API.Data;
using ProjetoDotnet_API.Models;

namespace ProjetoDotnet_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoController : Controller
    {
        private readonly EscolaContext _context;
        public AlunoController(EscolaContext context)
            {
            // construtor
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Aluno>> GetAll() 
        {
            return _context.AlunoApi.ToList();
        }

        [HttpGet("{AlunoId}")]
        public ActionResult <Aluno> Get (int AlunoId) 
        {
            try
            {
                var result = _context.AlunoApi.Find(AlunoId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> post(Aluno model)
        {
            try
            {
                _context.AlunoApi.Add(model);
                if (await _context.SaveChangesAsync() == 1)
                {
                    //return Ok();
                    return Created($"/api/aluno/{model.ra}",model);
                }
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
            // retorna BadRequest se não conseguiu incluir
            return BadRequest();
        }

        [HttpDelete("{AlunoId}")]
        public async Task<ActionResult> delete(int AlunoId)
        {
            try
            {
                //verifica se existe aluno a ser excluído
                var aluno = await _context.AlunoApi.FindAsync(AlunoId);
                if (aluno == null)
                {
                    //método do EF
                    return NotFound();
                }

                _context.Remove(aluno);
                await _context.SaveChangesAsync();
                return NoContent();
            }

            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falha no acesso ao banco de dados.");
            }
        }

        [HttpPut("{AlunoId}")]
        public async Task<IActionResult> put(int AlunoId, Aluno dadosAlunoAlt)
        {
            try 
            {
                //verifica se existe aluno a ser alterado
                var result = await _context.AlunoApi.FindAsync(AlunoId);

                if (AlunoId != result.id)
                {
                    return BadRequest();
                }

                result.ra = dadosAlunoAlt.ra;
                result.nome = dadosAlunoAlt.nome;
                result.codCurso = dadosAlunoAlt.codCurso;
                await _context.SaveChangesAsync();
                return Created($"/api/aluno/{dadosAlunoAlt.id}", dadosAlunoAlt);
            }

            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Falhano acesso ao banco de dados.");
            }
        }
    }
}