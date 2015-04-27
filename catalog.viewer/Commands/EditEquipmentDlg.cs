namespace catalog.viewer.Commands
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using catalog.viewer.Dlg;

    internal sealed class EditEquipmentDlg : ICommand
    {
        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        void ICommand.Execute(object parameter)
        {
            EquipmentDlg dlg = new EquipmentDlg();
            if (dlg.ShowDialog() ?? false)
                return;
        }
    }
}