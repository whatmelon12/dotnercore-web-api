using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Contracts;

namespace AmazonRekognitionService
{
    public class RekognitionService : IRekognitionService
    {
        private const float similarityThreshold = 95F;
        private readonly ILoggerManager _logger;
        private readonly AmazonRekognitionClient _client;

        public RekognitionService(ILoggerManager logger)
        {
            _logger = logger;
            _client = new AmazonRekognitionClient();
        }

        /// <summary>
        /// Use AWS Rekognition SDK to compare two face image streams.
        /// </summary>
        /// <param name="source">Source image stream.</param>
        /// <param name="target">Target image stream.</param>
        /// <returns>AWS response with compared faces statistics.</returns>
        public async Task<CompareFacesResponse> CompareFaceAsync(MemoryStream source, MemoryStream target)
        {
            Image imageSource = new Image();
            Image imageTarget = new Image();

            try
            {
                imageSource.Bytes = source;
                imageTarget.Bytes = target;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception writing image bytes. {ex.Message}", ex);
            }

            CompareFacesRequest compareFacesRequest = new CompareFacesRequest()
            {
                SourceImage = imageSource,
                TargetImage = imageTarget,
                SimilarityThreshold = similarityThreshold
            };

            CompareFacesResponse compareFacesResponse = await _client.CompareFacesAsync(compareFacesRequest);

            return compareFacesResponse;
        }

        public Task IndexFaceAsync()
        {
            throw new NotImplementedException();
        }
    }
}
