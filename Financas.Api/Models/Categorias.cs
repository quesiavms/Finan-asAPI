using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("categorias")]
public class Categorias
{
    [Key]
    [Column("idcategoria")]
    public int IdCategoria {get;set;}
    [Column("categoria")]
    public string Categoria {get;set;}
    [Column("ativo")]
    public bool IsActive {get;set;}
}