using Microsoft.AspNetCore.Http;
namespace Entities.DataTransferObjects
{
    public class FaceCompareDto
    {
        public IFormFile SourceImage { get; set; }
        public IFormFile TargetImage { get; set; }
    }
}
