# run this script from the same directory where you downloaded launch.exe

if (!([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) 
{ 
    Start-Process powershell.exe "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`"" -Verb RunAs; exit 
}

$executableFileName = "launch.exe"
$installationPath = "C:\Program Files\AppLauncher\"

$path = $env:PATH;

if (!(Test-Path $executableFileName)) {
    Write-Host "$executableFileName not found."
    exit 1
}

if (!(Test-Path $installationPath)) {
    Write-Host "Creating directory $installationPath..."
    New-Item -ItemType "directory" -Path $installationPath
}

Write-Host "Copying $executableFileName to $installationPath..."

$source = Resolve-Path "launch.exe"
Copy-Item -Path $source -Destination $installationPath -Force

Write-Host "Adding $installationPath to PATH..."
if (!($path.Contains($installationPath))) 
{
    [System.Environment]::SetEnvironmentVariable("PATH", $path + ";$installationPath", "Machine")
}

Write-Host "Installation complete"
exit 0