// namespace PlrIntake.Features.Records;

// using FluentValidation;
// using Microsoft.EntityFrameworkCore;

// using PlrIntake.Data;

// public class Index
// {
//     public class Query : IQuery<List<Model>>
//     {
//         public string CollegeId { get; set; } = string.Empty;
//         public DateTime Birthdate { get; set; }
//     }

//     public class Model
//     {
//         public string Ipc { get; set; } = string.Empty;
//         public string? IdentifierType { get; set; }
//     }

//     public class QueryValidator : AbstractValidator<Query>
//     {
//         public QueryValidator()
//         {
//             this.RuleFor(x => x.CollegeId).NotEmpty();
//             this.RuleFor(x => x.Birthdate).NotEmpty();
//         }
//     }

//     public class QueryHandler : IQueryHandler<Query, List<Model>>
//     {
//         private readonly PlrDbContext context;

//         public QueryHandler(PlrDbContext context) => this.context = context;

//         public async Task<List<Model>> HandleAsync(Query query)
//         {
//             return await this.context.PlrRecords
//                 .Where(record => record.CollegeId == query.CollegeId
//                     && record.DateOfBirth!.Value.Date == query.Birthdate.Date)
//                 .Select(record => new Model
//                 {
//                     Ipc = record.Ipc,
//                     IdentifierType = record.IdentifierType
//                 })
//                 .ToListAsync();
//         }
//     }
// }
