using System;

namespace Neovolve.Windows.Forms.Controls
{
    /// <summary>
    /// The <see cref="WizardPageDictionaryEventArgs"/> class contains information about a
    /// <see cref="WizardPage"/>class as it is added and removed from the dictionary.
    /// </summary>
    public class WizardPageDictionaryEventArgs : EventArgs
    {
        /// <summary>
        /// Stores the key of the item in the <see cref="WizardPageDictionary"/> class.
        /// </summary>
        private readonly String _key;

        /// <summary>
        /// Stores the page instance that is contained in the <see cref="WizardPageDictionary"/> class.
        /// </summary>
        private readonly WizardPage _page;

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardPageDictionaryEventArgs"/> class.
        /// </summary>
        /// <param name="key">
        /// The page key.
        /// </param>
        /// <param name="page">
        /// The wizard page.
        /// </param>
        public WizardPageDictionaryEventArgs(String key, WizardPage page)
        {
            _key = key;
            _page = page;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The page key.
        /// </value>
        public String Key
        {
            get
            {
                return _key;
            }
        }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <value>
        /// The wizard page.
        /// </value>
        public WizardPage Page
        {
            get
            {
                return _page;
            }
        }
    }
}