using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels
{
    [QueryProperty(nameof(TemplateId), "templateId")]
    public partial class TemplateDetailViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty]
        private int _templateId;

        [ObservableProperty]
        private string _templateName = string.Empty;

        [ObservableProperty]
        private ObservableCollection<TemplateExercise> _templateExercises = new();

        [ObservableProperty]
        private bool _isListEmpty = true;

        [ObservableProperty]
        private string _emptyMessage = "Добавь упражнения в шаблон";

        public TemplateDetailViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LoadTemplateDataAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();

            var template = await _dbContext.WorkoutTemplates
                .Include(t => t.TemplateExercises)
                .ThenInclude(te => te.Exercise)
                .FirstOrDefaultAsync(t => t.Id == TemplateId);

            if (template != null)
            {
                TemplateName = template.Name;

                // Пересоздаём коллекцию для гарантированного обновления UI
                var exercises = template.TemplateExercises
                    .OrderBy(te => te.Order)
                    .ToList();

                TemplateExercises = new ObservableCollection<TemplateExercise>(exercises);

                IsListEmpty = TemplateExercises.Count == 0;
            }
        }

        [RelayCommand]
        private async Task GoToAddExerciseToTemplateAsync()
        {
            await Shell.Current.GoToAsync($"SelectExerciseForTemplatePage?templateId={TemplateId}");
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}