git pull;
rm -rf .PublishFiles;
dotnet build;
dotnet publish -o /home/BCVP/BCVP.Api/bin/Debug/net5.0;
cp -r /home/BCVP/BCVP.Api/bin/Debug/net5.0 .PublishFiles;
echo "Successfully!!!! ^ please see the file .PublishFiles";