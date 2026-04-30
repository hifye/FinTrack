using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Context;

public class FinTrackContext(DbContextOptions<FinTrackContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
