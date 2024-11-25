using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostService.BusinessLogic;
using PostService.Data;
using PostService.Models;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostHandler _postHandler;

    public PostController(IPostHandler postHandler)
    {
        _postHandler = postHandler;

    }


    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        // Retrieve posts from the database
        List<Post> posts = await _postHandler.GetAllPosts();
        return Ok(posts);
    }



    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] Post post)
    {
        if (post == null)
        {
            return BadRequest();
        }

        Post newPost = await _postHandler.CreatePost(post);

        return CreatedAtAction(nameof(GetPosts), new { id = post.Id }, newPost);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetPostsByUser(int userId)
    {
        List<Post> posts = await _postHandler.GetPostByUser(userId);
        return Ok(posts);
    }

   

}
