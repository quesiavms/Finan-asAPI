using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

[ApiController]
[Route("api/v1/[controller]")] // https://localhost/api/v1/entradas
public class EntradasController : ControllerBase
{
    private readonly DataBaseContext _dbcontext;

    public EntradasController(DataBaseContext context)
    {
        _dbcontext = context;
    }

    [HttpGet]
    public IActionResult GetAllEntradas()
    {
        return Ok(_dbcontext.Entradas.ToList());
    }

    [HttpPost]
    public IActionResult AddNewEntrada([FromBody] EntradasDto entradas)
    {
        var newEntradas = new Entradas
        {
            Valor = entradas.Valor,
            Nome = entradas.Nome,
            Descricao = entradas.Descricao,
            Date = DateTime.UtcNow 
        };

        _dbcontext.Entradas.Add(newEntradas);
        _dbcontext.SaveChanges();

        return Ok(newEntradas);
    }

    [HttpDelete("{id}")]
    public IActionResult RemoveEntrada([FromRoute] int id)
    {
        var entradaToDelete =_dbcontext.Entradas.Where(x => x.IdEntrada == id).FirstOrDefault();

        if(entradaToDelete == null)
        {
            return NotFound("Entrada nao encontrada!");
        }

        _dbcontext.Entradas.Remove(entradaToDelete);
        _dbcontext.SaveChanges();


        return Ok("Entrada foi deletada com sucesso");
    }

    [HttpPut("{id}")]
    public IActionResult UpdateEntrada([FromRoute] int id, [FromBody] EntradasDto entradaUpdated)
    {
        var entradaToUpdate = _dbcontext.Entradas.Where(x => x.IdEntrada == id).FirstOrDefault();
        
        if(entradaToUpdate == null)
        {
            return NotFound("Entrada nao encontrada pra atualizar");
        }

        entradaToUpdate.Valor = entradaUpdated.Valor;
        entradaToUpdate.Nome = entradaUpdated.Nome;
        entradaToUpdate.Descricao = entradaUpdated.Descricao;        
        entradaToUpdate.Date = entradaUpdated.Date;

        _dbcontext.SaveChanges();

        return Ok("Entrada atualizada com sucesso");
    }
}