using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MIPTCore;
using Xunit;

namespace IntegrationTestProject
{
    public class UsersWebDefaultRequestShould
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        
        public UsersWebDefaultRequestShould()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }
        
        [Fact]
        public async Task HundredOfRequestReturnsOk()
        {
            //Arrange
            const int numberOfRequests = 100;

            var taskPool = Enumerable.Range(1, numberOfRequests)
                .Select(
                    i => _client.GetAsync("/users")
                ).ToList();
            
            // Act
            var response = await _client.GetAsync("/users");
            response.EnsureSuccessStatusCode();

            var results = await Task.WhenAll(taskPool);
            
            foreach (var result in results)
            {
                result.EnsureSuccessStatusCode();
            }
        }

    }
}