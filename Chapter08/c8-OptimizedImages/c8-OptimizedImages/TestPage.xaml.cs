using System.Collections.ObjectModel;

namespace c8_DebugVsRelease;

public partial class TestPage : LoadingTimePage
{
    public TestPage()
    {
        InitializeComponent();

        var items = new ObservableCollection<Item>();
        for (var i = 1; i < 30; i++) items.Add(new Item(ImageSource.FromFile(Path.Combine(FileSystem.Current.AppDataDirectory, "test.png"))));
        CollectionView.ItemsSource = items;
    }
}

public class Item(ImageSource icon)
{
    public ImageSource Icon { get; set; } = icon;
}