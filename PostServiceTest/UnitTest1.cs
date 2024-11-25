using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using PostService.BusinessLogic;
using PostService.Data;
using PostService.Models;

namespace PostServiceTest
{
    public class UnitTest1
    {

        private readonly PostController _controller;
        private readonly IPostHandler _mockPostHandler;

        public UnitTest1()
        {
            _mockPostHandler = Substitute.For<IPostHandler>(null!); 
            _controller = new PostController(_mockPostHandler);
        }

        [Fact]
        public async Task GetPosts_ReturnsOkWithPosts()
        {
            // Arrange
            var samplePosts = new List<Post>
        {
            new Post { Id = 1, UserID = 1, Content = "Post 1" },
            new Post { Id = 2, UserID = 2, Content = "Post 2" }
        };

            _mockPostHandler.GetAllPosts().Returns(samplePosts);

            // Act
            var result = await _controller.GetPosts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPosts = Assert.IsType<List<Post>>(okResult.Value);

            Assert.Equal(2, returnedPosts.Count);
            Assert.Equal("Post 1", returnedPosts[0].Content);
        }

        [Fact]
        public async Task CreatePost_ReturnsCreatedAtAction()
        {
            // Arrange
            var newPost = new Post { Id = 1, UserID = 1, Content = "New Post" };
            _mockPostHandler.CreatePost(newPost).Returns(newPost);

            // Act
            var result = await _controller.CreatePost(newPost);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedPost = Assert.IsType<Post>(createdResult.Value);

            Assert.Equal("New Post", returnedPost.Content);
            Assert.Equal(1, returnedPost.Id);
        }

        [Fact]
        public async Task CreatePost_ReturnsBadRequestWhenPostIsNull()
        {
            // Arrange
            Post nullPost = null;

            // Act
            var result = await _controller.CreatePost(nullPost);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetPostsByUser_ReturnsOkWithUserPosts()
        {
            // Arrange
            int userId = 1;
            var userPosts = new List<Post>
        {
            new Post { Id = 1, UserID = 1, Content = "User Post 1" }
        };

            _mockPostHandler.GetPostByUser(userId).Returns(userPosts);

            // Act
            var result = await _controller.GetPostsByUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPosts = Assert.IsType<List<Post>>(okResult.Value);

            Assert.Single(returnedPosts);
            Assert.Equal("User Post 1", returnedPosts[0].Content);
        }
    }
}