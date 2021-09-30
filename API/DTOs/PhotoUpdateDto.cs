using Microsoft.AspNetCore.Http;

namespace API.DTOs
{
    public class PhotoUpdateDto
    {
        public IFormFile File { get; set; }
        public bool Remove { get; set; }
    }
}
