using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Cartog.IO
{
    public interface IAdaptor
    {
        public bool Exists(string id);

        public byte[] Load(string id);

        public void Save(string id, byte[] data);

        /// <summary>
        /// List all items that match the pattern
        /// </summary>
        /// <param name="idPattern">Pattern to match</param>
        /// <returns></returns>
        public IEnumerable<string> ListAvailable(Regex idPattern);
    }
}
