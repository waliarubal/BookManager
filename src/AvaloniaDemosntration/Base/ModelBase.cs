using ReactiveUI;

namespace AvaloniaDemosntration.Base;
public abstract class ModelBase: ReactiveObject
{
    /// <summary>
    /// Validate model object.
    /// </summary>
    /// <returns>First encountered validation error.</returns>
    public abstract string Validate();
}
