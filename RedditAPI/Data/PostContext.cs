using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RedditAPP.Shared.Models
{
    public class PostContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
		public PostContext(DbContextOptions<PostContext> options)
		   : base(options)
		{
			// Den her er tom. Men ": base(options)" sikre at constructor
			// på DbContext super-klassen bliver kaldt.
		}
	}
}