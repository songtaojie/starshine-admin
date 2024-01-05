#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Hx.Admin.Web.Entry/Hx.Admin.Web.Entry.csproj", "Hx.Admin.Web.Entry/"]
COPY ["Hx.Admin.Web.Core/Hx.Admin.Web.Core.csproj", "Hx.Admin.Web.Core/"]
COPY ["Hx.Admin.Services/Hx.Admin.Services.csproj", "Hx.Admin.Services/"]
COPY ["Hx.Admin.IServices/Hx.Admin.IServices.csproj", "Hx.Admin.IServices/"]
COPY ["Hx.Admin.Models/Hx.Admin.Models.csproj", "Hx.Admin.Models/"]
COPY ["Hx.Admin.Core/Hx.Admin.Core.csproj", "Hx.Admin.Core/"]
COPY ["Hx.Admin.Tasks/Hx.Admin.Tasks.csproj", "Hx.Admin.Tasks/"]
RUN dotnet restore "Hx.Admin.Web.Entry/Hx.Admin.Web.Entry.csproj"
COPY . .
WORKDIR "/src/Hx.Admin.Web.Entry"
RUN dotnet build "Hx.Admin.Web.Entry.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hx.Admin.Web.Entry.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hx.Admin.Web.Entry.dll"]