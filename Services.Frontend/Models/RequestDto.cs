using static Services.Frontend.Utility.SD;

namespace Services.Frontend.Models
{
	public class RequestDto
	{
        public ApiType ApiType { get; set; } = ApiType.GET;

        public string Url { get; set; }

        public object Data { get; set; }

        public string AccessToken { get; set; }
    }
}
