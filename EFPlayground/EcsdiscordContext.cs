using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFPlayground;

public partial class EcsdiscordContext : DbContext
{
    private string connectionString;
    
    public EcsdiscordContext()
    {
    }

    public EcsdiscordContext(IConfiguration config, DbContextOptions<EcsdiscordContext> options)
        : base(options)
    {
        connectionString = config.GetConnectionString("MariaDbConnectionString");
    }

    public virtual DbSet<Autocreatepattern> Autocreatepatterns { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Coursealias> Coursealiases { get; set; }

    public virtual DbSet<Coursecategory> Coursecategories { get; set; }

    public virtual DbSet<Pendingverification> Pendingverifications { get; set; }

    public virtual DbSet<Servermessage> Servermessages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Verificationhistory> Verificationhistories { get; set; }

    public virtual DbSet<Verificationoverride> Verificationoverrides { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql(connectionString, ServerVersion.Parse("10.6.12-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Autocreatepattern>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("autocreatepatterns");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Pattern)
                .HasMaxLength(32)
                .HasColumnName("pattern");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PRIMARY");

            entity.ToTable("courses");

            entity.HasIndex(e => e.DiscordChannelSnowflake, "discordChannelSnowflake").IsUnique();

            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .HasColumnName("name");
            entity.Property(e => e.DiscordChannelSnowflake)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("discordChannelSnowflake");
        });

        modelBuilder.Entity<Coursealias>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("coursealiases");

            entity.HasIndex(e => e.Name, "name_UNIQUE").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Hidden).HasColumnName("hidden");
            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .HasColumnName("name");
            entity.Property(e => e.Target)
                .HasMaxLength(32)
                .HasColumnName("target");
        });

        modelBuilder.Entity<Coursecategory>(entity =>
        {
            entity.HasKey(e => e.DiscordSnowflake).HasName("PRIMARY");

            entity.ToTable("coursecategories");

            entity.Property(e => e.DiscordSnowflake)
                .ValueGeneratedNever()
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("discordSnowflake");
            entity.Property(e => e.AutoImportPattern)
                .HasMaxLength(256)
                .HasColumnName("autoImportPattern");
            entity.Property(e => e.AutoImportPriority)
                .HasDefaultValueSql("-1")
                .HasColumnType("int(11)")
                .HasColumnName("autoImportPriority");
        });

        modelBuilder.Entity<Pendingverification>(entity =>
        {
            entity.HasKey(e => e.Token).HasName("PRIMARY");

            entity.ToTable("pendingverifications");

            entity.HasIndex(e => e.DiscordSnowflake, "verificationDiscordSnowflake");

            entity.Property(e => e.Token)
                .HasMaxLength(32)
                .HasColumnName("token");
            entity.Property(e => e.CreationTime)
                .HasColumnType("bigint(20)")
                .HasColumnName("creationTime");
            entity.Property(e => e.DiscordSnowflake)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("discordSnowflake");
            entity.Property(e => e.EncryptedUsername)
                .HasMaxLength(5000)
                .HasColumnName("encryptedUsername");
        });

        modelBuilder.Entity<Servermessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PRIMARY");

            entity.ToTable("servermessages");

            entity.Property(e => e.MessageId)
                .ValueGeneratedNever()
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("messageID");
            entity.Property(e => e.ChannelId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("channelID");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.Created)
                .HasColumnType("bigint(20)")
                .HasColumnName("created");
            entity.Property(e => e.Creator)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("creator");
            entity.Property(e => e.Editor)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("editor");
            entity.Property(e => e.LastEdited)
                .HasColumnType("bigint(20)")
                .HasColumnName("lastEdited");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.DiscordSnowflake).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.DiscordSnowflake)
                .ValueGeneratedNever()
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("discordSnowflake");
            entity.Property(e => e.DisallowCourseJoin)
                .HasColumnType("tinyint(4)")
                .HasColumnName("disallowCourseJoin");
            entity.Property(e => e.EncryptedUsername)
                .HasMaxLength(5000)
                .HasColumnName("encryptedUsername");

            entity.HasMany(d => d.CourseNames).WithMany(p => p.UserDiscordSnowflakes)
                .UsingEntity<Dictionary<string, object>>(
                    "Usercourse",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseName")
                        .HasConstraintName("userCourseCourseName"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserDiscordSnowflake")
                        .HasConstraintName("userCourseDiscordSnowflake"),
                    j =>
                    {
                        j.HasKey("UserDiscordSnowflake", "CourseName")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("usercourses");
                        j.HasIndex(new[] { "CourseName" }, "userCourseCourseName");
                        j.IndexerProperty<ulong>("UserDiscordSnowflake")
                            .HasColumnType("bigint(20) unsigned")
                            .HasColumnName("userDiscordSnowflake");
                        j.IndexerProperty<string>("CourseName")
                            .HasMaxLength(32)
                            .HasColumnName("courseName");
                    });
        });

        modelBuilder.Entity<Verificationhistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("verificationhistory");

            entity.HasIndex(e => e.DiscordSnowflake, "discordIdIndex");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.DiscordSnowflake)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("discordSnowflake");
            entity.Property(e => e.EncryptedUsername)
                .HasMaxLength(5000)
                .HasColumnName("encryptedUsername");
            entity.Property(e => e.VerificationTime)
                .HasColumnType("bigint(20)")
                .HasColumnName("verificationTime");
        });

        modelBuilder.Entity<Verificationoverride>(entity =>
        {
            entity.HasKey(e => e.DiscordSnowflake).HasName("PRIMARY");

            entity.ToTable("verificationoverrides");

            entity.HasIndex(e => e.ObjectType, "TYPE");

            entity.Property(e => e.DiscordSnowflake)
                .ValueGeneratedNever()
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("discordSnowflake");
            entity.Property(e => e.ObjectType)
                .HasColumnType("enum('ROLE','USER')")
                .HasColumnName("objectType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
