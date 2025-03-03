using BartenderApp.ViewModels;

namespace BartenderApp.Views.Home;

public partial class SaleDetailPage : ContentPage
{
	public SaleDetailPage(SaleDetailViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}