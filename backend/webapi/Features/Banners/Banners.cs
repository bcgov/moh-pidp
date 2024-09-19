namespace Pidp.Features.Banners;

using Microsoft.EntityFrameworkCore;
using NodaTime;
using Pidp.Data;

public class Banners
{
    public class Query : IQuery<List<Model>>
    {
        public string Component { get; set; } = string.Empty;
    }

    public class Model
    {
        public int Id { get; set; }
        public string Component { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Header { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public Instant StartTime { get; set; }
        public Instant EndTime { get; set; }

    }


    public class QueryHandler : IQueryHandler<Query, List<Model>>
    {
        private readonly PidpDbContext context;

        public QueryHandler(PidpDbContext context) => this.context = context;

        public async Task<List<Model>> HandleAsync(Query query)
        {
            Console.WriteLine("query component : ");
            Console.WriteLine(query.Component);
            var banners = await this.context.Banners
                .Where(banner => banner.Component == query.Component)
                .Select(banner => new Model {
                    Id = banner.Id!,
                    Component = banner.Component,
                    Status = banner.Status,
                    Header = banner.Header,
                    Body = banner.Body!,
                    StartTime = banner.StartTime!,
                    EndTime = banner.EndTime!
                })
                .ToListAsync();

            Console.WriteLine(banners.ToArray());
            var currentTime = DateTime.UtcNow;
            var unixTime = ((DateTimeOffset)currentTime).ToUnixTimeMilliseconds();

            var activeBanners = banners.Where(banner => banner.StartTime.ToUnixTimeMilliseconds() < unixTime &&
                    banner.EndTime.ToUnixTimeMilliseconds() > unixTime
                ).ToList();

            return activeBanners;
        }
    }
}
