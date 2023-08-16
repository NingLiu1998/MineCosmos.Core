#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#这种模式是直接在构建镜像的内部编译发布dotnet项目。
#注意下容器内输出端口是9291
#如果你想先手动dotnet build成可执行的二进制文件，然后再构建镜像，请看.Api层下的dockerfile。


FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build
WORKDIR /src
COPY ["MineCosmos.Core.Api/MineCosmos.Core.Api.csproj", "MineCosmos.Core.Api/"]
COPY ["MineCosmos.Core.Extensions/MineCosmos.Core.Extensions.csproj", "MineCosmos.Core.Extensions/"]
COPY ["MineCosmos.Core.Tasks/MineCosmos.Core.Tasks.csproj", "MineCosmos.Core.Tasks/"]
COPY ["MineCosmos.Core.IServices/MineCosmos.Core.IServices.csproj", "MineCosmos.Core.IServices/"]
COPY ["MineCosmos.Core.Model/MineCosmos.Core.Model.csproj", "MineCosmos.Core.Model/"]
COPY ["MineCosmos.Core.Common/MineCosmos.Core.Common.csproj", "MineCosmos.Core.Common/"]
COPY ["MineCosmos.Core.Services/MineCosmos.Core.Services.csproj", "MineCosmos.Core.Services/"]
COPY ["MineCosmos.Core.Repository/MineCosmos.Core.Repository.csproj", "MineCosmos.Core.Repository/"]
COPY ["MineCosmos.Core.EventBus/MineCosmos.Core.EventBus.csproj", "MineCosmos.Core.EventBus/"]
RUN dotnet restore "MineCosmos.Core.Api/MineCosmos.Core.Api.csproj"
COPY . .
WORKDIR "/src/MineCosmos.Core.Api"
RUN dotnet build "MineCosmos.Core.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MineCosmos.Core.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 9291 
ENTRYPOINT ["dotnet", "MineCosmos.Core.Api.dll"]
