using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cartog.Map;
using Cartog.IO;

public class Test : MonoBehaviour
{
    [SerializeField]
    string id;

    [SerializeField]
    string[] location;

    RasterizedItem item;

    // Start is called before the first frame update
    void Start()
    {
        var adaptor = new UnityProjectDriveStorage(location);
        if (RasterizedItem.Exists(adaptor, id))
        {
            item = RasterizedItem.Load(adaptor, id);
            Debug.Log("Loaded");
        } else
        {
            item = new RasterizedItem(id, 1);
            Debug.Log("Created");
        }

        item.Save(adaptor);

        var legend = new MapLegend(adaptor);
        Debug.Log(string.Join(",", legend.SeasonItems(0)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
