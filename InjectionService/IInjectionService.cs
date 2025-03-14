using System;
using System.Collections.Generic;

namespace VarelaAloisio.InjectionService.Runtime {
    /// <summary>
    /// A Service that holds references and values that can be injected in a bi-dimensional database.
    /// <para>To have injectable values, declare <see cref="InjectedField"/>/<see cref="InjectedField{T}"/></para>
    /// <remarks>Keys are <see cref="Type"/> and <see cref="InjectedField.Id"/>.</remarks>
    /// </summary>
    public interface IInjectionService {

        /// <summary>Tries to add a new entry.</summary>
        /// <param name="id">The ID used to find or replace the reference</param>
        /// <param name="value">If null, this method will return false</param>
        /// <typeparam name="T">The type to map this reference to (It's recommended to use interfaces)</typeparam>
        /// <returns>true if the reference was added successfully</returns>
        bool TryAdd<T>(string id, T value);

        /// <summary>Same as <see cref="TryAdd{T}"/> but for multiple values at the same time.</summary>
        /// <param name="values"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>An array of the IDs and the results for each one of them</returns>
        (string key, bool result)[] TryAddRange<T>(Dictionary<string, object> values);

        /// <summary>Generic version of <see cref="TryAddVariant"/></summary>
        /// <typeparam name="TOriginal">Original type</typeparam>
        /// <typeparam name="TVariant">Variant type</typeparam>
        /// <returns>true if the variant is added.</returns>
        bool TryAddVariant<TOriginal, TVariant>();

        /// <summary>
        /// Adds a <see cref="variantType"/> to access the same values as the <see cref="originalType"/> 
        /// <remarks>Editing any of the entries afterward modifies both of them.</remarks>
        /// </summary>
        /// <param name="originalType">The original type <b>Must have been</b> added before calling this method.</param>
        /// <param name="variantType">The variant type <b>Cannot</b> be added before calling this method.</param>
        /// <returns>true if the variant is added.</returns>
        bool TryAddVariant(Type originalType, Type variantType);

        /// <summary>
        /// Tries to remove the entry corresponding to (<see cref="T"/>, <see cref="id"/>)
        /// </summary>
        /// <param name="id">The ID for the entry</param>
        /// <typeparam name="T">The type for the entry</typeparam>
        /// <returns>true if the entry is removed.</returns>
        bool TryRemove<T>(string id);

        /// <summary>Generic version </summary>
        /// <typeparam name="T">The type to remove.</typeparam>
        /// <returns>true if the values mapped to <see cref="T"/> are removed</returns>
        bool RemoveType<T>();

        /// <summary>Removes all values mapped to <see cref="Type"/></summary>
        /// <param name="type">the Type to remove.</param>
        /// <returns>true if the values mapped to <see cref="Type"/> are removed</returns>
        bool RemoveType(Type type);

        /// <summary>Adds an entry to (<see cref="T"/>, <see cref="id"/>) or replaces the value found in that position.</summary>
        /// <param name="id">ID for the entry. 2nd dimension filter.</param>
        /// <param name="value">Value for the entry.</param>
        /// <typeparam name="T">Type of the entry. 1st dimension filter.</typeparam>
        void AddOrReplace<T>(string id, T value);

        /// <summary>Injects all <see cref="InjectedField{T}"/> values found in the database.</summary>
        /// <remarks><see cref="IInjectable"/>.<see cref="IInjectable.HandleInjection"/> will be called after the injection.</remarks>
        /// <param name="target">The object to inject values into</param>
        /// <returns>the target as an <see cref="IInjectable"/></returns>
        IInjectable Inject(IInjectable target);
    }
}