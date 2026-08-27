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

$projects = Get-ChildItem -LiteralPath $repo -Recurse -Filter *.csproj |
    Where-Object { $_.FullName -notmatch '[\\/](bin|obj|node_modules)[\\/]' }
if ($projects.Count -ne 5) { throw "Expected 5 projects, found $($projects.Count)." }
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
