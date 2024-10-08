using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Json;
using RedditAPP.Shared.Models;
using RedditAPI.Service;
using Microsoft.Extensions.Hosting;
using System.Xml.Linq;


var builder = WebApplication.CreateBuilder(args);

// Sætter CORS så API'en kan bruges fra andre domæner
builder.Services.AddCors(options =>
{
    options.AddPolicy("policy",
                      policy =>
                      {
                          policy.AllowAnyOrigin();
                          policy.AllowAnyMethod();
                          policy.AllowAnyHeader();
                      });
});

// Tilføj DbContext factory som service.
builder.Services.AddDbContext<PostContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ContextSQLite")));

builder.Services.AddScoped<DataService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dataService = scope.ServiceProvider.GetRequiredService<DataService>();
    dataService.SeedData(); // Fylder data på, hvis databasen er tom. Ellers ikke.
}

app.UseHttpsRedirection();
app.UseCors("policy");

// Middlware der kører før hver request. Sætter ContentType for alle responses til "JSON".
app.Use(async (context, next) =>
{
    context.Response.ContentType = "application/json; charset=utf-8";
    await next(context);
});


//GET:
app.MapGet("/api/posts", (DataService service) =>
{
    return service.GetPosts();
});
app.MapGet("/api/posts/{id}", (DataService service, int id) =>
{
    return service.GetPost(id);
});

app.MapGet("/api/posts/{postId}/comments/{commentId}", (DataService service, int postId, int commentId) =>
{
    return service.GetComment(postId, commentId);
});

//PUT:
app.MapPut("/api/posts/{id}/upvote", (DataService service, int id) =>
{
    bool success = service.UpVotePost(id);
    if (success)
    {
        return Results.Ok(new { message = "Upvote successful" });
    }
    return Results.NotFound(new { message = "Post not found" });
});

app.MapPut("/api/posts/{id}/downvote", (DataService service, int id) =>
{
    bool success = service.DownVotePost(id);
    if (success)
    {
        return Results.Ok(new { message = "Downvote successful" });
    }
    return Results.NotFound(new { message = "Post not found" });
});

app.MapPut("/api/posts/{postid}/comments/{commentid}/upvote", (DataService service, int postid, long commentid) =>
{
    bool success = service.UpVoteComment(postid, commentid);
    if (success)
    {
        return Results.Ok(new { message = "Upvote successful" });
    }
    return Results.NotFound(new { message = "Post or comment not found" });
});
app.MapPut("/api/posts/{postid}/comments/{commentid}/downvote", (DataService service, int postid, long commentid) =>
{
    bool success = service.DownVoteComment(postid, commentid);
    if (success)
    {
        return Results.Ok(new { message = "Downvote successful" });
    }
    return Results.NotFound(new { message = "Post or comment not found" });
});

//POST:
app.MapPost("/api/posts", (DataService service, Post post) =>
{
    return service.AddPost(post);
});
app.MapPost("/api/posts/{id}/comments", (DataService service, int postId, Comment comment) =>
{
    return service.AddComment(postId, comment);
});

app.Run();



