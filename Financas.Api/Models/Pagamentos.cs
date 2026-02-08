using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("pagamentos")]
public class Pagamentos
{
    [Key]
    [Column("idpagamento")]
    public int IdPagamento {get;set;}
    [Column("tipopagamento")]
    public string TipoPagamento{get;set;}
}