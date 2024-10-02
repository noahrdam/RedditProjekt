namespace RedditAPP.Shared.Models
{
    public class Post
    {
        public int PostId { get; set; }
        
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public List<Comment> Comments { get; set; }
        //a
        public Post(int postId, DateTime postDate, string authorName, int upvotes, int downvotes, List<Comment> comments)
        {
            PostId = postId;
            PostDate = postDate;
            AuthorName = authorName;
            Upvotes = upvotes;
            Downvotes = downvotes;
            Comments = comments;
        }
        public Post() {
            PostId = 0;
            PostDate = DateTime.Now;
            AuthorName = "";
            Upvotes = 0;
            Downvotes = 0;
            Comments = new List<Comment>();
        }
        public override string ToString()
        {
            return $"Id: {PostId}, Title: {Title}, Content: {Content}, Upvotes: {Upvotes}, Downvotes: {Downvotes}, Author: {AuthorName}";
        }
    }
}
