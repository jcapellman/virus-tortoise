namespace virus_tortoise.lib.Parsers.Base
{
    public abstract class BaseParser
    {
        public abstract string FileType { get; }

        public abstract bool IsFile(byte[] data);

        public abstract bool IsValid(byte[] data);
    }
}