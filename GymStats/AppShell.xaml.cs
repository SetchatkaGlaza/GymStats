namespace GymStats;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("AddWorkoutPage", typeof(AddWorkoutPage));
        Routing.RegisterRoute("AddExerciseToWorkoutPage", typeof(AddExerciseToWorkoutPage));
        Routing.RegisterRoute("SelectExercisePage", typeof(SelectExercisePage));
        Routing.RegisterRoute("ExerciseDetailPage", typeof(ExerciseDetailPage));
        Routing.RegisterRoute("AddSetPage", typeof(AddSetPage));
        Routing.RegisterRoute("FinishWorkoutPage", typeof(FinishWorkoutPage));
        Routing.RegisterRoute("TemplateListPage", typeof(TemplateListPage));
        Routing.RegisterRoute("AddTemplatePage", typeof(AddTemplatePage));
        Routing.RegisterRoute("TemplateDetailPage", typeof(TemplateDetailPage));
        Routing.RegisterRoute("SelectExerciseForTemplatePage", typeof(SelectExerciseForTemplatePage));
        Routing.RegisterRoute("AnthropometryPage", typeof(AnthropometryPage)); // ДОБАВИТЬ
        Routing.RegisterRoute("AddMeasurementPage", typeof(AddMeasurementPage)); // ДОБАВИТЬ
    }
}