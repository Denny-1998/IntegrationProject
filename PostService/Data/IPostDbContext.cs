using Microsoft.EntityFrameworkCore;
using PostService.Models;

namespace PostService.Data
{
    public interface IPostDbContext
    {
        public DbSet<Post> Posts { get; set; }
    }
}
