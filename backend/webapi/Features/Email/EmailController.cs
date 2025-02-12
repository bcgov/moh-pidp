namespace Pidp.Features.EmailWithMassTransit;

using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class EmailController(ISendEndpointProvider sendEndpointProvider) : ControllerBase
{
    private readonly ISendEndpointProvider sendEndpointProvider = sendEndpointProvider;

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] SendEmailRequest request)
    {
        var email = new Email(
            from: EmailService.PidpEmail,
            to: new List<string> { request.To },
            cc: new List<string> { request.Cc } ?? [],
            subject: request.Subject,
            body: request.Body,
            attachments: []
        );
        var sendEndpoint = await this.sendEndpointProvider.GetSendEndpoint(new Uri("queue:send-email-queue"));
        await sendEndpoint.Send(email);

        return this.Ok();
    }
}

public class SendEmailRequest
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public string To { get; set; }
    public string Cc { get; set; }
}
