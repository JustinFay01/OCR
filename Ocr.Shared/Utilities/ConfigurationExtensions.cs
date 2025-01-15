﻿using Microsoft.Extensions.Configuration;

namespace Ocr.Shared.Utilities;

public static class ConfigurationExtensions
{
    public static string BuildConnectionString(this IConfiguration configuration, string name)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        
        var connectionString = configuration.GetConnectionString(name) 
            ?? throw new ArgumentException($"No {name} in appsettings.json");

        if (env == "Development") return connectionString;
        
        var postgresPasswordPath = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")
                                   ?? throw new ArgumentException("No POSTGRES_PASSWORD in Env vars");
            
        var pass = DockerSecretUtil.GetSecret(postgresPasswordPath, "POSTGRES_PASSWORD");
        connectionString += pass;

        return connectionString;
    }
}