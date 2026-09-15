using GymStats.ViewModels;

namespace GymStats;

public partial class AddMediaPage : ContentPage
{
    public AddMediaPage(AddMediaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}