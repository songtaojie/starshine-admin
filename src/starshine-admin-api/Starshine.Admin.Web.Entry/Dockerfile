#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Starshine.Admin.Web.Entry/Starshine.Admin.Web.Entry.csproj", "Starshine.Admin.Web.Entry/"]
COPY ["Starshine.Admin.Web.Core/Starshine.Admin.Web.Core.csproj", "Starshine.Admin.Web.Core/"]
COPY ["Starshine.Admin.Services/Starshine.Admin.Services.csproj", "Starshine.Admin.Services/"]
COPY ["Starshine.Admin.IServices/Starshine.Admin.IServices.csproj", "Starshine.Admin.IServices/"]
COPY ["Starshine.Admin.Models/Starshine.Admin.Models.csproj", "Starshine.Admin.Models/"]
COPY ["Starshine.Admin.Core/Starshine.Admin.Core.csproj", "Starshine.Admin.Core/"]
COPY ["Starshine.Admin.Tasks/Starshine.Admin.Tasks.csproj", "Starshine.Admin.Tasks/"]
RUN dotnet restore "Starshine.Admin.Web.Entry/Starshine.Admin.Web.Entry.csproj"
COPY . .
WORKDIR "/src/Starshine.Admin.Web.Entry"
RUN dotnet build "Starshine.Admin.Web.Entry.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Starshine.Admin.Web.Entry.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Starshine.Admin.Web.Entry.dll"]