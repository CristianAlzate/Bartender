using BartenderApp.DataAccess;
using BartenderApp.ViewModels;
using BartenderApp.Views.Home;
using BartenderApp.Views.Masters;
using BartenderApp.Views.Masters.Product;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace BartenderApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var dbContext = new SalesDbContext();
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();

            builder.Services.AddDbContext<SalesDbContext>();

            builder.Services.AddTransient<ProductsPage>();
            builder.Services.AddTransient<ProductViewModel>();

            builder.Services.AddTransient<ProductAddPage>();
            builder.Services.AddTransient<ProductAddViewModel>();

            builder.Services.AddTransient<SalePage>();
            builder.Services.AddTransient<SaleViewModel>();
            builder.Services.AddTransient<SaleAddPopUp>();
            builder.Services.AddTransient<SaleAddPage>();
            builder.Services.AddTransient<SaleDetailPage>();
            builder.Services.AddTransient<SaleAddViewModel>();
            builder.Services.AddTransient<SaleDetailViewModel>();


            Routing.RegisterRoute(nameof(ProductAddPage), typeof(ProductAddPage));
            Routing.RegisterRoute(nameof(SaleAddPage), typeof(SaleAddPage));
            Routing.RegisterRoute(nameof(SaleDetailPage), typeof(SaleDetailPage));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
