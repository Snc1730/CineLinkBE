using CineLinkBE.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Json;
public class CineLinkDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Genre> Genres { get; set; }

    public CineLinkDbContext(DbContextOptions<CineLinkDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Uid = "user1", Name = "John Doe" },
            new User { Id = 2, Uid = "user2", Name = "Jane Smith" }
            // Add more sample users as needed
        );

        modelBuilder.Entity<Post>().HasData(
            new Post { Id = 1, UserId = 1, Title = "Movie Title 1", ImageUrl = "image_url_1", Description = "Description 1", Length = "2 hours", DatePosted = DateTime.Now /* Add other properties */ },
            new Post { Id = 2, UserId = 2, Title = "Movie Title 2", ImageUrl = "image_url_2", Description = "Description 2", Length = "1 hour 30 minutes", DatePosted = DateTime.Now /* Add other properties */ }
            // Add more sample posts as needed
);


        modelBuilder.Entity<Review>().HasData(
            new Review { Id = 1, PostId = 1, UserId = 1, Content = "Review content 1", DatePosted = DateTime.Now, Rating = 4 /* Add other properties */ },
            new Review { Id = 2, PostId = 2, UserId = 2, Content = "Review content 2", DatePosted = DateTime.Now, Rating = 5 /* Add other properties */ }
            // Add more sample reviews as needed
        );


        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "Action", Description = "Action genre description" },
            new Genre { Id = 2, Name = "Comedy", Description = "Comedy genre description" }
            // Add more sample genres as needed
        );

        modelBuilder.Entity<Post>()
            .HasMany(o => o.Genres)
            .WithMany(mi => mi.Posts)
            .UsingEntity(j => j.ToTable("PostGenre"));

        modelBuilder.Entity<User>()
            .HasMany(u => u.Posts)
            .WithMany(p => p.Users)
            .UsingEntity(j => j.ToTable("Watchlist"));

        base.OnModelCreating(modelBuilder);
    }
}
