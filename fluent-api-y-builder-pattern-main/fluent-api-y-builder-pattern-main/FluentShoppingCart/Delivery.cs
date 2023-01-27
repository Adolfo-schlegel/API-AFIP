using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentShoppingCart
{
    public class Delivery
    {
        internal Delivery()
        {

        }
        public string Address { get; internal set; }
        public string PostalCode { get; internal set; }
    }
}
