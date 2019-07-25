using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dias.tracker.api.Data.Tables;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace dias.tracker.api.Data {
  public class TrackerContext : ApiAuthorizationDbContext<ApplicationUser> {
    public TrackerContext(
      DbContextOptions<TrackerContext> options,
      IOptions<OperationalStoreOptions> operationalStoreOptions
    ) : base (options, operationalStoreOptions) { }

    public DbSet<Hamborg>? HamborgText { get; set; }
    public DbSet<Player>? Players { get; set; }
    public DbSet<PlayerData>? PlayerData { get; set; }
    public DbSet<Poll>? Polls { get; set; }
    public DbSet<XivJob>? XivJobs { get; set; }

    public string user { get; set; } = "anonymous";

    public override int SaveChanges() {
      var changeSet = ChangeTracker.Entries<Common>();

      if (changeSet != null) {
        foreach (var entry in changeSet.Where(x => x.State != EntityState.Unchanged)) {
          if (entry.State == EntityState.Added) {
            entry.Entity.created = DateTime.Now;
            entry.Entity.createdBy = user;
          }

          entry.Entity.modified = DateTime.Now;
          entry.Entity.modifiedBy = user;
        }
      }

      return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
      var changeSet = ChangeTracker.Entries<Common>();

      if (changeSet != null) {
        foreach (var entry in changeSet.Where(x => x.State != EntityState.Unchanged)) {
          if (entry.State == EntityState.Added) {
            entry.Entity.created = DateTime.Now;
            entry.Entity.createdBy = user;
          }

          entry.Entity.modified = DateTime.Now;
          entry.Entity.modifiedBy = user;
        }
      }

      return base.SaveChangesAsync(cancellationToken);
    }
  }
}
