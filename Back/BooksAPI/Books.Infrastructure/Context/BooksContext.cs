﻿using Books.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Books.Infrastructure.Context;

public class BooksContext : DbContext
{
    public BooksContext(DbContextOptions<BooksContext> options) : base(options)
    {
    }
    
    public DbSet<Book> Books { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<BlackListed> BlackListeds { get; set; }
    public DbSet<UserActiveSessions> UserActiveSessions { get; set; }


    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(BooksContext).Assembly);
}