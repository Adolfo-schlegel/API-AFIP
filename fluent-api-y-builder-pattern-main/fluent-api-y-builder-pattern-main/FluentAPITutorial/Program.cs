using FluentShoppingCart;
using System;

namespace FluentAPITutorial
{
    class Program
    {
        static void Main(string[] args)
        {

            ShoppingCart cart =

                ShoppingCart.Create()
                .ForClient("Juan Pérez")
                .AddItem(i => i.WithName("Producto 1").WithPrice(100).WithQuantity(1))
                .AddItem(i => i.WithName("producto 2").WithPrice(250).WithQuantity(2))
                .AddItem(i=>i.WithName("un nombre").WithPrice(200).WithQuantity(1))
                .Checkout()
                .SetDelivery(d => d.WithAddress("direccion 1").WithPostalCode("123"))
                .SetPayment(p => p.WithName("visa").WithSecurityCode("123"))
                .Process();
        }
    }
}
