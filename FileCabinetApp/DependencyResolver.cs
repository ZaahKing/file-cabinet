using System;
using System.Collections.Generic;
using System.IO;
using FileCabinetApp.Validation;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp
{
    /// <summary>
    /// Resolve dependency using names in strings.
    /// </summary>
    public static class DependencyResolver
    {
        private const string DefaultValidatorName = "default";
        private const string DefaultServiceName = "memory";
        private const string StorageFilename = "cabinet-records.db";
        private const string ValidatorConfigFilename = "validation-rules.json";
        private const string LogFileName = "ServiceLogging.txt";

        private static readonly Dictionary<string, Func<IRecordValidator, IFileCabinetService>> FileCabinetServices = new ()
        {
            { "memory", GetFileCabinetMemoryService },
            { "file", GetFileCabinetFilesystemService },
        };

        private static readonly Dictionary<string, Func<IRecordValidator>> RecordValidators = new ()
        {
            {
                "default",
                () => new ValidatorBuilder()
                    .AddDefaultValidator()
                    .Create()
            },
            {
                "custom",
                () => new ValidatorBuilder()
                    .AddCustomValidator()
                    .Create()
            },
        };

        /// <summary>
        /// Return normalize known validator name.
        /// </summary>
        /// <param name="someName">Possible validator name.</param>
        /// <returns>Actual validator name.</returns>
        public static string NormalizeValidatorName(string someName)
        {
            return NormalizeName(RecordValidators, someName, DefaultValidatorName);
        }

        /// <summary>
        /// Return normalize known FileCabinetService name.
        /// </summary>
        /// <param name="someName">Possible FileCabinetService name.</param>
        /// <returns>Actual FileCabinetService name.</returns>
        public static string NormalizeFileCabinetServiceName(string someName)
        {
            return NormalizeName(FileCabinetServices, someName, DefaultServiceName);
        }

        /// <summary>
        /// Return validator.
        /// </summary>
        /// <param name="validatorName">Validator name.</param>
        /// <returns>Validator.</returns>
        public static IRecordValidator GetValidator(string validatorName)
        {
            var config = GetConfigurationFromJsonFile(ValidatorConfigFilename, validatorName);

            return new ValidatorBuilder()
                .AddFromConfiguration(config)
                .Create();
        }

        /// <summary>
        /// Return file cabinet service.
        /// </summary>
        /// <param name="serviceName">File cabinet service name.</param>
        /// <param name="validatorName">Validator name.</param>
        /// <returns>File cabinet service.</returns>
        public static IFileCabinetService GetFileCabinetService(string serviceName, string validatorName)
        {
            return FileCabinetServices[NormalizeFileCabinetServiceName(serviceName)](GetValidator(validatorName));
        }

        /// <summary>
        /// MeterDecorate.
        /// </summary>
        /// <param name="service">Any service.</param>
        /// <returns>Meter service.</returns>
        public static IFileCabinetService MeterDecorate(IFileCabinetService service)
        {
            return new ServiceMeter(service);
        }

        /// <summary>
        /// Loging Decorate.
        /// </summary>
        /// <param name="service">Any service.</param>
        /// <returns>Meter service.</returns>
        public static IFileCabinetService LoggingDecorate(IFileCabinetService service)
        {
            return new ServiceLogger(service, LogFileName);
        }

        private static IFileCabinetService GetFileCabinetFilesystemService(IRecordValidator validator)
        {
            return new FileCabinetFilesystemService(File.Open(StorageFilename, FileMode.OpenOrCreate), validator);
        }

        private static IFileCabinetService GetFileCabinetMemoryService(IRecordValidator validator)
        {
            return new FileCabinetMemoryService(validator);
        }

        private static string NormalizeName<T>(Dictionary<string, T> dictionary, string someName, string defaultName)
        {
            someName = someName.ToLower();
            if (dictionary.ContainsKey(someName))
            {
                return someName;
            }

            return defaultName;
        }

        private static ValidatorConfiguration GetConfigurationFromJsonFile(string filename, string validatorType)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(filename)
                .Build();
            return config.GetSection(validatorType).Get<ValidatorConfiguration>();
        }
    }
}
