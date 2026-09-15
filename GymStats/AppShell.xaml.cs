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
        Routing.RegisterRoute("AnthropometryPage", typeof(AnthropometryPage));
        Routing.RegisterRoute("AddMeasurementPage", typeof(AddMeasurementPage));
        Routing.RegisterRoute("AnalyticsPage", typeof(AnalyticsPage)); // ДОБАВИТЬ
        Routing.RegisterRoute("ExerciseProgressPage", typeof(ExerciseProgressPage)); // ДОБАВИТЬ
        Routing.RegisterRoute("BodyWeightProgressPage", typeof(BodyWeightProgressPage)); // ДОБАВИТЬ
        Routing.RegisterRoute("MediaArchivePage", typeof(MediaArchivePage)); // ДОБАВИТЬ
        Routing.RegisterRoute("AddMediaPage", typeof(AddMediaPage)); // ДОБАВИТЬ
    }
}