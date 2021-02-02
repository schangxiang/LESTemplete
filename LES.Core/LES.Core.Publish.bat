color B

del  .PublishFiles\*.*   /s /q

dotnet restore

dotnet build

cd LES.Core.Api

dotnet publish -o ..\LES.Core.Api\bin\Debug\net5.0\

md ..\.PublishFiles

xcopy ..\LES.Core.Api\bin\Debug\net5.0\*.* ..\.PublishFiles\ /s /e 

echo "Successfully!!!! ^ please see the file .PublishFiles"

cmd