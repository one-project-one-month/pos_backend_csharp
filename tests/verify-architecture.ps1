$ErrorActionPreference = 'Stop'
$repo = Split-Path -Parent $PSScriptRoot

function Assert-NoMatch([string]$Pattern, [string]$Message) {
    $matches = & rg -n -i --glob '!**/bin/**' --glob '!**/obj/**' --glob '!**/node_modules/**' $Pattern $repo
    if ($LASTEXITCODE -eq 0) { throw "$Message`n$matches" }
    if ($LASTEXITCODE -gt 1) { throw "ripgrep failed while checking: $Pattern" }
}

Assert-NoMatch ('DotNet' + '8') 'Legacy framework-version naming remains.'
Assert-NoMatch ('Interactive' + 'Server|AddInteractive' + 'Server|AddInteractive' + 'ServerRenderMode|@render' + 'mode') 'Interactive Blazor configuration remains.'
Assert-NoMatch ('Mud' + 'Blazor|Rad' + 'zen') 'Removed UI libraries remain.'
Assert-NoMatch ('local' + 'Storage|IJS' + 'Runtime|JS' + 'Runtime') 'Browser token or JS interop code remains.'
Assert-NoMatch '[?&](message|success|error)=' 'A notification is being passed in a URL.'

$backendFeatures = Join-Path $repo 'Pos.BackendApi/Features'

function Assert-NoBackendMatch([string]$Pattern, [string]$Message) {
    $matches = & rg -n --glob '!**/bin/**' --glob '!**/obj/**' $Pattern $backendFeatures
    if ($LASTEXITCODE -eq 0) { throw "$Message`n$matches" }
    if ($LASTEXITCODE -gt 1) { throw "ripgrep failed while checking backend: $Pattern" }
}

Assert-NoBackendMatch 'BL_|DL_' 'Legacy BL/DL naming remains in backend feature code.'
Assert-NoBackendMatch '\bI[A-Z][A-Za-z]+Repository\b|\b[A-Z][A-Za-z]+Repository\b' 'Repository layer naming remains in backend feature code.'
Assert-NoBackendMatch '\bI[A-Z][A-Za-z]+Service\b' 'Feature service interfaces remain in backend feature code.'

$apiFolders = Get-ChildItem -LiteralPath $backendFeatures -Recurse -Directory -Filter Api
if ($apiFolders.Count -gt 0) {
    throw "Api folders remain inside backend features.`n$($apiFolders.FullName -join "`n")"
}

$featureCodeFolders = Get-ChildItem -LiteralPath $backendFeatures -Recurse -Directory |
    Where-Object {
        $controllerCount = @(Get-ChildItem -LiteralPath $_.FullName -File -Filter '*Controller.cs').Count
        $serviceCount = @(Get-ChildItem -LiteralPath $_.FullName -File -Filter '*Service.cs').Count
        ($controllerCount + $serviceCount) -gt 0
    }

foreach ($folder in $featureCodeFolders) {
    $controllers = @(Get-ChildItem -LiteralPath $folder.FullName -File -Filter '*Controller.cs')
    $services = @(Get-ChildItem -LiteralPath $folder.FullName -File -Filter '*Service.cs')
    if ($controllers.Count -ne 1 -or $services.Count -ne 1) {
        throw "Feature folder must contain exactly one controller and one service: $($folder.FullName)`nControllers: $($controllers.Name -join ', ')`nServices: $($services.Name -join ', ')"
    }

    $featureName = Split-Path -Leaf $folder.FullName
    $expectedController = "$($featureName)Controller.cs"
    $expectedService = "$($featureName)Service.cs"
    if ($controllers[0].Name -ne $expectedController -or $services[0].Name -ne $expectedService) {
        throw "Feature folder files must be named after the feature: $($folder.FullName)`nExpected: $expectedController, $expectedService`nActual: $($controllers[0].Name), $($services[0].Name)"
    }
}

$layerFiles = Get-ChildItem -LiteralPath $backendFeatures -Recurse -File -Filter *.cs |
    Where-Object { $_.FullName -match '[\\/](Application|Infrastructure)[\\/]' }
if ($layerFiles.Count -gt 0) {
    throw "Application/Infrastructure feature layer files remain.`n$($layerFiles.FullName -join "`n")"
}

$projects = Get-ChildItem -LiteralPath $repo -Recurse -Filter *.csproj |
    Where-Object { $_.FullName -notmatch '[\\/](bin|obj|node_modules)[\\/]' }
if ($projects.Count -ne 6) { throw "Expected 6 projects, found $($projects.Count)." }
foreach ($project in $projects) {
    $xml = [xml](Get-Content -LiteralPath $project.FullName -Raw)
    if ($xml.Project.PropertyGroup.TargetFramework -notcontains 'net10.0') {
        throw "$($project.FullName) does not target net10.0."
    }
}

$app = Get-Content -LiteralPath (Join-Path $repo 'Pos.App/Program.cs') -Raw
if ($app -notmatch 'MapStaticAssets\(\)' -or $app -notmatch 'MapRazorComponents<App>\(\)') {
    throw 'Static SSR pipeline is incomplete.'
}

$razor = (Get-ChildItem -LiteralPath (Join-Path $repo 'Pos.App/Components') -Recurse -Filter *.razor |
    ForEach-Object { Get-Content -LiteralPath $_.FullName -Raw }) -join "`n"
if ($razor -notmatch '<AntiforgeryToken\s*/>') { throw 'No antiforgery-enabled static forms were found.' }
if ($razor -notmatch 'SupplyParameterFromForm') { throw 'Static form binding was not found.' }
if ($razor -notmatch 'SupplyParameterFromTempData') { throw 'TempData flash binding contract was not found.' }

Write-Host 'Architecture verification passed.' -ForegroundColor Green
