FROM microsoft/dotnet:2.1-sdk as builder

WORKDIR /worker
COPY worker.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /out worker.csproj

FROM microsoft/dotnet:2.1-aspnetcore-runtime

WORKDIR /app
ENTRYPOINT ["dotnet", "worker.dll"]
COPY --from=builder /out .