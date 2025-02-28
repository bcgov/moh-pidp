namespace Pidp.Infrastructure.Queue;

using MassTransit;
using System;

public class BCProviderSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid PartyId { get; set; }
    public string UserPrincipalName { get; set; }
    public Guid UserId { get; set; }
    public bool SAEformsEnroled { get; set; }
    public string Email { get; set; }
}
