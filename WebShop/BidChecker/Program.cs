
using DAL.Interfaces;
using DAL.Model;
using System.Net.Http.Json;

namespace BidChecker
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            Thread.Sleep(10000);

            HttpClient client = new HttpClient();

            bool isFirstRun = true;

            client.BaseAddress = new Uri("http://localhost:5285/api");

                var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));

                while (await timer.WaitForNextTickAsync())
                {
                    HttpResponseMessage responseMessage = await client.GetAsync("/api/itemList/GetAllItems");

                    var items = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<Item>>();

                if (isFirstRun)
                {
                    items.FirstOrDefault().ExpirationDate = DateTime.Now.AddSeconds(30);
                    await Console.Out.WriteLineAsync(items.FirstOrDefault().Title);
                    isFirstRun = false;

                }


                    
                foreach(var item in items)
                    {

                    if (DateTime.Now >= item.ExpirationDate && item.ExpirationDate != null && item.Sold != true)
                        {

                        client.GetAsync($"Shop/TimeoutBid/{item.ItemId}");

                        }                    
                    }    


                }
        }
    }
}
