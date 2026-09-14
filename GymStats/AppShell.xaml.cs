namespace GymStats;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Регистрируем маршруты
        Routing.RegisterRoute("AddWorkoutPage", typeof(AddWorkoutPage));
        Routing.RegisterRoute("AddExerciseToWorkoutPage", typeof(AddExerciseToWorkoutPage));
        Routing.RegisterRoute("SelectExercisePage", typeof(SelectExercisePage));
        Routing.RegisterRoute("ExerciseDetailPage", typeof(ExerciseDetailPage));
        Routing.RegisterRoute("AddSetPage", typeof(AddSetPage));
    }
}