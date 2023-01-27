using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentShoppingCart
{
    public class Item
    {
        internal Item()
        {

        }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; internal set; }
    }
}
