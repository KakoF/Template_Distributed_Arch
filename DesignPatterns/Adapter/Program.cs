using Adapter.Enums;
using Adapter.Interfaces;
using Adapter.Models;
using Adapter.PaymentMethods;
using Adapter.Services;


IHandlePayment handlePayment = FactoryPaymentMethod.GetPaymentMethod(EPayment.Pix);
Payment payment = new Payment(handlePayment);

PaymentModel paymentModel = new PaymentModel(DestinationPix: "321-435454", Amount: 10m, BarCode: null, Branch: null, CardNumber: null);
payment.Handle(paymentModel);
//payment.Handle("321.321.321.11", 10m);