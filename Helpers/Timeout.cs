using System;
using log4net;

namespace RestApiWrapUtility.Helpers
{
    /// <summary>
    /// This class is used for specifying a Timeout.
    /// </summary>
    public class Timeout
    {
        #region Private Static Variables

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(Timeout));

        #endregion

        #region Private Constant Variables

        private const string TIMEOUT_VALIDATION_MESSAGE = "The Timeout is measured in milliseconds and must be greater than zero!";

        #endregion

        #region Public Constant Variables

        public const int DEFAULT_TIMEOUT_MILLISECONDS = 45000;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor method for creating <see cref="Timeout"/> with <paramref name="milliseconds"/>.
        /// </summary>
        ///
        /// <param name="milliseconds">The <see cref="Milliseconds"/>.</param>
        public Timeout(int milliseconds)
        {
            SetMilliseconds(milliseconds);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to set safely the value of <see cref="Milliseconds"/>.
        /// </summary>
        ///
        /// <param name="milliseconds">The <see cref="Milliseconds"/>.</param>
        public void SetMilliseconds(int milliseconds)
        {
            ValidateMilliseconds(milliseconds);

            Milliseconds = milliseconds;
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// This method is used for validating the value of <paramref name="milliseconds"/>.
        /// </summary>
        ///
        /// <param name="milliseconds">The <see cref="Milliseconds"/>.</param>
        private static void ValidateMilliseconds(int milliseconds)
        {
            if (milliseconds <= 0)
            {
                LOGGER.Error(TIMEOUT_VALIDATION_MESSAGE);

                throw new ArgumentException(TIMEOUT_VALIDATION_MESSAGE);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The Timeout Milliseconds.
        /// </summary>
        public int Milliseconds { get; private set; }

        #endregion
    }
}
