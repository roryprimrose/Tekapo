namespace Neovolve.Windows.Forms
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using Neovolve.Windows.Forms.Properties;

    /// <summary>
    ///     The <see cref="Neovolve.Windows.Forms.WizardButtonSettingsTypeConverter" />
    ///     class is used to convert the
    ///     <see cref="Neovolve.Windows.Forms.WizardButtonSettings" />class to and from other types.
    /// </summary>
    public class WizardButtonSettingsTypeConverter : ExpandableObjectConverter
    {
        /// <summary>
        ///     Returns whether this converter can convert an object of the given type to the type of this converter, using the
        ///     specified context.
        /// </summary>
        /// <param name="context">
        ///     An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.
        /// </param>
        /// <param name="sourceType">
        ///     A <see cref="T:System.Type"></see> that represents the type you want to convert from.
        /// </param>
        /// <returns>
        ///     True if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == null)
            {
                throw new ArgumentNullException(nameof(sourceType));
            }

            if (sourceType.Equals(typeof(string)))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        ///     Returns whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <param name="context">
        ///     An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.
        /// </param>
        /// <param name="destinationType">
        ///     A <see cref="T:System.Type"></see> that represents the type you want to convert to.
        /// </param>
        /// <returns>
        ///     True if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException(nameof(destinationType));
            }

            if (destinationType.Equals(typeof(string)))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        ///     Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">
        ///     An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.
        /// </param>
        /// <param name="culture">
        ///     The <see cref="T:System.Globalization.CultureInfo"></see> to use as the current culture.
        /// </param>
        /// <param name="value">
        ///     The <see cref="Object" /> to convert.
        /// </param>
        /// <returns>
        ///     An <see cref="Object" /> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        ///     The conversion cannot be performed.
        /// </exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var newValue = value as string;

            if (newValue != null)
            {
                // Check if the value exists
                if (string.IsNullOrEmpty(newValue))
                {
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture,
                        Resources.FailedToConvertType,
                        typeof(string).FullName,
                        typeof(WizardButtonSettings).FullName));
                }

                // Split the parts of the value
                var parts = newValue.Split(',');

                // Get the text of the value
                var text = parts[0];

                var enabled = true;

                // Check if there is a valid enabled value
                if (parts.Length > 1)
                {
                    var enabledValue = parts[1].ToUpperInvariant();

                    if (enabledValue == "DISABLED")
                    {
                        enabled = false;
                    }
                    else if (bool.TryParse(enabledValue, out enabled) == false)
                    {
                        enabled = true;
                    }
                }

                var visible = true;

                // Check if there is a valid visible value
                if (parts.Length > 2)
                {
                    var visibleValue = parts[2].ToUpperInvariant();

                    if (visibleValue == "INVISIBLE")
                    {
                        visible = false;
                    }
                    else if (bool.TryParse(visibleValue, out visible) == false)
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
        ///     Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">
        ///     An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.
        /// </param>
        /// <param name="culture">
        ///     A <see cref="T:System.Globalization.CultureInfo"></see>. If null is passed, the current culture is assumed.
        /// </param>
        /// <param name="value">
        ///     The <see cref="Object" /> to convert.
        /// </param>
        /// <param name="destinationType">
        ///     The <see cref="T:System.Type"></see> to convert the value parameter to.
        /// </param>
        /// <returns>
        ///     An <see cref="Object" /> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        ///     The conversion cannot be performed.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     The destinationType parameter is null.
        /// </exception>
        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException(nameof(destinationType));
            }

            if (destinationType.Equals(typeof(string)))
            {
                var settings = (WizardButtonSettings) value;

                var convertedValue = settings.Text;

                convertedValue += ", " + (settings.Enabled ? "Enabled" : "Disabled");
                convertedValue += ", " + (settings.Visible ? "Visible" : "Invisible");

                return convertedValue;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}