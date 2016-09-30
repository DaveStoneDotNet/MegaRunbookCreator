using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace STAR.Originations.MegaRunbook.Contracts.PagingModels
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/Capital/Markets/2014/09")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class SortInfo
    {
        [DataMember] public string PropertyName { get; set; }
        [DataMember] public SortOrder Order { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => String.Format("{0}({1} {2})", this.GetType().Name, this.PropertyName, this.Order);

        #region Create
        public static SortInfo Create<TSource, TResult>(Expression<Func<TSource, TResult>> propertySelector, SortOrder order)
        {
            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            var memberExpression = propertySelector.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("Property selector expression must be a member expression", nameof(propertySelector));
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("Property selector expression must return a property", nameof(propertySelector));
            }

            return new SortInfo
            {
                PropertyName = propertyInfo.Name,
                Order = order
            };
        }
        #endregion Create
    }
}