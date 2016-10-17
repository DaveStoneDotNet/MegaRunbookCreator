using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

using STAR.Originations.MegaRunbook.Contracts.PagingModels;

namespace STAR.Originations.MRC.DataAccess.Query
{
    internal static class PagingExtensions
    {
        #region TakePage
        public static IQueryable<TSource> TakePage<TSource>(this IQueryable<TSource> source, Paging paging)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (paging == null)
            {
                return source;
            }

            if (paging.SortInfo != null && paging.SortInfo.Any())
            {
                var orderings = string.Join(", ", paging.SortInfo.Select((s, idx) => String.Format("{0} {1}", s.PropertyName, s.SortOrder == SortOrder.Ascending ? "ASC" : "DESC")));

                source = source.OrderBy(orderings);
            }

            return source.Skip(PagingExtensions.GetSkipCount(paging)).Take(paging.RecordsPerPage);
        }
        #endregion TakePage

        #region SanitizePaging
        public static Paging SanitizePaging(this Paging unsanitizedPaging)
        {
            if (unsanitizedPaging == null)
            {
                unsanitizedPaging = new Paging();
            }

            const int defaultPageSize = 10;

            var sanitizedPaging = new Paging
            {
                PageNumber = Math.Max(1, unsanitizedPaging.PageNumber),
                RecordsPerPage = unsanitizedPaging.RecordsPerPage == 0 ? defaultPageSize : Math.Max(1, unsanitizedPaging.RecordsPerPage),
                SortInfo = (unsanitizedPaging.SortInfo != null && unsanitizedPaging.SortInfo.Any()) ? unsanitizedPaging.SortInfo.ToList() : new List<SortInfo> { new SortInfo { PropertyName = "1", SortOrder = SortOrder.Ascending } }
            };

            return sanitizedPaging;
        }
        #endregion SanitizePaging

        #region ToPage
        public static Page<TSource> ToPage<TSource>(this IList<TSource> source, int totalRecordCount, Paging paging)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var skipCount = PagingExtensions.GetSkipCount(paging);
            var page = new Page<TSource>
            {
                HasNext = paging != null && totalRecordCount > 0 && skipCount + paging.RecordsPerPage < totalRecordCount,
                HasPrevious = paging != null && totalRecordCount > 0 && skipCount > 0,
                Items = source.ToList(),
                PageNumber = paging?.PageNumber ?? 1,
                RecordsPerPage = paging?.RecordsPerPage ?? 0,
                TotalPageCount = paging == null ? 1 : paging.RecordsPerPage == 0 ? 0 : (int)Math.Ceiling((float)totalRecordCount / (float)paging.RecordsPerPage),
                TotalRecordCount = totalRecordCount
            };

            return page;
        }
        #endregion ToPage

        #region GetSkipCount
        private static int GetSkipCount(Paging paging)
        {
            return (paging?.PageNumber - 1) * paging.RecordsPerPage ?? 0;
        }
        #endregion GetSkipCount

        #region OrderQuery
        public static IQueryable<TSource> OrderQuery<TSource>(this IQueryable<TSource> source, Paging paging)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (paging == null)
            {
                return source;
            }

            if (paging.SortInfo != null && paging.SortInfo.Any())
            {
                var orderings = string.Join(", ", paging.SortInfo.Select((s, idx) => String.Format("{0} {1}", s.PropertyName, s.SortOrder == SortOrder.Ascending ? "ASC" : "DESC")));

                source = source.OrderBy(orderings);
            }

            return source;
        }
        #endregion OrderQuery
    }
}
