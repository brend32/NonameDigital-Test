using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data
{
	public class ModsData
	{
		[JsonProperty("mods")]
		public List<ModPostData> Mods { get; set; } = new();
		[JsonProperty("categories")] 
		public List<string> Categories { get; set; } = new();
	}
}