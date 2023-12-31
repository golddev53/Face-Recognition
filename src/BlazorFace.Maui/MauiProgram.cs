// Copyright (c) Georg Jung. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BlazorFace.Services;
using Microsoft.Extensions.Logging;

namespace BlazorFace.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        BlazorFace.Startup.ConfigureBlazorFaceServices(builder.Services, builder.Configuration);
#if ANDROID
        builder.Services.AddSingleton<IFileOpener, MauiResourceOpener>();
        BlazorFace.Startup.AddBlazorFaceServices(builder.Services, new MauiResourceOpener());
#else
        builder.Services.AddSingleton<IFileOpener, DefaultFileOpener>();
        BlazorFace.Startup.AddBlazorFaceServices(builder.Services);
#endif

        return builder.Build();
    }
}
