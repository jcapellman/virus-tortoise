using System;

namespace virus_tortoise.lib
{
    public class PredictionService
    {
        public PredictionResponseItem PredictFile(byte[] fileData)
        {
            return new PredictionResponseItem
            {
                FileSize = fileData.Length,
                SHA1 = String.Empty,
                FileType = "Unknown",
                ErrorMessage = string.Empty
            };
        }
    }
}