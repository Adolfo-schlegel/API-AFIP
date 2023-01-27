using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentShoppingCart
{
    public class ShoppingCartBuilder : IClientBuilder,IItemsBuilder, IDeliveryBuilder, IPaymentBuilder, ICheckoutBuilder
    {
        ShoppingCart _cart;
        private static ShoppingCartBuilder _builder;
        internal ShoppingCartBuilder()
        {
            _cart = new();
            
        }
        public IItemsBuilder AddItem(Func<IItemBuilderStep1, Item> itemConfigurator)
        {
            Item _item = itemConfigurator.Invoke(new ItemBuilder());
            _cart.AddItem(_item);
            return this;
        }

        public ICheckoutBuilder Checkout()
        {
            return this;
        }

       
        public IPaymentBuilder SetPayment(Func<IPaymentBuilderStep1, Payment> paymentConfigurator)
        {
            _cart.Payment = paymentConfigurator.Invoke(new PaymentBuilder());

            return this;
        }

        public ShoppingCart Process()
        {
            return _cart;
        }

        public IDeliveryBuilder SetDelivery(Func<IDeliveryBuilderStep1,Delivery> deliveryConfigurator)
        {
            _cart.Delivery = deliveryConfigurator.Invoke(new DeliveryBuilder());
            return this;
        }

        public IItemsBuilder ForClient(string name)
        {
            _cart.Client = new(name);
            return this;
        }
    }


}
