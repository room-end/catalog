namespace catalog.viewer.Model.Photography
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Media.Imaging;
    using System.Xml.Linq;

    public interface iNode : INotifyPropertyChanged
    {
        string name { get; }
        iNode parent { get; set; }
        ObservableCollection<iNode> nodes { get; }
        ObservableCollection<BitmapImage> previews { get; }
        XElement toXML();
        void createPreviews(int width, int height);
        void createPreviews();
    }
}
