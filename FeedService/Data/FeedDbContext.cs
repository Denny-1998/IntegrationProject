using Microsoft.EntityFrameworkCore;
using FeedService.Models;
using System.Collections.Generic;

namespace FeedService.Data
{
    public class FeedDbContext : DbContext
    {
        public FeedDbContext(DbContextOptions<FeedDbContext> options) : base(options) { }

        public DbSet<FeedItem> FeedItems { get; set; }
    }
}
