namespace catalog.viewer.Model.Equipment
{
    using System.Collections.ObjectModel;

    public sealed class Label
    {
        internal Label()
        {
            articles = new ObservableCollection<iEquipment>();
        }

        public string name { get; set; }
        public override string ToString()
        {
            return name;
        }
        public ObservableCollection<iEquipment> articles { get; private set; }
    }
}
