using System.IO.Compression;
using System.Text;
using System.Text.Json;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;

namespace GymStats.Services;

public sealed class BackupService
{
    private readonly AppDbContext _db;
    public BackupService(AppDbContext db) => _db = db;

    public async Task<string> SaveBackupAsync()
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

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
        var path = Path.Combine(FileSystem.CacheDirectory, $"GymStats-backup-{DateTime.Now:yyyyMMdd-HHmmss}.zip");
        if (File.Exists(path)) File.Delete(path);

        using var archive = ZipFile.Open(path, ZipArchiveMode.Create);
        var dataEntry = archive.CreateEntry("gymstats.json", CompressionLevel.Optimal);
        await using (var stream = dataEntry.Open())
        await using (var writer = new StreamWriter(stream, new UTF8Encoding(false)))
            await writer.WriteAsync(json);

        foreach (var media in payload.Media)
        {
            if (!File.Exists(media.FilePath)) continue;
            var entry = archive.CreateEntry($"media/{media.Id}{Path.GetExtension(media.FilePath)}", CompressionLevel.Fastest);
            await using var source = File.OpenRead(media.FilePath);
            await using var destination = entry.Open();
            await source.CopyToAsync(destination);
        }

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
        public List<MediaEntry> Media { get; init; } = new();
    }
}