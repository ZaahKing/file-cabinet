using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Resolve dependency using names in strings.
    /// </summary>
    public static class DependencyResolver
    {
        private const string DefaultValidatorName = "default";
        private const string DefaultServiceName = "memory";
        private static Dictionary<string, Func<IRecordValidator, IFileCabinetService>> fileCabinetServices = new ()
        {
            { "memory", GetFileCabinetMemoryService },
            { "file", GetFileCabinetFilesystemService },
        };

        private static Dictionary<string, Func<IRecordValidator>> recordValidators = new ()
        {
            { "default", () => new DefaultValidator() },
            { "custom", () => new CustomValidator() },
        };

        /// <summary>
        /// Return normalize known validator name.
        /// </summary>
        /// <param name="someName">Possible validator name.</param>
        /// <returns>Actual validator name.</returns>
        public static string NormalizeValidatorName(string someName)
        {
            return NormalizeName(recordValidators, someName, DefaultValidatorName);
        }

        /// <summary>
        /// Return normalize known FileCabinetService name.
        /// </summary>
        /// <param name="someName">Possible FileCabinetService name.</param>
        /// <returns>Actual FileCabinetService name.</returns>
        public static string NormalizeFileCabinetServiceName(string someName)
        {
            return NormalizeName(fileCabinetServices, someName, DefaultServiceName);
        }

        /// <summary>
        /// Return validator.
        /// </summary>
        /// <param name="validatorName">Validator name.</param>
        /// <returns>Validator.</returns>
        public static IRecordValidator GetValidator(string validatorName)
        {
            return recordValidators[NormalizeValidatorName(validatorName)]();
        }

        /// <summary>
        /// Return file cabinet service.
        /// </summary>
        /// <param name="serviceName">File cabinet service name.</param>
        /// <param name="validatorName">Validator name.</param>
        /// <returns>File cabinet service.</returns>
        public static IFileCabinetService GetFileCabinetService(string serviceName, string validatorName)
        {
            return fileCabinetServices[NormalizeFileCabinetServiceName(serviceName)](GetValidator(validatorName));
        }

        private static IFileCabinetService GetFileCabinetFilesystemService(IRecordValidator validator)
        {
            return new FileCabinetFilesystemService(File.Open("cabinet-records.db", FileMode.Open), validator);
        }

        private static IFileCabinetService GetFileCabinetMemoryService(IRecordValidator validator)
        {
            return new FileCabinetMemoryService(new FileCabinetMemoryGateway(), validator);
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
    }
}
