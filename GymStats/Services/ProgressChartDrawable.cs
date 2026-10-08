using Microsoft.Maui.Graphics;

namespace GymStats.Services;

public sealed class ProgressChartDrawable : IDrawable
{
    public IReadOnlyList<double> Values { get; set; } = Array.Empty<double>();
    public IReadOnlyList<string> Labels { get; set; } = Array.Empty<string>();
    public string EmptyText { get; set; } = "Нет данных";
    public string Unit { get; set; } = string.Empty;

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();
        canvas.FontSize = 10;

        if (Values.Count < 2 || dirtyRect.Width < 80 || dirtyRect.Height < 80)
        {
            canvas.FontColor = Color.FromArgb("#8A938D");
            canvas.DrawString(EmptyText, dirtyRect.Center.X, dirtyRect.Center.Y, HorizontalAlignment.Center);
            canvas.RestoreState();
            return;
        }

        const float left = 38f;
        const float rightInset = 10f;
        const float top = 18f;
        const float bottomInset = 28f;
        var right = dirtyRect.Width - rightInset;
        var bottom = dirtyRect.Height - bottomInset;

        var min = Values.Min();
        var max = Values.Max();
        var padding = Math.Max((max - min) * 0.12, Math.Max(Math.Abs(max) * 0.02, 0.5));
        min -= padding;
        max += padding;
        var range = Math.Max(max - min, 1);

        canvas.StrokeSize = 1;
        canvas.StrokeColor = Color.FromArgb("#DDE5DF");
        for (var i = 0; i <= 3; i++)
        {
            var y = top + (bottom - top) * i / 3f;
            canvas.DrawLine(left, y, right, y);
        }

        canvas.FontColor = Color.FromArgb("#718078");
        canvas.DrawString(FormatValue(max), 2, top - 4, HorizontalAlignment.Left);
        canvas.DrawString(FormatValue((max + min) / 2), 2, (top + bottom) / 2 - 4, HorizontalAlignment.Left);
        canvas.DrawString(FormatValue(min), 2, bottom - 4, HorizontalAlignment.Left);

        canvas.StrokeColor = Color.FromArgb("#2F8F5B");
        canvas.StrokeSize = 2.5f;
        canvas.FillColor = Color.FromArgb("#2F8F5B");

        PointF? previous = null;
        for (var i = 0; i < Values.Count; i++)
        {
            var x = left + (right - left) * i / (Values.Count - 1);
            var y = bottom - (float)((Values[i] - min) / range) * (bottom - top);

            if (previous is { } p)
                canvas.DrawLine(p.X, p.Y, x, y);

            canvas.FillCircle(x, y, 3.5f);
            previous = new PointF(x, y);
        }

        if (Labels.Count == Values.Count)
        {
            canvas.FontColor = Color.FromArgb("#718078");
            canvas.DrawString(Labels[0], left, bottom + 8, HorizontalAlignment.Left);
            canvas.DrawString(Labels[^1], right, bottom + 8, HorizontalAlignment.Right);

            if (Labels.Count > 2)
            {
                var middle = Labels.Count / 2;
                var middleX = left + (right - left) * middle / (Labels.Count - 1);
                canvas.DrawString(Labels[middle], middleX, bottom + 8, HorizontalAlignment.Center);
            }
        }

        canvas.FontColor = Color.FromArgb("#247247");
        canvas.DrawString($"{Values[^1]:0.#} {Unit}".Trim(), right, top - 4, HorizontalAlignment.Right);

        canvas.RestoreState();
    }

    private static string FormatValue(double value) =>
        Math.Abs(value) >= 1000 ? value.ToString("0.#") : value.ToString("0.##");
}