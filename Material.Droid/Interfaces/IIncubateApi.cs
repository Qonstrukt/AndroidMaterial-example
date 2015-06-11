using System;
using System.Threading.Tasks;
using Refit;

namespace Material.Droid
{
	public interface IIncubateApi
	{
		[Get ("/news-items")]
		Task<Result<NewsItemList>> GetNewsItems ();
	}
}

