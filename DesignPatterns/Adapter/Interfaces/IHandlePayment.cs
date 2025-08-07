
using Adapter.Models;

namespace Adapter.Interfaces
{
	public interface IHandlePayment
	{
		void Execute(PaymentModel payment);
	}
}
