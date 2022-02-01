// namespace PlrIntake.Features.Records;

// using FluentValidation;
// using Microsoft.EntityFrameworkCore;

// using PlrIntake.Data;

// public class Details
// {
//     public class Query : IQuery<Model?>
//     {
//         public string Ipc { get; set; } = string.Empty;
//     }

//     public class Model
//     {
//         public string? StatusCode { get; set; }
//         public string? StatusReasonCode { get; set; }
//     }

//     public class QueryValidator : AbstractValidator<Query>
//     {
//         public QueryValidator() => this.RuleFor(x => x.Ipc).NotEmpty();
//     }

//     public class QueryHandler : IQueryHandler<Query, Model?>
//     {
//         private readonly PlrDbContext context;

//         public QueryHandler(PlrDbContext context) => this.context = context;

//         public async Task<Model?> HandleAsync(Query query)
//         {
//             return await this.context.PlrRecords
//                 .Where(record => record.Ipc == query.Ipc)
//                 .Select(record => new Model
//                 {
//                     StatusCode = record.StatusCode,
//                     StatusReasonCode = record.StatusReasonCode
//                 })
//                 .SingleOrDefaultAsync();
//         }
//     }
// }
