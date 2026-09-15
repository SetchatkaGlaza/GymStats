using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels
{
    public partial class MediaArchiveViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<MediaEntry> _mediaEntries = new();

        [ObservableProperty]
        private bool _isListEmpty = true;

        public MediaArchiveViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LoadMediaAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();

            var list = await _dbContext.MediaEntries
                .OrderByDescending(m => m.Date)
                .ToListAsync();

            MediaEntries = new ObservableCollection<MediaEntry>(list);
            IsListEmpty = MediaEntries.Count == 0;
        }

        [RelayCommand]
        private async Task DeleteMediaAsync(MediaEntry entry)
        {
            // Удаляем сам файл с диска
            if (File.Exists(entry.FilePath))
            {
                File.Delete(entry.FilePath);
            }

            _dbContext.MediaEntries.Remove(entry);
            await _dbContext.SaveChangesAsync();
            await LoadMediaAsync();
        }

        [RelayCommand]
        private async Task GoToAddMediaAsync()
        {
            await Shell.Current.GoToAsync("AddMediaPage");
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}