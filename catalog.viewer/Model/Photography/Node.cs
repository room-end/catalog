namespace catalog.viewer.Model.Photography
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Media.Imaging;
    using System.Xml.Linq;

    public abstract class Node : iNode
    {
        protected Node()
        {
            nodes = new ObservableCollection<iNode>();
        }

        private iNode _parent;

        [Browsable(false)]
        public iNode parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
                OnPropertyChanged("parent");
            }
        }

        [Browsable(false)]
        public ObservableCollection<iNode> nodes { get; private set; }

        [Browsable(false)]
        public virtual ObservableCollection<BitmapImage> previews
        {
            get
            {
                ObservableCollection<BitmapImage> _previews = new ObservableCollection<BitmapImage>();
                return _previews;
            }
        }

        public virtual void createPreviews(int width, int height)
        {
            foreach (var node in nodes)
            {
                node.createPreviews(width, height);
            }
        }

        public virtual void createPreviews()
        {
            foreach (var node in nodes)
            {
                node.createPreviews();
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract string name {get;}

        public abstract XElement toXML();
    }
}
