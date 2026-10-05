using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;

namespace GymStats.ViewModels;

public partial class AddTemplateViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    [ObservableProperty] private string _templateName = string.Empty;
    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private bool _isStatusVisible;

    public AddTemplateViewModel(AppDbContext db) => _db = db;

    [RelayCommand]
    private async Task SaveTemplateAsync()
    {
        if (string.IsNullOrWhiteSpace(TemplateName))
        {
            StatusMessage = "Введи название шаблона.";
            IsStatusVisible = true;
            return;
        }

        _db.WorkoutTemplates.Add(new WorkoutTemplate { Name = TemplateName.Trim(), CreatedAt = DateTime.Now });
        await _db.SaveChangesAsync();
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand] private Task CancelAsync() => Shell.Current.GoToAsync("..");
}