#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#这种模式是直接在构建镜像的内部编译发布dotnet项目。
#注意下容器内输出端口是9291
#如果你想先手动dotnet build成可执行的二进制文件，然后再构建镜像，请看.Api层下的dockerfile。


FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["BCVP.Api/BCVP.Api.csproj", "BCVP.Api/"]
COPY ["BCVP.Extensions/BCVP.Extensions.csproj", "BCVP.Extensions/"]
COPY ["BCVP.Tasks/BCVP.Tasks.csproj", "BCVP.Tasks/"]
COPY ["BCVP.IServices/BCVP.IServices.csproj", "BCVP.IServices/"]
COPY ["BCVP.Model/BCVP.Model.csproj", "BCVP.Model/"]
COPY ["BCVP.Common/BCVP.Common.csproj", "BCVP.Common/"]
COPY ["BCVP.Services/BCVP.Services.csproj", "BCVP.Services/"]
COPY ["BCVP.Repository/BCVP.Repository.csproj", "BCVP.Repository/"]
COPY ["BCVP.EventBus/BCVP.EventBus.csproj", "BCVP.EventBus/"]
RUN dotnet restore "BCVP.Api/BCVP.Api.csproj"
COPY . .
WORKDIR "/src/BCVP.Api"
RUN dotnet build "BCVP.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BCVP.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 9291 
ENTRYPOINT ["dotnet", "BCVP.Api.dll"]