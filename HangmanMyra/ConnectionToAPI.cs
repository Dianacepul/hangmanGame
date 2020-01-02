using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HangmanMyra
{
    internal class ConnectionToAPI
    {
        private static HttpClient client = new HttpClient() { BaseAddress = new Uri("https://api.datamuse.com/") };

        public async Task<List<string>> GetWord(string topics)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("words?topics=" + topics);
            if (response.IsSuccessStatusCode)
            {
                JArray result = JsonConvert.DeserializeObject<JArray>(await response.Content.ReadAsStringAsync());

                return result.Select(i => (string)i["word"]).Where(i => i != null).ToList();
            }
            return null;
        }
    }
}