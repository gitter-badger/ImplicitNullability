﻿using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;

namespace ImplicitNullability.Plugin
{
    [ZoneDefinition]
    [ZoneDefinitionConfigurableFeature(AssemblyConsts.Title, AssemblyConsts.Description, /*IsInProductSection:*/ false)]
    public interface IImplicitNullabilityZone : IPsiLanguageZone, IRequire<ILanguageCSharpZone>, IRequire<DaemonEngineZone>
    {
    }

    [ZoneMarker]
    public class ZoneMarker : IRequire<IImplicitNullabilityZone>
    {
    }
}