
//---------------------------------------------------------------- 

// Copyright (c) Microsoft Corporation.  All rights reserved. 

//---------------------------------------------------------------- 

namespace Microsoft.Cosmic.Monitoring.Library
{
    public sealed record WorkspaceInfo
    {
        public string SubscriptionId { get; set; }
        public string WorkspaceId { get; set; }
        public string WorkspaceKey { get; set; }
        public string ResourceGroupName { get; set; }
        public string WorkspaceName { get; set; }
        public string TenantId { get; set; }
    }
}
