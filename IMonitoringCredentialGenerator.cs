
//---------------------------------------------------------------- 

// Copyright (c) Microsoft Corporation.  All rights reserved. 

//---------------------------------------------------------------- 

using System.Threading.Tasks;
using Azure.Core;

namespace Microsoft.Cosmic.Monitoring.Library
{ 
    public interface IMonitoringCredentialGenerator
    {
        Task<TokenCredential> GetToken(string ring, string managedIdentityId);
    }
}
