param (
	[string] $PackageSources,
	[string] $SolutionName,
	[string] $ClickMotivePackageSource
)
#The nuget update command requires the SolutionDir property inside each of the .csproj files to be overridden, so we set it and set it back while it runs.

$CurrentValue = [Environment]::GetEnvironmentVariable("SolutionDir")
[Environment]::SetEnvironmentVariable("SolutionDir", $(Get-Location))
Get-ChildItem -recurse -include packages.config | foreach-object{.\.nuget\NuGet.exe install $_.FullName -source $PackageSources -o .\packages}
Start-Process ".nuget\NuGet.exe" -ArgumentList "update $SolutionName -source $ClickMotivePackageSource -verbose" -Wait -NoNewWindow
Get-ChildItem -recurse -include packages.config | foreach-object{.\.nuget\NuGet.exe install $_.FullName -source $PackageSources -o .\packages}
[Environment]::SetEnvironmentVariable("SolutionDir", $CurrentValue)
