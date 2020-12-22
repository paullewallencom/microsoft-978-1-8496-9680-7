using System.ComponentModel;
using System.Windows.Navigation;

namespace SpaceAim3D.ViewModels
{
    /// <summary>The abstract class representing the view model.</summary>
    public abstract class ViewModel : INotifyPropertyChanged
    {
        /// <summary>Gets or sets a navigation service.</summary>
        public NavigationService NavigationService { get; set; }

        /// <summary>Event that is called when a property value is changed.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Indicates that a particular property value is changed.</summary>
        /// <param name="name">A name of the property, which value is modified.</param>
        protected void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
