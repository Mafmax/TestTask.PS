#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Mafmax.FileConverter.BackgroundServices/Mafmax.FileConverter.BackgroundServices.csproj", "src/Mafmax.FileConverter.BackgroundServices/"]
COPY ["src/Mafmax.FileConverter.BusinessLogic/Mafmax.FileConverter.BusinessLogic.csproj", "src/Mafmax.FileConverter.BusinessLogic/"]
COPY ["src/Mafmax.FileConverter.DataAccess/Mafmax.FileConverter.DataAccess.csproj", "src/Mafmax.FileConverter.DataAccess/"]
COPY ["src/Mafmax.FileConverter.SharedConfiguration/Mafmax.FileConverter.SharedConfiguration.csproj", "src/Mafmax.FileConverter.SharedConfiguration/"]
COPY ["src/Mafmax.FileConverter.Utils/Mafmax.FileConverter.Utils.csproj", "src/Mafmax.FileConverter.Utils/"]
RUN dotnet restore "src/Mafmax.FileConverter.BackgroundServices/Mafmax.FileConverter.BackgroundServices.csproj"
COPY . .
WORKDIR "/src/src/Mafmax.FileConverter.BackgroundServices"
RUN dotnet build "Mafmax.FileConverter.BackgroundServices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mafmax.FileConverter.BackgroundServices.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mafmax.FileConverter.BackgroundServices.dll"]