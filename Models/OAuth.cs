namespace RestApiWrapUtility.Models
{
    /// <summary>
    /// This is the data class (POCO) that is used for storing the response information from OAuth.
    /// </summary>
    public class OAuth
    {
        #region Public Properties

        /// <summary>
        /// The OAuth Access Token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// The refresh OAuth Access Token.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// The type of the OAuth Access Token.
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// When the OAuth Access Token expires (seconds).
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Who has permission.
        /// </summary>
        public string Scope { get; set; }

        #endregion
    }
}
