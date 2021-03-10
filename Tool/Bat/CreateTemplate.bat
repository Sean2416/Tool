set arg1=%1
set arg2=%2
cd d:\
cd /d D:\Tmp
dotnet new ABP_JWT -n %arg1%
mkdir %arg2%