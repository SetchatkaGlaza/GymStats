using GymStats.ViewModels;

namespace GymStats;

public partial class AddSetPage : ContentPage
{
    public AddSetPage(AddSetViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}