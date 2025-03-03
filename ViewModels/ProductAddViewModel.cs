using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using BartenderApp.DataAccess;
using BartenderApp.DTOs;
using Microsoft.EntityFrameworkCore;
using BartenderApp.Utils.Messages.Product;
using BartenderApp.Models;

namespace BartenderApp.ViewModels
{
    public partial class ProductAddViewModel : ObservableObject , IQueryAttributable
    {
        private readonly SalesDbContext _dbContext;

        [ObservableProperty]
        private ProductDTO productDto = new ProductDTO();

        [ObservableProperty]
        private string? pageTitle;

        private int Id;

        [ObservableProperty]
        private bool loadingVisible = false;

        public ProductAddViewModel(SalesDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            int id = int.Parse(query["id"].ToString());
            Id = id;
            if (Id == 0)
                PageTitle = "Nuevo Producto";
            else
            {
                PageTitle = "Editar Producto";
                LoadingVisible = true;
                await Task.Run(async () => 
                { 
                    var found = await _dbContext.Products.FirstAsync(e => e.Id == Id);
                    ProductDto.Id = found.Id;
                    ProductDto.Name = found.Name;
                    ProductDto.Price = found.Price;
                    ProductDto.CostPrice = found.CostPrice;
                    ProductDto.SearchName = found.SearchName;

                    MainThread.BeginInvokeOnMainThread(() => { LoadingVisible = false; });
                });

            }
        }

        [RelayCommand]
        private async Task Save()
        {
            LoadingVisible = true;
            ProductMessage message = new ProductMessage();
            await Task.Run(async () =>
            {
                if(Id == 0)
                {
                    var Product = new Product
                    {
                        Name = ProductDto.Name,
                        Price = ProductDto.Price,
                        CostPrice = ProductDto.CostPrice,
                        SearchName = ProductDto.SearchName
                    };
                    _dbContext.Add(Product);
                    await _dbContext.SaveChangesAsync();

                    ProductDto.Id = Product.Id;

                    message = new ProductMessage()
                    {
                        IdAdd = true,
                        ProductDto = ProductDto
                    };
                }
                else
                {
                    var found = await _dbContext.Products.FirstAsync(e => e.Id == Id);
                    found.Name = ProductDto.Name;
                    found.Price = ProductDto.Price;
                    found.CostPrice = ProductDto.CostPrice;
                    found.SearchName = ProductDto.SearchName;

                    await _dbContext.SaveChangesAsync();

                    message = new ProductMessage()
                    {
                        IdAdd = false,
                        ProductDto = ProductDto
                    };
                }

                MainThread.BeginInvokeOnMainThread(async () => 
                { 
                    LoadingVisible = false;
                    WeakReferenceMessenger.Default.Send(new ProductMessaging(message));
                    await Shell.Current.Navigation.PopAsync();
                });
            });
        }

        [RelayCommand]
        private async Task SaveDetail()
        {

        }
    }
}
