# Microsoft_Intern_Project
Intern Project I have worked on during my Internship from May 11th to July 5th 

# DataRing Monitoring Library

This repository contains the DataRing Monitoring Library, a C# library for managing Azure resources, particularly focused on handling Azure Key Vault, Azure Active Directory (AAD), and Azure Log Analytics Workspace (LAW) operations. 

## Features

- Retrieve and manage Azure Key Vault certificates.
- Generate JWT tokens for authentication.
- Sign JWT tokens using Azure Key Vault keys.
- Fetch workspace information from Azure Log Analytics Workspace.

## Prerequisites

- .NET SDK
- Azure subscription
- Key Vault set up with the necessary certificates
- Managed Identity configured with appropriate permissions

## Getting Started

## Installation

You can install the DataRing Monitoring Library via NuGet:

```sh
dotnet add package DataRing.Monitoring.Library
```
### MonitoringCredentialGenerator Class
- The MonitoringCredentialGenerator class is responsible for generating credentials required to access Azure resources.

## Methods
GetToken(string ring, string managedIdentityId): Retrieves a TokenCredential using the provided ring and managed identity ID.
FetchCertificate(string keyVaultUrl, string certificateName, ManagedIdentityCredential credentials): Fetches a certificate from Azure Key Vault.
CreateJwtToken(string azureAdAudienceClaimUrl, string clientId, X509Certificate2 x509Certificate): Creates a JWT token.
SignJwtToken(JwtSecurityToken jwtToken, Uri keyId, TokenCredential credentials): Signs the JWT token using Azure Key Vault keys.

### WorkspaceInfoProvider Class
The WorkspaceInfoProvider class provides information about the Azure Log Analytics Workspace.

## Methods
GetWorkspaceInfo(string ring, TokenCredential tokenCredential): Retrieves the workspace information for the specified ring.
GetWorkspaceKeyAsync(DataRing data, TokenCredential tokenCredential): Fetches the workspace key.
GetWorkspaceId(DataRing data, TokenCredential tokenCredential): Fetches the workspace ID.
Configuration
Ensure that the DataRingConfigurations dictionary is populated with the necessary configurations for each ring.
