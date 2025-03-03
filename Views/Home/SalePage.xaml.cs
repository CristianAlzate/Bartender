using BartenderApp.DataAccess;
using BartenderApp.ViewModels;
using CommunityToolkit.Maui.Views;

namespace BartenderApp.Views.Home;

public partial class SalePage : ContentPage
{
    public SalePage(SaleViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnDateChanged(object sender, DateChangedEventArgs e)
    {
        if (BindingContext is SaleViewModel viewModel)
        {
            viewModel.DateChangedCommand.Execute(e.NewDate);
        }
    }

    
}