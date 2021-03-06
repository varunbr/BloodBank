using System.Text.Json;
using API.Helpers;
using Microsoft.AspNetCore.Http;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int pageNumber, int pageSize, int totalPages, int totalCount)
        {
            var pagination = new PaginationHeader(pageNumber, pageSize, totalPages, totalCount);
            response.Headers.Add("Pagination", JsonSerializer.Serialize(pagination,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
