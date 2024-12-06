using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Polly.Retry;
using PostService.Data;
using PostService.Models;

namespace PostService.BusinessLogic
{
    public class PostHandler : IPostHandler
    {
        private readonly PostDbContext _context;
        private readonly ILogger<PostHandler> _logger;
        private readonly AsyncRetryPolicy _retryPolicy;
        public PostHandler(PostDbContext context, ILogger<PostHandler> logger)
        {
            _context = context;
            _logger = logger;
            _retryPolicy = DatabaseRetryPolicy.CreateRetryPolicy();
        }

        public async Task<Post> GetPostByID(int id)
        {

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    Post post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);
                    return post;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating post");
                    throw;
                }
            });
        }

        public async Task<List<Post>> GetAllPosts ()
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    return await _context.Posts.ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving all posts");
                    throw;
                }
            });
        }

        public async Task<List<Post>> GetPostByUser(int userId)
        {
            List<Post> posts = await _context.Posts.Where(p => p.UserID == userId).ToListAsync();
            return posts;
        }

        public async Task<Post> CreatePost(Post post)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    post.CreatedDate = DateTime.Now;
                    _context.Posts.Add(post);
                    await _context.SaveChangesAsync();
                    return post;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating post");
                    throw;
                }
            });
        }

        private async Task syncFeed(int userId)
        {
            //TODO coming soon
            //this method needs to sync the feed for every person that follows this user. 
            //probably through a sync endpoint int the userService
        }
    }   
}
