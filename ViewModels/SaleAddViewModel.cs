using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using BartenderApp.DataAccess;
using BartenderApp.DTOs;
using BartenderApp.Utils.Messages.Sale;
using BartenderApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using __XamlGeneratedCode__;
using CommunityToolkit.Maui.Views;
using BartenderApp.Views.Home;

namespace BartenderApp.ViewModels
{
    public partial class SaleAddViewModel : ObservableObject, IQueryAttributable
    {
        private readonly SalesDbContext _dbContext;

        [ObservableProperty]
        private SaleDTO saleDto = new SaleDTO();

        [ObservableProperty]
        private SaleDetailDTO saleDetailDto = new SaleDetailDTO();

        [ObservableProperty]
        private ObservableCollection<ProductDTO> productList = new ObservableCollection<ProductDTO>();

        [ObservableProperty]
        private string? pageTitle;

        [ObservableProperty]
        private string? errorName;
        [ObservableProperty]
        private string? errorProduct;

        [ObservableProperty]
        private bool isEdit;

        private int Id;

        [ObservableProperty]
        private bool loadingVisible = false;

        [ObservableProperty]
        private string filterText;

        public IRelayCommand<string> TextChangedCommand { get; }

        public IRelayCommand<ProductDTO> ProductSelectedCommand { get; }

        private void OnTextChanged(string newText)
        {
            GetProducts(newText);
        }
        private void OnProductSelected(ProductDTO product)
        {
            SaleDetailDto.IdSale = Id;
            SaleDetailDto.Name = product.Name;
            SaleDetailDto.Price = product.Price;
            SaleDetailDto.CostPrice = product.CostPrice;
            SaleDetailDto.Quantity = 1;

        }
        public SaleAddViewModel(SalesDbContext dbContext)
        {
            _dbContext = dbContext;
            TextChangedCommand = new RelayCommand<string>(OnTextChanged);
            ProductSelectedCommand = new RelayCommand<ProductDTO>(OnProductSelected);
            GetProducts("");
        }

        public SaleAddViewModel(SalesDbContext dbContext, int IdSale)
        {
            _dbContext = dbContext;
            Id = IdSale;
        }

        public void GetProducts(string filter)
        {
            ProductList.Clear();
            List<Product> list = new List<Product>();
            if (filter == string.Empty)
                list = _dbContext.Products.ToList();
            else
                list = _dbContext.Products.Where(x => x.Name.ToLower().Contains(filter) || x.SearchName.ToLower().Contains(filter)).ToList();

            if (list.Any())
            {
                foreach (var item in list)
                {
                    ProductList.Add(new ProductDTO
                    {
                        Id = item.Id,
                        CostPrice = item.CostPrice,
                        Name = item.Name,
                        Price = item.Price,
                        SearchName = item.SearchName
                    });
                }
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            int id = int.Parse(query["id"].ToString());
            Id = id;
            await Task.Run(async () =>
                MainThread.BeginInvokeOnMainThread(() => { LoadingVisible = false; }));

        }

        [RelayCommand]
        private async Task Save()
        {
            LoadingVisible = true;
            SaleMessage message = new SaleMessage();
            if (string.IsNullOrEmpty(SaleDto.Name))
            {
                ErrorName = "El nombre es requerido";
                LoadingVisible = false;
            }
            else
            {
                await Task.Run(async () =>
                {
                    if (Id == 0)
                    {

                        var Sale = new Sale
                        {
                            Name = SaleDto.Name,
                            Amount = 0,
                            Pay = 0,
                            DateSale = new DateTime(DateTime.Now.AddHours(-6).Year, DateTime.Now.AddHours(-6).Month, DateTime.Now.AddHours(-6).Day),
                            Cancelled = false
                        };
                        _dbContext.Add(Sale);
                        await _dbContext.SaveChangesAsync();

                        SaleDto.Id = Sale.Id;

                        message = new SaleMessage()
                        {
                            IdAdd = true,
                            SaleDto = SaleDto
                        };
                    }
                    else
                    {
                        var found = await _dbContext.Sales.FirstAsync(e => e.Id == Id);
                        found.Name = SaleDto.Name;
                        found.Amount = SaleDto.Amount;
                        found.Pay = SaleDto.Pay;
                        found.DateSale = SaleDto.DateSale;
                        found.Cancelled = SaleDto.Cancelled;

                        await _dbContext.SaveChangesAsync();

                        message = new SaleMessage()
                        {
                            IdAdd = false,
                            SaleDto = SaleDto
                        };
                    }
                    ErrorName = string.Empty;

                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        LoadingVisible = false;
                        WeakReferenceMessenger.Default.Send(new SaleMessaging(message));
                        await Shell.Current.Navigation.PopAsync();
                    });
                });
            }
        }
        [RelayCommand]
        private async Task AddPay()
        {
            LoadingVisible = true;
            SaleMessage message = new SaleMessage();
            if (string.IsNullOrEmpty(SaleDto.AddPay))
            {
                ErrorName = "La cantidad es requerido";
                LoadingVisible = false;
            }
            else
            {
                await Task.Run(async () =>
                {
                    ErrorName = string.Empty;
                    var sale = await _dbContext.Sales.FirstAsync(x => x.Id == Id);
                    sale.Pay = sale.Pay + int.Parse(SaleDto.AddPay);
                    await _dbContext.SaveChangesAsync();

                    SaleDto.Id = Id;
                    SaleDto.Name = sale.Name;
                    SaleDto.Amount = sale.Amount;
                    SaleDto.Pay = sale.Pay;
                    SaleDto.AddPay = string.Empty;
                    SaleDto.DateSale = sale.DateSale;
                    SaleDto.Cancelled = sale.Cancelled;

                    message = new SaleMessage()
                    {
                        IdAdd = false,
                        SaleDto = SaleDto
                    };
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        LoadingVisible = false;
                        WeakReferenceMessenger.Default.Send(new SaleMessaging(message));
                        await Shell.Current.Navigation.PopAsync();
                    });

                });
                

            }
        }

        [RelayCommand]
        private async Task SaveDetail()
        {
            if (!string.IsNullOrEmpty(SaleDetailDto.Name))
            {
                LoadingVisible = true;

                SaleMessage message = new SaleMessage();
                await Task.Run(async () =>
                {
                    var saleDetail = new SaleDetail
                    {
                        IdSale = Id,
                        CostPrice = SaleDetailDto.CostPrice,
                        Name = SaleDetailDto.Name,
                        Price = SaleDetailDto.Price,
                        Quantity = SaleDetailDto.Quantity
                    };



                    _dbContext.Add(saleDetail);
                    await _dbContext.SaveChangesAsync();

                    SaleDetailDto.Id = saleDetail.Id;

                    var sale = _dbContext.Sales.Where(x => x.Id == Id).FirstOrDefault();
                    sale.Amount = sale.Amount + (SaleDetailDto.Price * SaleDetailDto.Quantity);
                    _dbContext.SaveChanges();

                    SaleDto.Amount = sale.Amount;
                    SaleDto.Id = sale.Id;

                    message = new SaleMessage()
                    {
                        IdAdd = false,
                        SaleDto = SaleDto,
                        SaleDetailDto = SaleDetailDto
                    };
                    ErrorProduct = string.Empty;



                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        LoadingVisible = false;
                        WeakReferenceMessenger.Default.Send(new SaleMessaging(message));
                        await Shell.Current.Navigation.PopAsync();
                    });
                });
            }
            else
            {
                ErrorProduct = "Debes seleccionar un producto";
            }
        }
    }
}
