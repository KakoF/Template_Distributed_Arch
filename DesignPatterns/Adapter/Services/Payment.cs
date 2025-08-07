using Adapter.Interfaces;
using Adapter.Models;

namespace Adapter.Services
{
	public class Payment
	{
		private readonly IHandlePayment _payment;

		public Payment(IHandlePayment payment)
		{
			_payment = payment;
		}
		public void Handle(PaymentModel paymentModel)
		{
			_payment.Execute(paymentModel);
		}
	}
}
