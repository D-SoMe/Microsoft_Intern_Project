
//---------------------------------------------------------------- 

// Copyright (c) Microsoft Corporation.  All rights reserved. 

//---------------------------------------------------------------- 

using System.Threading.Tasks;
using Azure.Core;
 
namespace Microsoft.Cosmic.Monitoring.Library
{
    public interface IWorkspaceInfoProvider
    {
        Task<WorkspaceInfo> GetWorkspaceInfo(string ring, TokenCredential token);
    }
}