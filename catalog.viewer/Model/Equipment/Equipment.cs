namespace catalog.viewer.Model.Equipment
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Xml.Linq;

    public sealed class Equipment
    {
        public static Equipment Instance
        {
            get
            {
                if (_self == null)
                {
                    _self = new Equipment();
                }
                return _self;
            }
        }

        private static Equipment _self;
        private Equipment()
        {
            XElement _conf = XElement.Load("equipment.xml");

            labels = new ObservableCollection<Label>();
            foreach (XElement _label in _conf.Elements("label"))
            {
                XAttribute name = _label.Attribute("name");
                Label label = new Label()
                {
                    name = name.Value,
                };
                labels.Add(label);
                foreach (XElement lens in _label.Elements("position").Where(_ => _.Attribute("type").Value == "lens"))
                {
                    name = lens.Attribute("name");
                    Lens _ = new Lens()
                    {
                        label = label,
                        name = name.Value
                    };
                    label.articles.Add(_);
                }
                foreach (XElement camera in _label.Elements("position").Where(_ => _.Attribute("type").Value == "camera"))
                {
                    name = camera.Attribute("name");
                    Camera _ = new Camera()
                    {
                        label = label,
                        name = name.Value
                    };
                    label.articles.Add(_);
                }
            }
        }

        public ObservableCollection<Label> labels { get; private set; }

    }
}
