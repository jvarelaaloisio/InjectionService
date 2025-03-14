namespace VarelaAloisio.InjectionService.Runtime {
    /// <summary>
    /// Represents a field that can be injected from the outside. It's meant to be injected via reflection though.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InjectedField<T> : InjectedField
    {
        public T Value { get; private set; }
        public static implicit operator T(InjectedField<T> original) => original.Value;
    }

    /// <summary>
    /// The base class for <see cref="InjectedField{T}"/>.
    /// <para>This class is needed for the injection process</para>
    /// </summary>
    [System.Serializable]
    public abstract class InjectedField
    {
        public string Id { get; set; } = string.Empty;
    }
}