namespace Pidp.Data;

using Mediator;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Models;

public class PidpDbContext(
    DbContextOptions<PidpDbContext> options,
    ILogger<PidpDbContext> logger,
    IClock clock,
    IMediator mediator) : DbContext(options)
{
    private readonly ILogger<PidpDbContext> logger = logger;
    private readonly IClock clock = clock;
    private readonly IMediator mediator = mediator;

    public DbSet<AccessRequest> AccessRequests { get; set; } = default!;
    public DbSet<Banner> Banners { get; set; } = default!;
    public DbSet<BusinessEvent> BusinessEvents { get; set; } = default!;
    public DbSet<ClientLog> ClientLogs { get; set; } = default!;
    public DbSet<Credential> Credentials { get; set; } = default!;
    public DbSet<CredentialLinkErrorLog> CredentialLinkErrorLogs { get; set; } = default!;
    public DbSet<CredentialLinkTicket> CredentialLinkTickets { get; set; } = default!;
    public DbSet<EmailLog> EmailLogs { get; set; } = default!;
    public DbSet<EndorsementRelationship> EndorsementRelationships { get; set; } = default!;
    public DbSet<EndorsementRequest> EndorsementRequests { get; set; } = default!;
    public DbSet<Endorsement> Endorsements { get; set; } = default!;
    public DbSet<FeedbackLog> FeedbackLogs { get; set; } = default!;
    public DbSet<HcimAccountTransfer> HcimAccountTransfers { get; set; } = default!;
    public DbSet<InvitedEntraAccount> InvitedEntraAccounts { get; set; } = default!;
    public DbSet<MSTeamsClinic> MSTeamsClinics { get; set; } = default!;
    public DbSet<MSTeamsClinicMemberEnrolment> MSTeamsClinicMemberEnrolments { get; set; } = default!;
    public DbSet<PartyLicenceDeclaration> PartyLicenceDeclarations { get; set; } = default!;
    public DbSet<Party> Parties { get; set; } = default!;
    public DbSet<Pharmacy> Pharmacies { get; set; } = default!;
    public DbSet<PharmacyPartyRole> PharmacyPartyRoles { get; set; } = default!;
    public DbSet<PharmacyEnrolment> PharmacyEnrolments { get; set; } = default!;
    public DbSet<VerifiedEmail> VerifiedEmails { get; set; } = default!;

    /// <summary>
    /// Do not use. Use SaveChangesAsync Instead.
    /// </summary>
    public override int SaveChanges() => this.SaveChangesAsync().GetAwaiter().GetResult();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await this.DispatchDomainEventsAsync();
        this.ApplyAudits();
        this.LogDatabaseChanges();

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PidpDbContext).Assembly);
    }

    private async Task DispatchDomainEventsAsync()
    {
        var eventEntities = this.ChangeTracker.Entries<BaseEntity>()
            .Select(x => x.Entity)
            .Where(entity => entity.DomainEvents.Count != 0);

        foreach (var entity in eventEntities)
        {
            var events = entity.DomainEvents.ToArray();
            entity.DomainEvents.Clear();

            foreach (var domainEvent in events)
            {
                await this.mediator.Publish(domainEvent);
            }
        }
    }

    private void ApplyAudits()
    {
        var updated = this.ChangeTracker.Entries<BaseAuditable>()
            .Where(x => x.State is EntityState.Added or EntityState.Modified);

        var currentInstant = this.clock.GetCurrentInstant();

        foreach (var entry in updated)
        {
            entry.CurrentValues[nameof(BaseAuditable.Modified)] = currentInstant;

            if (entry.State == EntityState.Added)
            {
                entry.CurrentValues[nameof(BaseAuditable.Created)] = currentInstant;
            }
            else
            {
                entry.Property(nameof(BaseAuditable.Created)).IsModified = false;
            }
        }
    }

    private void LogDatabaseChanges()
    {
        var changes = this.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

        foreach (var entry in changes)
        {
            var entityName = entry.Entity.GetType().Name;
            var state = entry.State.ToString();
            
            string keyInfo = "";
            var keyName = entry.Metadata.FindPrimaryKey()?.Properties.Select(x => x.Name).FirstOrDefault();
            if (keyName != null)
            {
                var keyValue = entry.Property(keyName).CurrentValue;
                keyInfo = $" (Key: {keyName} = {keyValue})";
            }

            this.logger.LogDatabaseChange(entityName, keyInfo, state);
        }
    }

    // Uncomment for SQL logging
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.LogTo(Console.WriteLine);
}

public static partial class PidpDbContextLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Database Change: {entityName}{keyInfo} was {state}")]
    public static partial void LogDatabaseChange(this ILogger<PidpDbContext> logger, string entityName, string keyInfo, string state);
}
