
using Microsoft.EntityFrameworkCore;
using Moderation.Model;

namespace Moderation.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options){}
    
    public DbSet<ModerationLog> ModerationLogs { get; set; }
}