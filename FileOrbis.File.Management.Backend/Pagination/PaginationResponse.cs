namespace FileOrbis.File.Management.Backend.Pagination
{
    public class PaginationResponse<T>
    {
        public List<T> Content { get; set; }
        public int TotalElement { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public bool Last { get; set; }
        public bool Exceed { get; set; }

        public PaginationResponse(List<T> content, int pageNo, int pageSize)
        {
            TotalElement = content.Count;
            PageNo = pageNo;

            if (TotalElement % pageSize == 0)
                TotalPages = TotalElement / pageSize;
            else
                TotalPages = TotalElement / pageSize + 1;

            if (TotalPages == pageNo)
                Last = true;
            else
                Last = false;

            Content = new List<T>();
            for (int i = ((pageNo - 1) * pageSize); i < (pageNo * pageSize); i++)
            {
                if (TotalElement > i)
                    Content.Add(content[i]);
            }
            PageSize = Content.Count;

            if (Content.Count == 0)
                Exceed = true;
            else
                Exceed = false;

        }

    }

}
