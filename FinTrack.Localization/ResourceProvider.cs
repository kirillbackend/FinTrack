using FinTrack.Localization.Conrtacts;
using System.Globalization;
using System.Resources;

namespace FinTrack.Localization
{
    internal class ResourceProvider : IResourceProvider
    {
        private CultureInfo _culture;
        private ResourceManager _resourceManager;

        public ResourceProvider(string locale)
        {
            _culture = CultureInfo.GetCultureInfo(locale ?? "ru");
        }

        protected ResourceManager Manager
        {
            get
            {
                if (_resourceManager == null)
                {
                    _resourceManager = new ResourceManager(typeof(Properties.Resources));
                }
                return _resourceManager;
            }
        }

        public string Get(string resourceName)
        {
            return Manager.GetString(resourceName, _culture);
        }
    }
}
