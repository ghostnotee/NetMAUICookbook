using c6_OfflineDataSyncClient.Model;
using CommunityToolkit.Datasync.Client;
using CommunityToolkit.Datasync.Client.Offline;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace c6_OfflineDataSyncClient;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private bool _isRefreshing = true;

    [ObservableProperty] private ConcurrentObservableCollection<Blog> _items = [];

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddBlogCommand))]
    public string _newItemText = string.Empty;

    [ObservableProperty] private bool _outOfSync;

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        using var context = new LocalAppDbContext();
        SetNonSyncedItems(context);
        Items = new ConcurrentObservableCollection<Blog>(context.Blogs);
        try
        {
            await context.SynchronizeAsync();
            Items = new ConcurrentObservableCollection<Blog>(context.Blogs);
            OutOfSync = false;
        }
        catch
        {
            OutOfSync = true;
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    private void SetNonSyncedItems(LocalAppDbContext context)
    {
        var queuedBlogs = context.DatasyncOperationsQueue.Where(x => x.EntityType == typeof(Blog).FullName);
        foreach (var blog in queuedBlogs)
            if (blog.State != OperationState.Completed)
            {
                var item = context.Blogs.Find(blog.ItemId);
                item.InSync = false;
            }
    }

    [RelayCommand(CanExecute = nameof(CanAddBlog))]
    private void AddBlog()
    {
        var newBlog = new Blog
        {
            Title = NewItemText,
            InSync = false
        };
        Items.Add(newBlog);
        NewItemText = string.Empty;
        SaveChangedItem(newBlog);
    }

    private bool CanAddBlog() => !string.IsNullOrEmpty(NewItemText);

    private void SaveChangedItem(Blog newBlog)
    {
        Task.Run(async () =>
        {
            await using var context = new LocalAppDbContext();
            context.Blogs.Add(newBlog);
            await context.SaveChangesAsync();
            var pushResult = await context.PushAsync();
            if (pushResult.IsSuccessful) UpdateCollectionItem(newBlog.Id);
        });
    }

    private void UpdateCollectionItem(string itemId)
    {
        using var context = new LocalAppDbContext();
        var freshItem = context.Blogs.Find(itemId);
        Shell.Current.Dispatcher.Dispatch(() => { Items.ReplaceIf(item => item.Id == itemId, freshItem); });
    }
}