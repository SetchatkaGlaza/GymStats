using CommunityToolkit.Mvvm.ComponentModel;
using GymStats.Data;
using GymStats.Models;
using GymStats.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels;

public partial class AnalyticsViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    private readonly AnalyticsService _analytics;

    [ObservableProperty] private int _workoutCount;
    [ObservableProperty] private double _totalVolume;
    [ObservableProperty] private double _averageRpe;
    [ObservableProperty] private int _workoutsThisMonth;
    [ObservableProperty] private ObservableCollection<Exercise> _exercises = new();
    [ObservableProperty] private Exercise? _selectedExercise;
    [ObservableProperty] private ObservableCollection<ExerciseProgressPoint> _exerciseProgress = new();
    [ObservableProperty] private ObservableCollection<BodyProgressPoint> _bodyProgress = new();
    [ObservableProperty] private string _selectedBodyMetric = "Вес";
    [ObservableProperty] private bool _hasExerciseData;
    [ObservableProperty] private bool _hasBodyData;

    public ObservableCollection<string> BodyMetrics { get; } = new(new[] { "Вес", "Грудь", "Талия", "Бедра", "Бицепс", "Бедро", "Икра", "Шея" });

    public AnalyticsViewModel(AppDbContext db, AnalyticsService analytics)
    {
        _db = db;
        _analytics = analytics;
    }

    public async Task LoadAsync()
    {
        await _db.EnsureReadyAsync();
        WorkoutCount = await _analytics.GetWorkoutCountAsync();
        TotalVolume = await _analytics.GetTotalVolumeAsync();
        AverageRpe = await _analytics.GetAverageRpeAsync();
        WorkoutsThisMonth = await _analytics.GetWorkoutCountSinceAsync(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));

        Exercises = new ObservableCollection<Exercise>(await _db.Exercises.AsNoTracking().OrderBy(x => x.MuscleGroup).ThenBy(x => x.Name).ToListAsync());
        BodyProgress = new ObservableCollection<BodyProgressPoint>(await _analytics.GetBodyProgressAsync());
        HasBodyData = BodyProgress.Count > 1;

        if (SelectedExercise is null && Exercises.Count > 0)
            SelectedExercise = Exercises[0];
        else
            await LoadExerciseAsync();
    }

    partial void OnSelectedExerciseChanged(Exercise? value) => _ = LoadExerciseAsync();
    partial void OnSelectedBodyMetricChanged(string value) => OnPropertyChanged(nameof(BodyMetricValues));

    public IReadOnlyList<double> BodyMetricValues => BodyProgress.Select(x => SelectedBodyMetric switch
    {
        "Грудь" => x.Chest, "Талия" => x.Waist, "Бедра" => x.Hips, "Бицепс" => x.Biceps,
        "Бедро" => x.Thigh, "Икра" => x.Calf, "Шея" => x.Neck, _ => x.Weight
    }).ToList();

    private async Task LoadExerciseAsync()
    {
        if (SelectedExercise is null)
        {
            ExerciseProgress.Clear();
            HasExerciseData = false;
            return;
        }
        ExerciseProgress = new ObservableCollection<ExerciseProgressPoint>(await _analytics.GetExerciseProgressAsync(SelectedExercise.Id));
        HasExerciseData = ExerciseProgress.Count > 1;
    }
}