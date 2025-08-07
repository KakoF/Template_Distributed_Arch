namespace Adapter.PaymentMethods.Card
{
	public class CardPayment
	{
		public void DoCardTransfer(string branch, string cardNumber, decimal amount)
		{
			Console.WriteLine($"Pagamento realizado para o cartao {branch}-{cardNumber}, no valor de {amount}");
		}
	}
}
