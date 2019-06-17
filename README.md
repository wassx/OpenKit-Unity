# OpenKit-Unity
Simple example how to use OpenKit with Unity as an extension service of xrtk.

Dynatrace OpenKit https://github.com/Dynatrace/openkit-dotnet

XRTK: https://github.com/XRTK/XRTK-Core

## Install
- OpenKit libs are in the example, update if necessary.
- Add XRTK to the manifest.json as shown in the XRTK installation guidelines.
- Modify "import_certs.ps1" to point to your unity installation ($UnityPath)
- Run "import_certs.ps1" to import root certificates to Resources to allow ssl and run monitoring tests in editor.
