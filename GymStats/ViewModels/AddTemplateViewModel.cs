using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;

namespace GymStats.ViewModels
{
    public partial class AddTemplateViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty]
        private string _templateName = string.Empty;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        [ObservableProperty]
        private bool _isStatusVisible = false;

        public AddTemplateViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [RelayCommand]
        private async Task SaveTemplateAsync()
        {
            if (string.IsNullOrWhiteSpace(TemplateName))
            {
                StatusMessage = "Введи название шаблона";
                IsStatusVisible = true;
                return;
            }

            var newTemplate = new WorkoutTemplate
            {
                Name = TemplateName,
                CreatedAt = DateTime.Now
            };

            await _dbContext.WorkoutTemplates.AddAsync(newTemplate);
            await _dbContext.SaveChangesAsync();

            StatusMessage = "Шаблон создан!";
            IsStatusVisible = true;

            await Task.Delay(1000);
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}