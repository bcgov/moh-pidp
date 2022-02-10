namespace Pidp.Features;

using DomainResults.Common;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Services;
using Pidp.Models;

[Produces("application/json")]
[ApiController]
public class PidpControllerBase : ControllerBase
{
    protected IPidpAuthorizationService AuthorizationService { get; }

    protected PidpControllerBase(IPidpAuthorizationService authService) => this.AuthorizationService = authService;

    /// <summary>
    /// Checks that the given Party both exists and is owned by the current User before exicuting the handler.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="partyId"></param>
    /// <param name="handler"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    protected async Task<IDomainResult> AuthorizePartyThen<TRequest>(int partyId, IRequestHandler<TRequest, IDomainResult> handler, TRequest request)
    {
        var access = await this.AuthorizationService.CheckResourceAccessibility((Party p) => p.Id == partyId, this.User);
        if (access.IsSuccess)
        {
            return await handler.HandleAsync(request);
        }

        return access;
    }

    /// <summary>
    /// Checks that the given Party both exists and is owned by the current User before exicuting the handler.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="partyId"></param>
    /// <param name="handler"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    protected async Task<IDomainResult<TResult>> AuthorizePartyThen<TRequest, TResult>(int partyId, IRequestHandler<TRequest, IDomainResult<TResult>> handler, TRequest request)
    {
        var access = await this.AuthorizationService.CheckResourceAccessibility((Party p) => p.Id == partyId, this.User);
        if (access.IsSuccess)
        {
            return await handler.HandleAsync(request);
        }

        return access.To<TResult>();
    }
}
