using System;
using System.ComponentModel;
using System.Globalization;
using Neovolve.Windows.Forms.Properties;

namespace Neovolve.Windows.Forms
{
    /// <summary>
    /// The <see cref="Neovolve.Windows.Forms.WizardButtonSettingsTypeConverter"/>
    /// class is used to convert the
    /// <see cref="Neovolve.Windows.Forms.WizardButtonSettings"/>class to and from other types.
    /// </summary>
    public class WizardButtonSettingsTypeConverter : ExpandableObjectConverter
    {
        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.
        /// </param>
        /// <param name="sourceType">
        /// A <see cref="T:System.Type"></see> that represents the type you want to convert from.
        /// </param>
        /// <returns>
        /// True if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType.Equals(typeof(String)))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Returns whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.
        /// </param>
        /// <param name="destinationType">
        /// A <see cref="T:System.Type"></see> that represents the type you want to convert to.
        /// </param>
        /// <returns>
        /// True if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override Boolean CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType.Equals(typeof(String)))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.
        /// </param>
        /// <param name="culture">
        /// The <see cref="T:System.Globalization.CultureInfo"></see> to use as the current culture.
        /// </param>
        /// <param name="value">
        /// The <see cref="Object"/> to convert.
        /// </param>
        /// <returns>
        /// An <see cref="Object"/> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The conversion cannot be performed. 
        /// </exception>
        public override Object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, Object value)
        {
            String newValue = value as String;

            if (newValue != null)
            {
                // Check if the value exists
                if (String.IsNullOrEmpty(newValue))
                {
                    throw new NotSupportedException(
                        String.Format(
                            CultureInfo.InvariantCulture,
                            Resources.FailedToConvertType,
                            typeof(String).FullName,
                            typeof(WizardButtonSettings).FullName));
                }

                // Split the parts of the value
                String[] parts = newValue.Split(
                    new[]
                        {
                            ','
                        });

                // Get the text of the value
                String text = parts[0];

                Boolean enabled = true;

                // Check if there is a valid enabled value
                if (parts.Length > 1)
                {
                    String enabledValue = parts[1].ToUpperInvariant();

                    if (enabledValue == "DISABLED")
                    {
                        enabled = false;
                    }
                    else if (Boolean.TryParse(enabledValue, out enabled) == false)
                    {
                        enabled = true;
                    }
                }

                Boolean visible = true;

                // Check if there is a valid visible value
                if (parts.Length > 2)
                {
                    String visibleValue = parts[2].ToUpperInvariant();

                    if (visibleValue == "INVISIBLE")
                    {
                        visible = false;
                    }
                    else if (Boolean.TryParse(visibleValue, out visible) == false)
                    {
                        visible = true;
                    }
                }

                // Return the new object
                return new WizardButtonSettings(text, enabled, visible);
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.
        /// </param>
        /// <param name="culture">
        /// A <see cref="T:System.Globalization.CultureInfo"></see>. If null is passed, the current culture is assumed.
        /// </param>
        /// <param name="value">
        /// The <see cref="Object"/> to convert.
        /// </param>
        /// <param name="destinationType">
        /// The <see cref="T:System.Type"></see> to convert the value parameter to.
        /// </param>
        /// <returns>
        /// An <see cref="Object"/> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The conversion cannot be performed. 
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// The destinationType parameter is null. 
        /// </exception>
        public override Object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, Object value, Type destinationType)
        {
            if (destinationType.Equals(typeof(String)))
            {
                WizardButtonSettings settings = (WizardButtonSettings)value;

                String convertedValue = settings.Text;

                convertedValue += ", " + (settings.Enabled ? "Enabled" : "Disabled");
                convertedValue += ", " + (settings.Visible ? "Visible" : "Invisible");

                return convertedValue;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}