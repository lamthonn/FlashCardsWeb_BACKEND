using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Collections.ObjectModel;
using System.Data;

namespace backend_v3.Seriloger
{
    public class Serilogger
    {
        /// <summary>
        /// Log in console
        /// </summary>
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
            (context, configuration) =>
            {
                var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
                var enviromentName = context.HostingEnvironment.EnvironmentName ?? "Development";
                configuration.WriteTo.Console(
                        outputTemplate: "[{Timestamp:HH:mm:ss}] {SourceContext}{NewLine}{Exception}{NewLine}")
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithProperty("Environment", enviromentName)
                    .Enrich.WithProperty("Application", applicationName)
                    .ReadFrom.Configuration(context.Configuration);
            };

        /// <summary>
        /// Log to database
        /// </summary>
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogToDatabase =>
            (context, configuration) =>
            {
                var columnOptions = new ColumnOptions
                {
                    AdditionalColumns = new Collection<SqlColumn>
                       {
                           new SqlColumn("UserName", SqlDbType.NVarChar),
                           new SqlColumn("IP", SqlDbType.VarChar),
                           new SqlColumn("PhanLoai", SqlDbType.VarChar),
                           new SqlColumn("DonVi", SqlDbType.VarChar)
                       }
                };

                configuration
                    .WriteTo.MSSqlServer(context.Configuration.GetConnectionString("connect"), sinkOptions: new MSSqlServerSinkOptions { TableName = "NhatKyHeThong" }
                        , null, null, LogEventLevel.Information, null, columnOptions, null, null)
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.With<UserEnricher>()
                    .ReadFrom.Configuration(context.Configuration);
            };

        /// <summary>
        /// Log to file
        /// </summary>
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogToFile =>
            (context, configuration) =>
            {
                var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
                var enviromentName = context.HostingEnvironment.EnvironmentName ?? "Development";

                configuration
                        .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithProperty("Environment", enviromentName)
                        .Enrich.WithProperty("Application", applicationName)
                        .ReadFrom.Configuration(context.Configuration);
            };

    }
}
