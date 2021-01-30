using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace dotnet_utils.Rest
{
    public class JsonUtils<T1, T2>
    {
        private readonly T1 _request;
        private readonly string _url;

        public JsonUtils(T1 Request, string url)
        {
            _request = Request;
            _url = url;
        }

        public JsonUtils(string url)
        {
            _url = url;
        }

        public T2 Call()
        {
            string json = JsonConvert.SerializeObject(_request, Formatting.Indented);

            using var client = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = client.PostAsync(_url, content).Result;

            var responseObj = JsonConvert.DeserializeObject<T2>(result.Content.ReadAsStringAsync().Result);

            if (result.IsSuccessStatusCode)
            {
                return responseObj;
            }
            else
            {
                return default;
            }
        }

        public T2 Get()
        {
            using var client = new HttpClient();
            var result = client.GetAsync(_url).Result;

            var responseObj = JsonConvert.DeserializeObject<T2>(result.Content.ReadAsStringAsync().Result);

            if (result.IsSuccessStatusCode)
            {
                return responseObj;
            }
            else
            {
                return default;
            }
        }
    }
}
