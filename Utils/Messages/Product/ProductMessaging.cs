
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BartenderApp.Utils.Messages.Product
{
    public class ProductMessaging : ValueChangedMessage<ProductMessage>
    {
        public ProductMessaging(ProductMessage value) : base(value)
        {
                
        }
    }
}
