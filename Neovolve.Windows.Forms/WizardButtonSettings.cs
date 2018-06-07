using System;
using System.ComponentModel;

namespace Neovolve.Windows.Forms
{
    /// <summary>
    /// The <see cref="Neovolve.Windows.Forms.WizardButtonSettings"/> class stores the settings for a button control in the
    /// <see cref="Neovolve.Windows.Forms.WizardForm"/>form.
    /// </summary>
    [TypeConverter(typeof(WizardButtonSettingsTypeConverter))]
    public class WizardButtonSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Neovolve.Windows.Forms.WizardButtonSettings"/> class.
        /// </summary>
        /// <param name="textValue">
        /// The text value.
        /// </param>
        public WizardButtonSettings(String textValue)
            : this(textValue, true, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Neovolve.Windows.Forms.WizardButtonSettings"/> class.
        /// </summary>
        /// <param name="textValue">
        /// The text value.
        /// </param>
        /// <param name="enabledValue">
        /// If set to 
        /// <item>
        /// True
        /// </item>
        /// [enabled value].
        /// </param>
        public WizardButtonSettings(String textValue, Boolean enabledValue)
            : this(textValue, enabledValue, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Neovolve.Windows.Forms.WizardButtonSettings"/> class.
        /// </summary>
        /// <param name="textValue">
        /// The text value.
        /// </param>
        /// <param name="enabledValue">
        /// If set to 
        /// <item>
        /// True
        /// </item>
        /// [enabled value].
        /// </param>
        /// <param name="visibleValue">
        /// If set to 
        /// <item>
        /// True
        /// </item>
        /// [visible value].
        /// </param>
        public WizardButtonSettings(String textValue, Boolean enabledValue, Boolean visibleValue)
        {
            Text = textValue;
            Enabled = enabledValue;
            Visible = visibleValue;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public WizardButtonSettings Clone()
        {
            // Return the new setting object
            return new WizardButtonSettings(Text, Enabled, Visible);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Neovolve.Windows.Forms.WizardButtonSettings"/> is enabled.
        /// </summary>
        /// <value>
        /// <item>
        /// True
        /// </item>
        /// if enabled; otherwise, 
        /// <item>
        /// False
        /// </item>
        /// .
        /// </value>
        [Category("Display")]
        [Description("Determines whether the button is enabled.")]
        [NotifyParentProperty(true)]
        [DefaultValue(true)]
        public Boolean Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The button text.
        /// </value>
        [Category("Display")]
        [Description("Determines the text of the button.")]
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        public String Text
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Neovolve.Windows.Forms.WizardButtonSettings"/> is visible.
        /// </summary>
        /// <value>
        /// <item>
        /// True
        /// </item>
        /// if visible; otherwise, 
        /// <item>
        /// False
        /// </item>
        /// .
        /// </value>
        [Category("Display")]
        [Description("Determines whether the button is visible.")]
        [NotifyParentProperty(true)]
        [DefaultValue(true)]
        public Boolean Visible
        {
            get;
            set;
        }
    }
}