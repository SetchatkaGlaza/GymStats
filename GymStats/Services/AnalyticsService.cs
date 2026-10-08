using GymStats.Data;
using Microsoft.EntityFrameworkCore;

namespace GymStats.Services;

public sealed record ExerciseProgressPoint(DateTime Date, double Weight, double Volume, int Reps, double EstimatedOneRepMax);
public sealed record BodyProgressPoint(DateTime Date, double Weight, double Chest, double Waist, double Hips, double Biceps, double Thigh, double Calf, double Neck);
public sealed record WorkoutProgressPoint(DateTime Date, double Volume, double AverageRpe);

public sealed class AnalyticsService
{
    private readonly AppDbContext _db;

    public AnalyticsService(AppDbContext db) => _db = db;

    public async Task<int> GetWorkoutCountAsync() =>
        await _db.Workouts.CountAsync();

    public async Task<double> GetTotalVolumeAsync() =>
        await _db.Sets
            .Join(_db.WorkoutExercises, s => s.WorkoutExerciseId, we => we.Id, (_, we) => new { Set = _, we.WorkoutId })
            .SumAsync(x => x.Set.Weight * x.Set.Reps);

    public async Task<double> GetAverageRpeAsync()
    {
        var values = await _db.Sets.Select(s => (double?)s.RPE).ToListAsync();
        return values.Count == 0 ? 0 : values.Average(v => v ?? 0);
    }

    public async Task<int> GetWorkoutCountSinceAsync(DateTime since) =>
        await _db.Workouts.CountAsync(w => w.Date >= since);

    public async Task<List<ExerciseProgressPoint>> GetExerciseProgressAsync(int exerciseId)
    {
        var rows = await (
            from we in _db.WorkoutExercises
            join w in _db.Workouts on we.WorkoutId equals w.Id
            join s in _db.Sets on we.Id equals s.WorkoutExerciseId
            where we.ExerciseId == exerciseId
            select new { w.Date, s.Weight, s.Reps }
        ).ToListAsync();

        return rows
            .GroupBy(x => x.Date.Date)
            .OrderBy(g => g.Key)
            .Select(g =>
            {
                var best = g.OrderByDescending(x => x.Weight).ThenByDescending(x => x.Reps).First();
                var volume = g.Sum(x => x.Weight * x.Reps);
                var e1rm = best.Reps > 0 ? best.Weight * (1 + best.Reps / 30.0) : 0;
                return new ExerciseProgressPoint(g.Key, best.Weight, volume, best.Reps, e1rm);
            })
            .ToList();
    }

    public async Task<List<WorkoutProgressPoint>> GetWorkoutProgressAsync()
    {
        var rows = await (
            from w in _db.Workouts
            join we in _db.WorkoutExercises on w.Id equals we.WorkoutId
            join s in _db.Sets on we.Id equals s.WorkoutExerciseId
            select new { w.Id, w.Date, s.Weight, s.Reps, s.RPE }
        ).ToListAsync();

        return rows
            .GroupBy(x => new { x.Id, Date = x.Date.Date })
            .OrderBy(g => g.Key.Date)
            .Select(g => new WorkoutProgressPoint(
                g.Key.Date,
                g.Sum(x => x.Weight * x.Reps),
                g.Average(x => x.RPE)))
            .ToList();
    }

    public async Task<List<BodyProgressPoint>> GetBodyProgressAsync()
    {
        var rows = await _db.BodyMeasurements.OrderBy(x => x.Date).ToListAsync();
        return rows.Select(x => new BodyProgressPoint(
            x.Date, x.Weight, x.Chest, x.Waist, x.Hips, x.Biceps, x.Thigh, x.Calf, x.Neck)).ToList();
    }
}