using GymStats.Data;
using GymStats.Services;
using GymStats.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymStats;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "gymstats.db3");
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Filename={dbPath}"));

        builder.Services.AddSingleton<AnalyticsService>();
        builder.Services.AddSingleton<BackupService>();
        builder.Services.AddSingleton<MediaService>();

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<AddWorkoutPage>();
        builder.Services.AddTransient<AddExerciseToWorkoutPage>();
        builder.Services.AddTransient<SelectExercisePage>();
        builder.Services.AddTransient<AddExercisePage>();
        builder.Services.AddTransient<ExerciseDetailPage>();
        builder.Services.AddTransient<AddSetPage>();
        builder.Services.AddTransient<FinishWorkoutPage>();
        builder.Services.AddTransient<TemplateListPage>();
        builder.Services.AddTransient<AddTemplatePage>();
        builder.Services.AddTransient<TemplateDetailPage>();
        builder.Services.AddTransient<SelectExerciseForTemplatePage>();
        builder.Services.AddTransient<AnthropometryPage>();
        builder.Services.AddTransient<AddMeasurementPage>();
        builder.Services.AddTransient<AnalyticsPage>();
        builder.Services.AddTransient<MediaPage>();
        builder.Services.AddTransient<BackupPage>();

        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<AddWorkoutViewModel>();
        builder.Services.AddTransient<AddExerciseToWorkoutViewModel>();
        builder.Services.AddTransient<SelectExerciseViewModel>();
        builder.Services.AddTransient<AddExerciseViewModel>();
        builder.Services.AddTransient<ExerciseDetailViewModel>();
        builder.Services.AddTransient<AddSetViewModel>();
        builder.Services.AddTransient<FinishWorkoutViewModel>();
        builder.Services.AddTransient<TemplateListViewModel>();
        builder.Services.AddTransient<AddTemplateViewModel>();
        builder.Services.AddTransient<TemplateDetailViewModel>();
        builder.Services.AddTransient<SelectExerciseForTemplateViewModel>();
        builder.Services.AddTransient<AnthropometryViewModel>();
        builder.Services.AddTransient<AddMeasurementViewModel>();
        builder.Services.AddTransient<AnalyticsViewModel>();
        builder.Services.AddTransient<MediaViewModel>();
        builder.Services.AddTransient<BackupViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}