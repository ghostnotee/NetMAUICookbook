using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Datasync.Server.EntityFrameworkCore;

namespace c6_OfflineDataSync;

public class Blog : EntityTableData
{
    [Required] [MinLength(1)] public string Title { get; set; } = string.Empty;
}