using GymStats.ViewModels;

namespace GymStats;

public partial class BackupPage : ContentPage
{
    public BackupPage(BackupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}