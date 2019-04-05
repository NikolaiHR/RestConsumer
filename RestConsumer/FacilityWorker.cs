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
    class FacilityWorker
    {
        
        

            private const string URI = "http://localhost:62917/api/facilities";

            public FacilityWorker()
            {

            }

            public void Start()
            {
                List<Facility> facilities = GetAll();


                foreach (var facility in facilities)
                {
                    Console.WriteLine("Facility:: " + facility);
                }


            }

            private List<Facility> GetAll()
            {
                List<Facility> facilities = new List<Facility>();

                using (HttpClient client = new HttpClient())
                {
                    Task<string> resTask = client.GetStringAsync(URI);
                    string jsonStr = resTask.Result;

                    facilities = JsonConvert.DeserializeObject<List<Facility>>(jsonStr);
                }

                return facilities;
            }

            private Facility GetOne(int id)
            {
                Facility facility = new Facility();

                using (HttpClient client = new HttpClient())
                {
                    Task<string> resTask = client.GetStringAsync(URI + "/" + id);
                    string jsonStr = resTask.Result;

                    facility = JsonConvert.DeserializeObject<Facility>(jsonStr);
                }

                return facility;
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

            private bool Post(Facility facility)
            {
                bool ok = true;

                using (HttpClient client = new HttpClient())
                {
                    string jsonStr = JsonConvert.SerializeObject(facility);
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
