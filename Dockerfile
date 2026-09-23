# ---------- build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY TermPaper.Domain/*.csproj TermPaper.Domain/
COPY TermPaper.Application/*.csproj TermPaper.Application/
COPY TermPaper.Infrastructure/*.csproj TermPaper.Infrastructure/
COPY TermPaper.Web/*.csproj TermPaper.Web/
RUN dotnet restore TermPaper.Web/TermPaper.Web.csproj

COPY TermPaper.Domain/ TermPaper.Domain/
COPY TermPaper.Application/ TermPaper.Application/
COPY TermPaper.Infrastructure/ TermPaper.Infrastructure/
COPY TermPaper.Web/ TermPaper.Web/

RUN dotnet publish TermPaper.Web/TermPaper.Web.csproj \
    -c Release \
    -o /app/publish

# ---------- runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV PORT=8080
EXPOSE 8080

ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://+:$PORT dotnet TermPaper.Web.dll"]