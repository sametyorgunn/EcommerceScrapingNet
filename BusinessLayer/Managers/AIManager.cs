using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto.Product;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers
{
    public class AIManager : IAIService
    {
        public async Task<bool> isTrueProduct(isTrueProductDto dto)
        {
            string apiKey = "sk-svcacct-6SMeVkuXfE--CXzBbpNa5o4iteTtcNw5InEC13qua2xLqjp6breh6Zg3CM27dwFPi1T3BlbkFJrue9zE7FLhh-itM27kBqh5sX7CTq_dfkdywERrsmGB6h-fv2EIqkWFdhTxFk2z3RwA";
            string url = "https://api.openai.com/v1/chat/completions";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    //model = "gpt-4o", 
                    messages = new[]
                    {
                    new { role = "system", content = "Sen bir asistan olarak çalışıyorsun." },
                    new { role = "user", content = $"{dto.ProductName} bu ürün ile {dto.ProductNamePlatform} bu ürün aynı " +
                    $"ürün mü evet ise True, değil ise False yaz." }
                },
                    max_tokens = 150, 
                    temperature = 0.7
                };

                var jsonRequest = JsonConvert.SerializeObject(requestBody);
                var response = await client.PostAsync(url, new StringContent(jsonRequest, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Yanıt:");
                    Console.WriteLine(responseContent);

                    var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    string reply = responseObject.choices[0].message.content;
                    Console.WriteLine("ChatGPT: " + reply);
                    if(reply == "True")
                    {
                        return true;
                    }
                    else if(reply == "False")
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine($"Hata: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(errorContent);
                    return false;
                }
                return true;
            }
        }
    }
}
