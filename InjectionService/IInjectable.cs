namespace VarelaAloisio.InjectionService.Runtime
{
    /// <summary>
    /// An object that handles an injection.
    /// </summary>
    public interface IInjectable
    {
        /// <summary>
        /// This method is called after the injection.
        /// <para>To have injectable values, declare <see cref="InjectedField"/>/<see cref="InjectedField{T}"/></para>
        /// <para>To inject those values, use an <see cref="IInjectionService"/> (impl: <see cref="InjectionService"/>)</para>
        /// </summary>
        public void HandleInjection();
    }
}