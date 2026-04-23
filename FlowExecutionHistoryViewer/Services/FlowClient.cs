using FlowExecutionHistoryViewer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowExecutionHistoryViewer.Services
{
    public class FlowClient
    {
        private readonly string _envId;
        private readonly string _token;
        private readonly string _baseUrl;

        public FlowClient(string envId, string token, string regionUrl)
        {
            _envId = envId;
            _token = token;
            _baseUrl = regionUrl;
        }

        public List<FlowRun> GetFlowRuns(string flowId)
        {
            var runs = new List<FlowRun>();
            // Utilisation de l'API-Version standard
            string url = $"{_baseUrl}/providers/Microsoft.ProcessSimple/environments/{_envId}/flows/{flowId}/runs?api-version=2016-11-01";

            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                // Utilisation de GetAwaiter().GetResult() pour éviter les blocages de thread en WinForms
                var response = client.GetAsync(url).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    dynamic data = JsonConvert.DeserializeObject(json);

                    if (data.value != null)
                    {
                        foreach (var item in data.value)
                        {
                            runs.Add(new FlowRun
                            {
                                Id = item.name,
                                Status = item.properties.status,
                                StartDate = item.properties.startTime,
                                EndDate = item.properties.endTime,
                                Url = $"https://make.powerautomate.com/environments/{_envId}/flows/{flowId}/runs/{item.name}"
                            });
                        }
                    }
                }
                else
                {
                    // Permet de voir l'erreur réelle renvoyée par Microsoft (ex: FlowNotFound)
                    var errorBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    throw new Exception($"Erreur Power Automate : {response.StatusCode} - {errorBody}");
                }
            }
            return runs;
        }
    }
}
