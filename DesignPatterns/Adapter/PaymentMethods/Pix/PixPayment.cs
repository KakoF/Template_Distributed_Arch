namespace Adapter.PaymentMethods.Pix
{
	public class PixPayment
	{
		public void DoPix(string destinationPixKey, decimal amount)
		{
			Console.WriteLine($"Pagamento realizado para a chave {destinationPixKey}, no valor de {amount}");
		}
	}
}
