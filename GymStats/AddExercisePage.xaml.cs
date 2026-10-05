using GymStats.ViewModels;

namespace GymStats;

public partial class AddExercisePage : ContentPage
{
    public AddExercisePage(AddExerciseViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        MuscleGroups = new[] { "Грудь","Спина","Ноги","Плечи","Бицепс","Трицепс","Пресс","Икры","Другое" };
        EquipmentOptions = new[] { "Штанга","Гантели","Тренажер","Турник","Брусья","Скамья","Без оборудования","Другое" };
        MovementOptions = new[] { "Жим","Тяга","Сгибание","Разгибание","Сведение","Отведение","Приседание","Выпад","Изометрия","Другое" };
        AngleOptions = new[] { "Горизонтальный","Наклонный","Вертикальный","Односторонний","Другое" };
    }

    public string[] MuscleGroups { get; }
    public string[] EquipmentOptions { get; }
    public string[] MovementOptions { get; }
    public string[] AngleOptions { get; }
}