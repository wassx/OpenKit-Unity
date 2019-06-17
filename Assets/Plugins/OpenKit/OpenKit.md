# OpenKit with UWP and Unity

## How to build - Unity

* Change assembly name in openkit-dotnetfull-4.6 to openkit
* Build openkit-dotnetfull-4.6
* Copy to Plugins/Openkit
* Set Platforms to Editor and Standalone
* Run import_certs.ps1 for Cert import to resources/Certs
* Check if `-r:System.Net.Http.dll` is in file Assets/mcs.rsp

## How to build for UWP

* Change assembly name in openkit-dotnet-uwp to openkit
* Build openkit-dotnet-uwp
* Copy to Plugins/Openkit/WSA
* Set Platforms to WSA
* Set Placeholder to Assets/Plugins/OpenKit/openkit.dll

## Problems
* With .Net 3.5: No System.Net class available, switch to .Net 4.6
* UWP does not support .Net Core dlls -> only UWP dlls
* UWP dlls have limitations: no Threads
* Mono has own cert store: needs to be filled with `mono "C:\Program Files\Unity\Editor\Data\MonoBleedingEdge\lib\mono\4.5\mozroots.exe" --sync --import`
* Cert store needs to be filled not only on dev machine but also where the player is deployed -> import_certs.ps1
