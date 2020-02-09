namespace virus_tortoise.lib.Parsers.Base
{
    public abstract class BaseParser
    {
        public abstract string FileType { get; }

        public abstract (string FileType, bool IsValid, string[] AnalysisNotes) Analyze(byte[] data);
    }
}