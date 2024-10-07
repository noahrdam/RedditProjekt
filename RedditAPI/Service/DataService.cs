using RedditAPP.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace RedditAPI.Service
{
	public class DataService
	{
		private PostContext db { get; }

		public DataService(PostContext db)
		{
			this.db = db;
		}

		public void SeedData()
		{
			Post post = db.Posts.FirstOrDefault();
			if (post == null)
			{
				Post post1 = new Post
				{
					Title = "First post",
					PostDate = DateTime.Now,
					Content = "This is the first post",
					AuthorName = "John Doe",
					Upvotes = 5,
					Downvotes = 2,
					Comments = new List<Comment>
			{
				new Comment
				{
					Content = "This is a great post!",
					CommentDate = DateTime.Now,
					Upvotes = 10,
					Downvotes = 1,
					AuthorName = "Alice"
				},
				new Comment
				{
					Content = "I don't agree with this post.",
					CommentDate = DateTime.Now,
					Upvotes = 3,
					Downvotes = 5,
					AuthorName = "Bob"
				}
			}
				};

				Post post2 = new Post
				{
					Title = "Second post",
					PostDate = DateTime.Now,
					Content = "This is the second post",
					AuthorName = "Jane Doe",
					Upvotes = 8,
					Downvotes = 1,
					Comments = new List<Comment>
			{
				new Comment
				{
					Content = "Interesting perspective!",
					CommentDate = DateTime.Now,
					Upvotes = 12,
					Downvotes = 0,
					AuthorName = "Charlie"
				}
			}
				};

				db.Posts.Add(post1);
				db.Posts.Add(post2);

				db.SaveChanges();
			}
		}

		public List<Post> GetPosts()
		{
			return db.Posts.Include(p => p.Comments).ToList();
		}

		public Post GetPost(int id)
		{
			return db.Posts.Include(p => p.Comments).FirstOrDefault(p => p.PostId == id);
		}

		public Comment GetComment(int postId, long commentId)
		{
			Post post = db.Posts.Include(p => p.Comments).FirstOrDefault(p => p.PostId == postId);
			if (post != null)
			{
				return post.Comments.FirstOrDefault(c => c.CommentId == commentId);
			}
			return null;
		}

		public bool UpVotePost(int id)
		{
			Post post = db.Posts.FirstOrDefault(p => p.PostId == id);
			if (post != null)
			{
				post.Upvotes++;
				db.SaveChanges();
				return true;  // Success
			}
			return false;  // Post not found
		}


		public bool DownVotePost(int id)
		{
			Post post = db.Posts.FirstOrDefault(p => p.PostId == id);
			if (post != null)
			{
				post.Upvotes--;
				db.SaveChanges();
				return true;  // Success
			}
			return false;  // Post not found
		}

		public bool UpVoteComment(int postId, long commentId)
		{
			Post post = db.Posts.Include(p => p.Comments).FirstOrDefault(p => p.PostId == postId);
			if (post != null)
			{
				Comment comment = post.Comments.FirstOrDefault(c => c.CommentId == commentId);
				if (comment != null)
				{
					comment.Upvotes++;
					db.SaveChanges();
					return true;
				}
				else { return false; }
			}

			return false;
		}

		public bool DownVoteComment(int postId, long commentId)
		{
			Post post = db.Posts.Include(p => p.Comments).FirstOrDefault(p => p.PostId == postId);
			if (post != null)
			{
				Comment comment = post.Comments.FirstOrDefault(c => c.CommentId == commentId);
				if (comment != null)
				{
					comment.Upvotes--;
					db.SaveChanges();
					return true;
				}
			}

			return false;
		}

		public string AddComment(int postId, Comment comment)
		{
			Post post = db.Posts.FirstOrDefault(p => p.PostId == postId);
			if (post != null)
			{
				post.Comments.Add(comment);
				db.SaveChanges();
				return "Comment added";
			}
			else
			{
				return "Post not found";
			}
		}

		public string AddPost(Post post)
		{
			db.Posts.Add(post);
			db.SaveChanges();
			return "Post added";
		}
	}

	
}
