namespace Services.Dtos.Shared
{
    public interface IPagedResult<T>
    {
        int PageIndex { get; }

        int PageSize { get; }

        int TotalCount { get; }

        int TotalPages { get; }

        T[] Items { get; }

        bool HasPreviousPage { get; }

        bool HasNextPage { get; }

        object ExtraData { get; set; }
    }
}
