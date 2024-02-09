FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /home/app
EXPOSE 80
EXPOSE 5166

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /home/app
COPY . ./

RUN dotnet build "TodoApp.csproj" -c Release -o /home/app/build

FROM build AS publish
RUN dotnet publish "TodoApp.csproj" -c Release -o /home/app/publish

FROM base AS final
WORKDIR /home/app
COPY --from=publish /home/app/publish .
ENTRYPOINT ["dotnet", "TodoApp.dll"]








