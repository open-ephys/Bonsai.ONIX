using Bonsai.Expressions;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bonsai.ONIX
{
    class ONIHardwareSlotTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var stringValue = value as string;
            object result = null;

            if (!string.IsNullOrEmpty(stringValue))
            {
                var matches = Regex.Match(stringValue, @"(?<=\().+?(?=\))").Value.Split(',');

                if (matches.Length == 2)
                {
                    var driver = matches[0];
                    var idx = Convert.ToInt32(matches[1]);

                    result = new ONIHardwareSlot { Driver = driver, Index = idx };
                }

            }

            return result ?? base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object result = null;

            if (value is ONIHardwareSlot slot && destinationType == typeof(string))
            {
                result = slot.ToString();
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context != null)
            {
                var workflowBuilder = (WorkflowBuilder)context.GetService(typeof(WorkflowBuilder));
                if (workflowBuilder != null)
                {
                    var hw_slots = (from builder in workflowBuilder.Workflow.Descendants()
                                    let ctx_config = ExpressionBuilder.GetWorkflowElement(builder) as ONIContext
                                    where ctx_config != null && !string.IsNullOrEmpty(ctx_config.ContextConfiguration.Slot.Driver)
                                    select ctx_config.ContextConfiguration.Slot)
                                     .Concat(ONIContextManager.LoadConfiguration())
                                     .Distinct()
                                     .ToList();

                    return new StandardValuesCollection(hw_slots);
                }
            }

            return base.GetStandardValues(context);
        }
    }
}




