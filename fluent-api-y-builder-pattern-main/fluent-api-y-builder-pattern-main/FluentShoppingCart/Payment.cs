using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentShoppingCart
{
    public class Payment
    {
        internal Payment()
        {

        }
        public string Name { get; internal set; }
        public string SecurityCode { get; internal set; }

    }
}
