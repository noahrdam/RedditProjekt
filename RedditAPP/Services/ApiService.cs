using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RedditAPP.Shared.Models;
using System;
using System.Threading.Tasks;

namespace RedditAPP.Data
{
    public class ApiService
    {
        private readonly HttpClient http;
        private readonly IConfiguration configuration;
        private readonly string baseAPI = "";

        // Event to notify components that a refresh is required
        public event Action RefreshRequired;

        public ApiService(HttpClient http, IConfiguration configuration)
        {
            this.http = http;
            this.configuration = configuration;
            this.baseAPI = configuration["base_api"];
        }

        public async Task<bool> CreatePost(Post newpost)
        {
            string url = $"{baseAPI}posts/";
            var response = await http.PostAsJsonAsync(url, newpost);

            return response.IsSuccessStatusCode;
        }
        public async Task<Post[]> GetPosts()
        {
            string url = $"{baseAPI}posts/";
            return await http.GetFromJsonAsync<Post[]>(url);
        }

        public async Task<Post> GetPost(int id)
        {
            string url = $"{baseAPI}posts/{id}/";
            return await http.GetFromJsonAsync<Post>(url);
        }


        public async Task<Comment> CreateComment(string content, int postId, string author)
        {
            string url = $"{baseAPI}posts/{postId}/comments";

            // Post JSON to API, save the HttpResponseMessage
            HttpResponseMessage msg = await http.PostAsJsonAsync(url, new { content, author });

            // Get the JSON string from the response
            string json = msg.Content.ReadAsStringAsync().Result;

            Console.WriteLine(json);

            // Deserialize the JSON string to a Comment object
            Comment? newComment = JsonSerializer.Deserialize<Comment>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties 
            });

            Console.WriteLine(newComment);

            // Trigger the refresh event
            OnRefreshRequired();

            // Return the new comment 
            return newComment;
        }

        public async Task<Post> UpvotePost(int id)
        {
            string url = $"{baseAPI}posts/{id}/upvote/";

            // Post JSON to API, save the HttpResponseMessage
            HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");

            // Get the JSON string from the response
            string json = msg.Content.ReadAsStringAsync().Result;

            // Deserialize the JSON string to a Post object
            Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
            });

            // Trigger the refresh event
            OnRefreshRequired();

            // Return the updated post (vote increased)
            return updatedPost;
        }

        public async Task<Post> DownvotePost(int id)
        {
            string url = $"{baseAPI}posts/{id}/downvote/";

            // Post JSON to API, save the HttpResponseMessage
            HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");

            // Get the JSON string from the response
            string json = msg.Content.ReadAsStringAsync().Result;

            // Deserialize the JSON string to a Post object
            Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
            });

            // Trigger the refresh event
            OnRefreshRequired();

            // Return the updated post (vote increased)
            return updatedPost;
        }

        public async Task<Post> UpvoteComment(int postId, long commentId)
        {
            string url = $"{baseAPI}posts/{postId}/comments/{commentId}/upvote";

            // Post JSON to API, save the HttpResponseMessage
            HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");

            // Get the JSON string from the response
            string json = msg.Content.ReadAsStringAsync().Result;

            // Deserialize the JSON string to a Post object
            Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
            });

            // Trigger the refresh event
            OnRefreshRequired();

            // Return the updated post (vote increased)
            return updatedPost;
        }


        public async Task<Post> DownvoteComment(int postId, long commentId)
        {
            string url = $"{baseAPI}posts/{postId}/comments/{commentId}/downvote";

            // Post JSON to API, save the HttpResponseMessage
            HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");

            // Get the JSON string from the response
            string json = msg.Content.ReadAsStringAsync().Result;

            // Deserialize the JSON string to a Post object
            Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties
            });

            // Trigger the refresh event
            OnRefreshRequired();

            // Return the updated post (vote increased)
            return updatedPost;
        }

        // Method to invoke the RefreshRequired event
        protected virtual void OnRefreshRequired()
        {
            RefreshRequired?.Invoke();
        }
    }
}
