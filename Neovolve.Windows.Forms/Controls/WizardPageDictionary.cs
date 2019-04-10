namespace Neovolve.Windows.Forms.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Neovolve.Windows.Forms.Properties;

    /// <summary>
    ///     The <see cref="WizardPageDictionary" /> class contains a set of
    ///     <see cref="WizardPage" />controls that are loaded by a
    ///     <see cref="Neovolve.Windows.Forms.WizardForm" />window.
    /// </summary>
    public class WizardPageDictionary : IDictionary<string, WizardPage>, IComponent
    {
        /// <summary>
        ///     Stores the collection of pages.
        /// </summary>
        private readonly Dictionary<string, WizardPage> _store;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WizardPageDictionary" /> class.
        /// </summary>
        public WizardPageDictionary()
        {
            // Create the dictionary
            _store = new Dictionary<string, WizardPage>();
        }

        /// <inheritdoc />
        public event EventHandler Disposed;

        /// <summary>
        ///     Raised when a <see cref="WizardPage" /> control is added to the
        ///     <see cref="WizardPageDictionary" />class.
        /// </summary>
        public event EventHandler<WizardPageDictionaryEventArgs> PageAdded;

        /// <summary>
        ///     Raised when a <see cref="WizardPage" /> control is removed from the
        ///     <see cref="WizardPageDictionary" />class.
        /// </summary>
        public event EventHandler<WizardPageDictionaryEventArgs> PageRemoved;

        /// <summary>
        ///     Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">
        ///     The object to use as the key of the element to add.
        /// </param>
        /// <param name="value">
        ///     The object to use as the value of the element to add.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     The <paramref name="key" /> value is <c>null</c>.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.IDictionary`2" /> is read-only.
        /// </exception>
        public void Add(string key, WizardPage value)
        {
            // Check if there is a key value
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            // Check if there is a page
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            // Add the page to the store
            _store.Add(key, value);

            // Raise the event
            OnPageAdded(new WizardPageDictionaryEventArgs(key, value));
        }

        /// <summary>
        ///     Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">
        ///     The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </param>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </exception>
        public void Add(KeyValuePair<string, WizardPage> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        ///     Adds the specified page.
        /// </summary>
        /// <param name="key">
        ///     The page key.
        /// </param>
        /// <param name="page">
        ///     The wizard page.
        /// </param>
        /// <param name="settings">
        ///     The page settings.
        /// </param>
        public void Add(string key, WizardPage page, WizardPageSettings settings)
        {
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            // Add the settings to the page
            page.PageSettings = settings;

            // Add the page to the collection
            Add(key, page);
        }

        /// <summary>
        ///     Adds the specified page.
        /// </summary>
        /// <param name="key">
        ///     The page key.
        /// </param>
        /// <param name="page">
        ///     The wizard page.
        /// </param>
        /// <param name="settings">
        ///     The page settings.
        /// </param>
        /// <param name="navigationSettings">
        ///     The navigation settings.
        /// </param>
        public void Add(
            string key,
            WizardPage page,
            WizardPageSettings settings,
            WizardPageNavigationSettings navigationSettings)
        {
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            // Add the settings to the page
            page.PageSettings = settings;
            page.NavigationSettings = navigationSettings;

            // Add the page to the collection
            Add(key, page);
        }

        /// <summary>
        ///     Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        public void Clear()
        {
            _store.Clear();
        }

        /// <summary>
        ///     Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <param name="item">
        ///     The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </param>
        /// <returns>
        ///     True if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<string, WizardPage> item)
        {
            return _store.ContainsKey(item.Key) && _store.ContainsValue(item.Value);
        }

        /// <summary>
        ///     Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the
        ///     specified key.
        /// </summary>
        /// <param name="key">
        ///     The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </param>
        /// <returns>
        ///     <c>true</c>if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key;
        ///     otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     The <paramref name="key" /> value is <c>null</c>.
        /// </exception>
        public bool ContainsKey(string key)
        {
            return _store.ContainsKey(key);
        }

        /// <summary>
        ///     Determines whether the specified value contains value.
        /// </summary>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <returns>
        ///     <item>
        ///         True
        ///     </item>
        ///     if the specified value contains value; otherwise,
        ///     <item>
        ///         False
        ///     </item>
        ///     .
        /// </returns>
        public bool ContainsValue(WizardPage value)
        {
            return _store.ContainsValue(value);
        }

        /// <summary>
        ///     Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an
        ///     <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from
        ///     <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have
        ///     zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        ///     The zero-based index in array at which copying begins.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     ArrayIndex is less than 0.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     Array is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     Array is multidimensional.
        ///     -or-
        ///     arrayIndex is equal to or greater than the length of array
        ///     .-or-
        ///     The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than
        ///     the available space from arrayIndex to the end of the destination array.
        ///     -or-
        ///     Type T cannot be cast automatically to the type of the destination array.
        /// </exception>
        public void CopyTo(KeyValuePair<string, WizardPage>[] array, int arrayIndex)
        {
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }

            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (arrayIndex >= array.Length)
            {
                throw new ArgumentException("The arrayIndex value is greater than or equal to the length of the array");
            }

            if (arrayIndex + Count >= array.Length)
            {
                throw new ArgumentException(
                    "The array size is not large enough to fit all items starting at the arrayIndex value");
            }

            // Loop through each page
            foreach (var key in Keys)
            {
                // Get the page for the key
                var page = this[key];

                // Create the pair for the array
                var pair = new KeyValuePair<string, WizardPage>(key, page);

                // Add the pair to the array
                array[arrayIndex] = pair;

                // Increment the index
                arrayIndex++;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<string, WizardPage>> GetEnumerator()
        {
            return _store.GetEnumerator();
        }

        /// <summary>
        ///     Gets the next page from the collection.
        /// </summary>
        /// <param name="page">
        ///     The wizard page.
        /// </param>
        /// <returns>
        ///     A <see cref="WizardPage" /> instance. Returns <c>null</c> if no relevant page exists.
        /// </returns>
        public WizardPage GetNextStoredPage(WizardPage page)
        {
            WizardPage previousItem = null;

            // Loop through each page
            foreach (var item in Values)
            {
                // Check if there isn't a page specified
                if (page == null)
                {
                    // Return the first page encountered
                    return item;
                }

                // Check if there is a previous item to check and that it matches the page being searched for
                if (previousItem != null
                    && previousItem.Equals(page))
                {
                    // Return the current item
                    return item;
                }

                // Store the current item for the next iteration
                previousItem = item;
            }

            // The page wasn't found
            return null;
        }

        /// <summary>
        ///     Raises the <see cref="PageAdded" /> event.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="WizardPageDictionaryEventArgs" /> instance containing the event data.
        /// </param>
        public void OnPageAdded(WizardPageDictionaryEventArgs e)
        {
            // Check if the event is handled
            if (PageAdded != null)
            {
                // Raise the event
                PageAdded(this, e);
            }
        }

        /// <summary>
        ///     Raises the <see cref="PageRemoved" /> event.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="WizardPageDictionaryEventArgs" /> instance containing the event data.
        /// </param>
        public void OnPageRemoved(WizardPageDictionaryEventArgs e)
        {
            // Check if the event is handled
            if (PageRemoved != null)
            {
                // Raise the event
                PageRemoved(this, e);
            }
        }

        /// <summary>
        ///     Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <param name="key">
        ///     The key of the element to remove.
        /// </param>
        /// <returns>
        ///     True if the element is successfully removed; otherwise, false.  This method also returns false if key was not found
        ///     in the original <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     Key is null.
        /// </exception>
        public bool Remove(string key)
        {
            // Get the page from the store
            var page = _store[key];

            // Remove the page
            var returnValue = _store.Remove(key);

            // Check if there are values for the event
            if (string.IsNullOrEmpty(key) == false
                && page != null)
            {
                // Raise the event
                OnPageRemoved(new WizardPageDictionaryEventArgs(key, page));
            }

            // Return whether the remove was successful
            return returnValue;
        }

        /// <summary>
        ///     Removes the first occurrence of a specific object from the
        ///     <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">
        ///     The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </param>
        /// <returns>
        ///     True if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>;
        ///     otherwise, false. This method also returns false if item is not found in the original
        ///     <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </exception>
        public bool Remove(KeyValuePair<string, WizardPage> item)
        {
            // Check if there is a key value supplied
            if (string.IsNullOrEmpty(item.Key))
            {
                throw new InvalidOperationException(Resources.FailedToRemovePageWithNoValidKey);
            }

            return Remove(item.Key);
        }

        /// <summary>
        ///     Tries the get value.
        /// </summary>
        /// <param name="key">
        ///     The page key.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <returns>
        ///     The try get value.
        /// </returns>
        public bool TryGetValue(string key, out WizardPage value)
        {
            return _store.TryGetValue(key, out value);
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _store.GetEnumerator();
        }

        /// <summary>
        ///     Disposes the instance.
        /// </summary>
        /// <param name="disposing"><c>true</c> if disposing managed resources; otherwise <c>false</c>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources

                foreach (var page in Values)
                {
                    page.Dispose();
                }

                Clear();

                Disposed?.Invoke(this, EventArgs.Empty);
            }

            // free native resources if there are any.
        }

        /// <summary>
        ///     Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <value>
        ///     The count.
        /// </value>
        /// <returns>
        ///     The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        public int Count => _store.Count;

        /// <summary>
        ///     Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        /// <value>
        ///     The is read only.
        /// </value>
        /// <returns>
        ///     True if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly => false;

        /// <summary>
        ///     Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <value>
        ///     The page keys.
        /// </value>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the object that implements
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </returns>
        public ICollection<string> Keys => _store.Keys;

        /// <inheritdoc />
        public ISite Site { get; set; }

        /// <summary>
        ///     Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the
        ///     <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <value>
        ///     The values.
        /// </value>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the object that
        ///     implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </returns>
        public ICollection<WizardPage> Values => _store.Values;

        /// <summary>
        ///     Gets or sets the <see cref="WizardPage" /> with the specified key.
        /// </summary>
        /// <param name="key">
        ///     The page key.
        /// </param>
        /// <value>
        ///     A <see cref="WizardPage" /> instance.
        /// </value>
        public WizardPage this[string key] { get => _store[key]; set => _store[key] = value; }
    }
}