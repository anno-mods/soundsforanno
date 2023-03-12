using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SoundsForAnno.Assetexport.Interfaces;
using SoundsForAnno.Assetexport.Services;
using SoundsForAnno.Transcription;
using SoundsForAnno.App;
using CommandLine;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<ISoundsForAnnoService, SoundsForAnnoService>();
        services.AddSingleton<IAutoGuidingService, AutoGuidingService>();
        services.AddSingleton<IGuidMappingService, GuidMappingService>();
        services.AddSingleton<IAudioAssetExportService, AudioAssetExportService>();
        services.AddSingleton<ITextAssetExportService, AudioTextAssetExportService>();
        services.AddSingleton<ITranscriptorService, TranscriptorService>();
        services.AddSingleton<IMultiLanguageMapService, MultiLanguageMapService>();
    })
    .ConfigureLogging(loggingBuilder => {
        loggingBuilder.ClearProviders();
        loggingBuilder.SetMinimumLevel(LogLevel.Information);
    })
    .Build();

Parser.Default.ParseArguments<SoundsForAnnoOptions>(args).MapResult(
    (SoundsForAnnoOptions o) => {
        var app = host.Services.GetRequiredService<ISoundsForAnnoService>();
        app.RunAsync(o).Wait();
        return 0; 
    },
    e => 1
);