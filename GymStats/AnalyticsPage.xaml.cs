using GymStats.ViewModels;

namespace GymStats;

public partial class AnalyticsPage : ContentPage
{
    public AnalyticsPage(AnalyticsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}