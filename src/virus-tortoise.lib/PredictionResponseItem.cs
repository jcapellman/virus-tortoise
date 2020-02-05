namespace virus_tortoise.lib
{
    public class PredictionResponseItem
    {
        public string SHA1 { get; set; }

        public long FileSize { get; set; }

        public string FileType { get; set; }

        public string ErrorMessage { get; set; }
    }
}