using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Cache.
    /// </summary>
    internal class FileCabinetRecordCache
    {
        private readonly Dictionary<string, IEnumerable<FileCabinetRecord>> cache = new ();

        /// <summary>
        /// Check is cached.
        /// </summary>
        /// <param name="key">By key.</param>
        /// <returns>True if cached.</returns>
        public bool IsCached(string key)
        {
            return this.cache.ContainsKey(key);
        }

        /// <summary>
        /// Get cache.
        /// </summary>
        /// <param name="key">By key.</param>
        /// <returns>List.</returns>
        public IEnumerable<FileCabinetRecord> GetCashe(string key)
        {
            return this.cache[key];
        }

        /// <summary>
        /// Put cache.
        /// </summary>
        /// <param name="key">key.</param>
        /// <param name="data">data.</param>
        public void PutCache(string key, IEnumerable<FileCabinetRecord> data)
        {
            this.cache.Add(key, data);
        }

        /// <summary>
        /// Clear cache.
        /// </summary>
        public void Clear()
        {
            this.cache.Clear();
        }

        /// <summary>
        /// Clear cache.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">data.</param>
        public void Clear(object sender, EventArgs e)
        {
            this.cache.Clear();
        }
    }
}