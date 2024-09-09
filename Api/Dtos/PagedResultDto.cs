namespace Api.Dtos
{
    public class PagedResultDto<T>
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public List<T> Items { get; set; }


        public PagedResultDto(int page, int totalCount, int pageSize, List<T> items)
        {
            Page = page;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling((double)totalCount / (double)pageSize);
            PageSize = pageSize;
            HasPreviousPage = page < 1;
            HasNextPage = page < TotalPages;
            Items = items;
        }
    }
}
