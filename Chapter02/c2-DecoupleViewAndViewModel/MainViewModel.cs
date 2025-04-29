using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace c2_DecoupleViewAndViewModel;

public class MainViewModel : INotifyPropertyChanged
{
    private int count;
    private string textValue = "Click Me!";

    public MainViewModel()
    {
        UpdateTextCommand = new Command(UpdateText);
    }

    public string TextValue
    {
        get => textValue;
        set
        {
            textValue = value;
            OnPropertyChanged();
        }
    }

    public ICommand UpdateTextCommand { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void UpdateText()
    {
        count++;
        TextValue = count == 1 ? $"Clicked {count} time" : $"Clicked {count} times";
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}