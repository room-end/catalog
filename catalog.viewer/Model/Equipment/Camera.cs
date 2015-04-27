namespace catalog.viewer.Model.Equipment
{
    using System.ComponentModel;

    public sealed class Camera : iEquipment, INotifyPropertyChanged
    {
        public Label label { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public override string ToString()
        {
            return string.Format("{0} {1}", (label != null)? label.name : string.Empty, name);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}