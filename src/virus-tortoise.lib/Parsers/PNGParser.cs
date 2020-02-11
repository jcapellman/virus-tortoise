using System;
using System.IO;
using System.Linq;
using System.Text;

using NLog;

using virus_tortoise.lib.Extensions;
using virus_tortoise.lib.Parsers.Base;

namespace virus_tortoise.lib.Parsers
{
    public class PNGParser : BaseParser
    {
        private static readonly Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private const int ChunkIdSize = 4;
        private const int ChunkInfoSize = 4;

        private const int MaxByteDepth = 3;

        public class IEND { }

        public class IHDR
        {
            public Int32 Width;

            public Int32 Height;

            public byte BitDepth;

            public byte ColorType;

            public byte Compression;

            public byte FilterMethod;

            public byte Interlace;

            public IHDR(byte[] data)
            {
                Width = data.ToInt32();

                Height = data.ToInt32(4);
            }
        }

        public class IDAT
        {
            // Properties
        }

        private byte[] FileMagicBytes = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };

        public override string FileType => "PNG";

        public override (string FileType, bool IsValid, string[] AnalysisNotes) Analyze(byte[] data)
        {
            try
            {
                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                using var ms = new MemoryStream(data);

                var fileMagic = new byte[FileMagicBytes.Length];

                ms.Read(fileMagic, 0, fileMagic.Length);

                if (!fileMagic.SequenceEqual(FileMagicBytes))
                {
                    return (string.Empty, false, null);
                }

                // Skip the first 7 bytes from the header
                ms.Seek(FileMagicBytes.Length, SeekOrigin.Begin);

                while (ms.Position != data.Length)
                {
                    var chunkInfo = new byte[ChunkInfoSize];

                    ms.Read(chunkInfo, 0, chunkInfo.Length);

                    var chunkSize = chunkInfo.ToInt32();

                    var chunkIdBytes = new byte[ChunkIdSize];

                    ms.Read(chunkIdBytes, 0, ChunkIdSize);

                    var chunkId = Encoding.UTF8.GetString(chunkIdBytes);

                    var chunk = new byte[chunkSize];

                    ms.Read(chunk, 0, chunkSize);

                    switch (chunkId)
                    {
                        case nameof(IHDR):
                            var header = new IHDR(chunk);

                            // Payload exceeds length
                            if (data.Length <= (header.Width * header.Height * MaxByteDepth) + ms.Position)
                            {
                                break;
                            }

                            return (FileType, false, new[] { "SUSPICIOUS: Payload is larger than what the size should be" });
                        case nameof(IDAT):
                            // Build Embedded file from the chunks
                            break;
                        case nameof(IEND):
                            // Note that the PNG had an end
                            break;
                    }
                }

                return (FileType, true, null);
            }
            catch (Exception ex)
            {
                Log.Error($"PNGParser::Analyze: Failed to process {ex}");

                return (string.Empty, false, new [] { ex.ToString() });
            }
        }
    }
}