using GymStats.Services;
using GymStats.ViewModels;

namespace GymStats;

public partial class AnalyticsPage : ContentPage
{
    private PropertyChangedEventHandler? _handler;

    public AnalyticsPage(AnalyticsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is not AnalyticsViewModel vm) return;
        await vm.LoadAsync();
        RefreshCharts(vm);
        _handler ??= (_, e) =>
        {
            if (e.PropertyName is nameof(AnalyticsViewModel.ExerciseProgress) or nameof(AnalyticsViewModel.BodyProgress)
                or nameof(AnalyticsViewModel.SelectedExercise) or nameof(AnalyticsViewModel.SelectedBodyMetric)
                or nameof(AnalyticsViewModel.BodyMetricValues))
                MainThread.BeginInvokeOnMainThread(() => RefreshCharts(vm));
        };
        vm.PropertyChanged += _handler;
    }

    protected override void OnDisappearing()
    {
        if (BindingContext is AnalyticsViewModel vm && _handler is not null)
            vm.PropertyChanged -= _handler;
        base.OnDisappearing();
    }

    private void RefreshCharts(AnalyticsViewModel vm)
    {
        ExerciseChart.Drawable = new ProgressChartDrawable { Values = vm.ExerciseProgress.Select(x => x.Weight).ToList(), EmptyText = "Недостаточно данных" };
        VolumeChart.Drawable = new ProgressChartDrawable { Values = vm.ExerciseProgress.Select(x => x.Volume).ToList(), EmptyText = "Недостаточно данных" };
        BodyChart.Drawable = new ProgressChartDrawable { Values = vm.BodyMetricValues, EmptyText = "Добавь хотя бы два замера" };
        ExerciseChart.Invalidate();
        VolumeChart.Invalidate();
        BodyChart.Invalidate();
    }
}