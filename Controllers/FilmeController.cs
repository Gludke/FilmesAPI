using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using FilmesAPI.Profiles;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmesAPI.Controllers
{
    [ApiController] // classe para criar a API
    [Route("[controller]")] // define q a rota para acessa a API será sempre 'nome' + 'Controller', ou seja, FilmeController
    public class FilmeController : ControllerBase
    {
        private readonly FilmeContext _context;
        private readonly IMapper _mapper;

        public FilmeController(FilmeContext context, IMapper mapper) //preenchido por injeção de dependência
        {
            this._context = context;
            this._mapper = mapper;
        }

        // palavra reservada do método Rest, que significa CRIAR alguma coisa
        [HttpPost] // estou dizendo que a inf enviada pelo Programa Postman vai postar a info no server da minha API
        public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
        {
            Filme filme = _mapper.Map<Filme>(filmeDto); //estou convertendo filmeDto para a classe Filme e.. 
                //..armazenando na variável filme.
           _context.Filmes.Add(filme);
           _context.SaveChanges();
           return CreatedAtAction(nameof(RecuperaFilmesPorId), new { Id = filme.Id }, filme); 
        }

        [HttpGet]
        public IEnumerable<Filme> RecuperarFilmes() //podemos voltar ao retorno IEnumerable. Aqui o IAction não é necessário
        {
            return _context.Filmes.ToList();
        }

        [HttpGet("{id}")] // o id é passado pela url http://... do Postman para realizar a busca
        public IActionResult RecuperaFilmesPorId(int id)
        {
            var filme = _context.Filmes.Where(f => f.Id == id).FirstOrDefault();
            if (filme != null)
            {
                ReadFilmeDto filmeDto = _mapper.Map<ReadFilmeDto>(filme);

                return Ok(filmeDto); // padrão de retorno quando dá certo a requisição GET. também retorna o objeto pesquisado
            }
            return NotFound(); //padrão em caso do objeto não ser encontrado.
        }
        
        // IActionResult retorna algum resultado ao usuário de uma ação que foi executada.

        [HttpPut("{id}")] //"PUT" é a requisição utilizada para atualizar
        public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto) //estamos recenbendo como..
        { //..parâmetros o id para achar o objeto e o novo objeto
            Filme filme = _context.Filmes.Where(f => f.Id == id).FirstOrDefault();
            if (filme == null)
            {
                return NotFound();
            }
            _mapper.Map(filmeDto, filme);//dessa vez estamos jogando as informações de filmeDto para filme e..
            //..salvando no BD um objeto do tipo Filme.
            _context.SaveChanges();
            return NoContent(); //boa prática retornar 'semConteúdo' para updates
        }
        
        [HttpDelete("{id}")] //requisição para deletar por ID
        public IActionResult ExcluirFilme(int id)
        {
            var filme = _context.Filmes.Where(f => f.Id == id).FirstOrDefault();
            if (filme == null)
            {
                return NotFound();
            }
            _context.Filmes.Remove(filme);
            _context.SaveChanges();
            return NoContent(); //boa prática retornar 'semConteúdo' para deletes
        }
    }
}
