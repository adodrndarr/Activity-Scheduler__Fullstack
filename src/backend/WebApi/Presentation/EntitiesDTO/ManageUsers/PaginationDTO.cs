namespace ActivityScheduler.Presentation.EntitiesDTO
{
    public class PaginationDTO
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public bool IncludeAdmin { get; set; }
        public string IncludeName { get; set; }
    }
}
