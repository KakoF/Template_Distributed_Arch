using Adapter.Interfaces;
using Adapter.Models;

namespace Adapter.PaymentMethods.Pix
{
	public class PixAdapterPayment : IHandlePayment
	{
		private readonly PixPayment _pixPayment;
		public PixAdapterPayment(PixPayment pixPayment)
		{
			_pixPayment = pixPayment;
		}
		public void Execute(PaymentModel payment)
		{
			_pixPayment.DoPix(payment.DestinationPix, payment.Amount);
		}
	}
}
