#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

# ��������� �������
RUN rm -rf /etc/localtime \
    && ln -s /usr/share/zoneinfo/Europe/Moscow /etc/localtime \
    && echo "Europe/Moscow" > /etc/timezone

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VPOBot/VPOBot.csproj", "VPOBot/"]
RUN dotnet restore "VPOBot/VPOBot.csproj"
COPY . .
WORKDIR "/src/VPOBot"
RUN dotnet build "VPOBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VPOBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VPOBot.dll"]