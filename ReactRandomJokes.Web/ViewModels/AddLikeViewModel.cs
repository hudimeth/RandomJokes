namespace ReactRandomJokes.Web.ViewModels
{
    public class AddLikeViewModel
    {
        public int UserId { get; set; }
        public int JokeId { get; set; }
        public bool Liked { get; set; }
    }
}
