
//---------------------------------------------------------------- 

// Copyright (c) Microsoft Corporation.  All rights reserved. 

//---------------------------------------------------------------- 

using System.Collections.Generic;
using static System.Net.WebRequestMethods;

namespace Microsoft.Cosmic.Monitoring.Library
{

    public static class DataRingConfigurations
    {
        // Define a dictionary to store DataRing configurations keyed by some identifier
        internal static readonly Dictionary<string, DataRing> DataRingConfigs = new Dictionary<string, DataRing>()
        {
            { "Gallatin", new DataRing()
                {
                    KeyvaultUrl = "https://cosmi.vult.azure.cn/", 
                    TenantId = "TenantId",
                    ClientId = "ClientId",
                    SubscriptionId = "SubscriptionId",
                    LogAnalyticsWorkspaceName = "LogAnalyticsWorkspaceName",
                    ResourcegroupName = "ResourcegroupName",
                    CertificateName = "CertificateName", 
                    WorkspaceUrl = "https://CertificateName/resourcegroups//providers//workspaces/", 
                    AzureAdTokenEndpoint = "https://AzureAdTokenEndpoint/oauth2/v2.0/token",
                    AzureAdAudienceClaimUrl = "https://AzureAdAudienceClaimUrl/v2.0",
                    AuthenticationEndpoint ="https://AuthenticationEndpoint/",
                    ChinaAuthenticationEndpoint = "https://ChinaAuthenticationEndpoint/",
                    ManagementScope = "ManagementScope"
                }
            },
            { "ring2", new DataRing()
                {
                    KeyvaultUrl = "https://vault.azure.net/",
                    TenantId = "your-tenant-id-2",
                    ClientId = "your-client-id-2",
                    SubscriptionId = "your-subscription-id-2",
                    LogAnalyticsWorkspaceName = "workspace-2",
                    ResourcegroupName = "resource-group-2",
                    CertificateName = "certificate-2",
                    WorkspaceUrl = "https://workspace-2.logs.azure.com/",
                    AzureAdTokenEndpoint = "https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token",
                    AzureAdAudienceClaimUrl = "https://monitoring.azure.com/",
                    AuthenticationEndpoint = "xwz"
                }
            }
            // Add more entries as needed for additional rings
        };
    }
}
