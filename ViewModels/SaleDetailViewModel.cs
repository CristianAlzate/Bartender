using BartenderApp.DataAccess;
using BartenderApp.DTOs;
using BartenderApp.Utils.Messages.Sale;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace BartenderApp.ViewModels
{
    public partial class SaleDetailViewModel : ObservableObject, IQueryAttributable
    {
        private readonly SalesDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<SaleDetailDTO> saleDetailList = new ObservableCollection<SaleDetailDTO>();

        private int Id;

        [ObservableProperty]
        private bool loadingVisible = false;

        public SaleDetailViewModel(SalesDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            int id = int.Parse(query["id"].ToString());
            Id = id;
            MainThread.BeginInvokeOnMainThread(new Action(async () => await GetDetails(Id)));
        }

        public async Task GetDetails(int id)
        {
            SaleDetailList.Clear();
            var list = await _dbContext.SaleDetails.Where(x => x.IdSale == id).ToListAsync();
            if (list.Any())
            {
                foreach (var item in list)
                {
                    SaleDetailList.Add(new SaleDetailDTO
                    {
                        Id = item.Id,
                        CostPrice = item.CostPrice,
                        Name = item.Name,
                        IdSale = item.IdSale,
                        Price = item.Price,
                        Quantity = item.Quantity,
                    });
                }
            }
        }
        [RelayCommand]
        private async Task EditDetail(SaleDetailDTO saleDetailDto)
        {
            if (saleDetailDto.AddQuantity != null)
            {
                LoadingVisible = true;
                SaleMessage message = new SaleMessage();
                await Task.Run(async () =>
                {

                    var sale = await _dbContext.Sales.FirstOrDefaultAsync(x => x.Id == saleDetailDto.IdSale);
                    var saleDetail = await _dbContext.SaleDetails.FirstOrDefaultAsync(x => x.Id == saleDetailDto.Id);
                    saleDetail.Quantity = saleDetail.Quantity + int.Parse(saleDetailDto.AddQuantity);
                    sale.Amount = sale.Amount + (int.Parse(saleDetailDto.AddQuantity) * saleDetail.Price);

                    await _dbContext.SaveChangesAsync();

                    var saleDto = new SaleDTO
                    {
                        Amount = sale.Amount,
                        DateSale = sale.DateSale,
                        Id = sale.Id,
                        Name = sale.Name,
                        Pay = sale.Pay,
                        Cancelled = sale.Cancelled
                    };

                    message = new SaleMessage()
                    {
                        IdAdd = false,
                        SaleDto = saleDto
                    };
                    saleDetailDto.Quantity = saleDetailDto.Quantity + int.Parse(saleDetailDto.AddQuantity);

                    LoadingVisible = false;
                    WeakReferenceMessenger.Default.Send(new SaleMessaging(message));
                    saleDetailDto.AddQuantity = string.Empty;
                });

            }
        }
    }
}
