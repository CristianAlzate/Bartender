using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using BartenderApp.DataAccess;
using System.Collections.ObjectModel;
using BartenderApp.DTOs;
using BartenderApp.Utils.Messages.Sale;
using BartenderApp.Views.Home;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Maui.Views;

namespace BartenderApp.ViewModels
{
    public partial class SaleViewModel : ObservableObject
    {
        private readonly SalesDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<SaleDTO> saleList = new ObservableCollection<SaleDTO>();

        [ObservableProperty]
        private DateTime filterDate = DateTime.Now.AddHours(-6);
        [ObservableProperty]
        private DateTime maxDate = DateTime.Now.AddHours(-6);

        public IRelayCommand<DateTime> DateChangedCommand { get; }
        private async void OnDateChanged(DateTime newDate)
        {
            await GetAll();
        }

        public SaleViewModel(SalesDbContext context)
        {
            _dbContext = context;
            DateChangedCommand = new RelayCommand<DateTime>(OnDateChanged);
            MainThread.BeginInvokeOnMainThread(new Action(async () => await GetAll()));

            WeakReferenceMessenger.Default.Register<SaleMessaging>(this, (r, m) =>
            {
                SaleMessageReceived(m.Value);
            });
        }

        public async Task GetAll()
        {
            DateTime date = new DateTime(filterDate.Year, filterDate.Month, filterDate.Day);
            SaleList.Clear();
            var list = await _dbContext.Sales.Where(x => x.DateSale == date).ToListAsync();
            if (list.Any())
            {
                foreach (var item in list)
                {
                    SaleList.Add(new SaleDTO
                    {
                        Id = item.Id,
                        Amount = item.Amount,
                        Name = item.Name,
                        DateSale = item.DateSale,
                        Pay = item.Pay,
                        Cancelled = item.Cancelled
                    });
                }
            }
        }

        [RelayCommand]
        private async Task AddDetail(SaleDTO sale)
        {
            var uri = $"{nameof(SaleAddPage)}?id={sale.Id}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task ViewDetails(SaleDTO sale)
        {
            var uri = $"{nameof(SaleDetailPage)}?id={sale.Id}";
            await Shell.Current.GoToAsync(uri);
        }

        private void SaleMessageReceived(SaleMessage message)
        {
            var saleDto = message.SaleDto;
            if (message.IdAdd)
            {
                SaleList.Add(saleDto);
            }
            else
            {
                var found = SaleList.First(e => e.Id == saleDto.Id);
                found.Name = saleDto.Name;
                found.Amount = saleDto.Amount;
                found.DateSale = saleDto.DateSale;
                found.Pay = saleDto.Pay;
                found.Cancelled = saleDto.Cancelled;
            }

        }

        [RelayCommand]
        private async Task ShowAddPayPopup(SaleDTO saleDto)
        {
            var popup = new SalePayPopUp(new SaleAddViewModel(_dbContext, saleDto.Id));
            Shell.Current.CurrentPage.ShowPopup(popup);
        }

        [RelayCommand]
        private async Task ShowPopup()
        {
            var popup = new SaleAddPopUp(new SaleAddViewModel(_dbContext));
            Shell.Current.CurrentPage.ShowPopup(popup);
        }


    }
}
