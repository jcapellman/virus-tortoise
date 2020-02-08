using System;

using virus_tortoise.lib.Parsers.Base;

namespace virus_tortoise.lib.Parsers
{
    public class PNGParser : BaseParser
    {
        private byte[] FileMagicBytes = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };

        public override string FileType => "PNG";

        public override bool IsFile(byte[] data) => data.AsSpan().Slice(0, FileMagicBytes.Length).SequenceEqual(FileMagicBytes);

        public override bool IsValid(byte[] data)
        {
            var span = data.AsSpan();

            var header = span.Slice(0, 8);

            var clen = BitConverter.ToInt32(span.Slice(8, 4));

            if (clen != 13)
            {
                return false;
            }

            var chuckId = span.Slice(12, 4);

            return true;
        }
    }
}