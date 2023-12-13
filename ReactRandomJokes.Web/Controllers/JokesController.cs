using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactRandomJokes.Data;
using ReactRandomJokes.Web.ViewModels;

namespace ReactRandomJokes.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JokesController : ControllerBase
    {
        private string _connectionString;
        public JokesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpGet]
        [Route("randomjoke")]
        [AllowAnonymous]
        public Joke GetRandomJoke()
        {
            var joke = JokesAPI.GetJoke();
            var repo = new JokesRepo(_connectionString);
            if (!repo.JokeIsInDb(joke.OriginalId))
            {
                repo.AddJoke(joke);
                return joke;
            }
            return repo.GetJokeByOriginalId(joke.OriginalId);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getlikescount")]
        public LikesAndDislikes GetLikesCountForJoke(int id)
        {
            var repo = new JokesRepo(_connectionString);
            if (!repo.JokeHasLikes(id))
            {
                return null;
            }
            var jokeLikes = repo.GetLikesForJoke(id);
            return new LikesAndDislikes
            {
                Likes = jokeLikes.Count(l => l.Liked),
                Dislikes = jokeLikes.Count(l => !l.Liked)
            };
        }

        [HttpPost]
        [Route("addupdatelikeforjoke")]
        public void AddUpdateLikeForJoke(UserJokeLikeViewModel like)
        {
            var repo = new JokesRepo(_connectionString);
            var jokeLike = repo.GetJokeLikeForUser(like.UserId, like.JokeId);
            if ( jokeLike == null)
            {
                repo.AddLikeForJoke(like.UserId, like.JokeId, like.Liked);
            }
            else
            {
                repo.UpdateLikeForJoke(jokeLike, like.Liked);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getinteractionstatus")]
        public InteractionStatusViewModel GetInteractionStatus(int jokeId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new()
                {
                    Status = InteractionStatus.Unauthenticated
                };
            }
            var userRepo = new UsersRepo(_connectionString);
            var user = userRepo.GetByEmail(User.Identity.Name);
            var jokeRepo = new JokesRepo(_connectionString);
            var joke = jokeRepo.GetJokeWithLikes(jokeId);
            var jokeLike = joke.UserJokeLikes.FirstOrDefault(like => like.UserId == user.Id);
            if (jokeLike == null)
            {
                return new()
                {
                    Status = InteractionStatus.NeverInteracted
                };
            }
            if (jokeLike.Time.AddMinutes(5) < DateTime.Now)
            {
                return new()
                {
                    Status = InteractionStatus.CanNoLongerInteract
                };
            }
            return jokeLike.Liked ? new() { Status = InteractionStatus.Liked } : new() { Status = InteractionStatus.Disliked };
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getall")]
        public List<Joke> GetAllJokes()
        {
            var repo = new JokesRepo(_connectionString);
            return repo.GetAllJokes();
        }
    }
}
