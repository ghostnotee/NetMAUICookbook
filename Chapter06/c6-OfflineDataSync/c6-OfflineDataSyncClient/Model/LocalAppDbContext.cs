using CommunityToolkit.Datasync.Client.Http;
using CommunityToolkit.Datasync.Client.Offline;
using Microsoft.EntityFrameworkCore;

namespace c6_OfflineDataSyncClient.Model;

public class LocalAppDbContext : OfflineDbContext
{
    public DbSet<Blog> Blogs => Set<Blog>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "local.db");
        optionsBuilder.UseSqlite($"Filename={dbPath}");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnDatasyncInitialization(DatasyncOfflineOptionsBuilder optionsBuilder)
    {
        HttpClientOptions clientOptions = new()
        {
            Endpoint = new Uri("https://zggwkbtz-7128.euw.devtunnels.ms/")
        };
        _ = optionsBuilder.UseHttpClientOptions(clientOptions);
    }

    public async Task SynchronizeAsync(CancellationToken cancellationToken = default)
    {
        var pushResult = await this.PushAsync(cancellationToken);
        if (!pushResult.IsSuccessful) throw new ApplicationException($"Push failed: {pushResult.FailedRequests.FirstOrDefault().Value.ReasonPhrase}");

        var pullResult = await this.PullAsync(cancellationToken);
        if (!pullResult.IsSuccessful) throw new ApplicationException($"Pull failed: {pullResult.FailedRequests.FirstOrDefault().Value.ReasonPhrase}");
    }
}