using System;
using System.IO;
using System.Linq;
using System.Text;
using virus_tortoise.lib.Extensions;
using virus_tortoise.lib.Parsers.Base;

namespace virus_tortoise.lib.Parsers
{
    public class PNGParser : BaseParser
    {
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
                Width = data.Take(4).ToArray().ToInt32();

                Height = data.Skip(4).Take(4).ToArray().ToInt32();
            }
        }

        private byte[] FileMagicBytes = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };

        public override string FileType => "PNG";

        public override bool IsFile(byte[] data) => data.AsSpan().Slice(0, FileMagicBytes.Length).SequenceEqual(FileMagicBytes);

        public override bool IsValid(byte[] data)
        {
            try
            {
                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                using var ms = new MemoryStream(data);

                // Skip the first 7 bytes from the header
                ms.Seek(FileMagicBytes.Length, SeekOrigin.Begin);

                while (ms.CanRead)
                {
                    byte[] chunkInfo = new byte[4];

                    ms.Read(chunkInfo, 0, chunkInfo.Length);

                    var chunkSize = chunkInfo.ToInt32();

                    byte[] chunkIdBytes = new byte[4];

                    ms.Read(chunkIdBytes, 0, 4);

                    var chunkId = Encoding.UTF8.GetString(chunkIdBytes);

                    byte[] chunk = new byte[chunkSize];

                    ms.Read(chunk, 0, chunkSize);

                    switch (chunkId)
                    {
                        case "IHDR":
                            var header = new IHDR(chunk);

                            // Payload exceeds length
                            if (data.Length <= (header.Width * header.Height * 24) + ms.Position)
                            {
                                break;
                            }

                            return false;
                    }
                }

                return true;
            } catch (Exception ex)
            {
                // TODO: LOG HERE

                return false;
            }
        }
    }
}