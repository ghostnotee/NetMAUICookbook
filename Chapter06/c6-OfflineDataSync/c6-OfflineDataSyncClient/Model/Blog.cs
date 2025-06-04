using System.ComponentModel.DataAnnotations.Schema;

namespace c6_OfflineDataSyncClient.Model;

public class Blog : OfflineClientEntity
{
    public string Title { get; set; } = string.Empty;

    [NotMapped] public bool InSync { get; set; } = true;
}