using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AssetModel { 
    public string Host { get; set; }
    public string AssetName { get; set; }
    public string Path { get; set; }
    public DateTime ProcessDate { get; set; }
    public bool Loaded { get; set; }
}
