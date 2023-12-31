using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SL.Application.Common.Interfaces;
using SL.Domain.Entities;

namespace SL.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }

    public DbSet<Link> Links => Set<Link>();
    public DbSet<LinkVisit> LinkVisits => Set<LinkVisit>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}