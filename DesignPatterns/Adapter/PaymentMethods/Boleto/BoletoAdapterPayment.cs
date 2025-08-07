using Adapter.Interfaces;
using Adapter.Models;
namespace Adapter.PaymentMethods.Boleto
{
	internal class BoletoAdapterPayment : IHandlePayment
	{
		private readonly BoletoPayment _boletoPayment;
		public BoletoAdapterPayment(BoletoPayment boletoPayment)
		{
			_boletoPayment = boletoPayment;
		}
		public void Execute(PaymentModel payment)
		{
			_boletoPayment.DoTransferBarCode(payment.BarCode);
		}
	}
}