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
        [Route("getjokelikeforuser")]
        public UserJokeLike GetJokeLikeForUser(int userId, int jokeId)
        {
            var repo = new JokesRepo(_connectionString);
            return repo.GetJokeLikeForUser(userId, jokeId);
        }

        [HttpGet]
        [Route("jokelikes/{jokeId}")]
        public List<UserJokeLike> GetJokeLikesForJoke(int jokeId)
        {
            var repo = new JokesRepo(_connectionString);
            return repo.GetLikesForJoke(jokeId);
        }

        //[HttpPost]
        //[Route("updatelikeforjoke")]
        //public void UpdateLikeForJoke(UserJokeLike like)
        //{
        //    var repo = new JokesRepo(_connectionString);
        //    repo.UpdateLikeForJoke(like);
        //}

    }
}
