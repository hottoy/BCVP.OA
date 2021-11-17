color B

del  .PublishFiles\*.*   /s /q

dotnet restore

dotnet build

cd BCVP.Api

dotnet publish -o ..\BCVP.Api\bin\Debug\net5.0\

md ..\.PublishFiles

xcopy ..\BCVP.Api\bin\Debug\net5.0\*.* ..\.PublishFiles\ /s /e 

echo "Successfully!!!! ^ please see the file .PublishFiles"

cmd