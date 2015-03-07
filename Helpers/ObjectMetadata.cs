using System;
using log4net;
using System.Linq.Expressions;

namespace RestApiWrapUtility.Helpers
{
    /// <summary>
    /// This class is a helper utility which is used for extracting metadata from C# objects.
    /// </summary>
    public static class ObjectMetadata
    {
        #region Private Static Variables

        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(ObjectMetadata));

        #endregion

        #region Public Static Methods

        /// <summary>
        /// This method is used for extracting the member name of the C# object contained in <paramref name="memberExpression"/>.
        /// </summary>
        ///
        /// <param name="memberExpression">The member expression which contains a C# object.</param>
        /// 
        /// <returns>The name of the member.</returns>
        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            if (memberExpression == null)
            {
                LOGGER.ErrorFormat(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, "memberExpression"));

                throw new ArgumentException(string.Format(Constants.CANNOT_BE_NULL_ERROR_MESSAGE_FORMAT, "memberExpression"));
            }

            var expressionBody = (MemberExpression)memberExpression.Body;

            return expressionBody.Member.Name;
        }

        #endregion
    }
}
