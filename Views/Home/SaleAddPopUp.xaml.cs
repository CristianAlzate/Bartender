using BartenderApp.ViewModels;
using CommunityToolkit.Maui.Views;

namespace BartenderApp.Views.Home;

public partial class SaleAddPopUp : Popup
{
	public SaleAddPopUp(SaleAddViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

    }
    private void OnClose(object sender, EventArgs e)
    {
        if(!string.IsNullOrEmpty(EntryName.Text))
		    Close();
    }

    private void Close(object sender, EventArgs e)
    {
            Close();
    }
}