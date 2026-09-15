using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using Microcharts;
using SkiaSharp;

namespace GymStats.ViewModels
{
    public partial class BodyWeightProgressViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty]
        private Chart _weightChart;

        [ObservableProperty]
        private bool _isChartVisible = false;

        [ObservableProperty]
        private string _chartTitle = "Динамика веса тела";

        public BodyWeightProgressViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LoadChartDataAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();

            var measurements = await _dbContext.BodyMeasurements
                .OrderBy(m => m.Date)
                .ToListAsync();

            if (measurements.Count == 0)
            {
                ChartTitle = "Сначала добавь замеры тела";
                IsChartVisible = false;
                return;
            }

            var entries = new List<ChartEntry>();
            foreach (var m in measurements)
            {
                entries.Add(new ChartEntry((float)m.Weight)
                {
                    Label = m.Date.ToString("dd.MM"),
                    ValueLabel = m.Weight.ToString("F1"),
                    Color = SKColor.Parse("#2E8B57")
                });
            }

            WeightChart = new LineChart
            {
                Entries = entries,
                LineMode = LineMode.Straight,
                PointMode = PointMode.Circle,
                PointSize = 12,
                BackgroundColor = SKColors.Transparent,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                IsAnimated = true
            };

            IsChartVisible = true;
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}