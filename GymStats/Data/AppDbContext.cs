using GymStats.Models;
using Microsoft.EntityFrameworkCore;

namespace GymStats.Data;

public class AppDbContext : DbContext
{
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutTemplate> WorkoutTemplates => Set<WorkoutTemplate>();
    public DbSet<TemplateExercise> TemplateExercises => Set<TemplateExercise>();
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();
    public DbSet<Set> Sets => Set<Set>();
    public DbSet<BodyMeasurement> BodyMeasurements => Set<BodyMeasurement>();
    public DbSet<MediaEntry> MediaEntries => Set<MediaEntry>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exercise>().HasIndex(x => x.Name);
        modelBuilder.Entity<Exercise>().HasIndex(x => x.MuscleGroup);
        modelBuilder.Entity<Workout>().HasIndex(x => x.Date);
        modelBuilder.Entity<BodyMeasurement>().HasIndex(x => x.Date);
        modelBuilder.Entity<MediaEntry>().HasIndex(x => x.Date);

        modelBuilder.Entity<WorkoutExercise>().HasOne(x => x.Exercise).WithMany()
            .HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<TemplateExercise>().HasOne(x => x.Template).WithMany(x => x.TemplateExercises)
            .HasForeignKey(x => x.TemplateId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<TemplateExercise>().HasOne(x => x.Exercise).WithMany()
            .HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Restrict);
    }

    public async Task EnsureReadyAsync()
    {
        await Database.EnsureCreatedAsync();
        await AddColumnIfMissingAsync("Exercises", "MovementType", "TEXT NOT NULL DEFAULT ''");
        await AddColumnIfMissingAsync("Exercises", "Angle", "TEXT NOT NULL DEFAULT ''");
        await AddColumnIfMissingAsync("Exercises", "Description", "TEXT NOT NULL DEFAULT ''");
        await AddColumnIfMissingAsync("BodyMeasurements", "Neck", "REAL NOT NULL DEFAULT 0");
        await SeedDataAsync();
    }

    private async Task AddColumnIfMissingAsync(string table, string column, string definition)
    {
        var connection = Database.GetDbConnection();
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = $"PRAGMA table_info([{table}]);";
        var exists = false;
        await using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                if (string.Equals(reader.GetString(1), column, StringComparison.OrdinalIgnoreCase))
                {
                    exists = true;
                    break;
                }
            }
        }

        if (!exists)
        {
            command.CommandText = $"ALTER TABLE [{table}] ADD COLUMN [{column}] {definition};";
            await command.ExecuteNonQueryAsync();
        }
        await connection.CloseAsync();
    }

    private async Task SeedDataAsync()
    {
        if (await Exercises.AnyAsync()) return;

        var items = new (string Name, string Muscle, string Equipment, string Movement, string Angle)[]
        {
            ("Жим штанги лежа","Грудь","Штанга","Жим","Горизонтальный"),
            ("Жим гантелей лежа","Грудь","Гантели","Жим","Горизонтальный"),
            ("Жим гантелей на наклонной скамье","Грудь","Гантели","Жим","Наклонный"),
            ("Разводка гантелей лежа","Грудь","Гантели","Сведение","Горизонтальный"),
            ("Сведение в кроссовере","Грудь","Тренажер","Сведение","Горизонтальный"),
            ("Отжимания на брусьях","Грудь","Брусья","Жим","Вертикальный"),
            ("Становая тяга","Спина","Штанга","Тяга","Наклонный"),
            ("Подтягивания","Спина","Турник","Тяга","Вертикальный"),
            ("Тяга штанги в наклоне","Спина","Штанга","Тяга","Горизонтальный"),
            ("Тяга верхнего блока","Спина","Тренажер","Тяга","Вертикальный"),
            ("Тяга горизонтального блока","Спина","Тренажер","Тяга","Горизонтальный"),
            ("Приседания со штангой","Ноги","Штанга","Приседание","Вертикальный"),
            ("Жим ногами","Ноги","Тренажер","Жим","Вертикальный"),
            ("Выпады с гантелями","Ноги","Гантели","Выпад","Вертикальный"),
            ("Румынская тяга","Ноги","Штанга","Тяга","Наклонный"),
            ("Разгибания ног в тренажере","Ноги","Тренажер","Разгибание","Горизонтальный"),
            ("Сгибания ног в тренажере","Ноги","Тренажер","Сгибание","Горизонтальный"),
            ("Подъем на носки стоя","Икры","Тренажер","Сгибание","Вертикальный"),
            ("Жим штанги стоя","Плечи","Штанга","Жим","Вертикальный"),
            ("Жим гантелей сидя","Плечи","Гантели","Жим","Вертикальный"),
            ("Махи гантелями в стороны","Плечи","Гантели","Отведение","Вертикальный"),
            ("Подъем штанги на бицепс","Бицепс","Штанга","Сгибание","Вертикальный"),
            ("Подъем гантелей на бицепс","Бицепс","Гантели","Сгибание","Вертикальный"),
            ("Молоток","Бицепс","Гантели","Сгибание","Вертикальный"),
            ("Жим узким хватом","Трицепс","Штанга","Жим","Горизонтальный"),
            ("Французский жим","Трицепс","Штанга","Разгибание","Горизонтальный"),
            ("Разгибания на трицепс в блоке","Трицепс","Тренажер","Разгибание","Вертикальный"),
            ("Скручивания","Пресс","Без оборудования","Сгибание","Горизонтальный"),
            ("Подъем ног в висе","Пресс","Турник","Сгибание","Вертикальный"),
            ("Планка","Пресс","Без оборудования","Изометрия","Горизонтальный")
        };

        await Exercises.AddRangeAsync(items.Select(x => new Exercise
        {
            Name = x.Name, MuscleGroup = x.Muscle, Equipment = x.Equipment,
            MovementType = x.Movement, Angle = x.Angle, IsCustom = false
        }));
        await SaveChangesAsync();
    }
}