namespace Api.Utils
{
    public class CountedResult<T>
    {
        public int TotalCount { get; set; }
        public List<T> Result { get; set; } = new List<T>();
    }
}
