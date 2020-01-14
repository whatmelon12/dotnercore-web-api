using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Rekognition.Model;

namespace Contracts
{
    public interface IRekognitionService
    {
        Task<CompareFacesResponse> CompareFaceAsync(MemoryStream source, MemoryStream target);
        Task IndexFaceAsync();
    }
}
