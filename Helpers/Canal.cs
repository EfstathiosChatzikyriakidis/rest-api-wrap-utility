namespace RestApiWrapUtility.Helpers
{
    /// <summary>
    /// This class is used for specifying a Canal.
    /// </summary>
    public class Canal
    {
        #region Known Values

        /// <summary>
        /// Canal for Android.
        /// </summary>
        public static Canal ANDROID = new Canal (1);

        /// <summary>
        /// Canal for iPhone.
        /// </summary>
        public static Canal IPHONE = new Canal (2);

        /// <summary>
        /// Canal for Web.
        /// </summary>
        public static Canal WEB = new Canal (3);

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor method for creating <see cref="Canal"/> with <paramref name="id"/>.
        /// </summary>
        /// 
        /// <param name="id">The <see cref="Id"/>.</param>
        private Canal(byte id)
        {
            Id = id;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The Canal Id.
        /// </summary>
        public byte Id { get; private set; }

        #endregion
    }
}
