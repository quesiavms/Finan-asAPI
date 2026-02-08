public class SaidasDto
{
    public string Descricao { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime DataCompra { get; set; }

    public int IdCategoria { get; set; }
    public int IdTipoPagamento { get; set; }

    public int TotalParcelas { get; set; } = 1;
}