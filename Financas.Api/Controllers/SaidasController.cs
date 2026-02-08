using System.Collections.Frozen;
using System.IO.Compression;
using System.Numerics;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

[ApiController]
[Route("api/v1")] // https://localhost/api/v1/saidas
public class SaidasController : ControllerBase
{
    
    private readonly DataBaseContext _dbcontext;

    public SaidasController(DataBaseContext context)
    {
        _dbcontext = context;
    }

    [HttpGet("Saida")]
    public IActionResult getAllSaidas()
    {
        var dto = _dbcontext.Saidas
        .Include(s => s.Categoria)
        .Include(s => s.TipoPagamento)
        .Include(s => s.Parcelas)
        .Select( s=> new SaidasResponseDto
        {
            IdSaida = s.IdSaida,
            Descricao = s.Descricao,
            ValorTotal = s.ValorTotal,
            DataCompra = s.DataCompra,
            Categoria = s.Categoria.Categoria,
            TipoPagamento = s.TipoPagamento.TipoPagamento,
            Parcelas = s.Parcelas.Select( p => new ParcelasResponseDto
            {
                IdParcela = p.IdParcela,
                NumeroParcela = p.NumeroParcela,
                Valor = p.ValorParcela,
                Vencimento = p.Vencimento,
                Pago = p.Pago,
                DataPagamento = p.DataPagamento
            }).ToList()
        })
        .ToList();

        return Ok(dto);    
    }

    [HttpGet("SaidaById")]
    public IActionResult getSaidabyId([FromQuery] int idSaida)
    {
        var s = _dbcontext.Saidas
            .Include(s => s.Categoria)
            .Include(s => s.TipoPagamento)
            .Include(s => s.Parcelas)
            .Where(s => s.IdSaida == idSaida)
            .Select(s => new SaidasResponseDto
            {
                IdSaida = s.IdSaida,
                Descricao = s.Descricao,
                ValorTotal = s.ValorTotal,
                DataCompra = s.DataCompra,
                Categoria = s.Categoria.Categoria,
                TipoPagamento = s.TipoPagamento.TipoPagamento,
                TotalParcelas = s.TotalParcelas,
                Parcelas = s.Parcelas.Select(p => new ParcelasResponseDto
                {
                    IdParcela = p.IdParcela,
                    NumeroParcela = p.NumeroParcela,
                    Valor = p.ValorParcela,
                    Vencimento = p.Vencimento,
                    Pago = p.Pago,
                    DataPagamento = p.DataPagamento
                }).ToList()
            })
            .FirstOrDefault();

            if(s == null)
                return NotFound("Saida nao encontrada!");
            
            return Ok(s);
    }

    [HttpPost("Saida")]
    public IActionResult criarSaida([FromBody] SaidasDto saida)
    {
        if(saida == null)
        {
            return BadRequest("Saida nao pode ser nulo aqui!");    
        }

        var categoria = _dbcontext.Categorias.Where(c => c.IdCategoria == saida.IdCategoria && c.IsActive == true).FirstOrDefault();

        if(categoria == null)
            return BadRequest("Categoria invalida ou inativa");

        var tipoPagamento = _dbcontext.Pagamentos.Where(p => p.IdPagamento == saida.IdTipoPagamento).FirstOrDefault();

        if (tipoPagamento == null)
            return BadRequest("Tipo de pagamento inválido.");

        var novaSaida = new Saidas
        {
            Descricao = saida.Descricao,
            ValorTotal = saida.ValorTotal,
            DataCompra = saida.DataCompra,
            TotalParcelas = saida.TotalParcelas,
            IdCategoria = saida.IdCategoria,
            IdTipoPagamento = saida.IdTipoPagamento
        };

        _dbcontext.Saidas.Add(novaSaida);
        _dbcontext.SaveChanges();

        var listaParcelas = new List<Parcelas>();
        
        if(novaSaida.TotalParcelas > 1) // criando parcelas
        {
            decimal valor_parcela = Math.Round(novaSaida.ValorTotal / novaSaida.TotalParcelas, 2);

            for (int i = 1; i <= novaSaida.TotalParcelas; i++)
            {
                listaParcelas.Add(new Parcelas
                {
                    IdSaida = novaSaida.IdSaida,
                    NumeroParcela = i,
                    ValorParcela = valor_parcela,
                    Vencimento = novaSaida.DataCompra.AddMonths(i -1),
                    Pago = false
                });
            }
            
            _dbcontext.Parcelas.AddRange(listaParcelas);
            _dbcontext.SaveChanges();
        }
        else
        {
            _dbcontext.Parcelas.Add( new Parcelas
            {
               IdSaida = novaSaida.IdSaida,
               NumeroParcela =1,
               ValorParcela = saida.ValorTotal,
               Vencimento = novaSaida.DataCompra,
               Pago = false 
            });
            _dbcontext.SaveChanges();
        }

        // Select LINQ => percorre cada elemento da coleção e transforma ele em outra coisa.
        var listaParcelasDto = listaParcelas.Select(p => new ParcelasResponseDto // para cada elemento da lista, ele criar uma response dto
        {
            IdParcela = p.IdParcela,
            NumeroParcela = p.NumeroParcela,
            Valor = p.ValorParcela,
            Vencimento = p.Vencimento,
            Pago = p.Pago,
            DataPagamento = p.DataPagamento
        }).ToList();

        var dto = new SaidasResponseDto
        {
            IdSaida = novaSaida.IdSaida,
            Descricao = novaSaida.Descricao,
            ValorTotal = novaSaida.ValorTotal,
            DataCompra = novaSaida.DataCompra,
            Categoria = categoria.Categoria,
            TipoPagamento = tipoPagamento.TipoPagamento,
            TotalParcelas = novaSaida.TotalParcelas,
            Parcelas = listaParcelasDto
        };

        return Ok(dto);
    }

    [HttpPut("Parcela/PagarParcelaUnica")]
    public IActionResult pagarParcela([FromQuery] int idSaida)
    {
        var parcela = _dbcontext.Parcelas.Where(x => x.IdSaida == idSaida && x.Pago == false).FirstOrDefault();
        
        if(parcela == null)
            return NotFound("Parcela nao encontrada!");

        parcela.Pago = true;
        parcela.DataPagamento = DateTime.UtcNow;
        _dbcontext.SaveChanges();

        var dto = new ParcelasResponseDto
        {
            IdParcela = parcela.IdParcela,
            NumeroParcela = parcela.NumeroParcela,
            Valor = parcela.ValorParcela,
            Pago = parcela.Pago,
            DataPagamento = parcela.DataPagamento
        };

        return Ok(dto);
    }

    [HttpPatch("Parcela/PagarParcelaEspecifica")]
    public IActionResult pagarParcelaEspecifica([FromQuery] int idSaida,[FromQuery] int numeroDaParcela)
    {
        var parcela = _dbcontext.Parcelas.Where(p => p.IdSaida == idSaida && p.NumeroParcela == numeroDaParcela).FirstOrDefault();
        
        if(parcela == null)
            return NotFound("Parcela nao encontrada");
        
        if(parcela.Pago)
            return BadRequest("Parcela ja foi paga");
        
        parcela.Pago = true;
        parcela.DataPagamento = DateTime.UtcNow;
        _dbcontext.SaveChanges();

        var dto = new ParcelasResponseDto
        {
            IdParcela = parcela.IdParcela,
            NumeroParcela = parcela.NumeroParcela,
            Valor = parcela.ValorParcela,
            Pago = parcela.Pago,
            DataPagamento = parcela.DataPagamento
        };

        return Ok(dto);
    }       

    [HttpDelete("Saida")]
    public IActionResult deletarSaida([FromQuery] int idsaida)
    {
        var saida = _dbcontext.Saidas
            .Include(x => x.Parcelas)
            .Where(x => x.IdSaida == idsaida).FirstOrDefault();

        if(saida == null)
            return NotFound("Saida nao encontrada");


        _dbcontext.Parcelas.RemoveRange(saida.Parcelas);
        _dbcontext.Saidas.Remove(saida);
        _dbcontext.SaveChanges();

        return Ok("Saida e Parcelas deletadas com sucesso");
    }
}
