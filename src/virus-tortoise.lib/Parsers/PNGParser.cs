using System;

using virus_tortoise.lib.Parsers.Base;

namespace virus_tortoise.lib.Parsers
{
    public class PNGParser : BaseParser
    {
        private byte[] FileMagicBytes = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };

        public override string FileType => "PNG";

        public override bool IsFile(byte[] data) => data.AsSpan().Slice(0, FileMagicBytes.Length).SequenceEqual(FileMagicBytes);
    }
}