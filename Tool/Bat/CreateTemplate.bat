set templatePath=%1
set tmpPath=%2
set projectName=%3

dotnet new -u %templatePath%
dotnet new -i %templatePath%
cd d:\
cd /d %tmpPath%
dotnet new ABP_JWT -n %projectName%