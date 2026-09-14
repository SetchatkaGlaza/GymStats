using GymStats.ViewModels;

namespace GymStats;

public partial class TemplateDetailPage : ContentPage
{
    public TemplateDetailPage(TemplateDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is TemplateDetailViewModel vm)
        {
            await vm.LoadTemplateDataAsync();
        }
    }
}