using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BartenderApp.DTOs
{
    public partial class SaleDTO : ObservableObject
    {
        [ObservableProperty]
        public int id;
        [ObservableProperty]
        public string? name;
        [ObservableProperty]
        public DateTime? dateSale;
        [ObservableProperty]
        public int amount;
        [ObservableProperty]
        public int pay;

        [ObservableProperty]
        public bool cancelled;
        public int Debt => Amount - Pay;
        [ObservableProperty]
        public string addPay;

        partial void OnAmountChanged(int value)
        {
            OnPropertyChanged(nameof(Debt)); // Notifica que Result ha cambiado
        }

        partial void OnPayChanged(int value)
        {
            OnPropertyChanged(nameof(Debt)); // Notifica que Result ha cambiado
        }
    }
}
