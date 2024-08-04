
using X.PagedList;

namespace Course_Overview.ViewModel
{
    public abstract class AbstractModel<T>
    {
        public AbstractModel()
        {
            Page = Page > 0 ? Page : 1;
            PageSize = PageSize > 0 ? PageSize : 15;
            SortExpression ??= "asc";
            Offset = (Page - 1) * PageSize;
        }
        public int Page { get; set; }
        public int Offset { get; set; }
        public int PageSize { get; set; }
        public string SortDirection { get; set; }
        public string SortExpression { get; set; }
        public long TotalItems { get; set; }
        public IEnumerable<T> ListResult { get; set; }
        public IPagedList<T> PagedList { get; set; }

    }
}
