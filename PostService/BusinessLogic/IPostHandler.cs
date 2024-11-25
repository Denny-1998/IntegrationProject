using System.Collections.Generic;
using System.Threading.Tasks;
using PostService.Models;

namespace PostService.BusinessLogic
{
    public interface IPostHandler
    {
        Task<Post> GetPostByID(int id);
        Task<List<Post>> GetAllPosts();
        Task<List<Post>> GetPostByUser(int userId);
        Task<Post> CreatePost(Post post);
    }
}
