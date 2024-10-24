using Microsoft.AspNetCore.Mvc;
using FeedService.Data;
using FeedService.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Newtonsoft.Json;

namespace FeedService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly FeedDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;


        public FeedController(IHttpClientFactory httpClientFactory, FeedDbContext context)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<FeedItem>>> GetFeed(int userId)
        {
            var feedItems = await _context.FeedItems
                .Where(fi => fi.UserId == userId)
                .ToListAsync();

            if (feedItems == null || !feedItems.Any())
            {
                return NotFound();
            }

            return Ok(feedItems);
        }

        [HttpPost]
        public async Task<ActionResult<FeedItem>> AddFeedItem(FeedItem feedItem)
        {
            _context.FeedItems.Add(feedItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFeed), new { userId = feedItem.UserId }, feedItem);
        }



        [HttpPost("sync")]
        public async Task<IActionResult> SyncFeed([FromBody] SyncFeedRequest request)
        {
            
            if (request == null || request.UserId <= 0)
            {
                return BadRequest("Invalid request");
            }

            // Fetch posts from the PostService 
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetAsync($"http://postservice:8080/api/post?userId={request.UserId}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var posts = JsonConvert.DeserializeObject<List<FeedItem>>(jsonResponse);



                    return Ok();
                }

                return StatusCode((int)response.StatusCode);
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }  
        }

        

        void sync(List<FeedItem> posts)
        {
            foreach (var post in posts)
            {
                _context.FeedItems.AddAsync(post);
                _context.SaveChangesAsync();
            }
        }

    }


    public class SyncFeedRequest
    {
        public int UserId { get; set; }
    }
}
