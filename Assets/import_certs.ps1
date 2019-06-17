$UnityPath = "$env:programfiles\Unity\Hub\Editor\2019.1.4f1\Editor"


$monoCerts  = $env:APPDATA + "\.mono\certs\Trust\*"
$unityCertDir = ".\Assets\Resources\Certs\"

if (Test-Path $unityCertDir) {
	Remove-Item -path "$unityCertDir\*"
}

& "$UnityPath\Data\MonoBleedingEdge\bin\mono.exe" "$UnityPath\Data\MonoBleedingEdge\lib\mono\4.5\mozroots.exe" --sync --import
New-Item -ItemType Directory -Force -Path $unityCertDir
Get-ChildItem "$unityCertDir\*.*" | ForEach-Object { $_.Delete()}
Copy-item -Force -Recurse $monoCerts -Destination $unityCertDir
Get-ChildItem "$unityCertDir\*.cer" | Rename-Item -newname { [io.path]::ChangeExtension($_.name, "bytes") }
