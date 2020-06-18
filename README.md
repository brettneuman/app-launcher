# app-launcher
Run a windows application using different domain credentials without needing to type your password over and over again.

![](https://github.com/brettneuman/app-launcher/workflows/BuildMaster/badge.svg)



# Prerequisites
- dotnet core 3.1 SDK
   - [Windows SDK Link (for developer teams)](https://download.visualstudio.microsoft.com/download/pr/5aad9c2c-7bb6-45b1-97e7-98f12cb5b63b/6f6d7944c81b043bdb9a7241529a5504/dotnet-sdk-3.1.102-win-x64.exe)
   - [Latest Downloads Page](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- TRACK Artifacts Feed as a nuget source _GLOBALLY_
   - This command will list out your current sources
     `dotnet nuget list source`
   - Look for a source with the following URL
     https://pkgs.dev.azure.com/pwc-us-tax-tech/_packaging/TRACK/nuget/v3/index.json
   - If that URL is not in your list of sources, you can add it with the following command
     `dotnet nuget add source "https://pkgs.dev.azure.com/pwc-us-tax-tech/_packaging/TRACK/nuget/v3/index.json" -n TRACK`
   - Or you can add it manually by modifying your nuget.config file. You'll want to consult the documentation for the current location and inheritance rules, but you can find your personal global file here currently:
     `C:\Users\%username%\AppData\Roaming\NuGet`
   - I need to confirm if this is necessary, but just in case, I'll put this link here:
      - you may need to install the azure credential provider, you can find instructions on how to set that up here: 
     https://github.com/microsoft/artifacts-credprovider#azure-artifacts-credential-provider

# Command to install
Once you have completed setting up the prerequisites, you can use the following command in install **_LAUNCH_**
`dotnet tool install launch --global`
> NOTE: We are installing this tool using the `--global` flag which means that this tool will be accessible from any working directory and without needing to use the DOTNET CLI. You should just be able to type `launch` now!

---
# **Commands to Use**
Using the **_LAUNCH_** tool is pretty simple. 
1. You will first need to add your credentials. We associate a `name` with each domain/user/pw combination to make using the credentials easier. 
2. You will also want to add your applications. This part is optional, but will give you an alias to use so that you don't have to type out the entire path to the executable each time. 

## 1. Create a new credential

###structure:
`launch save cred --name {name} --domain {pwcamtax} --username {pwcguid001} --password {clear-text-password}`
###example:
`launch save cred -n work -d allata -u brettneuman -p Password123`

>- *NOTE:* yes, the password will appear in clear-text so don't use this on a webex or where everyone can see your screen!


## 2. Create a new application reference

###structure:
`launch save app --name {name} --target {full-path-executable} --working-directory {path-to-working-directory}`
###example:
`launch save app -n ssms -t "C:\Program Files (x86)\Microsoft SQL Server Management Studio 18\Common7\IDE\Ssms.exe -w "C:\Program Files (x86)\Microsoft SQL Server\140\Tools\Binn\ManagementStudio\"`

>- *NOTE:* working directory is optional, but I've found some apps can be finicky if you don't set this

## 3. Launch an application

###structure:
`launch --credential {cred-name} --application {app-name}`
###example:
`launch -c pwc -a ssms`

## 4. Create a shortcut for your applications
- Create a shortcut for your application and then pin it to the start menu or task bar
   - just type the command directly in the *Target* box
- Remember to set the "Run as Administrator" option under the **Advanced...** options
- Also, it's a good idea to change your icon otherwise, it will show a generic icon for all links that use **_LAUNCH_**

---
# Starter Settings File
The settings file is located at `c:\users\{UserName}\AppData\Local\app-launcher`

You can past the **applications** section in directly, but the **credentials** will need to be added using the commands.
```
{
  "Credentials": [ ],
  "Applications": [
    {
      "Name": "ssms",
      "Target": "C:\\Program Files (x86)\\Microsoft SQL Server Management Studio 18\\Common7\\IDE\\Ssms.exe",
      "WorkingDirectory": "C:\\Program Files (x86)\\Microsoft SQL Server\\140\\Tools\\Binn\\ManagementStudio\\",
      "IconPath": null,
      "RunAsAdmin": false
    },
    {
      "Name": "vs",
      "Target": "C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Enterprise\\Common7\\IDE\\devenv.exe",
      "WorkingDirectory": "C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Enterprise\\Common7\\IDE\\",
      "IconPath": null,
      "RunAsAdmin": false
    },
    {
      "Name": "code",
      "Target": "C:\\Program Files\\Microsoft VS Code\\Code.exe",
      "WorkingDirectory": "C:\\Program Files\\Microsoft VS Code\\",
      "IconPath": null,
      "RunAsAdmin": false
    },
    {
      "Name": "data",
      "Target": "C:\\Program Files\\Azure Data Studio\\azuredatastudio.exe",
      "WorkingDirectory": null,
      "IconPath": null,
      "RunAsAdmin": false
    },
    {
      "Name": "con",
      "Target": "C:\\Program Files\\ConEmu\\ConEmu64.exe",
      "WorkingDirectory": null,
      "IconPath": null,
      "RunAsAdmin": false
    }
  ]
}
```
