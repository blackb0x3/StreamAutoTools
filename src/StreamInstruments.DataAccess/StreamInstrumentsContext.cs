using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StreamInstruments.DataAccess.Services;
using StreamInstruments.DataObjects;

namespace StreamInstruments.DataAccess;

public class StreamInstrumentsContext : DbContext
{
    public StreamInstrumentsContext(DbContextOptions<StreamInstrumentsContext> options) : base(options)
    {
    }

    public DbSet<Rule> Rules { get; set; } = null!;

    public DbSet<RuleAction> RuleActions { get; set; } = null!;

    public DbSet<Command> Commands { get; set; } = null!;

    public DbSet<Variable> Variables { get; set; } = null!;

    public DbSet<Reward> Rewards { get; set; } = null!;

    public override int SaveChanges()
    {
        PreSaveChanges();

        return base.SaveChanges();
    }

    // ReSharper disable once OptionalParameterHierarchyMismatch
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        PreSaveChanges();

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SetupRulesTable(modelBuilder);
        SetupRuleActionsTable(modelBuilder);
        SetupCommandsTable(modelBuilder);
        SetupVariablesTable(modelBuilder);
        SetupRewardsTable(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private void SetupRulesTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rule>().ToTable("Rules").HasKey(r => r.Id);
        modelBuilder.Entity<Rule>().Property(s => s.Id).HasValueGenerator<PrimaryKeyGenerator<Rule>>();

        modelBuilder.Entity<Rule>()
            .Property(r => r.Event)
            .HasConversion<string>();

        modelBuilder.Entity<Rule>()
            .HasMany(r => r.Actions)
            .WithOne(ra => ra.Rule)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void SetupRuleActionsTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RuleAction>().ToTable("RuleActions").HasKey(ra => ra.Id);
        modelBuilder.Entity<RuleAction>().Property(s => s.Id).HasValueGenerator<PrimaryKeyGenerator<RuleAction>>();

        modelBuilder.Entity<RuleAction>()
            .Property(ra => ra.Type)
            .HasConversion<string>();
    }

    private void SetupCommandsTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Command>().ToTable("Commands").HasKey(cmd => cmd.Id);
        modelBuilder.Entity<Command>().Property(s => s.Id).HasValueGenerator<PrimaryKeyGenerator<Command>>();

        modelBuilder.Entity<Command>()
            .Property(cmd => cmd.AccessLevel)
            .HasConversion<string>();
        modelBuilder.Entity<Command>()
            .Property(cmd => cmd.ResponseDestination)
            .HasConversion<string>();
    }

    private void SetupVariablesTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Variable>().ToTable("Variables").HasKey(v => v.Id);
        modelBuilder.Entity<Variable>().Property(s => s.Id).HasValueGenerator<PrimaryKeyGenerator<Variable>>();

        modelBuilder.Entity<Variable>()
            .Property(v => v.Type)
            .HasConversion<string>();
    }

    private void SetupRewardsTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reward>().ToTable("Rewards").HasKey(rwd => rwd.Id);
        modelBuilder.Entity<Reward>().Property(s => s.Id).HasValueGenerator<PrimaryKeyGenerator<Reward>>();
    }

    private void PreSaveChanges()
    {
        var modifiedEntries = ChangeTracker.Entries().Where(e => e.Entity is EntityBase && e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in modifiedEntries)
        {
            UpdateTimeStamps(entry);
        }
    }

    private void UpdateTimeStamps(EntityEntry entry)
    {
        var utcDtn = DateTime.UtcNow;

        if (entry.State == EntityState.Added)
        {
            ((EntityBase)entry.Entity).CreatedOn = utcDtn;
        }

        ((EntityBase)entry.Entity).LastUpdatedOn = utcDtn;
    }
}