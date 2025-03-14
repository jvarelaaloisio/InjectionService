using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VarelaAloisio.InjectionService.Runtime
{
    /// <inheritdoc/>
    public class InjectionService : IInjectionService
    {
        private readonly Dictionary<Type, Dictionary<string, object>> _refDictionary = new();

        /// <inheritdoc/>
        public bool TryAdd<T>(string id, T value)
        {
            if (value is null)
                return false;
            if (_refDictionary.TryGetValue(typeof(T), out var dictionary))
                return dictionary.TryAdd(id, value);

            dictionary = new Dictionary<string, object>();
            _refDictionary.Add(typeof(T), dictionary);

            return dictionary.TryAdd(id, value);
        }

        /// <inheritdoc/>
        public (string key, bool result)[] TryAddRange<T>(Dictionary<string, object> values)
        {
            var results = new (string key, bool result)[values.Count];
            if (!_refDictionary.TryGetValue(typeof(T), out var dictionary))
            {
                dictionary = new Dictionary<string, object>();
                _refDictionary.Add(typeof(T), dictionary);
            }

            var resultIndex = 0;
            foreach (var value in values)
            {
                (string key, bool result) result = new (value.Key, dictionary.TryAdd(value.Key, value.Value));
                results[resultIndex++] = result;
            }
            return results;
        }

        /// <inheritdoc/>
        public bool TryAddVariant<TOriginal, TVariant>()
            => TryAddVariant(typeof(TOriginal), typeof(TVariant));

        /// <inheritdoc/>
        public bool TryAddVariant(Type originalType, Type variantType)
            => _refDictionary.TryGetValue(originalType, out var dictionary)
               && _refDictionary.TryAdd(variantType, dictionary);

        /// <inheritdoc/>
        public bool TryRemove<T>(string id)
        {
            if (!_refDictionary.TryGetValue(typeof(T), out var dictionary))
                return false;
            dictionary.Remove(id);
            return true;
        }

        /// <inheritdoc/>
        public bool RemoveType<T>()
            => RemoveType(typeof(T));

        /// <inheritdoc/>
        public bool RemoveType(Type type)
            => _refDictionary.Remove(type);

        /// <inheritdoc/>
        public void AddOrReplace<T>(string id, T value)
        {
            if (TryAdd(id, value))
                return;
            if (_refDictionary.TryGetValue(typeof(T), out var dictionary))
                dictionary[id] = value;
        }

        /// <inheritdoc/>
        public IInjectable Inject(IInjectable target)
        {
            var type = target.GetType();
            var fields = type
                         .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                         .Where(field => field.FieldType.IsSubclassOf(typeof(InjectedField)));
            foreach (var field in fields)
            {
                var argumentType = field.FieldType.GenericTypeArguments.FirstOrDefault();
                if (field.GetValue(target) is not InjectedField injectedField)
                    continue;
                if (argumentType != null)
                    InjectField(injectedField, argumentType, injectedField.Id);
            }

            target.HandleInjection();
            return target;
        }

        /// <summary>
        /// Searches for the property <see cref="InjectedField{T}.Value"/> and tries to find an entry for it.
        /// If an entry is found, the value will be injected into the target field.
        /// </summary>
        /// <param name="target">The field to inject</param>
        /// <param name="type">Type of the entry. 1st dimension filter.</param>
        /// <param name="id">ID for the entry. 2nd dimension filter.</param>
        private void InjectField(InjectedField target, Type type, string id)
        {
            //We give a random type because we just need the name of the field.
            const string propertyName = nameof(InjectedField<int>.Value);
            var property = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (property != null
                && _refDictionary.TryGetValue(type, out var valuesDictionary)
                && valuesDictionary.TryGetValue(id, out var value))
                property.SetValue(target, value);
        }
    }
}