using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels;

public partial class TemplateListViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    [ObservableProperty] private ObservableCollection<WorkoutTemplate> _templates = new();
    [ObservableProperty] private bool _isListEmpty = true;

    public TemplateListViewModel(AppDbContext db) => _db = db;

    public async Task LoadTemplatesAsync()
    {
        await _db.EnsureReadyAsync();
        Templates = new ObservableCollection<WorkoutTemplate>(
            await _db.WorkoutTemplates.Include(x => x.TemplateExercises).AsNoTracking().OrderBy(x => x.Name).ToListAsync());
        IsListEmpty = Templates.Count == 0;
    }

    [RelayCommand] private Task GoToAddTemplateAsync() => Shell.Current.GoToAsync("AddTemplatePage");
    [RelayCommand] private Task OpenTemplateAsync(WorkoutTemplate template) => Shell.Current.GoToAsync($"TemplateDetailPage?templateId={template.Id}");
    [RelayCommand] private Task GoBackAsync() => Shell.Current.GoToAsync("..");
}