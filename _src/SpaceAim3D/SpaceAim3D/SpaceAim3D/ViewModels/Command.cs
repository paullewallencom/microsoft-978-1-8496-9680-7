using System;
using System.Windows.Input;

namespace SpaceAim3D.ViewModels
{
    /// <summary>The class representing a command taken after performing some action in the user interface.</summary>
    public class Command : ICommand
    {
        private Action m_action;

        /// <summary>Initializes a new instance of the Command class.</summary>
        /// <param name="action">Action that should be taken while executing the command.</param>
        public Command(Action action)
        {
            this.m_action = action;
        }

        /// <summary>Executes the command.</summary>
        /// <param name="parameter">Additional parameter (not used).</param>
        public void Execute(object parameter)
        {
            this.m_action();
        }

        /// <summary>Event that is fired when a value indicating whether the command can be executed is changed.</summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>Returns a value indicating whether the command can be executed.</summary>
        /// <param name="parameter">Additional parameter (not used).</param>
        /// <returns>A value indicating whether the command can be executed.</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
