using GymStats.ViewModels;

namespace GymStats;

public partial class AddTemplatePage : ContentPage
{
    public AddTemplatePage(AddTemplateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}