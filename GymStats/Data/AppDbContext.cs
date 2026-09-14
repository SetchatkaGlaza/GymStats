using Microsoft.EntityFrameworkCore;
using GymStats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GymStats.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutTemplate> WorkoutTemplates { get; set; }
        public DbSet<TemplateExercise> TemplateExercises { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<BodyMeasurement> BodyMeasurements { get; set; }
        public DbSet<MediaEntry> MediaEntries { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public async Task SeedDataAsync()
        {
            await Database.EnsureCreatedAsync();

            if (!await Exercises.AnyAsync())
            {
                var exercises = new List<Exercise>
                {
                    new Exercise { Name = "Жим штанги лежа", MuscleGroup = "Грудь", Equipment = "Штанга", IsCustom = false },
                    new Exercise { Name = "Жим гантелей лежа", MuscleGroup = "Грудь", Equipment = "Гантели", IsCustom = false },
                    new Exercise { Name = "Жим гантелей на наклонной скамье", MuscleGroup = "Грудь", Equipment = "Гантели", IsCustom = false },
                    new Exercise { Name = "Разводка гантелей лежа", MuscleGroup = "Грудь", Equipment = "Гантели", IsCustom = false },
                    new Exercise { Name = "Отжимания на брусьях", MuscleGroup = "Грудь", Equipment = "Брусья", IsCustom = false },
                    new Exercise { Name = "Сведение в кроссовере", MuscleGroup = "Грудь", Equipment = "Тренажер", IsCustom = false },
                    new Exercise { Name = "Становая тяга", MuscleGroup = "Спина", Equipment = "Штанга", IsCustom = false },
                    new Exercise { Name = "Подтягивания", MuscleGroup = "Спина", Equipment = "Турник", IsCustom = false },
                    new Exercise { Name = "Тяга штанги в наклоне", MuscleGroup = "Спина", Equipment = "Штанга", IsCustom = false },
                    new Exercise { Name = "Тяга гантели в наклоне", MuscleGroup = "Спина", Equipment = "Гантели", IsCustom = false },
                    new Exercise { Name = "Тяга верхнего блока", MuscleGroup = "Спина", Equipment = "Тренажер", IsCustom = false },
                    new Exercise { Name = "Тяга горизонтального блока", MuscleGroup = "Спина", Equipment = "Тренажер", IsCustom = false },
                    new Exercise { Name = "Приседания со штангой", MuscleGroup = "Ноги", Equipment = "Штанга", IsCustom = false },
                    new Exercise { Name = "Жим ногами", MuscleGroup = "Ноги", Equipment = "Тренажер", IsCustom = false },
                    new Exercise { Name = "Выпады с гантелями", MuscleGroup = "Ноги", Equipment = "Гантели", IsCustom = false },
                    new Exercise { Name = "Румынская тяга", MuscleGroup = "Ноги", Equipment = "Штанга", IsCustom = false },
                    new Exercise { Name = "Разгибания ног в тренажере", MuscleGroup = "Ноги", Equipment = "Тренажер", IsCustom = false },
                    new Exercise { Name = "Сгибания ног в тренажере", MuscleGroup = "Ноги", Equipment = "Тренажер", IsCustom = false },
                    new Exercise { Name = "Подъем на носки стоя", MuscleGroup = "Ноги", Equipment = "Тренажер", IsCustom = false },
                    new Exercise { Name = "Жим штанги стоя", MuscleGroup = "Плечи", Equipment = "Штанга", IsCustom = false },
                    new Exercise { Name = "Жим гантелей сидя", MuscleGroup = "Плечи", Equipment = "Гантели", IsCustom = false },
                    new Exercise { Name = "Махи гантелями в стороны", MuscleGroup = "Плечи", Equipment = "Гантели", IsCustom = false },
                    new Exercise { Name = "Махи гантелями в наклоне", MuscleGroup = "Плечи", Equipment = "Гантели", IsCustom = false },
                    new Exercise { Name = "Тяга штанги к подбородку", MuscleGroup = "Плечи", Equipment = "Штанга", IsCustom = false },
                    new Exercise { Name = "Подъем штанги на бицепс", MuscleGroup = "Бицепс", Equipment = "Штанга", IsCustom = false },
                    new Exercise { Name = "Подъем гантелей на бицепс", MuscleGroup = "Бицепс", Equipment = "Гантели", IsCustom = false },
                    new Exercise { Name = "Молоток", MuscleGroup = "Бицепс", Equipment = "Гантели", IsCustom = false },
                    new Exercise { Name = "Сгибания на бицепс в тренажере", MuscleGroup = "Бицепс", Equipment = "Тренажер", IsCustom = false },
                    new Exercise { Name = "Жим узким хватом", MuscleGroup = "Трицепс", Equipment = "Штанга", IsCustom = false },
                    new Exercise { Name = "Французский жим", MuscleGroup = "Трицепс", Equipment = "Штанга", IsCustom = false },
                    new Exercise { Name = "Разгибания на трицепс в блоке", MuscleGroup = "Трицепс", Equipment = "Тренажер", IsCustom = false },
                    new Exercise { Name = "Отжимания от скамьи", MuscleGroup = "Трицепс", Equipment = "Скамья", IsCustom = false },
                    new Exercise { Name = "Скручивания", MuscleGroup = "Пресс", Equipment = "Без оборудования", IsCustom = false },
                    new Exercise { Name = "Подъем ног в висе", MuscleGroup = "Пресс", Equipment = "Турник", IsCustom = false },
                    new Exercise { Name = "Планка", MuscleGroup = "Пресс", Equipment = "Без оборудования", IsCustom = false },
                    new Exercise { Name = "Русский твист", MuscleGroup = "Пресс", Equipment = "Без оборудования", IsCustom = false }
                };

                await Exercises.AddRangeAsync(exercises);
                await SaveChangesAsync();
            }
        }
    }
}