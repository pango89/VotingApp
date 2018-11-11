FROM microsoft/dotnet:2.1-sdk

WORKDIR /worker
COPY worker.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /out

ENTRYPOINT ["dotnet", "out/worker.dll"]