# WeKey Password Verification API

## Table of Contents
- [Overview](#overview)
- [How it works](#how-it-works)
- [Implementation Guide](#implementation-guide)
  - [Create your hashed password list](#create-your-hashed-password-list)
  - [Add the `wekey.cs` file to your project](#add-the-wekeycs-file-to-your-project)
  - [Update the Gist URL](#update-the-gist-url)
  - [Use the API in your project](#use-the-api-in-your-project)
- [Code Snippets](#code-snippets)
  - [API Implementation (`api.cs`)](#api-implementation-apics)
  - [Example of usage (`form1.cs`)](#example-of-usage-form1cs)
- [Security Considerations](#security-considerations)
- [Conclusion](#conclusion)
- [Contributors](#contributors)

## Overview

WeKey is a robust password verification solution that allows you to check if a password matches a list of hashed passwords stored in a GitHub Gist. Using the SHA256 hashing algorithm, the API ensures security and integrity of data.

## How it works

1. Hashes a provided password using SHA256.
2. Checks the hashed password against a list of hashes stored in a GitHub Gist.
3. Returns whether the password's hash matches any of the stored hashes.

## Implementation Guide

### Create your hashed password list
- Navigate to [MiracleSalad's SHA256 Hash Generator](https://www.miraclesalad.com/webtools/sha256.php) and hash your passwords.
- Create a GitHub Gist and add the hashed passwords.

### Add the `wekey.cs` file to your project
Make sure to include the `wekeyapi` class from the provided `api.cs` snippet.

### Update the Gist URL
Replace the `GistUrl` constant value in the `wekeyapi` class with your GitHub Gist raw URL.

```csharp
private const string GistUrl = "YOUR_GITHUB_GIST_RAW_URL_HERE";
```
## Code Snippets

### API Implementation (`api.cs`)
```csharp
private const string GistUrl = "YOUR_GITHUB_GIST_RAW_URL_HERE";
```
### Example of usage (`form1.cs`)
```csharp
private const string GistUrl = "YOUR_GITHUB_GIST_RAW_URL_HERE";
```

## Security Considerations
The WeKey API uses a custom certificate validation mechanism to ensure that the connection to GitHub is secure. The thumbprint of GitHub's certificate is hardcoded into the API for this purpose. Ensure that the thumbprint is kept up to date.

## Conclusion
By following this guide, you can easily integrate the WeKey Password Verification API into your project. Always ensure that you handle passwords with care and never store plain-text passwords in your applications or Gists. This project is purely for educational purposes.

## Contributors
- [Franco](https://github.com/Altyd)