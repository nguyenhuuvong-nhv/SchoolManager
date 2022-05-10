namespace Services.Dtos.Shared.Inputs
{
    public class PagingDto
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public PagingDto()
        {

        }

        public PagingDto(int page, int pageSize)
        {
            PageNumber = page;
            PageSize = pageSize;
        }
    }
}
