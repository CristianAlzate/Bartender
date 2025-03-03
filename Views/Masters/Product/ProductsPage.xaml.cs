using BartenderApp.ViewModels;

namespace BartenderApp.Views.Masters.Product;

public partial class ProductsPage : ContentPage
{
	public ProductsPage(ProductViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is ProductViewModel viewModel)
        {
            viewModel.TextChangedCommand.Execute(e.NewTextValue);
        }
    }
}