# WebFileManager

Web app to browse files on server.  
Simple *ASP.NET Core MVC* project.

## Features

- browse directores
- preview and download files
- image thumbnails
- multiple locations
- light & dark mode

## Dependencies

- .Net 6
- ImageSharp (for thumbnails)
- frontend: pico.css
- testing: NUnit, Moq

## Configuration

Setup directores in `appsettings.json`:

```json
"Storage": {
  "ListPageSize": 100,
  "Locations": [
    {
      "Name": "My dir 1",
      "Path": "C:\\Example\\Path\\1"
    }
  ]
}
```

## Run from source
```
dotnet run --project Wfm.Web
```

## Publish
```
dotnet publish Wfm.Web -c Release
```
