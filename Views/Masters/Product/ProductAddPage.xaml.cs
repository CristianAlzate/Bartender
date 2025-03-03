
using BartenderApp.ViewModels;

namespace BartenderApp.Views.Masters.Product;

public partial class ProductAddPage : ContentPage
{
	public ProductAddPage(ProductAddViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}