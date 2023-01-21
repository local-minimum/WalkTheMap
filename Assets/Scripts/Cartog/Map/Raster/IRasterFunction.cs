using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cartog.IO;

namespace Cartog.Map.Raster
{
    public interface IRasterFunction
    {        
        public IEnumerable<Vector2> GetCoordinates(Rect area, int scale);

        public void Save(IAdaptor adaptor);
    }
}
