namespace catalog.viewer.Commands
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using catalog.viewer.Model;
    using Microsoft.Win32;
    using System.Linq;
    using catalog.viewer.Model.Photography;

    internal sealed class AddFilmFromPathCmd : ICommand
    {
        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        void ICommand.Execute(object parameter)
        {
            Year year = parameter as Year;
            if (year == null)
                return;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "TIFF files (*.tiff, *.tiff)|???.tif";
            ofd.Multiselect = true;
            FolderSelectDialog fsd = new FolderSelectDialog();
            if (!fsd.ShowDialog())
                return;
            
            if (ofd.ShowDialog() ?? false)
                return;
            //foreach (Frame frame in Frame.FromPath(ofd.FileNames))
            //{
            //    if (year.nodes.OfType<Frame>().Where(_ => _.number == frame.number).Any())
            //        continue;
            //    year.nodes.Add(frame);
            //}
        }

    }
}
