FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . /src


RUN dotnet restore "./API/ClinicaCare.sln"

RUN dotnet publish "./API/ClinicaCare.sln" -c Release -o /app --no-restore
	
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ClinicaCare.dll"]
