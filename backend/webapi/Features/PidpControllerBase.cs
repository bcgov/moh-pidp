namespace Pidp.Features;

using DomainResults.Common;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Services;

[Produces("application/json")]
[ApiController]
public class PidpControllerBase : ControllerBase
{
    internal static class Cookies
    {
        public static class CredentialLinkTicket
        {
            public const string Key = "credential-link-ticket";

            public sealed record Values(Guid CredentialLinkToken);
        }
    }

    protected IPidpAuthorizationService AuthorizationService { get; }

    protected PidpControllerBase(IPidpAuthorizationService authService) => this.AuthorizationService = authService;

    /// <summary>
    /// Checks that the given Party both exists and is owned by the current User before executing the handler.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="partyId"></param>
    /// <param name="handler"></param>
    /// <param name="request"></param>
    protected async Task<IDomainResult> AuthorizePartyBeforeHandleAsync<TRequest>(int partyId, IRequestHandler<TRequest, IDomainResult> handler, TRequest request)
    {
        var access = await this.AuthorizationService.CheckPartyAccessibilityAsync(partyId, this.User);
        if (access.IsSuccess)
        {
            return await handler.HandleAsync(request);
        }

        return access;
    }

    /// <summary>
    /// Checks that the given Party both exists and is owned by the current User before executing the handler.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="partyId"></param>
    /// <param name="handler"></param>
    /// <param name="request"></param>
    protected async Task<IDomainResult<TResult>> AuthorizePartyBeforeHandleAsync<TRequest, TResult>(int partyId, IRequestHandler<TRequest, IDomainResult<TResult>> handler, TRequest request)
    {
        var access = await this.AuthorizationService.CheckPartyAccessibilityAsync(partyId, this.User);
        if (access.IsSuccess)
        {
            return await handler.HandleAsync(request);
        }

        return access.To<TResult>();
    }

    /// <summary>
    /// Checks that the given Party both exists and is owned by the current User before executing the handler.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="partyId"></param>
    /// <param name="handler"></param>
    /// <param name="request"></param>
    protected async Task<IDomainResult> AuthorizePartyBeforeHandleAsync<TRequest>(int partyId, IRequestHandler<TRequest> handler, TRequest request)
    {
        var access = await this.AuthorizationService.CheckPartyAccessibilityAsync(partyId, this.User);
        if (access.IsSuccess)
        {
            await handler.HandleAsync(request);
            return DomainResult.Success();
        }

        return access;
    }

    /// <summary>
    /// Checks that the given Party both exists and is owned by the current User before executing the handler.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="partyId"></param>
    /// <param name="handler"></param>
    /// <param name="request"></param>
    protected async Task<IDomainResult<TResult>> AuthorizePartyBeforeHandleAsync<TRequest, TResult>(int partyId, IRequestHandler<TRequest, TResult> handler, TRequest request)
    {
        var access = await this.AuthorizationService.CheckPartyAccessibilityAsync(partyId, this.User);
        if (access.IsSuccess)
        {
            return DomainResult.Success(await handler.HandleAsync(request));
        }

        return access.To<TResult>();
    }
}
