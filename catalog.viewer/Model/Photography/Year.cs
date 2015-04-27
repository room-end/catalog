namespace catalog.viewer.Model.Photography
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public sealed class Year : Node
    {
        public int year { get; set; }
        
        public override string name
        {
            get
            {
                return year.ToString();
            }
        }

        internal static IEnumerable<Year> FromPath(DirectoryInfo diri)
        {
            Regex dir_rx = new Regex(@"(?<T>[12])(?<H>[90])(?<D>[\d])(?<X>[\d])");
            foreach (DirectoryInfo di in diri.EnumerateDirectories("????"))
            {
                string dirname = di.Name;
                string dirfullname = di.FullName;
                Match match = dir_rx.Match(dirname);
                if (!match.Success)
                    continue;
                string _T = match.Groups["T"].Value;
                if (_T == null)
                    continue;
                string _H = match.Groups["H"].Value;
                if (_H == null)
                    continue;
                string _D = match.Groups["D"].Value;
                if (_D == null)
                    continue;
                string _X = match.Groups["X"].Value;
                if (_X == null)
                    continue;
                int number = int.Parse(_T) * 1000 + int.Parse(_H) * 100 + int.Parse(_D) * 10 + int.Parse(_X);
                Year year = new Year()
                {
                    year = number,
                };
                foreach (Film film in Film.FromPath(di).OrderBy(_ => _.author).ThenBy(_ => _.type).ThenByDescending(_ => _.number))
                {
                    film.parent = year;
                    if (film != null)
                        year.nodes.Add(film);
                }
                yield return year;
            }
        }

        public override XElement toXML()
        {
            XElement _ = new XElement("year");
            _.SetAttributeValue("y", year);
            foreach (var node in nodes)
            {
                XElement xnode = node.toXML();
                _.Add(xnode);
            }
            return _;
        }

        internal static IEnumerable<Year> fromXML(XElement _catalog)
        {
            foreach (XElement _year in _catalog.Elements("year"))
            {
                XAttribute y = _year.Attribute("y");
                Year year = new Year() { year = (int)y };
                foreach (Film film in Film.fromXML(_year).OrderBy(_ => _.author).ThenBy(_ => _.type).ThenByDescending(_ => _.number))
                {
                    film.parent = year;
                    if (film != null)
                        year.nodes.Add(film);
                }
                yield return year;
            }
        }
    }
}
