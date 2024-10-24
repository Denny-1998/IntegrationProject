using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Models;
using Newtonsoft.Json;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    private readonly UserDbContext _context;



    public UserController(IHttpClientFactory httpClientFactory, UserDbContext context)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
    }

    [HttpGet("posts")]
    public async Task<IActionResult> GetPosts()
    {
        var client = _httpClientFactory.CreateClient();
        try
        {
            var response = await client.GetAsync("http://postservice:8080/api/post"); 
            if (response.IsSuccessStatusCode)
            {
                var posts = await response.Content.ReadAsStringAsync();
                return Ok(posts);
            }

            return StatusCode((int)response.StatusCode);
        }
        catch (Exception ex) 
        {
            string em = ex.Message;
            return BadRequest(em);
        }       
    }

    [HttpGet("UserPosts")]
    public async Task<IActionResult> GetUserPosts(int userId)
    {
        var client = _httpClientFactory.CreateClient();
        try
        {
            // Update the URL to call the GetPostsByUser method
            var response = await client.GetAsync($"http://postservice:8080/api/post/user/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var posts = await response.Content.ReadAsStringAsync();
                return Ok(posts);
            }

            return StatusCode((int)response.StatusCode);
        }
        catch (Exception ex)
        {
            string em = ex.Message;
            return BadRequest(em);
        }
    }



    [HttpPost]
    public async Task<IActionResult> CreateUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetUsers()
    {
        List<User> users = _context.Users.ToList();

      return Ok(users);
    }



    [HttpPost("{userId}/follow/{followUserId}")]
    public async Task<IActionResult> FollowUser(int userId, int followUserId)
    {
        var user = await _context.Users.FindAsync(userId);
        var followUser = await _context.Users.FindAsync(followUserId);

        if (user == null || followUser == null)
        {
            return NotFound("User not found");
        }

        if (!user.Following.Contains(followUserId))
        {
            user.Following.Add(followUserId);
            followUser.Followers.Add(userId);
            await _context.SaveChangesAsync();
        }
        await syncFeed(userId);

        return Ok("User followed successfully");
    }

    [HttpPost("{userId}/unfollow/{unfollowUserId}")]
    public async Task<IActionResult> UnfollowUser(int userId, int unfollowUserId)
    {
        var user = await _context.Users.FindAsync(userId);
        var unfollowUser = await _context.Users.FindAsync(unfollowUserId);

        if (user == null || unfollowUser == null)
        {
            return NotFound("User not found");
        }

        if (user.Following.Contains(unfollowUserId))
        {
            user.Following.Remove(unfollowUserId);
            unfollowUser.Followers.Remove(userId);
            await _context.SaveChangesAsync();
        }

        return Ok("User unfollowed successfully");
    }


    public async Task syncFeed(int userId)
    {
        var content = new StringContent(JsonConvert.SerializeObject(new { userId }), Encoding.UTF8, "application/json");

        var client = _httpClientFactory.CreateClient();

        //fire and forget approach
        try
        {
            var response = await client.PostAsync($"http://feedservice:8080/api/feed/sync", content);
        } catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }



}
