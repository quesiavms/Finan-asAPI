public class ParcelasResponseDto
{
    public int IdParcela { get; set; }
    public int NumeroParcela { get; set; }
    public decimal Valor { get; set; }
    public DateTime Vencimento { get; set; }
    public bool Pago { get; set; }
    public DateTime? DataPagamento { get;set; }
}
