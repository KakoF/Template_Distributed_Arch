using Adapter.Enums;
using Adapter.Interfaces;
using Adapter.PaymentMethods.Boleto;
using Adapter.PaymentMethods.Card;
using Adapter.PaymentMethods.Pix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter.PaymentMethods
{
	public static class FactoryPaymentMethod
	{
		public static IHandlePayment GetPaymentMethod(EPayment ePayment)
		{
			switch (ePayment)
			{
				case EPayment.Pix:
					return new PixAdapterPayment(new PixPayment());
				case EPayment.Card:
					return new CardAdapterPayment(new CardPayment());
				case EPayment.Boleto:
					return new BoletoAdapterPayment(new BoletoPayment());
				default:
					throw new Exception("Não foi encontrado o meio de pagamento");
			}
		}
	}
}
