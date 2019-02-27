using WebApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Services
{
    public class UserService
    {
        private const string Endpoint = "https://localhost:44392/api";
        //private readonly RestClient _client;

        public UserService()
        {
            //_client = new RestClient(Endpoint);
        }

        //public async Task<List<User>> GetAll()
        //{
        //    var request = new RestRequest("users", Method.GET);
        //    IRestResponse response = await _client.ExecuteTaskAsync(request);

        //    List<User> users = JsonConvert.DeserializeObject<List<User>>(response.Content);
        //    return users;
        //}

        //public async Task<User> GetById(string id)
        //{
        //    var request = new RestRequest("users/{id}", Method.GET);
        //    request.AddUrlSegment("id", id);
        //    IRestResponse response = await _client.ExecuteTaskAsync(request);

        //    User user = JsonConvert.DeserializeObject<User>(response.Content);
        //    return user;
        //}

        //public async Task<User> GetByAuth0Id(string id)
        //{
        //    var request = new RestRequest("users/by_auth0_id/{id}", Method.GET);
        //    request.AddUrlSegment("id", id);
        //    IRestResponse response = await _client.ExecuteTaskAsync(request);

        //    User user = JsonConvert.DeserializeObject<User>(response.Content);
        //    return user;

        //}

        //public async Task Insert(User user)
        //{
        //    var request = new RestRequest("users/create", Method.POST);
        //    request.AddJsonBody(user);
        //    IRestResponse response = await _client.ExecuteTaskAsync(request);

        //    if (!response.IsSuccessful)
        //    {
        //        throw new Exception(response.ErrorMessage);
        //    }

        //}

        //public async Task Update(User user)
        //{
        //    var request = new RestRequest("users/update", Method.PUT);
        //    request.AddJsonBody(user);
        //    IRestResponse response = await _client.ExecuteTaskAsync(request);

        //    if (!response.IsSuccessful)
        //    {
        //        throw new Exception(response.ErrorMessage);
        //    }
        //}
    }
}
