git pull;
find .PublishFiles/ -type f -and ! -path '*/wwwroot/images/*' ! -name 'appsettings.*' |xargs rm -rf
dotnet build;
rm -rf /home/MineCosmos.Core/MineCosmos.Core.Api/bin/Debug/.PublishFiles;
dotnet publish -o /home/MineCosmos.Core/MineCosmos.Core.Api/bin/Debug/.PublishFiles;
# cp -r /home/MineCosmos.Core/MineCosmos.Core.Api/bin/Debug/.PublishFiles ./;
awk 'BEGIN { cmd="cp -ri /home/MineCosmos.Core/MineCosmos.Core.Api/bin/Debug/.PublishFiles ./"; print "n" |cmd; }'
echo "Successfully!!!! ^ please see the file .PublishFiles";