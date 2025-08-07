using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter.Models
{
	public record class PaymentModel(string DestinationPix, string BarCode, string Branch, string CardNumber, decimal Amount);
}
