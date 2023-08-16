color B

del  .PublishFiles\*.*   /s /q

dotnet restore

dotnet build

cd MineCosmos.Core.Api

dotnet publish -o ..\MineCosmos.Core.Api\bin\Debug\net6.0\

md ..\.PublishFiles

xcopy ..\MineCosmos.Core.Api\bin\Debug\net6.0\*.* ..\.PublishFiles\ /s /e 

echo "Successfully!!!! ^ please see the file .PublishFiles"

cmd