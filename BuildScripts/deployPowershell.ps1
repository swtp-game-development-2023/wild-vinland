param (
    [string]$platform
)

$unityExecutable = "C:/Program Files/Unity/Hub/Editor/2022.2.16f1/Editor/Unity.exe"
$buildDirectory = ""

function cleanup($buildDirectory) {
    if (Test-Path $buildDirectory) {
        Write-Host "Cleaning up build directory: $buildDirectory"
        Remove-Item -Recurse -Force $buildDirectory
    }
}
function performBuild($buildMethod) {
    Write-Host "Starting Build Process $buildMethod ... this may take a while"
    #& $unityExecutable -quit -batchmode -projectPath ../ -executeMethod $buildMethod
	$process = Start-Process -FilePath $unityExecutable -ArgumentList "-quit", "-batchmode", "-projectPath", "..\", "-executeMethod", $buildMethod -PassThru
    $process.WaitForExit()
	Write-Host "Finished Build Process $buildMethod"
}
if ($platform -eq "WebGL") {
	$buildDirectory = "../builds/WebGL"
    cleanup $buildDirectory	
    performBuild "BuildScript.PerformBuildWebGL"
	Write-Host "Copying server4webgl.py to $buildDirectory"
	Copy-Item -Path "./server4webgl.py" -Destination "$buildDirectory/server4webgl.py" -Force
}
elseif ($platform -eq "Windows") {
	$buildDirectory = "../builds/Windows/"
	cleanup $buildDirectory
	performBuild "BuildScript.PerformBuildWindows"
}
elseif ($platform -eq "Mac") {
	$buildDirectory = "../builds/Mac/"
	cleanup $buildDirectory
	performBuild "BuildScript.PerformBuildMac"
}
elseif ($platform -eq "Linux") {
	$buildDirectory = "../builds/Linux/"
	cleanup $buildDirectory
	performBuild "BuildScript.PerformBuildLinux"
}
else {
    Write-Host "Invalid platform specified."
    exit 1
}