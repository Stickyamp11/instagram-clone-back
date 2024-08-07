namespace Instagram_Api.Application.Publications.Models
{
    public class PaginatedList<T>
    {
        public PaginatedList(List<T> items, int range_low, int range_max, int pageSize, int totalCount)
        {
            Items = items;
            //Page = page;
            Range_low = range_low;
            Range_max = range_max;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public List<T> Items { get; set; }

        //public int Page { get; }
        public int Range_low { get; set; }
        public int Range_max { get; set; }

        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        //public bool HasNextPage => Page * PageSize < TotalCount;
        //public bool HasPreviousPage => Page > 1;

    }
}
