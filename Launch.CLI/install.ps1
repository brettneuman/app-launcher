
# install the "launch" dotnet global tool

if (!([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) 
{ 
    Start-Process powershell.exe "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`"" -Verb RunAs; 
    exit 
}

# first make sure we have the latest version of dotnet
Invoke-WebRequest 'https://dot.net/v1/dotnet-install.ps1' -OutFile 'dotnet-install.ps1';

# check if there was an error and if so just exit out of the script

$ScriptBlockContent =
{
    ./dotnet-install.ps1 -InstallDir '~/.dotnet' -Channel LTS
}
Invoke-Command -ScriptBlock $ScriptBlockContent

# install the latest LTS supported version of dotnet
Invoke-Command dotnet-install.ps1 -Channel LTS

# install launch global tool
Invoke-Command dotnet tool install launch --global --version 0.1.0-beta06


