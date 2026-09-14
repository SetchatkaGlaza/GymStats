using Microsoft.EntityFrameworkCore;
using GymStats.Models;

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

        // При первом создании БД можно сразу заполнить базу упражнений (Seed)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Заполняем базовую базу упражнений
            modelBuilder.Entity<Exercise>().HasData(
                // ГРУДЬ
                new Exercise { Id = 1, Name = "Жим штанги лежа", MuscleGroup = "Грудь", Equipment = "Штанга", IsCustom = false },
                new Exercise { Id = 2, Name = "Жим гантелей лежа", MuscleGroup = "Грудь", Equipment = "Гантели", IsCustom = false },
                new Exercise { Id = 3, Name = "Жим гантелей на наклонной скамье", MuscleGroup = "Грудь", Equipment = "Гантели", IsCustom = false },
                new Exercise { Id = 4, Name = "Разводка гантелей лежа", MuscleGroup = "Грудь", Equipment = "Гантели", IsCustom = false },
                new Exercise { Id = 5, Name = "Отжимания на брусьях", MuscleGroup = "Грудь", Equipment = "Брусья", IsCustom = false },
                new Exercise { Id = 6, Name = "Сведение в кроссовере", MuscleGroup = "Грудь", Equipment = "Тренажер", IsCustom = false },

                // СПИНА
                new Exercise { Id = 7, Name = "Становая тяга", MuscleGroup = "Спина", Equipment = "Штанга", IsCustom = false },
                new Exercise { Id = 8, Name = "Подтягивания", MuscleGroup = "Спина", Equipment = "Турник", IsCustom = false },
                new Exercise { Id = 9, Name = "Тяга штанги в наклоне", MuscleGroup = "Спина", Equipment = "Штанга", IsCustom = false },
                new Exercise { Id = 10, Name = "Тяга гантели в наклоне", MuscleGroup = "Спина", Equipment = "Гантели", IsCustom = false },
                new Exercise { Id = 11, Name = "Тяга верхнего блока", MuscleGroup = "Спина", Equipment = "Тренажер", IsCustom = false },
                new Exercise { Id = 12, Name = "Тяга горизонтального блока", MuscleGroup = "Спина", Equipment = "Тренажер", IsCustom = false },

                // НОГИ
                new Exercise { Id = 13, Name = "Приседания со штангой", MuscleGroup = "Ноги", Equipment = "Штанга", IsCustom = false },
                new Exercise { Id = 14, Name = "Жим ногами", MuscleGroup = "Ноги", Equipment = "Тренажер", IsCustom = false },
                new Exercise { Id = 15, Name = "Выпады с гантелями", MuscleGroup = "Ноги", Equipment = "Гантели", IsCustom = false },
                new Exercise { Id = 16, Name = "Румынская тяга", MuscleGroup = "Ноги", Equipment = "Штанга", IsCustom = false },
                new Exercise { Id = 17, Name = "Разгибания ног в тренажере", MuscleGroup = "Ноги", Equipment = "Тренажер", IsCustom = false },
                new Exercise { Id = 18, Name = "Сгибания ног в тренажере", MuscleGroup = "Ноги", Equipment = "Тренажер", IsCustom = false },
                new Exercise { Id = 19, Name = "Подъем на носки стоя", MuscleGroup = "Ноги", Equipment = "Тренажер", IsCustom = false },

                // ПЛЕЧИ
                new Exercise { Id = 20, Name = "Жим штанги стоя", MuscleGroup = "Плечи", Equipment = "Штанга", IsCustom = false },
                new Exercise { Id = 21, Name = "Жим гантелей сидя", MuscleGroup = "Плечи", Equipment = "Гантели", IsCustom = false },
                new Exercise { Id = 22, Name = "Махи гантелями в стороны", MuscleGroup = "Плечи", Equipment = "Гантели", IsCustom = false },
                new Exercise { Id = 23, Name = "Махи гантелями в наклоне", MuscleGroup = "Плечи", Equipment = "Гантели", IsCustom = false },
                new Exercise { Id = 24, Name = "Тяга штанги к подбородку", MuscleGroup = "Плечи", Equipment = "Штанга", IsCustom = false },

                // БИЦЕПС
                new Exercise { Id = 25, Name = "Подъем штанги на бицепс", MuscleGroup = "Бицепс", Equipment = "Штанга", IsCustom = false },
                new Exercise { Id = 26, Name = "Подъем гантелей на бицепс", MuscleGroup = "Бицепс", Equipment = "Гантели", IsCustom = false },
                new Exercise { Id = 27, Name = "Молоток", MuscleGroup = "Бицепс", Equipment = "Гантели", IsCustom = false },
                new Exercise { Id = 28, Name = "Сгибания на бицепс в тренажере", MuscleGroup = "Бицепс", Equipment = "Тренажер", IsCustom = false },

                // ТРИЦЕПС
                new Exercise { Id = 29, Name = "Жим узким хватом", MuscleGroup = "Трицепс", Equipment = "Штанга", IsCustom = false },
                new Exercise { Id = 30, Name = "Французский жим", MuscleGroup = "Трицепс", Equipment = "Штанга", IsCustom = false },
                new Exercise { Id = 31, Name = "Разгибания на трицепс в блоке", MuscleGroup = "Трицепс", Equipment = "Тренажер", IsCustom = false },
                new Exercise { Id = 32, Name = "Отжимания от скамьи", MuscleGroup = "Трицепс", Equipment = "Скамья", IsCustom = false },

                // ПРЕСС
                new Exercise { Id = 33, Name = "Скручивания", MuscleGroup = "Пресс", Equipment = "Без оборудования", IsCustom = false },
                new Exercise { Id = 34, Name = "Подъем ног в висе", MuscleGroup = "Пресс", Equipment = "Турник", IsCustom = false },
                new Exercise { Id = 35, Name = "Планка", MuscleGroup = "Пресс", Equipment = "Без оборудования", IsCustom = false },
                new Exercise { Id = 36, Name = "Русский твист", MuscleGroup = "Пресс", Equipment = "Без оборудования", IsCustom = false }
            );
        }
    }
}