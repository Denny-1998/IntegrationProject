using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using PostService.Data;
using PostService.Models;

namespace PostService.BusinessLogic
{
    public class PostHandler : IPostHandler
    {
        private readonly PostDbContext _context;
        public PostHandler(PostDbContext context)
        {
            _context = context;
        }

        public async Task<Post> GetPostByID(int id)
        {
            Post post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);
            return post;
        }

        public async Task<List<Post>> GetAllPosts ()
        {
            List<Post> posts = await _context.Posts.ToListAsync();
            return posts;
        }

        public async Task<List<Post>> GetPostByUser(int userId)
        {
            List<Post> posts = await _context.Posts.Where(p => p.UserID == userId).ToListAsync();
            return posts;
        }

        public async Task<Post> CreatePost(Post post)
        {
            post.CreatedDate = DateTime.Now;

            _context.Posts.Add(post);
            _context.SaveChanges();

            syncFeed(post.UserID);

            return post;
        }

        private async Task syncFeed(int userId)
        {
            //TODO coming soon
            //this method needs to sync the feed for every person that follows this user. 
            //probably through a sync endpoint int the userService
        }
    }   
}
