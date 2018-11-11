FROM microsoft/dotnet:2.1-sdk-nanoserver-1809 as builder

WORKDIR /worker
COPY worker.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /out worker.csproj

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1809

WORKDIR /app
ENTRYPOINT ["dotnet", "worker.dll"]
COPY --from=builder /out .