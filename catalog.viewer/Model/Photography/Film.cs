namespace catalog.viewer.Model.Photography
{
    using catalog.viewer.Model.Equipment;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public abstract class Film : Node
    {
        [NotifyParentProperty(true)]
        public decimal number { get; set; }
        public Author author { get; set; }
        public Camera camera { get; set; }
        public string description { get; set; }
        public string copyright { get; set; }
        public string software { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public Ranking ranking { get; set; }
        public FilmScanStatus status { get; set; }

        public abstract FilmType type { get; }
        public abstract int type2 { get; }

        public override string name
        {
            get
            {
                return string.Format("{0}{1}{2}_{3}{4:000}", author.ToString().Substring(0, 1), (camera != null) ? camera.id : string.Empty, (int)type, type2, number);
            }
        }

        internal static Film create(Match match)
        {
            Film film = null;

            if (!match.Success)
                return film;
            string _author = match.Groups["author"].Value;
            if (_author == null)
                return film;
            string format = match.Groups["format"].Value;
            if (format == null)
                return film;
            string type = match.Groups["type"].Value;
            if (type == null)
                return film;
            string format2 = match.Groups["format2"].Value;
            if (format2 == null)
                return film;
            string number = match.Groups["number"].Value;
            if (number == null)
                return film;
            switch (type)
            {
                case "1":
                    film = new sw_Film();
                    break;
                case "2":
                    film = new fn_Film();
                    break;
                case "4":
                    film = new sd_Film();
                    break;
                case "6":
                    film = new fp_Film();
                    break;
                default:
                    return film;
            }
            film.author = toAuthor(_author);
            film.camera = new Camera();
            film.camera.id = format;
            film.number = decimal.Parse(number);
            return film;
        }

        private static Author toAuthor(string _author)
        {
            switch (_author)
            {
                case "i":
                    return Author.ivana;
                case "r":
                    return Author.roumen;
                case "s":
                    return Author.silvia;
                default:
                    return Author.unknown;
            }
        }

        private static readonly Regex dir_rx = new Regex(@"(?<author>[irs])(?<format>[123456])(?<type>[126])_(?<format2>[1234])(?<number>\d{3})");

        internal static IEnumerable<Film> FromPath(DirectoryInfo diri)
        {
            foreach (var di in diri.EnumerateDirectories("???_????"))
            {
                string dirname = di.Name;
                Match match = dir_rx.Match(dirname);
                Film film = create(match);
                if (film == null)
                    continue;
                foreach (Frame frame in Frame.FromPath(di).OrderBy(_ => _.number))
                {
                    frame.parent = film;
                    if (frame != null)
                        film.nodes.Add(frame);
                }
                IEnumerable<Frame> frames = film.nodes.OfType<Frame>();
                if (frames.Count() > 0)
                {
                    film.description = frames.GroupBy(_ => _.description).Select(_ => new { _.Key, count = _.Count() }).OrderByDescending(_ => _.count).ToList().FirstOrDefault().Key;
                    film.copyright = frames.GroupBy(_ => _.copyright).Select(_ => new { _.Key, count = _.Count() }).OrderByDescending(_ => _.count).ToList().FirstOrDefault().Key;
                    film.software = frames.GroupBy(_ => _.software).Select(_ => new { _.Key, count = _.Count() }).OrderByDescending(_ => _.count).ToList().FirstOrDefault().Key;
                    film.make = frames.GroupBy(_ => _.make).Select(_ => new { _.Key, count = _.Count() }).OrderByDescending(_ => _.count).ToList().FirstOrDefault().Key;
                    film.model = frames.GroupBy(_ => _.model).Select(_ => new { _.Key, count = _.Count() }).OrderByDescending(_ => _.count).ToList().FirstOrDefault().Key;
                }
                yield return film;
            }
        }

        public override XElement toXML()
        {
            XElement _ = new XElement("film");
            _.SetAttributeValue("number", type2 * 1000 + number);
            _.SetAttributeValue("author", author);
            if (!string.IsNullOrWhiteSpace(camera.ToString()))
                _.SetAttributeValue("camera", camera);
            _.SetAttributeValue("type", type);
            _.SetAttributeValue("xumber", name);
            if (!string.IsNullOrWhiteSpace(description))
                _.SetAttributeValue("description", description);
            foreach (var node in nodes)
            {
                XElement xnode = node.toXML();
                _.Add(xnode);
            }
            return _;
        }

        internal static IEnumerable<Film> fromXML(XElement _year)
        {
            foreach (XElement _film in _year.Elements("film"))
            {
                XAttribute xumber = _film.Attribute("xumber");
                if (xumber == null)
                    continue;
                Match match = dir_rx.Match(xumber.Value);
                Film film = create(match);
                if (film == null)
                    continue;
                foreach (Frame frame in Frame.fromXML(_film).OrderBy(_ => _.number))
                {
                    frame.parent = film;
                    if (frame != null)
                        film.nodes.Add(frame);
                }
                yield return film;
            }
        }
    }
}
