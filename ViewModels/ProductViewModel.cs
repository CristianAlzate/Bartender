using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using BartenderApp.DataAccess;
using System.Collections.ObjectModel;
using BartenderApp.DTOs;
using Microsoft.EntityFrameworkCore;

using BartenderApp.Utils.Messages.Product;
using BartenderApp.Views.Masters.Product;

namespace BartenderApp.ViewModels
{
    public partial class ProductViewModel : ObservableObject
    {
        private readonly SalesDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<ProductDTO> productList = new ObservableCollection<ProductDTO>();

        [ObservableProperty]
        private string filterText;

        public IRelayCommand<string> TextChangedCommand { get; }

        private void OnTextChanged(string newText)
        {
            Filter(newText.ToLower());
        }

        public ProductViewModel(SalesDbContext context)
        {
            _dbContext = context;
            TextChangedCommand = new RelayCommand<string>(OnTextChanged);
            MainThread.BeginInvokeOnMainThread(new Action(async () => await GetAll()));

            WeakReferenceMessenger.Default.Register<ProductMessaging>(this, (r, m) =>
            {
                ProductMessageReceived(m.Value);
            });
        }


        public async Task GetAll()
        {
            var list = await _dbContext.Products.ToListAsync();
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

        private void ProductMessageReceived(ProductMessage message)
        {
            var productDto = message.ProductDto;
            if (message.IdAdd)
            {
                ProductList.Add(productDto);
            }
            else
            {
                var found = ProductList.First(e => e.Id == productDto.Id);
                found.Name = productDto.Name;
                found.Price = productDto.Price;
                found.CostPrice = productDto.CostPrice;
                found.SearchName = productDto.SearchName;
            }

        }

        [RelayCommand]
        private async Task Create()
        {
            var uri = $"{nameof(ProductAddPage)}?id=0";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Edit(ProductDTO product)
        {
            var uri = $"{nameof(ProductAddPage)}?id={product.Id}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Delete(ProductDTO product)
        {
            bool answer = await Shell.Current.DisplayAlert("Mensaje", "¿Desea eliminar el producto", "Si", "No");
            if (answer) {
                var found = await _dbContext.Products.FirstAsync(e => e.Id == product.Id);
                _dbContext.Remove(found);
                await _dbContext.SaveChangesAsync();
                ProductList.Remove(product);
            }
        }

        private async Task Filter(string text)
        {
            var list = await _dbContext.Products.Where(x=> x.Name.ToLower().Contains(text) || x.SearchName.ToLower().Contains(text)).ToListAsync();
            ProductList.Clear();
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
}
