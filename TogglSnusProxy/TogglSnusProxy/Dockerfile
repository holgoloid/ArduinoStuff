FROM microsoft/dotnet:2.0-sdk as builder  
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

RUN mkdir -p /root/src/app  
WORKDIR /root/src/app  
COPY .     TogglSnusProxy  
WORKDIR /root/src/app/TogglSnusProxy

RUN dotnet restore ./TogglSnusProxy.csproj  
RUN dotnet publish -c release -o published -r linux-arm

FROM microsoft/dotnet:2.0.0-runtime-stretch-arm32v7

WORKDIR /root/  
COPY --from=builder /root/src/app/TogglSnusProxy/published .

CMD ["dotnet", "./TogglSnusProxy.dll"]





# docker build -t hogler/togglproxy:0.3 .
# docker push hogler/togglproxy:0.3