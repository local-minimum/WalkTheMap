using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Cartog.IO
{
    public class UnityProjectDriveStorage : IAdaptor
    {
        private static readonly string[] assetsPath = new[] { ".", "Assets" };

        string directory;        

        public UnityProjectDriveStorage(params string[] folder)
        {
            var fullPath = new List<string>(assetsPath);
            fullPath.AddRange(folder);

            directory = Path.Combine(fullPath.ToArray());

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Debug.Log($"Created folder {Path.Combine(folder)}");
            }
        }

        public bool Exists(string id)
        {
            return File.Exists(Path.Join(directory, id));
        }

        public byte[] Load(string id)
        {
            return File.ReadAllBytes(Path.Join(directory, id));
        }

        public void Save(string id, byte[] data)
        {
            File.WriteAllBytes(Path.Join(directory, id), data);
        }
    }
}
