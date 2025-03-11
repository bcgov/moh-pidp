namespace Pidp.Infrastructure.Queue;

using MassTransit;
using System;

public class BCProviderSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public required string CurrentState { get; set; }
    public Guid PartyId { get; set; }
    public required string UserPrincipalName { get; set; }
    public Guid UserId { get; set; }
    public bool SAEformsEnroled { get; set; }
    public required string Email { get; set; }
}
