using Microsoft.EntityFrameworkCore;
using SL.Domain.Entities;

namespace SL.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Link> Links { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}