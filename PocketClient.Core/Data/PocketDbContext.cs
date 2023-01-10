using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PocketClient.Core.Models;

namespace PocketClient.Core.Data;

public class PocketDbContext : DbContext
{
    public PocketDbContext(DbContextOptions<PocketDbContext> options) : base(options)
    {

    }

    public DbSet<PocketItem> Items
    {
        get; set;
    }

    public DbSet<Author> Authors
    {
        get; set;
    }

    public DbSet<Tag> Tags
    {
        get; set;
    }

    public DbSet<ItemAuthor> ItemAuthors
    {
        get; set;
    }

    public DbSet<ItemTag> ItemTags
    {
        get; set;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.LogTo(message => Debug.WriteLine(message));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PocketItem>().ToTable("Items").HasKey(entity => entity.Id);

        modelBuilder.Entity<Author>().ToTable("Authors").HasKey(entity => entity.Id);

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tags");
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<PocketItem>()
            .HasMany(entity => entity.Authors)
            .WithMany(author => author.Items)
            .UsingEntity<ItemAuthor>(
                item => item
                    .HasOne(pt => pt.Author)
                    .WithMany(p => p.ItemAuthors)
                    .HasForeignKey(pt => pt.AuthorId),
                item => item
                    .HasOne(pt => pt.Item)
                    .WithMany(t => t.ItemAuthors)
                    .HasForeignKey(pt => pt.ItemId),
                item => item.ToTable("ItemAuthors").HasKey(t => new { t.ItemId, t.AuthorId }));

        modelBuilder.Entity<PocketItem>()
            .HasMany(entity => entity.Tags)
            .WithMany(tag => tag.Items)
            .UsingEntity<ItemTag>(
                item => item
                    .HasOne(pt => pt.Tag)
                    .WithMany(p => p.ItemTags)
                    .HasForeignKey(pt => pt.TagId),
                item => item
                    .HasOne(pt => pt.Item)
                    .WithMany(t => t.ItemTags)
                    .HasForeignKey(pt => pt.ItemId),
                item => item.ToTable("ItemTags").HasKey(t => new { t.ItemId, t.TagId }));
    }
}
