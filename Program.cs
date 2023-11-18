using CineLinkBE;
using CineLinkBE.Models;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<CineLinkDbContext>(builder.Configuration["CineLinkDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// User Endpoints
// Check if user exists
app.MapGet("/checkuser/{uid}", (CineLinkDbContext db, string uid) =>
{
    var user = db.Users.Where(x => x.Uid == uid).ToList();
    if (uid == null)
    {
        return Results.NotFound();
    }
    else
    {
        return Results.Ok(user);
    }
});

// Create User
app.MapPost("/api/user", (CineLinkDbContext db, User user) =>
{
    db.Users.Add(user);
    db.SaveChanges();
    return Results.Created($"/api/user/{user.Id}", user);
});

// Get single User by id
app.MapGet("/api/user/{id}", (CineLinkDbContext db, int id) =>
{
    var user = db.Users.Single(u => u.Id == id);
    return user;
});

// Review Endpoints
// Check if Review exists
app.MapGet("/checkreview/{id}", (CineLinkDbContext db, int id) =>
{
    var review = db.Reviews.Find(id);
    if (review == null)
    {
        return Results.NotFound();
    }
    else
    {
        return Results.Ok(review);
    }
});

// Delete Review by id
app.MapDelete("/api/review/{id}", (CineLinkDbContext db, int id) =>
{
    var review = db.Reviews.Find(id);
    if (review == null)
    {
        return Results.NotFound();
    }

    db.Reviews.Remove(review);
    db.SaveChanges();
    return Results.NoContent();
});

// Update Review by id
app.MapPut("/api/review/{id}", (CineLinkDbContext db, int id, Review updatedReview) =>
{
    var existingReview = db.Reviews.Find(id);
    if (existingReview == null)
    {
        return Results.NotFound();
    }

    // Update the review properties
    // Assuming properties like Content, Rating, etc., need to be updated

    db.SaveChanges();
    return Results.Ok();
});

// Get Reviews by PostId
app.MapGet("/api/post/{postId}/reviews", (CineLinkDbContext db, int postId) =>
{
    var reviews = db.Reviews.Where(r => r.PostId == postId).ToList();
    return Results.Ok(reviews);
});

// Create Review
app.MapPost("/api/review", (CineLinkDbContext db, Review review) =>
{
    db.Reviews.Add(review);
    db.SaveChanges();
    return Results.Created($"/api/review/{review.Id}", review);
});

// Post Endpoints
// Delete Post by id
app.MapDelete("/api/post/{id}", (CineLinkDbContext db, int id) =>
{
    var post = db.Posts.Find(id);
    if (post == null)
    {
        return Results.NotFound();
    }

    db.Posts.Remove(post);
    db.SaveChanges();
    return Results.NoContent();
});

// Update Post by id
app.MapPut("/api/post/{id}", (CineLinkDbContext db, int id, Post updatedPost) =>
{
    var existingPost = db.Posts.Find(id);
    if (existingPost == null)
    {
        return Results.NotFound();
    }

    // Update the post properties
    // Assuming properties like Title, ImageUrl, Description, etc., need to be updated

    db.SaveChanges();
    return Results.Ok();
});

// Get All Posts
app.MapGet("/api/posts", (CineLinkDbContext db) =>
{
    var posts = db.Posts.OrderBy(p => p.Id).ToList();
    return Results.Ok(posts);
});

// Get Post by id
app.MapGet("/api/post/{id}", (CineLinkDbContext db, int id) =>
{
    var post = db.Posts.SingleOrDefault(p => p.Id == id);
    if (post == null)
    {
        return Results.NotFound();
    }
    else
    {
        return Results.Ok(post);
    }
});

// Create Post
app.MapPost("/api/post", (CineLinkDbContext db, Post post) =>
{
    db.Posts.Add(post);
    db.SaveChanges();
    return Results.Created($"/api/post/{post.Id}", post);
});

// Join Table Endpoints
// Associate Genre with Post
app.MapPost("/api/PostGenre", (int postId, int genreId, CineLinkDbContext db) =>
{
    var post = db.Posts.Include(p => p.Genres).FirstOrDefault(p => p.Id == postId);

    if (post == null)
    {
        return Results.NotFound("Post not found.");
    }

    var genre = db.Genres.FirstOrDefault(g => g.Id == genreId);

    if (genre == null)
    {
        return Results.NotFound("Genre not found.");
    }

    post.Genres.Add(genre);
    db.SaveChanges();

    return Results.Ok("Genre associated with post successfully.");
});

// Dissociate Genre from Post
app.MapDelete("/api/PostGenre", (int postId, int genreId, CineLinkDbContext db) =>
{
    var post = db.Posts.Include(p => p.Genres).FirstOrDefault(p => p.Id == postId);

    if (post == null)
    {
        return Results.NotFound("Post not found.");
    }

    var genre = db.Genres.FirstOrDefault(g => g.Id == genreId);

    if (genre == null)
    {
        return Results.NotFound("Genre not found.");
    }

    post.Genres.Remove(genre);
    db.SaveChanges();

    return Results.Ok("Genre dissociated from post successfully.");
});

// Add Film to Watchlist
app.MapPost("/api/Watchlist", (int userId, int postId, CineLinkDbContext db) =>
{
    var user = db.Users.Include(u => u.Posts).FirstOrDefault(u => u.Id == userId);

    if (user == null)
    {
        return Results.NotFound("User not found.");
    }

    var post = db.Posts.FirstOrDefault(p => p.Id == postId);

    if (post == null)
    {
        return Results.NotFound("Post not found.");
    }

    user.Posts.Add(post);
    db.SaveChanges();

    return Results.Ok("Post added to watchlist successfully.");
});

// Remove Film from Watchlist
app.MapDelete("/api/Watchlist", (int userId, int postId, CineLinkDbContext db) =>
{
    var user = db.Users.Include(u => u.Posts).FirstOrDefault(u => u.Id == userId);

    if (user == null)
    {
        return Results.NotFound("User not found.");
    }

    var post = db.Posts.FirstOrDefault(p => p.Id == postId);

    if (post == null)
    {
        return Results.NotFound("Post not found.");
    }

    user.Posts.Remove(post);
    db.SaveChanges();

    return Results.Ok("Post removed from watchlist successfully.");
});

app.Run();
