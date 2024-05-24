using System.ComponentModel.DataAnnotations.Schema;

namespace Pigga.Models
{
    public class Chef
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImgUrls { get; set; }
        [NotMapped]
        public IFormFile? ImgFile { get; set; }

    }
}
