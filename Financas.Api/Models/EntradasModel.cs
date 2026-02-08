using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("entradas")]
public class Entradas
{
    [Key]
    [Column("identrada")]
    public int IdEntrada {get;set;}
    [Column("valor")]
    public float Valor {get;set;}
    [Column("nome")]
    public string Nome {get;set;}
    [Column("descricao")]
    public string Descricao {get;set;}
    [Column("date")]
    public DateTime Date {get;set;}
}