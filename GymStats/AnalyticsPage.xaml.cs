using GymStats.Services;
using GymStats.ViewModels;

namespace GymStats;

public partial class AnalyticsPage : ContentPage
{
    public AnalyticsPage(AnalyticsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AnalyticsViewModel vm)
        {
            await vm.LoadAsync();
            RefreshCharts(vm);
            vm.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName is nameof(AnalyticsViewModel.ExerciseProgress) or nameof(AnalyticsViewModel.BodyProgress) or nameof(AnalyticsViewModel.SelectedExercise))
                    MainThread.BeginInvokeOnMainThread(() => RefreshCharts(vm));
            };
        }
    }

    private void RefreshCharts(AnalyticsViewModel vm)
    {
        ExerciseChart.Drawable = new ProgressChartDrawable
        {
            Values = vm.ExerciseProgress.Select(x => x.Weight).ToList(),
            EmptyText = "Недостаточно данных для графика"
        };
        BodyChart.Drawable = new ProgressChartDrawable
        {
            Values = vm.BodyProgress.Select(x => x.Weight).ToList(),
            EmptyText = "Добавь хотя бы два замера"
        };
        ExerciseChart.Invalidate();
        BodyChart.Invalidate();
    }
}