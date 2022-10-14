using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostLookupController : ControllerBase
    {
        private readonly ILogger<PostLookupController> _logger;
        private readonly IQueryDispatcher<PostEntity> _dispatcher;

        public PostLookupController(ILogger<PostLookupController> logger, IQueryDispatcher<PostEntity> dispatcher)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPostsAsync()
        {
            try
            {
                var posts = await _dispatcher.SendAsync(new FindAllPostsQuery());

                return NormalResponse(posts);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSSAGE = "Error while processing request to retrieve all posts!";
                
                return ErrorResponse(ex, SAFE_ERROR_MESSSAGE);            
            }
        }

        [HttpGet("byId/{postId}")]
        public async Task<ActionResult> GetPostByIdAsync(Guid postId)
        {
            try
            {
                var posts = await _dispatcher.SendAsync(new FindPostByIdQuery() { Id = postId });

                if (posts == null || !posts.Any())
                    return NoContent();

                var count = posts.Count;
                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = $"Succesfully returned post!"
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSSAGE = "Error while processing request to find post by ID";
                
                return ErrorResponse(ex, SAFE_ERROR_MESSSAGE);       
            }
        }

        [HttpGet("byAuthor/{author}")]
        public async Task<ActionResult> GetPostsByAuthorAsync(string author)
        {
            try
            {
                var posts = await _dispatcher.SendAsync(new FindPostsByAuthorQuery { Author = author } );

                return NormalResponse(posts);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSSAGE = "Error while processing request to find posts by Author!";
                
                return ErrorResponse(ex, SAFE_ERROR_MESSSAGE);           
            }
        }

        [HttpGet("withComments")]
        public async Task<ActionResult> GetPostsWithCommentsAsync()
        {
            try
            {
                var posts = await _dispatcher.SendAsync(new FindPostsWithCommentQuery());

                return NormalResponse(posts);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSSAGE = "Error while processing request to find posts with comments!";

                return ErrorResponse(ex, SAFE_ERROR_MESSSAGE);
            }
        }      

        [HttpGet("withLikes/{numberOfLikes}")]
        public async Task<ActionResult> GetPostsWithLikesAsync(int numberOfLikes)
        {
            try
            {
                var posts = await _dispatcher.SendAsync(new FindPostsWithLikesQuery { NumberOfLikes = numberOfLikes });

                return NormalResponse(posts);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSSAGE = "Error while processing request to find posts with likes!";

                return ErrorResponse(ex, SAFE_ERROR_MESSSAGE);
            }
        }        

        private ActionResult NormalResponse(List<PostEntity> posts)
        {
            if (posts == null || !posts.Any())
                return NoContent();

            var count = posts.Count;
            return Ok(new PostLookupResponse
            {
                Posts = posts,
                Message = $"Succesfully returned {count} post{(count > 1 ? "s" : String.Empty)}!"
            });
        }

        private ActionResult ErrorResponse(Exception ex, string safeErrorMessage)
        {
            _logger.Log(LogLevel.Error, ex, safeErrorMessage);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = safeErrorMessage
            });
        }
    }
}