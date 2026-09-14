using GymStats.ViewModels;

namespace GymStats;

public partial class TemplateListPage : ContentPage
{
    public TemplateListPage(TemplateListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is TemplateListViewModel vm)
        {
            await vm.LoadTemplatesAsync();
        }
    }
}