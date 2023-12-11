using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReactRandomJokes.Data
{
    public static class JokesAPI
    {
        private class JokeAPIJoke
        {
            public int JokeId { get; set; }
            public string Setup { get; set; }
            public string Punchline { get; set; }
        }
        public static Joke GetJoke()
        {
            using var client = new HttpClient();
            var json = client.GetStringAsync("https://jokesapi.lit-projects.com/jokes/programming/random").Result;
            var joke = JsonSerializer.Deserialize<List<JokeAPIJoke>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }).FirstOrDefault();

            return new Joke
            {
                OriginalId = joke.JokeId,
                Setup = joke.Setup,
                Punchline = joke.Punchline
            };
        }
    }
}
