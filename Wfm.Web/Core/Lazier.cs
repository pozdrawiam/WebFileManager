namespace Wfm.Web.Core;

internal class Lazier<T> : Lazy<T> 
    where T : class
{
    public Lazier(IServiceProvider provider)
        : base(provider.GetRequiredService<T>)
    {
    }
}
