
//---------------------------------------------------------------- 

// Copyright (c) Microsoft Corporation.  All rights reserved. 

//---------------------------------------------------------------- 

using Azure.Core;
using Azure.ResourceManager.OperationalInsights.Models;
using Azure.ResourceManager.OperationalInsights;
using Azure.ResourceManager;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Net.Http.Headers;

 
namespace Microsoft.Cosmic.Monitoring.Library
{
    public class WorkspaceInfoProvider : IWorkspaceInfoProvider
    {                                                                 
        public async Task<WorkspaceInfo> GetWorkspaceInfo(string ring, TokenCredential tokenCredential) 
        {
            if (!DataRingConfigurations.DataRingConfigs.TryGetValue(ring, out DataRing data))
            {
                throw new KeyNotFoundException($"The specified ring '{ring}' was not found in the configurations.");
            }
            string workspaceId = await this.GetWorkspaceId(data, tokenCredential);
            string workspaceKey = await this.GetWorkspaceKeyAsync(data, tokenCredential);
            return new WorkspaceInfo
            {
                SubscriptionId = data.SubscriptionId,
                WorkspaceId = workspaceId,
                WorkspaceKey = workspaceKey,
                ResourceGroupName = data.ResourcegroupName,
                WorkspaceName = data.LogAnalyticsWorkspaceName,
                TenantId = data.TenantId
            };
        }
        private async Task<string> GetWorkspaceKeyAsync(DataRing data, TokenCredential tokenCredential) 
        { 
            ArmClient client = new ArmClient(tokenCredential);
            ResourceIdentifier operationalInsightsWorkspaceResourceId = OperationalInsightsWorkspaceResource.CreateResourceIdentifier(data.SubscriptionId, data.ResourcegroupName, data.LogAnalyticsWorkspaceName);
            OperationalInsightsWorkspaceResource operationalInsightsWorkspace = client.GetOperationalInsightsWorkspaceResource(operationalInsightsWorkspaceResourceId);
            OperationalInsightsWorkspaceSharedKeys result = await operationalInsightsWorkspace.GetSharedKeysAsync();
            return result.PrimarySharedKey;
        }

        private async Task<string> GetWorkspaceId(DataRing data, TokenCredential tokenCredential)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var tokenRequestContext = new TokenRequestContext(new[] { data.ManagementScope });
                var tokenResponse = await tokenCredential.GetTokenAsync(tokenRequestContext, default);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Token);
                string requestUrl = data.WorkspaceUrl;
                var response = await httpClient.GetAsync(requestUrl);
                var responseContent = await response.Content.ReadAsStringAsync();

                using JsonDocument doc = JsonDocument.Parse(responseContent);
                JsonElement root = doc.RootElement;
                if (root.TryGetProperty("properties", out JsonElement properties) &&
                    properties.TryGetProperty("customerId", out JsonElement customerId))
                {
                    return customerId.GetString() ?? throw new Exception("Failed to retrieve the workspace ID. The response might be missing required fields.");
                }

                throw new Exception("Failed to retrieve the workspace ID. The response might be missing required fields.");
            }
        }


    }
}