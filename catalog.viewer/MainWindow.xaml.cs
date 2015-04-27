namespace catalog.viewer
{
    using catalog.viewer.Model.Photography;
    using catalog.viewer.Properties;
    using Microsoft.Win32;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Xml.Linq;


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string _catalog_ = Settings.Default.catalog;
            DirectoryInfo diri = new DirectoryInfo(_catalog_);
            this.catalogs = new ObservableCollection<Catalog>();
            Catalog catalog = null;// new Catalog(_catalog_, _previews_);
            if (diri.Exists)
            {
                catalog = Catalog.FromPath(new DirectoryInfo(_catalog_));
            }
            else
            {
                XElement _catalog = XElement.Load("catalog.xml");
                catalog = Catalog.fromXML(_catalog);
            }
            catalog.path_previews = Settings.Default.previews;
            this.catalogs.Add(catalog);
            var list_sw =catalog.nodes.SelectMany(_ => _.nodes).OfType<sw_Film>();
            _max_numner_sw = list_sw.Any() ? list_sw.Max(_ => _.number) : -1;
            var list_sd = catalog.nodes.SelectMany(_ => _.nodes).OfType<sd_Film>();
            _max_numner_sd = list_sd.Any() ? list_sd.Max(_ => _.number) : -1;
            var list_fn = catalog.nodes.SelectMany(_ => _.nodes).OfType<fn_Film>();
            _max_numner_fn = list_fn.Any() ? list_fn.Max(_ => _.number) : -1;
            var list_fp = catalog.nodes.SelectMany(_ => _.nodes).OfType<fp_Film>();
            _max_numner_fp = list_fp.Any() ? list_fp.Max(_ => _.number) : -1;
            _tree.DataContext = catalog;
            _tree.ItemsSource = catalogs;
            Queue_Resize.instance.resize_message += resize_message;

        }

        void resize_message(Model.Photography.Frame frame, string message)
        {
            Dispatcher.BeginInvoke(new Action(() => _status_.Text = message));
        }

        private decimal _max_numner_sw;
        private decimal _max_numner_sd;
        private decimal _max_numner_fn;
        private decimal _max_numner_fp;

        public ObservableCollection<Catalog> catalogs { get; set; }
        public string status { get; set; }

        private void onSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            iNode node = e.NewValue as iNode;
            if (node == null)
                return;
            _props.SelectedObject = node;
            try
            {
                _preview_.Source = node.previews.FirstOrDefault();
            }
            catch (Exception)
            { }
            if (_preview_.Source == null)
                return;
            double _preview_ratio = _preview_.Source.Width / _preview_.Source.Height;
            double _prev_panel_ratio = _prev_panel_.ActualWidth / _prev_panel_.ActualHeight;
            if (_preview_ratio >= _prev_panel_ratio)
            {
                if (_preview_ratio >= 1)
                {
                    _preview_.MaxWidth = _prev_panel_.ActualWidth;
                }
                else
                {
                    _preview_.MaxHeight = _prev_panel_.ActualHeight;
                }
            }
            else
            {
                if (_preview_ratio >= 1)
                {
                    _preview_.MaxHeight = _prev_panel_.ActualHeight;
                }
                else
                {
                    _preview_.MaxWidth = _prev_panel_.ActualWidth;
                }
            }
        }

        private void MenuItem_Add(object sender, RoutedEventArgs e)
        {
            var _o = (sender as MenuItem).DataContext;
            if (_o is Year)
            {
                sw_Film x = new sw_Film() { number = ++_max_numner_sw };
                (_o as Year).nodes.Add(x);
                _props.SelectedObject = x;
            }
        }

        private void MenuItem_Generate_previews(object sender, RoutedEventArgs e)
        {
            var _o = (sender as MenuItem).DataContext;
            iNode node = _o as iNode;
            if (node != null)
            {
                node.createPreviews();
            }

        }

        private void _props_PropertyValueChanged(object sender, System.Windows.Controls.WpfPropertyGrid.PropertyValueChangedEventArgs e)
        {

        }

        private void onClosing(object sender, CancelEventArgs e)
        {
            Queue_Resize.instance.cancel();
            e.Cancel = false;
        }

        private void onPreviewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_preview_.Source == null)
                return;
            double _preview_ratio = _preview_.Source.Width / _preview_.Source.Height;
            double _prev_panel_ratio = e.NewSize.Width / e.NewSize.Height;
            if (_preview_ratio >= _prev_panel_ratio)
            {
                _preview_.MaxWidth = Math.Min(e.NewSize.Width, _preview_.Source.Width * _preview_ratio);
                _preview_.MaxHeight = Math.Min(e.NewSize.Height, _preview_.Source.Height / _preview_ratio);
            }
            else
            {
                _preview_.MaxWidth = Math.Min(e.NewSize.Width, _preview_.Source.Width / _preview_ratio);
                _preview_.MaxHeight = Math.Min(e.NewSize.Height, _preview_.Source.Height * _preview_ratio);
            }
        }

        private void onOpenFromXML(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XML files (*.xml)p|*.xml";
            dlg.DefaultExt = ".xml";
            dlg.InitialDirectory = Settings.Default.previews;
            dlg.FileName = "catalog.xml";
            dlg.CheckPathExists = true;
            if (!dlg.ShowDialog() ?? false)
                return;
            XElement _catalog = XElement.Load(dlg.FileName);
            Catalog catalog = Catalog.fromXML(_catalog);
            catalog.path_previews = Settings.Default.previews;
            catalogs.Clear();
            catalogs.Add(catalog);
            var list_sw = catalog.nodes.SelectMany(_ => _.nodes).OfType<sw_Film>();
            _max_numner_sw = list_sw.Any() ? list_sw.Max(_ => _.number) : -1;
            var list_sd = catalog.nodes.SelectMany(_ => _.nodes).OfType<sd_Film>();
            _max_numner_sd = list_sd.Any() ? list_sd.Max(_ => _.number) : -1;
            var list_fn = catalog.nodes.SelectMany(_ => _.nodes).OfType<fn_Film>();
            _max_numner_fn = list_fn.Any() ? list_fn.Max(_ => _.number) : -1;
            var list_fp = catalog.nodes.SelectMany(_ => _.nodes).OfType<fp_Film>();
            _max_numner_fp = list_fp.Any() ? list_fp.Max(_ => _.number) : -1;
            _tree.DataContext = catalog;
            _tree.ItemsSource = catalogs;
        }

        private void onMergeWithXML(object sender, RoutedEventArgs e)
        {

        }

        private void onSaveXML(object sender, RoutedEventArgs e)
        {

        }

        private void onSaveAsXML(object sender, RoutedEventArgs e)
        {

        }
        
    }
}
