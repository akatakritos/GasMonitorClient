using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using Newtonsoft.Json;

namespace GasTrackerDemo.Data
{
    public class GasTrackerClient
    {
        private readonly HttpClient _client;

        public GasTrackerClient(string rootUrl, string apiKey)
        {
            // It's nice to hang on to the underlying http client for the
            // duration of your custom client. If you want to implement IDisposable
            // you can
            _client = new HttpClient();

            // Since you're keeping the HttpClient instance around, you
            // can easily set the default request headers so that you
            // don't have to remember to set up the API key in every client
            // method
            _client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

            // Setting base address means you don't have to manually concatenate
            // rootUrl and your routes together in each method
            _client.BaseAddress = new Uri(rootUrl);
        }

        public StatusResponse GetStatus()
        {
            var json = _client.GetStringAsync("status").Result;
            var status = JsonConvert.DeserializeObject<StatusResponse>(json);
            return status;
        }

        public OwnerCreatedResult CreateOwner(CreateOwnerCommand owner)
        {
            var json = JsonConvert.SerializeObject(owner);

            // Use a custom JsonContent class to specify Encoding and Mime/Type
            // Alternatively, you have to use `new StringContent(json, Encoding.UTF8, "application/json")` everywhere
            var response = _client.PostAsync("owners", new JsonContent(json)).Result;
            var responseJson = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<OwnerCreatedResult>(responseJson);
        }

        public OwnerResponse GetOwner(Guid id)
        {
            var response = _client.GetStringAsync($"owners/{id:N}").Result;
            return JsonConvert.DeserializeObject<OwnerResponse>(response);
        }

        public void DeleteOwner(Guid id)
        {
            var response = _client.DeleteAsync($"owners/{id:N}").Result;
            response.EnsureSuccessStatusCode();
        }

        public VehicleResponse CreateVehicle(Guid ownerId, CreateVehicleCommand vehicle)
        {
            var json = JsonConvert.SerializeObject(vehicle);
            var response = _client.PostAsync($"owners/{ownerId:N}/vehicles", new JsonContent(json)).Result;
            return JsonConvert.DeserializeObject<VehicleResponse>(response.Content.ReadAsStringAsync().Result);
        }

        public VehicleWithStats GetVehicle(Guid id)
        {
            var json = _client.GetStringAsync($"vehicles/{id:N}").Result;
            return JsonConvert.DeserializeObject<VehicleWithStats>(json);
        }

        public IEnumerable<VehicleWithStats> GetVehicles(Guid ownerId)
        {
            var response = _client.GetAsync($"owners/{ownerId:N}/vehicles").Result;
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<VehicleWithStats[]>(json);
        }

        public VehicleResponse UpdateVehicle(Guid id, VehiclePatch patch)
        {
            var json = JsonConvert.SerializeObject(patch);
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"vehicles/{id:N}")
            {
                Content = new JsonContent(json)
            };
            var response = _client.SendAsync(request).Result;
            return JsonConvert.DeserializeObject<VehicleResponse>(response.Content.ReadAsStringAsync().Result);
        }

        public void DeleteVehicle(Guid id)
        {
            var response = _client.DeleteAsync($"vehicles/{id:N}").Result;
            response.EnsureSuccessStatusCode();
        }

        public FillUpResponse LogFillUp(Guid vehicleId, FillUpRecord record)
        {
            var json = JsonConvert.SerializeObject(record);
            var response = _client.PostAsync($"vehicles/{vehicleId:N}/fillups", new JsonContent(json)).Result;
            return JsonConvert.DeserializeObject<FillUpResponse>(response.Content.ReadAsStringAsync().Result);
        }

        public FillUpResponse GetFillUp(Guid id)
        {
            var json = _client.GetStringAsync($"fillups/{id:N}").Result;
            return JsonConvert.DeserializeObject<FillUpResponse>(json);
        }

        public IEnumerable<FillUpResponse> GetFillUps(Guid vehicleId)
        {
            var json = _client.GetStringAsync($"vehicles/{vehicleId:N}/fillups").Result;
            return JsonConvert.DeserializeObject<FillUpResponse[]>(json);
        }

        public void DeleteFillUp(Guid id)
        {
            var response = _client.DeleteAsync($"fillups/{id:N}").Result;
            response.EnsureSuccessStatusCode();
        }
    }
}