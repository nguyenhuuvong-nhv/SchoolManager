namespace Services.Dtos.Shared
{
    public class PagedResult<T> : IPagedResult<T>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public T[] Items { get; set; }

        public bool HasPreviousPage => PageIndex > 0;

        public bool HasNextPage => PageIndex + 1 < TotalPages;

        public object ExtraData { get; set; }

        public PagedResult() => Items = new T[0];
    }

    public static class EmptyPagedResult<T>
    {
        public static readonly IPagedResult<T> Instance = new PagedResult<T>();
    }
}
