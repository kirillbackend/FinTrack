
namespace FinTrack.Localization.Conrtacts
{
    public interface ILocalizationContextLocator
    {
        T GetContext<T>() where T : class, IContext;
     }
}
