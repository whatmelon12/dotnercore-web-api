using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace restfulDemo.API.Controllers
{
    [Route("api/rekognition")]
    [Authorize]
    public class RekognitionController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRekognitionService _rekognitionService;

        public RekognitionController(ILoggerManager logger, IRekognitionService rekognitionService)
        {
            _logger = logger;
            _rekognitionService = rekognitionService;
        }

        [HttpPost("comparefaces")]
        public async Task<IActionResult> CompareFaces([FromForm] FaceCompareDto faceCompare)
        {
            if(
                faceCompare.SourceImage == null ||
                faceCompare.SourceImage?.Length <= 0 ||
                faceCompare.TargetImage == null ||
                faceCompare.TargetImage?.Length <= 0)
            {
                throw new InvalidOperationException("Missing source or target image");
            }

            MemoryStream sourceImage = new MemoryStream();
            MemoryStream targetImage = new MemoryStream();

            await faceCompare.SourceImage.CopyToAsync(sourceImage);
            await faceCompare.TargetImage.CopyToAsync(targetImage);

            var result = await _rekognitionService.CompareFaceAsync(sourceImage, targetImage);

            return Accepted(result);
        }
    }
}
