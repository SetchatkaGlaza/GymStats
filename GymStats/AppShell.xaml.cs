namespace GymStats;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("AddWorkoutPage", typeof(AddWorkoutPage));
        Routing.RegisterRoute("AddExerciseToWorkoutPage", typeof(AddExerciseToWorkoutPage));
        Routing.RegisterRoute("SelectExercisePage", typeof(SelectExercisePage));
        Routing.RegisterRoute("AddExercisePage", typeof(AddExercisePage));
        Routing.RegisterRoute("ExerciseDetailPage", typeof(ExerciseDetailPage));
        Routing.RegisterRoute("AddSetPage", typeof(AddSetPage));
        Routing.RegisterRoute("FinishWorkoutPage", typeof(FinishWorkoutPage));
        Routing.RegisterRoute("AddTemplatePage", typeof(AddTemplatePage));
        Routing.RegisterRoute("TemplateDetailPage", typeof(TemplateDetailPage));
        Routing.RegisterRoute("SelectExerciseForTemplatePage", typeof(SelectExerciseForTemplatePage));
        Routing.RegisterRoute("AddMeasurementPage", typeof(AddMeasurementPage));
        Routing.RegisterRoute("BackupPage", typeof(BackupPage));
    }
}