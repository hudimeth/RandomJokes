using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using ReactRandomJokes.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReactRandomJokes.Data
{
    public class JokesRepo
    {
        private string _connectionString;
        public JokesRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddJoke(Joke joke)
        {
            var context = new RandomJokesDbContext(_connectionString);
            context.Jokes.Add(joke);
            context.SaveChanges();
        }

        public bool JokeIsInDb(int jokeOriginalId)
        {
            var context = new RandomJokesDbContext(_connectionString);
            return context.Jokes.Any(j => j.OriginalId == jokeOriginalId);
        }
        public Joke GetJokeByOriginalId(int jokeOriginalId)
        {
            var context = new RandomJokesDbContext(_connectionString);
            return context.Jokes.Include(j => j.UserJokeLikes).FirstOrDefault(j => j.OriginalId == jokeOriginalId);
        }
        public List<UserJokeLike> GetLikesForJoke(int id)
        {
            var context = new RandomJokesDbContext(_connectionString);
            return context.UserJokeLikes.Where(l => l.JokeId == id).ToList();
            //context.Jokes.Include(j => j.UserJokeLikes).FirstOrDefault(j => j.Id == id);
        }
        public bool JokeHasLikes(int jokeId)
        {
            var context = new RandomJokesDbContext(_connectionString);
            return context.UserJokeLikes.Any(l => l.JokeId == jokeId);
        }

        //public int GetLikesForJoke(int jokeId)
        //{
        //    var context = new RandomJokesDbContext(_connectionString);
        //    //var jokeLike2 = context.UserJokeLikes.FromSqlInterpolated($"Select Count(*) as Likes from UserJokeLikes Where JokeId ={jokeId} Group by JokeId");

        //    if (!JokeHasLikes(jokeId))
        //    {
        //        return 0;
        //    }
        //    var jokeLike = context.UserJokeLikes.GroupBy(j => j.JokeId).Where(j => j.Key == jokeId && j.All(l => l.Liked == true)).Select(l => new JokeLike
        //    {
        //        Likes = l.Count()
        //    }).ToList();
        //    return jokeLike[0].Likes;
        //}
        //public int GetDislikesForJoke(int jokeId)
        //{
        //    var context = new RandomJokesDbContext(_connectionString);
        //    //var jokeLike2 = context.UserJokeLikes.FromSqlInterpolated($"Select Count(*) as Likes from UserJokeLikes Where JokeId ={jokeId} Group by JokeId");

        //    if (!JokeHasLikes(jokeId))
        //    {
        //        return 0;
        //    }
        //    var jokeLike = context.UserJokeLikes.GroupBy(j => j.JokeId).Where(j => j.Key == jokeId && j.All(l => l.Liked == false)).Select(l => new JokeDislike
        //    {
        //        Dislikes = l.Count()
        //    }).ToList();
        //    return jokeLike[0].Dislikes;
        //}

        public void AddLikeForJoke(int userId, int jokeId, bool liked)
        {
            //maybe have to add validation here to see if the person already liked this joke in the past- then you update the like from dislike to like if its within the time frame...
            var context = new RandomJokesDbContext(_connectionString);
            context.UserJokeLikes.Add(new UserJokeLike
            {
                UserId = userId,
                JokeId = jokeId, 
                Liked = liked,
                Time = DateTime.Now
            });
            context.SaveChanges();
        }
        public void UpdateLikeForJoke(UserJokeLike like, bool likeUpdate)
        {
            var context = new RandomJokesDbContext(_connectionString);
            if(like.Time.AddMinutes(5) < DateTime.Now)
            {
                return;
            }
            context.Database.ExecuteSqlInterpolated($"UPDATE UserJokeLikes SET Liked={likeUpdate} WHERE UserId={like.UserId} AND JokeId={like.JokeId}");
            context.SaveChanges();
        }
        public UserJokeLike GetJokeLikeForUser(int userId, int jokeId)
        {
            var context = new RandomJokesDbContext(_connectionString);
            return context.UserJokeLikes.FirstOrDefault(l => l.UserId == userId && l.JokeId == jokeId);
        }
    }
}
