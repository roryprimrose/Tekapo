namespace Neovolve.Windows.Forms
{
    using System;
    using System.Collections.Specialized;
    using System.Runtime.Serialization;

    /// <summary>
    ///     The <see cref="StateCollection" />
    ///     class is used to provide a state persistence implementation.
    /// </summary>
    [Serializable]
    public class StateCollection : NameObjectCollectionBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StateCollection" /> class.
        /// </summary>
        public StateCollection()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StateCollection" /> class.
        /// </summary>
        /// <param name="info">
        ///     A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to
        ///     serialize the new <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.
        /// </param>
        /// <param name="context">
        ///     A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains the source and destination of
        ///     the serialized stream associated with the new
        ///     <see cref="T:System.Collections.Specialized.NameObjectCollectionBase" /> instance.
        /// </param>
        protected StateCollection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///     Gets or sets the <see cref="System.Object" /> with the specified key.
        /// </summary>
        /// <param name="key">
        ///     The key for the item in the collection.
        /// </param>
        /// <value>
        ///     An <see cref="object" /> instance or <c>null</c> if the key does not exist.
        /// </value>
        public object this[string key] { get => BaseGet(key); set => BaseSet(key, value); }

        /// <summary>
        ///     Gets or sets the <see cref="System.Object" /> at the specified index.
        /// </summary>
        /// <param name="index">
        ///     The index of the item in the collection.
        /// </param>
        /// <value>
        ///     An <see cref="object" /> instance.
        /// </value>
        public object this[int index] { get => BaseGet(index); set => BaseSet(index, value); }
    }
}