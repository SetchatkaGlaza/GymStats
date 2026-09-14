using GymStats.ViewModels;

namespace GymStats;

public partial class AddMeasurementPage : ContentPage
{
    public AddMeasurementPage(AddMeasurementViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}