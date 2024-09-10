using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData _Instance { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (_Instance != null && _Instance != this)
        {
            Destroy(this);
        }
        else
        {
            _Instance = this;
        }
    }

    public int _DucksCollectedInStage;
    public int _TotalDucksCollected;
    public int _ObjectPickedup;
    public int _KeyPickedup;
    public int _WateringCanPickedup;
    public int _WateringCanHasWater;
    public int _SeedPickedup;

    public List<int> _DuckIDs = new List<int>();
    public List<string> _CompletedLevels = new List<string>();
}
