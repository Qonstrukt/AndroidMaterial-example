using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Material.Droid
{
	[JsonArray]
	public class NewsItemList : List<NewsItem>
	{
	}

	[JsonObject (MemberSerialization.OptIn)]
	public class NewsItem
	{
		[JsonProperty ("id")]
		public string Id { get; set; }

		[JsonProperty ("title")]
		public string Title { get; set; }

		[JsonProperty ("date")]
		public DateTime Date { get; set; }

		[JsonProperty ("text")]
		public string Text { get; set; }

		[JsonProperty ("image")]
		public string Image { get; set; }


		public override string ToString ()
		{
			return Title;
		}
	}
}

