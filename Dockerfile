FROM microsoft/aspnetcore:2.0

COPY app /app
WORKDIR /app

EXPOSE 5000
ENTRYPOINT ["dotnet", "MIPTCore.dll"]