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
            if (e.PropertyName is nameof(AnalyticsViewModel.ExerciseProgress)
                or nameof(AnalyticsViewModel.WorkoutProgress)
                or nameof(AnalyticsViewModel.BodyProgress)
                or nameof(AnalyticsViewModel.SelectedExercise)
                or nameof(AnalyticsViewModel.SelectedBodyMetric)
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
        var exerciseLabels = vm.ExerciseProgress.Select(x => x.Date.ToString("dd.MM")).ToList();
        var workoutLabels = vm.WorkoutProgress.Select(x => x.Date.ToString("dd.MM")).ToList();
        var bodyLabels = vm.BodyProgress.Select(x => x.Date.ToString("dd.MM")).ToList();

        ExerciseChart.Drawable = new ProgressChartDrawable
        {
            Values = vm.ExerciseProgress.Select(x => x.Weight).ToList(),
            Labels = exerciseLabels,
            EmptyText = "Нужно 2+ тренировки",
            Unit = "кг"
        };
        OneRepMaxChart.Drawable = new ProgressChartDrawable
        {
            Values = vm.ExerciseProgress.Select(x => x.EstimatedOneRepMax).ToList(),
            Labels = exerciseLabels,
            EmptyText = "Нужно 2+ тренировки",
            Unit = "кг"
        };
        VolumeChart.Drawable = new ProgressChartDrawable
        {
            Values = vm.ExerciseProgress.Select(x => x.Volume).ToList(),
            Labels = exerciseLabels,
            EmptyText = "Нужно 2+ тренировки",
            Unit = "кг"
        };
        RepsChart.Drawable = new ProgressChartDrawable
        {
            Values = vm.ExerciseProgress.Select(x => (double)x.Reps).ToList(),
            Labels = exerciseLabels,
            EmptyText = "Нужно 2+ тренировки",
            Unit = "повт."
        };
        WorkoutVolumeChart.Drawable = new ProgressChartDrawable
        {
            Values = vm.WorkoutProgress.Select(x => x.Volume).ToList(),
            Labels = workoutLabels,
            EmptyText = "Нужно 2+ тренировки",
            Unit = "кг"
        };
        WorkoutRpeChart.Drawable = new ProgressChartDrawable
        {
            Values = vm.WorkoutProgress.Select(x => x.AverageRpe).ToList(),
            Labels = workoutLabels,
            EmptyText = "Нужно 2+ тренировки",
            Unit = "RPE"
        };
        BodyChart.Drawable = new ProgressChartDrawable
        {
            Values = vm.BodyMetricValues,
            Labels = bodyLabels,
            EmptyText = "Добавь 2+ замера",
            Unit = SelectedBodyUnit(vm.SelectedBodyMetric)
        };

        ExerciseChart.Invalidate();
        OneRepMaxChart.Invalidate();
        VolumeChart.Invalidate();
        RepsChart.Invalidate();
        WorkoutVolumeChart.Invalidate();
        WorkoutRpeChart.Invalidate();
        BodyChart.Invalidate();
    }

    private static string SelectedBodyUnit(string metric) => metric == "Вес" ? "кг" : "см";
}