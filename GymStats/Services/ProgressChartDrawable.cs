using Microsoft.Maui.Graphics;

namespace GymStats.Services;

public sealed class ProgressChartDrawable : IDrawable
{
    public IReadOnlyList<double> Values { get; set; } = Array.Empty<double>();
    public string EmptyText { get; set; } = "Нет данных";

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();
        canvas.StrokeSize = 2;
        canvas.FontSize = 11;

        if (Values.Count < 2)
        {
            canvas.FontColor = Color.FromArgb("#8A938D");
            canvas.DrawString(EmptyText, dirtyRect.Center.X, dirtyRect.Center.Y, HorizontalAlignment.Center);
            canvas.RestoreState();
            return;
        }

        var left = 12f;
        var right = dirtyRect.Width - 12f;
        var top = 16f;
        var bottom = dirtyRect.Height - 20f;
        var min = Values.Min();
        var max = Values.Max();
        var range = Math.Max(max - min, 1);

        canvas.StrokeColor = Color.FromArgb("#DCE7DF");
        canvas.DrawLine(left, bottom, right, bottom);
        canvas.DrawLine(left, top, left, bottom);

        canvas.StrokeColor = Color.FromArgb("#2F8F5B");
        canvas.FillColor = Color.FromArgb("#2F8F5B");

        PointF? previous = null;
        for (var i = 0; i < Values.Count; i++)
        {
            var x = Values.Count == 1 ? left : left + (right - left) * i / (Values.Count - 1);
            var y = bottom - (float)((Values[i] - min) / range) * (bottom - top);

            if (previous is { } p)
                canvas.DrawLine(p.X, p.Y, x, y);

            canvas.FillCircle(x, y, 3.5f);
            previous = new PointF(x, y);
        }

        canvas.FontColor = Color.FromArgb("#64706A");
        canvas.DrawString(max.ToString("0.#"), 4, top, HorizontalAlignment.Left);
        canvas.DrawString(min.ToString("0.#"), 4, bottom, HorizontalAlignment.Left);
        canvas.RestoreState();
    }
}
