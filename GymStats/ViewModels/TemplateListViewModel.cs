using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels
{
    public partial class TemplateListViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<WorkoutTemplate> _templates = new();

        [ObservableProperty]
        private bool _isListEmpty = true;

        [ObservableProperty]
        private string _emptyMessage = "Пока нет шаблонов. Создай первый!";

        public TemplateListViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LoadTemplatesAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();

            var templateList = await _dbContext.WorkoutTemplates
                .Include(t => t.TemplateExercises)
                .ThenInclude(te => te.Exercise)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            Templates.Clear();
            foreach (var template in templateList)
            {
                Templates.Add(template);
            }

            IsListEmpty = Templates.Count == 0;
        }

        [RelayCommand]
        private async Task GoToAddTemplateAsync()
        {
            await Shell.Current.GoToAsync("AddTemplatePage");
        }

        [RelayCommand]
        private async Task OpenTemplateAsync(WorkoutTemplate template)
        {
            await Shell.Current.GoToAsync($"TemplateDetailPage?templateId={template.Id}");
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}