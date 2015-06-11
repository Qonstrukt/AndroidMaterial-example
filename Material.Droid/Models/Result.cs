using System;
using Newtonsoft.Json;

namespace Material.Droid
{
	[JsonObject (MemberSerialization.OptIn)]
	public class Result<T>
	{
		[JsonProperty ("data")]
		public T Data { get; set; }
	}
}

