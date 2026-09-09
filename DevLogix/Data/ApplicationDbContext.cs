namespace DevLogix.Data;

using DevLogix.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ProjectMember> ProjectMembers { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<TimeLog> TimeLogs { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Attachment> Attachments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Project -> Ticket: Cascade
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tickets)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Project -> ProjectMember: Cascade
        modelBuilder.Entity<ProjectMember>()
            .HasOne(pm => pm.Project)
            .WithMany(p => p.Members)
            .HasForeignKey(pm => pm.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ticket -> Comment: Cascade
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Ticket)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ticket -> TimeLog: Cascade
        modelBuilder.Entity<TimeLog>()
            .HasOne(tl => tl.Ticket)
            .WithMany(t => t.TimeLogs)
            .HasForeignKey(tl => tl.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ticket -> Attachment: Cascade
        modelBuilder.Entity<Attachment>()
            .HasOne(a => a.Ticket)
            .WithMany(t => t.Attachments)
            .HasForeignKey(a => a.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ticket -> Notification (via RelatedTicketId, nullable): SetNull
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.RelatedTicket)
            .WithMany(t => t.Notifications)
            .HasForeignKey(n => n.RelatedTicketId)
            .OnDelete(DeleteBehavior.SetNull);

        // ApplicationUser -> Ticket (CreatedBy via CreatedById): Restrict
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.CreatedBy)
            .WithMany(u => u.CreatedTickets)
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        // ApplicationUser -> Ticket (AssignedTo via AssignedToId, nullable): Restrict
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.AssignedTo)
            .WithMany(u => u.AssignedTickets)
            .HasForeignKey(t => t.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);

        // ApplicationUser -> Comment (via UserId): Restrict
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ApplicationUser -> TimeLog (via UserId): Restrict
        modelBuilder.Entity<TimeLog>()
            .HasOne(tl => tl.User)
            .WithMany(u => u.TimeLogs)
            .HasForeignKey(tl => tl.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ApplicationUser -> Notification (via RecipientUserId): Restrict
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.RecipientUser)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.RecipientUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ApplicationUser -> Attachment (via UploadedById): Restrict
        modelBuilder.Entity<Attachment>()
            .HasOne(a => a.UploadedBy)
            .WithMany(u => u.UploadedAttachments)
            .HasForeignKey(a => a.UploadedById)
            .OnDelete(DeleteBehavior.Restrict);

        // ApplicationUser -> ProjectMember (via UserId): Restrict
        modelBuilder.Entity<ProjectMember>()
            .HasOne(pm => pm.User)
            .WithMany(u => u.ProjectMemberships)
            .HasForeignKey(pm => pm.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique index on ProjectMember for (ProjectId, UserId)
        modelBuilder.Entity<ProjectMember>()
            .HasIndex(pm => new { pm.ProjectId, pm.UserId })
            .IsUnique();

        SeedRoles(modelBuilder);
    }
}
