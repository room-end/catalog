namespace catalog.viewer.Dlg
{
    using catalog.viewer.Model.Equipment;
    using System.Windows;
    using System.Windows.Controls.WpfPropertyGrid;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for EquipmentDlg.xaml
    /// </summary>
    public partial class EquipmentDlg : Window
    {
        public EquipmentDlg()
        {
            InitializeComponent();
            Equipment _equipment = Equipment.Instance;
            _tree.DataContext = _equipment;
            _tree.ItemsSource = _equipment.labels;
        }

        private void onOK(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void onSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _props.SelectedObject = e.NewValue;
        }

        private void _props_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {

        }

        private void MenuItem_Add_Model(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Add_Lens(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Add_Film(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Add_Camera(object sender, RoutedEventArgs e)
        {
            var _o = (sender as MenuItem).DataContext;
            if (_o is Model.Equipment.Label)
            {
                Camera cam = new Camera() { label = _o as Model.Equipment.Label };
                (_o as Model.Equipment.Label).articles.Add(cam);
                _props.SelectedObject = cam;
            }
        }
    }
}
