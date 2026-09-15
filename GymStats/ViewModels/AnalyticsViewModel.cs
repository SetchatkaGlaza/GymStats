using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GymStats.ViewModels
{
    public partial class AnalyticsViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task GoToExerciseProgressAsync()
        {
            await Shell.Current.GoToAsync("ExerciseProgressPage");
        }

        [RelayCommand]
        private async Task GoToBodyWeightProgressAsync()
        {
            await Shell.Current.GoToAsync("BodyWeightProgressPage");
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}