namespace GymStats.Services;

public static class AnalyticsMath
{
    public static double Volume(double weight, int reps) => Math.Max(0, weight) * Math.Max(0, reps);

    public static double EstimatedOneRepMax(double weight, int reps) =>
        weight <= 0 || reps <= 0 ? 0 : weight * (1 + reps / 30.0);
}