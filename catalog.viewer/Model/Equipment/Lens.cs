namespace catalog.viewer.Model.Equipment
{
    using System.ComponentModel;

    public sealed class Lens : iEquipment, INotifyPropertyChanged
    {
        public Label label { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public string system { get; set; }
        public override string ToString()
        {
            return name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
