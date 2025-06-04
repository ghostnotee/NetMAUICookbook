using CommunityToolkit.Datasync.Server;
using CommunityToolkit.Datasync.Server.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace c6_OfflineDataSync.Controllers;

[Route("tables/[controller]")]
public class BlogController(AppDbContext context) : TableController<Blog>(new EntityTableRepository<Blog>(context))
{
}