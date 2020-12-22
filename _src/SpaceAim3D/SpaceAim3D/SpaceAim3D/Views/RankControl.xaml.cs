using System.Windows;
using System.Windows.Controls;

namespace SpaceAim3D.Views
{
    /// <summary>The class representing the single rank containing many rank items.</summary>
    public partial class RankControl : UserControl
    {
        /// <summary>A dependency property regarding a rank name.</summary>
        public static readonly DependencyProperty RankNameProperty = DependencyProperty.Register(
            "RankName",
            typeof(string),
            typeof(RankControl),
            null);

        /// <summary>Gets or sets a rank name.</summary>
        public string RankName
        {
            get { return (string)this.GetValue(RankNameProperty); }
            set { this.SetValue(RankNameProperty, value); }
        }

        /// <summary>Initializes a new instance of the RankControl class.</summary>
        public RankControl()
        {
            this.InitializeComponent();
        }
    }
}
