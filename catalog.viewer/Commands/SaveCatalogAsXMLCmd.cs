namespace catalog.viewer.Commands
{
    using catalog.viewer.Model.Photography;
    using Microsoft.Win32;
    using System;
    using System.Windows.Input;
    using System.Xml.Linq;

    internal sealed class SaveCatalogAsXMLCmd : ICommand
    {
        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        void ICommand.Execute(object parameter)
        {
            Catalog catalog = parameter as Catalog;
            if (catalog == null)
                return;
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "XML files (*.xml)|*.xml";
            dlg.DefaultExt = ".xml";
            dlg.InitialDirectory = catalog.path;
            dlg.FileName = "catalog.xml";
            dlg.OverwritePrompt = true;
            dlg.CheckPathExists = true;
            if (!dlg.ShowDialog() ?? false)
                return;
            XElement root = catalog.toXML();
            root.Save(dlg.FileName);
        }

    }
}
