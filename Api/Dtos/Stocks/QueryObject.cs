namespace Api.Dtos.Stocks
{
    public class QueryObject
    {
        public string? SearchValue { get; set; } = string.Empty;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
