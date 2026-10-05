using System.Text;
using System.Text.Json;
using GymStats.Data;
using Microsoft.EntityFrameworkCore;

namespace GymStats.Services;

public sealed class BackupService
{
    private readonly AppDbContext _db;

    public BackupService(AppDbContext db) => _db = db;

    public async Task<string> CreateBackupAsync()
    {
        var payload = new BackupPayload
        {
            ExportedAt = DateTimeOffset.Now,
            Exercises = await _db.Exercises.AsNoTracking().ToListAsync(),
            Templates = await _db.WorkoutTemplates.AsNoTracking().ToListAsync(),
            TemplateExercises = await _db.TemplateExercises.AsNoTracking().ToListAsync(),
            Workouts = await _db.Workouts.AsNoTracking().ToListAsync(),
            WorkoutExercises = await _db.WorkoutExercises.AsNoTracking().ToListAsync(),
            Sets = await _db.Sets.AsNoTracking().ToListAsync(),
            Measurements = await _db.BodyMeasurements.AsNoTracking().ToListAsync(),
            Media = await _db.MediaEntries.AsNoTracking().ToListAsync()
        };

        return JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
    }

    public async Task<string> SaveBackupAsync()
    {
        var json = await CreateBackupAsync();
        var fileName = $"GymStats-backup-{DateTime.Now:yyyyMMdd-HHmmss}.json";
        var path = Path.Combine(FileSystem.CacheDirectory, fileName);
        await File.WriteAllTextAsync(path, json, Encoding.UTF8);
        return path;
    }

    private sealed class BackupPayload
    {
        public DateTimeOffset ExportedAt { get; init; }
        public object Exercises { get; init; } = null!;
        public object Templates { get; init; } = null!;
        public object TemplateExercises { get; init; } = null!;
        public object Workouts { get; init; } = null!;
        public object WorkoutExercises { get; init; } = null!;
        public object Sets { get; init; } = null!;
        public object Measurements { get; init; } = null!;
        public object Media { get; init; } = null!;
    }
}
