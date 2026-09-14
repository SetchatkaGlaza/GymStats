using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GymStats.Data;
using GymStats.ViewModels;

namespace GymStats
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "gymstats.db3");
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite($"Filename={dbPath}");
            });

            // Страницы
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddWorkoutPage>();
            builder.Services.AddTransient<AddExerciseToWorkoutPage>();
            builder.Services.AddTransient<SelectExercisePage>();
            builder.Services.AddTransient<ExerciseDetailPage>();
            builder.Services.AddTransient<AddSetPage>();
            builder.Services.AddTransient<FinishWorkoutPage>();
            builder.Services.AddTransient<TemplateListPage>();
            builder.Services.AddTransient<AddTemplatePage>();
            builder.Services.AddTransient<TemplateDetailPage>();
            builder.Services.AddTransient<SelectExerciseForTemplatePage>();
            builder.Services.AddTransient<AnthropometryPage>(); // ДОБАВИТЬ
            builder.Services.AddTransient<AddMeasurementPage>(); // ДОБАВИТЬ

            // ViewModel
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<AddWorkoutViewModel>();
            builder.Services.AddTransient<AddExerciseToWorkoutViewModel>();
            builder.Services.AddTransient<SelectExerciseViewModel>();
            builder.Services.AddTransient<ExerciseDetailViewModel>();
            builder.Services.AddTransient<AddSetViewModel>();
            builder.Services.AddTransient<FinishWorkoutViewModel>();
            builder.Services.AddTransient<TemplateListViewModel>();
            builder.Services.AddTransient<AddTemplateViewModel>();
            builder.Services.AddTransient<TemplateDetailViewModel>();
            builder.Services.AddTransient<SelectExerciseForTemplateViewModel>();
            builder.Services.AddTransient<AnthropometryViewModel>(); // ДОБАВИТЬ
            builder.Services.AddTransient<AddMeasurementViewModel>(); // ДОБАВИТЬ

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}