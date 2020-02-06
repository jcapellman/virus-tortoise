using System;
using virus_tortoise.lib.Extensions;

namespace virus_tortoise.lib
{
    public class PredictionService
    {
        public PredictionResponseItem PredictFile(byte[] fileData)
        {
            return new PredictionResponseItem
            {
                FileSize = fileData.Length,
                SHA256 = fileData.ToSHA256(),
                FileType = "Unknown",
                ErrorMessage = string.Empty
            };
        }
    }
}