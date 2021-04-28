using System;
using System.Collections.Generic;
using System.Linq;


namespace ActivityScheduler.Presentation.EntitiesDTO
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<T> items, int pageNumber, int pageSize, int count)
        {
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;

            AddRange(items);
        }

        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize) 
        {
            int itemsToSkip = ((pageNumber - 1) * pageSize);
            itemsToSkip = itemsToSkip < 0 ? 0 : itemsToSkip;

            var count = source.Count();
            var items = source.Skip(itemsToSkip)
                              .Take(pageSize)
                              .ToList();

            return new PagedList<T>(items, pageNumber, pageSize, count);
        }

        public PagedList() { }
    }
}
