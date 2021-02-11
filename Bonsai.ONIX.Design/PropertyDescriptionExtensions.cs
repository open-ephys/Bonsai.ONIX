using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Bonsai.ONIX.Design
{
    public static class PropertyDescriptorExtensions
    {
        public static void SetReadOnlyAttribute(this PropertyDescriptor p, bool value)
        {
            var attributes = p.Attributes.Cast<Attribute>()
                .Where(x => !(x is ReadOnlyAttribute)).ToList();
            attributes.Add(new ReadOnlyAttribute(value));
            typeof(MemberDescriptor).GetProperty("AttributeArray",
                BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(p, attributes.ToArray());
        }
    }
}
