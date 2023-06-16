namespace Wfm.Web;

internal class Lazier<T> : Lazy<T> 
    where T : class
{
    public Lazier(IServiceProvider provider)
        : base(() => provider.GetRequiredService<T>())
    {
    }
}
