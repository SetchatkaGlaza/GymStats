using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels;

[QueryProperty(nameof(TemplateId), "templateId")]
public partial class TemplateDetailViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    [ObservableProperty] private int _templateId;
    [ObservableProperty] private string _templateName = string.Empty;
    [ObservableProperty] private ObservableCollection<TemplateExercise> _templateExercises = new();
    [ObservableProperty] private bool _isListEmpty = true;

    public TemplateDetailViewModel(AppDbContext db) => _db = db;

    public async Task LoadTemplateDataAsync()
    {
        await _db.EnsureReadyAsync();
        var template = await _db.WorkoutTemplates.Include(x => x.TemplateExercises).ThenInclude(x => x.Exercise).FirstOrDefaultAsync(x => x.Id == TemplateId);
        if (template is null) return;
        TemplateName = template.Name;
        TemplateExercises = new ObservableCollection<TemplateExercise>(template.TemplateExercises.OrderBy(x => x.Order));
        IsListEmpty = TemplateExercises.Count == 0;
    }

    [RelayCommand] private Task GoToAddExerciseToTemplateAsync() => Shell.Current.GoToAsync($"SelectExerciseForTemplatePage?templateId={TemplateId}");

    [RelayCommand]
    private async Task SaveTargetAsync(TemplateExercise item)
    {
        if (item.TargetSets < 1) item.TargetSets = 1;
        if (item.TargetReps < 1) item.TargetReps = 1;
        if (item.TargetWeight < 0) item.TargetWeight = 0;
        if (item.TargetRPE is < 1 or > 10) item.TargetRPE = 7;
        _db.TemplateExercises.Update(item);
        await _db.SaveChangesAsync();
    }

    [RelayCommand]
    private async Task RemoveExerciseAsync(TemplateExercise item)
    {
        _db.TemplateExercises.Remove(item);
        await _db.SaveChangesAsync();
        await LoadTemplateDataAsync();
    }

    [RelayCommand]
    private async Task DeleteTemplateAsync()
    {
        var template = await _db.WorkoutTemplates.FindAsync(TemplateId);
        if (template is null) return;
        if (!await Shell.Current.DisplayAlert("Удалить шаблон?", $"«{template.Name}» будет удалён.", "Удалить", "Отмена")) return;
        _db.WorkoutTemplates.Remove(template);
        await _db.SaveChangesAsync();
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand] private Task GoBackAsync() => Shell.Current.GoToAsync("..");
}