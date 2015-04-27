namespace catalog.viewer.Commands
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    internal sealed class AppCloseCmd : ICommand
    {
        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        void ICommand.Execute(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}

