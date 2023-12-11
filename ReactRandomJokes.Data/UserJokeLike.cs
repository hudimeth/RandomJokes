using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReactRandomJokes.Data
{
    public class UserJokeLike
    {
        public int UserId { get; set; }
        public int JokeId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public Joke Joke { get; set; }

        public bool Liked { get; set; }
        public DateTime Time { get; set; }
    }
}
