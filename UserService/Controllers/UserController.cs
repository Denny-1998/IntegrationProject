using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;


    public UserController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
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





    

}
