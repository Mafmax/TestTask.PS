#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
# Start Chromium downloading
RUN apt-get update && apt-get -f install && apt-get -y install wget gnupg2 apt-utils
RUN wget -q -O - https://dl.google.com/linux/linux_signing_key.pub | apt-key add -
RUN echo 'deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main' >> /etc/apt/sources.list
RUN apt-get update \
&& apt-get install -y google-chrome-stable --no-install-recommends --allow-downgrades fonts-ipafont-gothic fonts-wqy-zenhei fonts-thai-tlwg fonts-kacst fonts-freefont-ttf
# End Chromium downloading
ENV PUPPETEER_EXECUTABLE_PATH "/usr/bin/google-chrome-stable"
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Mafmax.FileConvernter.Api/Mafmax.FileConvernter.Api.csproj", "src/Mafmax.FileConvernter.Api/"]
COPY ["src/Mafmax.FileConverter.BusinessLogic/Mafmax.FileConverter.BusinessLogic.csproj", "src/Mafmax.FileConverter.BusinessLogic/"]
COPY ["src/Mafmax.FileConverter.DataAccess/Mafmax.FileConverter.DataAccess.csproj", "src/Mafmax.FileConverter.DataAccess/"]
COPY ["src/Mafmax.FileConverter.Utils/Mafmax.FileConverter.Utils.csproj", "src/Mafmax.FileConverter.Utils/"]
RUN dotnet restore "src/Mafmax.FileConvernter.Api/Mafmax.FileConvernter.Api.csproj"
COPY . .
WORKDIR "/src/src/Mafmax.FileConvernter.Api"
RUN dotnet build "Mafmax.FileConvernter.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mafmax.FileConvernter.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mafmax.FileConvernter.Api.dll"]