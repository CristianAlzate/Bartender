using CommunityToolkit.Mvvm.ComponentModel;


namespace BartenderApp.DTOs
{
    public partial class SaleDetailDTO : ObservableObject
    {
        [ObservableProperty]
        public int id;
        [ObservableProperty]
        public int idSale;
        [ObservableProperty]
        public string? name;
        [ObservableProperty]
        public int price;
        [ObservableProperty]
        public int costPrice;
        [ObservableProperty]
        public int quantity;
        [ObservableProperty]
        public string addQuantity;
        public int Total => Price * Quantity;
        partial void OnQuantityChanged(int value)
        {
            OnPropertyChanged(nameof(Total)); // Notifica que Result ha cambiado
        }
    }
}
