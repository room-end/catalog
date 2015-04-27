namespace catalog.viewer
{
    using BitMiracle.LibTiff.Classic;

    public sealed class EXIF
    {
        public string copyright { get; set; }
        public string description { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public float resolutionX { get; set; }
        public float resolutionY { get; set; }
        public ResUnit unit { get; set; }
        public Photometric photometric { get; set; }
        public string software { get; set; }
        public string make { get; set; }
        public string model { get; set; }
    }
}
