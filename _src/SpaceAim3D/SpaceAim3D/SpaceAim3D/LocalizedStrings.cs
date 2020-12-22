using SpaceAim3D.Resources;

namespace SpaceAim3D
{
    /// <summary>Provides access to string resources.</summary>
    public class LocalizedStrings
    {
        private static AppResources _localizedResources = new AppResources();

        /// <summary>Gets localized resources.</summary>
        public AppResources LocalizedResources
        {
            get
            {
                return _localizedResources;
            }
        }
    }
}