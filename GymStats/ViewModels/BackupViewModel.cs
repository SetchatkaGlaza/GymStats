using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Services;

namespace GymStats.ViewModels;

public partial class BackupViewModel : ObservableObject
{
    private readonly BackupService _backup;

    [ObservableProperty] private string _status = "Экспорт создаст JSON-файл со всей локальной историей.";

    public BackupViewModel(BackupService backup) => _backup = backup;

    [RelayCommand]
    private async Task ExportAsync()
    {
        try
        {
            var path = await _backup.SaveBackupAsync();
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Резервная копия GymStats",
                File = new ShareFile(path)
            });
            Status = "Резервная копия готова. Сохрани её в надёжном месте.";
        }
        catch (Exception ex)
        {
            Status = $"Не удалось создать резервную копию: {ex.Message}";
        }
    }
}