using System;
using System.Collections.Generic;
using System.Linq;

using virus_tortoise.lib.Extensions;
using virus_tortoise.lib.Parsers.Base;

namespace virus_tortoise.lib
{
    public class PredictionService
    {
        private static List<BaseParser> _parsers;

        public PredictionService()
        {
            if (_parsers == null)
            {
                _parsers = typeof(BaseParser).Assembly.GetTypes().Where(a => a.BaseType == typeof(BaseParser) && !a.IsAbstract).Select(b => 
                    (BaseParser)Activator.CreateInstance(b)).ToList();
            }
        }

        public PredictionResponseItem PredictFile(byte[] fileData)
        {
            var response = new PredictionResponseItem
            {
                FileSize = fileData.Length,
                SHA256 = fileData.ToSHA256(),
                SHA1 = fileData.ToSHA1(),
                MD5 = fileData.ToMD5(),
                FileType = "Unsupported File Type",
                ErrorMessage = string.Empty,
                IsValid = false
            };

            var parser = _parsers.FirstOrDefault(a => a.IsFile(fileData));

            if (parser != null)
            {
                response.FileType = parser.FileType;
                response.IsValid = parser.IsValid(fileData);
            }

            return response;
        }
    }
}