using Adapter.Interfaces;
using Adapter.Models;

namespace Adapter.PaymentMethods.Card
{
	public class CardAdapterPayment : IHandlePayment
	{
		private readonly CardPayment _cardPayment;

		public CardAdapterPayment(CardPayment cardPayment)
		{
			_cardPayment = cardPayment;
		}
		public void Execute(PaymentModel payment)
		{
			_cardPayment.DoCardTransfer(payment.Branch, payment.CardNumber, payment.Amount);
		}
	}
}