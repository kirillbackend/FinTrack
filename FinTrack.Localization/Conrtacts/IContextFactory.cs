
namespace FinTrack.Localization.Conrtacts
{
    public interface IContextFactory
    {
        LocaleContext CreateLocaleContext(string locale);
    }
}
