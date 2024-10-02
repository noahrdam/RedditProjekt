namespace RedditAPP.Shared.Models
{
    public class Comment
    {
        public long CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CommentDate { get; set; }
        public int Downvotes { get; set; }
        public int Upvotes { get; set; }
        public string AuthorName{ get; set; }
        
        public Comment(long commentId, string commentText, DateTime commentDate, int downvotes, int upvotes, string authorName)
        {
            CommentId = commentId;
            Content = commentText;
            CommentDate = commentDate;
            Downvotes = downvotes;
            Upvotes = upvotes;
            AuthorName = authorName;
        }
        public Comment() {
            CommentId = 0;
            Content = "";
            Upvotes = 0;
            Downvotes = 0;
        }
    }
}
