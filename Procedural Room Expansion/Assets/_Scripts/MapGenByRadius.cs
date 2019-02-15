using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Configurations;
using Generators;

public class MapGenByRadius : MonoBehaviour
{

    public TileConfiguration tileConfiguration;
    public MapConfig mConfig;

    private RadiusGenerator radGen;
    // Start is called before the first frame update
    void Start()
    {
        mConfig.parentObject = this.gameObject;

        radGen = new RadiusGenerator(tileConfiguration, mConfig);
        radGen.InitializeRooms();
    }

}
