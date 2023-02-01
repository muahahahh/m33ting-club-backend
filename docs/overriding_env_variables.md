# Overriding environment variables

## How environment variables overriding works?
Environment variables are set on 3 stages (each overrides previous):
1. appsettings.json (required)
2. appsettings.{env_name}.json (optional)
3. environment variables (optional)

## Possible environments:

- Development
- Testing
- Staging
- Production

This is related to: ASPNETCORE_ENVIRONMENT environment variable when starting an application   
For example: ASPNETCORE_ENVIRONMENT=Production

## Environment variable prefix
M33tingClub__

## Overriding with file with environment name

Example of appsettings.json:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Database": {
    "ConnectionString": "Host=localhost;Database=Db;Username=postgres;Password=postgres"
  },
  "Firebase": {
    "ProjectId": "abcd",
    "CredentialsPath": ""
  }
}
```

Example of appsettings.{env_name}.json:
```
{
  "Firebase": {
    "ProjectId": "123",
  }
}
```
**ProjectId will be set to 123.**

## Overriding with environment variables

Example of appsettings.json:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Database": {
    "ConnectionString": "Host=localhost;Database=Db;Username=postgres;Password=postgres"
  },
  "Firebase": {
    "ProjectId": "abcd",
    "CredentialsPath": ""
  }
}
```

Example of environment variable:
M33tingClub__Firebase__ProjectId=111

**ProjectId will be set to 111.**