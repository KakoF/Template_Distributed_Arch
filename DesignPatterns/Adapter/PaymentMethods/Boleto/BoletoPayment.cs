namespace Adapter.PaymentMethods.Boleto
{
	public class BoletoPayment
	{
		public void DoTransferBarCode(string barcode)
		{
			Console.WriteLine($"Pagamento do boleto feito");
		}
	}
}
