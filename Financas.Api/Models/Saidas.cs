using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("saidas")]
public class Saidas
{
    [Key]
    [Column("idsaida")]
    public int IdSaida {get;set;}
    [Column("descricao")]
    public string Descricao{get;set;}
    [Column("valor_total", TypeName = "numeric(10,2)")]
    public decimal ValorTotal {get;set;}
    [Column("data_compra")]
    public DateTime DataCompra {get;set;}
    [Column("idcategoria")]
    public int IdCategoria {get;set;}
    [Column("idtipopagamento")]
    public int IdTipoPagamento {get;set;} 
    [Column("total_parcelas")]
    public int TotalParcelas {get;set;} = 1;


    [ForeignKey(nameof(IdCategoria))] // nome da tabela que é fk
    public Categorias Categoria {get;set;}

    [ForeignKey(nameof(IdTipoPagamento))]
    public Pagamentos TipoPagamento {get;set;}

    public List<Parcelas>? Parcelas {get;set;}
}