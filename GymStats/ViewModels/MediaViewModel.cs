using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Models;
using GymStats.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels;

public partial class MediaViewModel : ObservableObject
{
    private readonly Data.AppDbContext _db;
    private readonly MediaService _media;
    [ObservableProperty] private ObservableCollection<MediaEntry> _entries = new();

    public MediaViewModel(Data.AppDbContext db, MediaService media) { _db = db; _media = media; }

    public async Task LoadAsync()
    {
        await _db.EnsureReadyAsync();
        Entries = new ObservableCollection<MediaEntry>(await _db.MediaEntries.AsNoTracking().OrderByDescending(x => x.Date).ToListAsync());
    }

    [RelayCommand] private async Task AddPhotoAsync() { var entry = await _media.PickAndStoreAsync(false); if (entry is not null) Entries.Insert(0, entry); }
    [RelayCommand] private async Task AddVideoAsync() { var entry = await _media.PickAndStoreAsync(true); if (entry is not null) Entries.Insert(0, entry); }
    [RelayCommand]
    private Task OpenAsync(MediaEntry entry) =>
        Launcher.Default.OpenAsync(new OpenFileRequest(
            entry.Description,
            new ReadOnlyFile(entry.FilePath)));

    [RelayCommand]
    private async Task DeleteAsync(MediaEntry entry)
    {
        if (!await Shell.Current.DisplayAlert("Удалить материал?", "Файл и запись будут удалены с устройства.", "Удалить", "Отмена")) return;
        await _media.DeleteAsync(entry);
        Entries.Remove(entry);
    }
}