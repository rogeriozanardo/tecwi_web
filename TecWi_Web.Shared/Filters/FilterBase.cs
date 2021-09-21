namespace TecWi_Web.Shared.Filters
{
    public class FilterBase
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public string Order { get; set; }
        public bool OrderAscending { get; set; }
    }
}
