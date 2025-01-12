    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /app
    
    COPY ./src/ ./
    
    RUN dotnet restore LightNap.sln
    
    RUN apt-get update && apt-get install -y curl \
        && curl -fsSL https://deb.nodesource.com/setup_18.x | bash - \
        && apt-get install -y nodejs \
        && apt-get clean -y && rm -rf /var/lib/apt/lists/*
    
    WORKDIR /app/lightnap-ng
    RUN npm install
    
    RUN npm run build
    
    WORKDIR /app
    RUN dotnet publish LightNap.WebApi/LightNap.WebApi.csproj -c Release -o out

    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
    WORKDIR /app
    
    COPY --from=build /app/out ./
    

    EXPOSE 80
    ENV ASPNETCORE_URLS=http://+:80
    
    # Run the app
    ENTRYPOINT ["dotnet", "LightNap.WebApi.dll"]
    