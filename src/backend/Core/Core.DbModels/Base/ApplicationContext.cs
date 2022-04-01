﻿using Microsoft.EntityFrameworkCore;

namespace Core.DbModels.Base;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options) : base(options){}
    
    public DbSet<Dialog> Dialogs { get; set; }
    public DbSet<DialogRequest> DialogRequests { get; set; }
    public DbSet<DialogSecret> DialogSecrets { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRoles> UserRoles { get; set; }
    public DbSet<Connection> Connections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Dialog>();
        modelBuilder.Entity<DialogRequest>();
        modelBuilder.Entity<DialogSecret>();
        modelBuilder.Entity<Message>();
        modelBuilder.Entity<RefreshToken>();
        modelBuilder.Entity<Role>();
        modelBuilder.Entity<Session>();
        modelBuilder.Entity<User>();
        modelBuilder.Entity<UserRoles>();
        modelBuilder.Entity<Connection>();

        modelBuilder.Entity<Dialog>().HasKey(x => x.Id);
        modelBuilder.Entity<DialogRequest>().HasKey(x => x.Id);
        modelBuilder.Entity<DialogSecret>().HasKey(x => x.Id);
        modelBuilder.Entity<Message>().HasKey(x => x.Id);
        modelBuilder.Entity<RefreshToken>().HasKey(x => x.Id);
        modelBuilder.Entity<Role>().HasKey(x => x.Id);
        modelBuilder.Entity<Session>().HasKey(x => x.Id);
        modelBuilder.Entity<User>().HasKey(x => x.Id);
        modelBuilder.Entity<UserRoles>().HasKey(x => x.Id);
        modelBuilder.Entity<Connection>().HasKey(x => x.Id);
        
        modelBuilder.Entity<Dialog>().HasIndex(x => x.User1Id);
        modelBuilder.Entity<Dialog>().HasIndex(x => x.User2Id);

        modelBuilder.Entity<DialogRequest>().HasIndex(x => new {x.OwnerUserId, x.RequestUserId});

        modelBuilder.Entity<Message>().HasIndex(x => x.AnswerMessageId);
        
        modelBuilder.Entity<DialogSecret>()
            .HasOne(x => x.Dialog);
        modelBuilder.Entity<RefreshToken>()
            .HasOne(x => x.User);
        modelBuilder.Entity<Session>()
            .HasOne(x => x.User);
        modelBuilder.Entity<UserRoles>()
            .HasOne(x => x.User);
        modelBuilder.Entity<UserRoles>()
            .HasOne(x => x.Role);
        modelBuilder.Entity<Connection>()
            .HasOne(x => x.Session);
        modelBuilder.Entity<Connection>()
            .HasOne(x => x.User)
            .WithMany(x => x.Connections)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Dialog>()
            .Property(x => x.Created)
            .HasComputedColumnSql("GETDATE()");
        modelBuilder.Entity<DialogRequest>()
            .Property(x => x.CreatedAt)
            .HasComputedColumnSql("GETDATE()");
        modelBuilder.Entity<DialogSecret>()
            .Property(x => x.CreatedAt)
            .HasComputedColumnSql("GETDATE()");
        modelBuilder.Entity<Message>()
            .Property(x => x.CreatedAt)
            .HasComputedColumnSql("GETDATE()");
        modelBuilder.Entity<RefreshToken>()
            .Property(x => x.CreatedAt)
            .HasComputedColumnSql("GETDATE()");
        modelBuilder.Entity<Session>()
            .Property(x => x.CreatedAt)
            .HasComputedColumnSql("GETDATE()");
        modelBuilder.Entity<User>()
            .Property(x => x.CreatedAt)
            .HasComputedColumnSql("GETDATE()");
        modelBuilder.Entity<Connection>()
            .Property(x => x.CreatedAt)
            .HasComputedColumnSql("GETDATE()");
    }
}