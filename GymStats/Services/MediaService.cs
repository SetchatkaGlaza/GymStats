using GymStats.Models;

namespace GymStats.Services;

public sealed class MediaService
{
    private readonly Data.AppDbContext _db;

    public MediaService(Data.AppDbContext db) => _db = db;

    public async Task<MediaEntry?> PickAndStoreAsync(bool video)
    {
        var options = new PickOptions
        {
            PickerTitle = video ? "Выберите видео прогресса" : "Выберите фото прогресса",
            FileTypes = video ? FilePickerFileType.Videos : FilePickerFileType.Images
        };

        var result = await FilePicker.Default.PickAsync(options);
        if (result is null) return null;

        var extension = Path.GetExtension(result.FileName);
        var mediaDirectory = Path.Combine(FileSystem.AppDataDirectory, "ProgressMedia");
        Directory.CreateDirectory(mediaDirectory);

        var target = Path.Combine(mediaDirectory, $"{Guid.NewGuid():N}{extension}");
        await using (var source = await result.OpenReadAsync())
        await using (var destination = File.Create(target))
        {
            await source.CopyToAsync(destination);
        }

        var entry = new MediaEntry
        {
            Date = DateTime.Now,
            FilePath = target,
            Description = result.FileName,
            MediaType = video ? "video" : "image"
        };

        _db.MediaEntries.Add(entry);
        await _db.SaveChangesAsync();
        return entry;
    }

    public async Task DeleteAsync(MediaEntry entry)
    {
        _db.MediaEntries.Remove(entry);
        await _db.SaveChangesAsync();

        if (File.Exists(entry.FilePath))
            File.Delete(entry.FilePath);
    }
}
