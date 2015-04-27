namespace catalog.viewer.Model.Photography
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public sealed class Catalog : Node
    {
        private Catalog()
        {
        }

        public override string name
        {
            get
            {
                return path;
            }
        }

        public string path { get; set; }
        public string path_previews { get; set; }

        internal static Catalog FromPath(DirectoryInfo diri)
        {
            Catalog catalog = new Catalog();
            catalog.path = diri.FullName;
            foreach (Year year in Year.FromPath(diri).OrderBy(_ => _.year))
            {
                year.parent = catalog;
                catalog.nodes.Add(year);
            }
            return catalog;
        }

        internal static Catalog fromXML(XElement _catalog)
        {
            Catalog catalog = new Catalog();
            foreach (Year year in Year.fromXML(_catalog).OrderBy(_ => _.year))           
            {
                year.parent = catalog;
                catalog.nodes.Add(year);
            }
            return catalog;
        }

        public override XElement toXML()
        {
            XElement _ = new XElement("catalog");
            _.SetAttributeValue("path", path);
            foreach (var node in nodes)
            {
                XElement xnode = node.toXML();
                _.Add(xnode);
            }
            return _;
        }
    }
}
