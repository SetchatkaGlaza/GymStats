using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels;

public partial class AddExerciseViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _muscleGroup = "Грудь";
    [ObservableProperty] private string _equipment = "Штанга";
    [ObservableProperty] private string _movementType = "Жим";
    [ObservableProperty] private string _angle = "Горизонтальный";
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private bool _isStatusVisible;

    public ObservableCollection<string> MuscleGroups { get; } = new(new[] { "Грудь","Спина","Ноги","Плечи","Бицепс","Трицепс","Пресс","Икры","Другое" });
    public ObservableCollection<string> EquipmentOptions { get; } = new(new[] { "Штанга","Гантели","Тренажер","Турник","Брусья","Скамья","Без оборудования","Другое" });
    public ObservableCollection<string> MovementOptions { get; } = new(new[] { "Жим","Тяга","Сгибание","Разгибание","Сведение","Отведение","Приседание","Выпад","Изометрия","Другое" });
    public ObservableCollection<string> AngleOptions { get; } = new(new[] { "Горизонтальный","Наклонный","Вертикальный","Односторонний","Другое" });

    public AddExerciseViewModel(AppDbContext db) => _db = db;

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            StatusMessage = "Укажи название упражнения.";
            IsStatusVisible = true;
            return;
        }

        var normalized = Name.Trim().ToLower();
        if (await _db.Exercises.AnyAsync(x => x.Name.ToLower() == normalized))
        {
            StatusMessage = "Такое упражнение уже есть в базе.";
            IsStatusVisible = true;
            return;
        }

        _db.Exercises.Add(new Exercise
        {
            Name = Name.Trim(), MuscleGroup = MuscleGroup, Equipment = Equipment,
            MovementType = MovementType, Angle = Angle, Description = Description.Trim(), IsCustom = true
        });
        await _db.SaveChangesAsync();
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand] private Task CancelAsync() => Shell.Current.GoToAsync("..");
}