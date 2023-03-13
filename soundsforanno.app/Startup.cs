using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SoundsForAnno.Assetexport.Interfaces;
using SoundsForAnno.Assetexport.Services;
using SoundsForAnno.Transcription;
using SoundsForAnno.App;
using CommandLine;
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
    .ConfigureLogging(builder => {
        builder.ClearProviders();
        builder.SetMinimumLevel(LogLevel.Information);
        builder.AddConsole();
    })
    .Build();

await Parser.Default.ParseArguments<SoundsForAnnoOptions>(args).MapResult(
    async (SoundsForAnnoOptions o) => {
        var app = host.Services.GetRequiredService<ISoundsForAnnoService>();
        await app.RunAsync(o);
    },
    err => Task.FromResult(-1)
);