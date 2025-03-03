using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BartenderApp.Utils.Messages.Sale
{
    public class SaleMessaging : ValueChangedMessage<SaleMessage>
    {
        public SaleMessaging(SaleMessage value) : base(value)
        {

        }
    }
}
