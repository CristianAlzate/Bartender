using CommunityToolkit.Mvvm.ComponentModel;

namespace BartenderApp.DTOs
{
    public partial class ProductDTO : ObservableObject
    {
        [ObservableProperty]
        public int id;
        [ObservableProperty]
        public string? name;
        [ObservableProperty]
        public int price;
        [ObservableProperty]
        public int costPrice;
        [ObservableProperty]
        public string? searchName;
    }
}
