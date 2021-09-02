namespace TecWi_Web.Shared.Filters
{
    public class FilterBase
    {
        public int Page { get; set; }
        public int PageSize { get; set; } = 100;
        public int PageNumber { get; set; }
        public string Order { get; set; }
        public bool OrderAscending { get; set; }
    }
}
