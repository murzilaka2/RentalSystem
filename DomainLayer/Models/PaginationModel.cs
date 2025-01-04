namespace RentalSystem.Models
{
    public class PaginationModel
    {
        public int TotalItems { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public string PageUrl { get; set; }
        public string Filter { get; set; }
        public string Status { get; set; }
        public string? SortItem { get; set; }
        public bool Desc { get; set; } = false;
        public decimal PriceRange { get; set; } = 1000;
        public List<string>? CarTypes { get; set; }
        public List<int>? CarTypesInt { get; set; }
        public List<string>? CarBrands { get; set; }
        public int? DealerId { get; set; }
        public int FirstItemIndex => (Page - 1) * PageSize + 1;
        public int LastItemIndex => Math.Min(Page * PageSize, TotalItems);

        public int[] PageSizes { get; set; } = { 5, 10, 25, 50, 100 };
        public string[] SelectOptions { get; set; }

        public PaginationModel(int totalItems, int currentPage, int pageSize, string pageUrl, string filter, string status)
        {
            TotalItems = totalItems;
            Page = currentPage;
            PageSize = pageSize;
            PageUrl = pageUrl;
            Filter = filter;
            Status = status;
        }
        public PaginationModel()
        {

        }

        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
    }
}
