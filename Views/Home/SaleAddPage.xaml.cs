using BartenderApp.ViewModels;

namespace BartenderApp.Views.Home;

public partial class SaleAddPage : ContentPage
{
	public SaleAddPage(SaleAddViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is SaleAddViewModel viewModel)
        {
            viewModel.TextChangedCommand.Execute(e.NewTextValue);
        }
    }

    private void ItemChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        var value = picker.SelectedItem;
        if (BindingContext is SaleAddViewModel viewModel)
        {
            viewModel.ProductSelectedCommand.Execute(value);
        }
    }
}