using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("parcelas")]
public class Parcelas
{
    [Key]
    [Column("idparcela")]
    public int IdParcela {get;set;}
    [Column("idsaida")]
    public int IdSaida{get;set;}
    [Column("numero_parcela")]
    public int NumeroParcela {get;set;}
    [Column("valor", TypeName = "numeric(10,2)")]
    public decimal ValorParcela {get;set;}
    [Column("vencimento")]
    public DateTime Vencimento {get;set;}
    [Column("pago")]
    public bool Pago {get;set;}
    [Column("data_pagamento")]
    public DateTime DataPagamento {get;set;}


    [ForeignKey(nameof(IdSaida))] // nome da tabela que é fk
    public Saidas Saida {get;set;}
}