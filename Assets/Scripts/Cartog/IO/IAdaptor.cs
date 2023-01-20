using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cartog.IO
{
    public interface IAdaptor
    {
        public bool Exists(string id);

        public byte[] Load(string id);

        public void Save(string id, byte[] data);
    }
}
