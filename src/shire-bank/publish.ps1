


#Get current script file path 
$outProjectRootPath = "$PSScriptRoot"
$outProjectRootPath =(get-item $PSScriptRoot ).Parent.FullName


#Shire bank service deployement 
Write-Host "Publishing ShireBank service.." -ForegroundColor red -BackgroundColor white
Write-Host "no newline test "

$out = "$outProjectRootPath\out\bank"; 
dotnet publish  $PSScriptRoot\ShireBank\ShireBankService.csproj --configuration Release --output $out  --self-contained True  -p:PublishTrimmed=true -p:PublishSingleFile=true


#Customer test client deployement 
Write-Host "Publishing Customer test service.." -ForegroundColor red -BackgroundColor white
Write-Host "no newline test "

$out = "$outProjectRootPath\out\customer"; 
dotnet publish  $PSScriptRoot\Customer\CustomerClient.csproj --configuration Release --output $out  --self-contained True  -p:PublishTrimmed=true -p:PublishSingleFile=true


#Inspector service deployement 
Write-Host "Publishing Inspector client.." -ForegroundColor red -BackgroundColor white
Write-Host "no newline test "

$out = "$outProjectRootPath\out\inspector"; 
dotnet publish  $PSScriptRoot\Inspector\InspectorClient.csproj --configuration Release --output $out  --self-contained True  -p:PublishTrimmed=true -p:PublishSingleFile=true


