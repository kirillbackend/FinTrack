using FinTrack.Localization.Conrtacts;

namespace FinTrack.Localization
{
    public class LocaleContext : IContext
    {
        public string Locale { get; }

        public IResourceProvider ResourceProvider { get; }

        public LocaleContext(string locale, IResourceProvider resourceProvider)
        {
            if (string.IsNullOrEmpty(locale))
            {
                throw new ArgumentNullException("locale");
            }

            Locale = locale;
            ResourceProvider = resourceProvider;
        }
    }
}
