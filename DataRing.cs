
//---------------------------------------------------------------- 

// Copyright (c) Microsoft Corporation.  All rights reserved. 

//---------------------------------------------------------------- 


namespace Microsoft.Cosmic.Monitoring.Library
{
    public sealed class DataRing
    {
        public string KeyvaultUrl { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string SubscriptionId { get; set; }
        public string LogAnalyticsWorkspaceName { get; set; }
        public string ResourcegroupName { get; set; }
        public string CertificateName { get; set; }
        public string WorkspaceUrl { get; set; }
        public string AzureAdTokenEndpoint { get; set; }
        public string AzureAdAudienceClaimUrl { get; set; }
        public string AuthenticationEndpoint { get; set; }
        public string ManagementScope { get; set; }
        public string ChinaAuthenticationEndpoint { get; set; }
    }
}