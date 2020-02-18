using System;
using Autofac;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.Extensions.DependencyInjection;
using Launch.Core.Services;

namespace Launch.Core
{
    public class LaunchCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CipherService>().As<ICipherService>();

            // TODO: should make the strings in this class configurable as well
            builder.RegisterType<SettingsFileManager>().As<ISettingsFileManager>();

            builder.RegisterType<SettingsManager>().As<ISettingsManager>();

            builder.RegisterType<Launcher>().As<ILauncher>();
        }
    }
}
