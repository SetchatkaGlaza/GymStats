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

            // Регистрация БД
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "gymstats.db3");
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite($"Filename={dbPath}");
            });

            // Регистрация Страниц
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddWorkoutPage>();
            builder.Services.AddTransient<AddExerciseToWorkoutPage>();
            builder.Services.AddTransient<SelectExercisePage>();
            builder.Services.AddTransient<ExerciseDetailPage>();
            builder.Services.AddTransient<AddSetPage>();

            // Регистрация ViewModel
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<AddWorkoutViewModel>();
            builder.Services.AddTransient<AddExerciseToWorkoutViewModel>();
            builder.Services.AddTransient<SelectExerciseViewModel>();
            builder.Services.AddTransient<ExerciseDetailViewModel>();
            builder.Services.AddTransient<AddSetViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}