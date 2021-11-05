using System.Collections.Generic;
using System.Reflection;

namespace MVVM
{
    public abstract class ComputedViewModelBase : ViewModelBase
    {
        public ComputedViewModelBase()
        {
            var properties = new Dictionary<string, HashSet<string>>();
            foreach (var property in GetType().GetTypeInfo().DeclaredProperties)
            {
                var computedAttribute = property.GetCustomAttribute<PropertySourceAttribute>();
                if (computedAttribute == null)
                {
                    continue;
                }

                foreach (var sourceName in computedAttribute.Sources)
                {
                    if (!properties.ContainsKey(sourceName))
                    {
                        properties[sourceName] = new HashSet<string>();
                    }

                    properties[sourceName].Add(property.Name);
                }
            }

            PropertyChanged += (sender, e) =>
            {
                if (properties.ContainsKey(e.PropertyName))
                {
                    foreach (var computedPropertyName in properties[e.PropertyName])
                    {
                        RaisePropertyChanged(computedPropertyName);
                    }
                }
            };
        }
    }
}