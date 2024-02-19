using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PtheroDemo.Domain.Shared.Base
{
    public class CustomPropertySelector : IPropertySelector
    {
        public bool InjectProperty(PropertyInfo propertyInfo, object instance)
        {
            return propertyInfo.CustomAttributes.Any(t => t.AttributeType == typeof(PropertyInjectAttribute));
        }
    }
}
