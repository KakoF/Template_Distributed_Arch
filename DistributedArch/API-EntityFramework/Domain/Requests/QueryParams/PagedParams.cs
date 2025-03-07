namespace Domain.Requests.QueryParams
{
	public class PagedParams
	{

		public PagedParams()
		{
		}

		public PagedParams(PagedParams parameters)
		{
			Page = parameters.Page;
			PageSize = parameters.PageSize;
		}

		public int Page { get; set; } = 1;

		public int PageSize { get; set; } = 10;
		
	}
}
