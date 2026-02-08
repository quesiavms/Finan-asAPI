public class SaidasResponseDto
{
    public int IdSaida { get; set; }
    public string? Descricao { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTime DataCompra { get; set; }

    public string? Categoria { get; set; }
    public string? TipoPagamento { get; set; }

    public int TotalParcelas { get; set; }
    public List<ParcelasResponseDto> Parcelas { get; set; }
}