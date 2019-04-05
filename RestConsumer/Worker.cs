using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ModelLibrary.Model;
using Newtonsoft.Json;

namespace RestConsumer
{
    internal class Worker
    {

        private const string URI = "http://localhost:62917/api/hotels";

        public Worker()
        {

        }

        public void Start()
        {
            List<Hotel> hotels = GetAll();


            foreach (var hotel in hotels)
            {
                Console.WriteLine("Hotel:: " + hotel);
            }


        }

        private List<Hotel> GetAll()
        {
            List<Hotel> hoteller = new List<Hotel>();

            using (HttpClient client = new HttpClient())
            {
                Task<string> resTask = client.GetStringAsync(URI);
                string jsonStr = resTask.Result;

                hoteller = JsonConvert.DeserializeObject<List<Hotel>>(jsonStr);
            }

            return hoteller;
        }

        private Hotel GetOne(int id)
        {
            Hotel hotel = new Hotel();

            using (HttpClient client = new HttpClient())
            {
                Task<string> resTask = client.GetStringAsync(URI + "/" + id);
                string jsonStr = resTask.Result;

                hotel = JsonConvert.DeserializeObject<Hotel>(jsonStr);
            }

            return hotel;
        }

        private bool Delete(int id)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                Task<HttpResponseMessage> deleteAsync = client.DeleteAsync(URI + "/" + id);

                HttpResponseMessage resp = deleteAsync.Result;
                if (resp.IsSuccessStatusCode)
                {
                    string jsonStr = resp.Content.ReadAsStringAsync().Result;
                    ok = JsonConvert.DeserializeObject<bool>(jsonStr);
                }
                else
                {
                    ok = false;
                }
            }

            return ok;
        }

        private bool Post(Hotel hotel)
        {
            bool ok = true;

            using (HttpClient client = new HttpClient())
            {
                string jsonStr = JsonConvert.SerializeObject(hotel);
                StringContent content = new StringContent(jsonStr, Encoding.ASCII, "application/json");
                Task<HttpResponseMessage> postAsync = client.PostAsync(URI, content);
                 
                HttpResponseMessage resp = postAsync.Result;
                if (resp.IsSuccessStatusCode)
                {
                    string jsonResStr = resp.Content.ReadAsStringAsync().Result;
                    ok = JsonConvert.DeserializeObject<bool>(jsonStr);
                }
                else
                {
                    ok = false;
                }
            }

            return ok;
        }
    }
}
