using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

[ApiController]
[Route("api/v1/[controller]")] // https://localhost/api/v1/categorias
public class CategoriasController : ControllerBase
{
    private readonly DataBaseContext _dbcontext;

    public CategoriasController(DataBaseContext context)
    {
        _dbcontext = context;
    }

    [HttpGet]
    public IActionResult GetAllCategorias()
    {
        return Ok(_dbcontext.Categorias.ToList());
    }

    [HttpPost]
    public IActionResult AddNewCategoria([FromBody] CategoriasDto categoriaDto)
    {
        if(categoriaDto == null)
        {
            return  BadRequest("Categoria nao pode ser um valor nulo");  
        }

        var newCategoria = new Categorias
        {
          Categoria = categoriaDto.Categoria,
          IsActive = true
        };

        _dbcontext.Categorias.Add(newCategoria);
        _dbcontext.SaveChanges();
        
        return Ok(newCategoria);
    }

    [HttpDelete("{id}")]
    public IActionResult DeactivatedCategoria([FromRoute] int id)
    {
        Categorias categoriaToDelete = _dbcontext.Categorias.Where(x => x.IdCategoria == id).FirstOrDefault();

        if(categoriaToDelete == null)
        {
            return NotFound("Categoria nao encontrada!");
        }

        categoriaToDelete.IsActive = false;
        _dbcontext.SaveChanges();

        return Ok($"Categoria {categoriaToDelete.Categoria} foi desativada com sucesso!");
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCategoria([FromRoute] int id, [FromBody] CategoriasDto categoria)
    {
        var categoriaToUpdate = _dbcontext.Categorias.Where(x => x.IdCategoria == id).FirstOrDefault();

        if(categoriaToUpdate == null)
        {
            return NotFound("Categoria nao encontada!");
        }

        categoriaToUpdate.Categoria = categoria.Categoria;
        categoriaToUpdate.IsActive = true;

        _dbcontext.SaveChanges();
        
        return Ok();
    }

}