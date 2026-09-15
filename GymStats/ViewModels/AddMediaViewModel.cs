using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;

namespace GymStats.ViewModels
{
    public partial class AddMediaViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty] private DateTime _mediaDate = DateTime.Today;
        [ObservableProperty] private string _description = string.Empty;
        [ObservableProperty] private ImageSource _imageSource;
        [ObservableProperty] private string _selectedFilePath = string.Empty;
        [ObservableProperty] private string _statusMessage = string.Empty;
        [ObservableProperty] private bool _isStatusVisible = false;
        [ObservableProperty] private bool _isImageSelected = false;

        public AddMediaViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [RelayCommand]
        private async Task PickPhotoAsync()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions { Title = "Выбери фото прогресса" });

                if (result != null)
                {
                    // Сохраняем файл в локальную папку приложения, чтобы он не потерялся
                    var localPath = Path.Combine(FileSystem.AppDataDirectory, $"media_{Guid.NewGuid()}.jpg");

                    using var stream = await result.OpenReadAsync();
                    using var newStream = File.OpenWrite(localPath);
                    await stream.CopyToAsync(newStream);

                    SelectedFilePath = localPath;
                    ImageSource = ImageSource.FromFile(localPath);
                    IsImageSelected = true;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка выбора фото: {ex.Message}";
                IsStatusVisible = true;
            }
        }

        [RelayCommand]
        private async Task SaveMediaAsync()
        {
            if (string.IsNullOrEmpty(SelectedFilePath))
            {
                StatusMessage = "Сначала выбери фото!";
                IsStatusVisible = true;
                return;
            }

            var newEntry = new MediaEntry
            {
                Date = MediaDate,
                FilePath = SelectedFilePath,
                Description = Description,
                MediaType = "Photo"
            };

            await _dbContext.MediaEntries.AddAsync(newEntry);
            await _dbContext.SaveChangesAsync();

            StatusMessage = "Фото успешно сохранено!";
            IsStatusVisible = true;

            await Task.Delay(1000);
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}