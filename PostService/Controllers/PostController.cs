using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostService.Data;
using PostService.Models;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{

    private readonly PostDbContext _context;

    public PostController(PostDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public IActionResult GetPosts()
    {
        // Retrieve posts from the database
        List<Post> posts = _context.Posts.ToList();
        return Ok(posts);
    }



    [HttpPost]
    public IActionResult CreatePost([FromBody] Post post)
    {
        if (post == null)
        {
            return BadRequest();
        }

        _context.Posts.Add(post);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetPosts), new { id = post.Id }, post);
    }

    [HttpGet("user/{userId}")]
    public IActionResult GetPostsByUser(int userId)
    {
        var posts = _context.Posts.Where(p => p.UserID == userId).ToList();
        return Ok(posts);
    }

}
