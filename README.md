# WebFileManager

Web app to browse files on server.  
Simple *ASP.NET Core MVC* project.

## Features

- browse directories
- preview and download files
- image thumbnails
- multiple locations
- light & dark mode

## Branches

- develop: current changes
- main: stable, tested version

## Dependencies

- .Net 8
- ImageSharp (for thumbnails)
- frontend: pico.css
- testing: NUnit, Moq

## Configuration

Setup directories in `appsettings.json`:

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
or use build script from _scripts_ directory.
