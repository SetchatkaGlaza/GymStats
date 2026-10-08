using Xunit;
using GymStats.Services;

namespace GymStats.Tests;

public class AnalyticsMathTests
{
    [Fact]
    public void Volume_IsWeightTimesReps()
    {
        Assert.Equal(1200, AnalyticsMath.Volume(80, 15));
    }

    [Fact]
    public void Volume_NeverReturnsNegative()
    {
        Assert.Equal(0, AnalyticsMath.Volume(-10, 12));
        Assert.Equal(0, AnalyticsMath.Volume(10, -12));
    }

    [Fact]
    public void E1Rm_UsesEpleyFormula()
    {
        Assert.Equal(120, AnalyticsMath.EstimatedOneRepMax(100, 6), 3);
    }

    [Fact]
    public void E1Rm_IsZeroForInvalidValues()
    {
        Assert.Equal(0, AnalyticsMath.EstimatedOneRepMax(100, 0));
        Assert.Equal(0, AnalyticsMath.EstimatedOneRepMax(-100, 6));
    }
}