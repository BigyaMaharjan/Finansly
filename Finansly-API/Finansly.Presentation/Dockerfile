FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Finansly.Presentation/Finansly.Presentation.csproj Finansly.Presentation/
COPY Finansly.Application/Finansly.Application.csproj Finansly.Application/
COPY Finansly.Domain/Finansly.Domain.csproj Finansly.Domain/
COPY Finansly.Infrastructure/Finansly.Infrastructure.csproj Finansly.Infrastructure/

RUN dotnet restore Finansly.Presentation/Finansly.Presentation.csproj

COPY . .
WORKDIR /src/Finansly.Presentation
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet","Finansly.Presentation.dll"]
