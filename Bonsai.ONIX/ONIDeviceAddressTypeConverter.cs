using Bonsai.Expressions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bonsai.ONIX
{
    // TODO: This thing is a true nightmare, but its OK for now
    internal class ONIDeviceAddressTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {

            object result = null;
            var stringValue = value as string;

            if (!string.IsNullOrEmpty(stringValue))
            {
                var matches = ReverseStringFormat("{0}/{1}/{2}", stringValue);

                result = new ONIDeviceAddress
                {
                    HardwareSlot = new ONIHardwareSlot { Driver = matches[0], Index = Convert.ToInt32(matches[1]) },
                    Address = Convert.ToUInt32(matches[2])
                };

            }

            return result ?? base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object result = null;

            if (value is ONIDeviceAddress idx && destinationType == typeof(string))
            {
                result = idx.ToString();
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

                    // This device
                    var deviceattribute = context.PropertyDescriptor.ComponentType.GetCustomAttributes(typeof(ONIXDeviceIDAttribute), true).FirstOrDefault() as ONIXDeviceIDAttribute;
                    DeviceID deviceID = deviceattribute == null ? DeviceID.Null : deviceattribute.deviceID;

                    // To fill after inspecting hardware
                    var deviceAddresses = new List<ONIDeviceAddress>();

                    foreach (var hw in hw_slots)
                    {
                        try
                        {
                            using (var c = ONIContextManager.ReserveContext(hw))
                            {
                                // Find valid device indices
                                var dev_matches = c.Context.DeviceTable
                                    //.Where(dev => dev.Value.ID == (uint)device.ID)
                                    .Where(dev =>
                                    {
                                        return deviceID == DeviceID.Null ?
                                            dev.Value.ID != (uint)DeviceID.Null :
                                            dev.Value.ID == (uint)deviceID;
                                    })
                                    .Select(x =>
                                    {
                                        var d = new ONIDeviceAddress
                                        {
                                            HardwareSlot = hw,
                                            Address = x.Key
                                        };
                                        return d;
                                    }).ToList();

                                deviceAddresses = deviceAddresses.Concat(dev_matches).ToList();
                            }
                        }
                        catch (InvalidProgramException) // Bad context initialization
                        {
                            return base.GetStandardValues(context);
                        }
                        catch (oni.ONIException) // Something happened during hardware init
                        {
                            return base.GetStandardValues(context);
                        }
                    }

                    return deviceAddresses.Count == 0 ?
                        base.GetStandardValues(context) :
                        new StandardValuesCollection(deviceAddresses);
                }
            }

            return base.GetStandardValues(context);
        }

        // NB: Taken from https://stackoverflow.com/questions/5346158/parse-string-using-format-template
        private static List<string> ReverseStringFormat(string template, string str)
        {
            //Handles regex special characters.
            template = Regex.Replace(template, @"[\\\^\$\.\|\?\*\+\(\)]", match => "\\"
             + match.Value);

            string pattern = "^" + Regex.Replace(template, @"\{[0-9]+\}", "(.*?)") + "$";

            Regex r = new Regex(pattern);
            Match m = r.Match(str);

            List<string> ret = new List<string>();

            for (int i = 1; i < m.Groups.Count; i++)
            {
                ret.Add(m.Groups[i].Value);
            }

            return ret;
        }
    }
}




