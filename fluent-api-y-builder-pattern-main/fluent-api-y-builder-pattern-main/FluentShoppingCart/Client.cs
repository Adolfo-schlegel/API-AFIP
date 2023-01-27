using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentShoppingCart
{
    public class Client
    {
        internal Client(string name)
        {
            this.Name = name;
        }
        public string Name { get; internal set; }
    }
}
