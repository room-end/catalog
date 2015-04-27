namespace catalog.viewer.Model.Photography
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Text.RegularExpressions;
    using BitMiracle.LibTiff.Classic;
    using System.Xml.Linq;
    using System;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;

    public sealed class Frame : Node
    {
        public decimal number { get; set; }
        public string filename { get; set; }
        public string copyright { get; set; }
        public string description { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public float resolutionX { get; set; }
        public float resolutionY { get; set; }
        public ResUnit unit { get; set; }
        public Photometric photometric { get; set; }
        public string software { get; set; }
        public string make { get; set; }
        public string model { get; set; }

        public override string name
        {
            get
            {
                return string.Format("{0}", number.ToString());
            }
        }

        public string path_preview
        {
            get
            {
                Film film = this.parent as Film;
                string film_dir = (film != null) ? film.name : string.Empty;
                Year year = film.parent as Year;
                string year_dir = (year != null) ? year.name : string.Empty;
                Catalog catalog = year.parent as Catalog;
                string _path = (catalog != null) ? catalog.path_previews : string.Empty;
                return Path.Combine(_path, year_dir, film_dir);
            }
        }

        public string preview_filename
        {
            get
            {
                string _name = Path.GetFileNameWithoutExtension(filename);
                return string.Format("{0}.jpeg", _name);
            }
        }

        public string path
        {
            get
            {
                Film film = this.parent as Film;
                string film_dir = (film != null) ? film.name : string.Empty;
                Year year = film.parent as Year;
                string year_dir = (year != null) ? year.name : string.Empty;
                Catalog catalog = year.parent as Catalog;
                string _path = (catalog != null) ? catalog.path : string.Empty;
                return Path.Combine(_path, year_dir, film_dir);
            }
        }

        internal static IEnumerable<Frame> FromPath(DirectoryInfo diri)
        {
            Regex dir_rx = new Regex(@"[1](?<number>[\d][\d\$])");
            foreach (var fi in diri.EnumerateFiles("???.tif"))
            {
                string filename = fi.Name;
                string dirname = fi.Directory.Name;
                string dirfullname = fi.Directory.FullName;
                Match match = dir_rx.Match(filename);
                if (!match.Success)
                    continue;
                string number = match.Groups["number"].Value;
                if (number == null)
                    continue;
                EXIF _exif = null;
                try
                {
                    _exif = TIFF.exif(dirfullname, filename);
                }
                catch (InvalidOperationException)
                {
                    continue;
                }
                Frame frame = new Frame()
                {
                    number = toNumber(number),
                    filename = filename,
                    copyright = _exif.copyright,
                    description = _exif.description,
                    height = _exif.height,
                    width = _exif.width,
                    photometric = _exif.photometric,
                    resolutionX = _exif.resolutionX,
                    resolutionY = _exif.resolutionY,
                    unit = _exif.unit,
                    software = _exif.software,
                    make = _exif.make,
                    model = _exif.model,
                };
                yield return frame;
            }
        }

        private static decimal toNumber(string number)
        {
            if (number == "0$")
                return -1;
            decimal result = -100;
            if (decimal.TryParse(number, out result))
                return result;
            throw new System.ArgumentException("Invalid Number of Film Format");
        }

        private static string toFilename(decimal number)
        {
            if (number == -1)
                return "10$";
            return (number + 100).ToString();
        }

        public override XElement toXML()
        {
            XElement _ = new XElement("frame");
            _.SetAttributeValue("number", number);
            _.SetAttributeValue("height", height);
            _.SetAttributeValue("width", width);
            return _;
        }

        public override void createPreviews(int width, int height)
        {
            Queue_Resize.instance.resize(this, width, height);
        }

        public override void createPreviews()
        {
            Queue_Resize.instance.resize(this);
        }

        public override ObservableCollection<BitmapImage> previews
        {
            get
            {
                ObservableCollection<BitmapImage> _previews = new ObservableCollection<BitmapImage>();
                string preview_file_fullname = Path.Combine(path_preview, preview_filename);
                if (File.Exists(preview_file_fullname))
                {
                    BitmapImage bimp = new BitmapImage(new Uri(preview_file_fullname));
                    _previews.Add(bimp);
                }

                return _previews;
            }
        }

        internal static IEnumerable<Frame> fromXML(XElement _film)
        {
            foreach (XElement _frame in _film.Elements("frame"))
            {
                Frame frame = new Frame() { };
                XAttribute _number = _frame.Attribute("number");
                int _number_ = 0;
                if ((_number != null) && int.TryParse(_number.Value, out _number_))
                    frame.number = _number_;
                XAttribute _width = _frame.Attribute("width");
                int _width_ = 0;
                if ((_width != null) && int.TryParse(_width.Value, out _width_))
                    frame.height = _width_;
                XAttribute _height = _frame.Attribute("height");
                int _height_ = 0;
                if ((_height != null) && int.TryParse(_height.Value, out _height_))
                    frame.height = _height_;
                frame.filename = toFilename(frame.number);
                yield return frame;
            }
        }
    }
}
