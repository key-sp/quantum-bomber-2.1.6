// <auto-generated>
// This code was auto-generated by a tool, every time
// the tool executes this code will be reset.
//
// If you need to extend the classes generated to add
// fields or methods to them, please create partial  
// declarations in another file.
// </auto-generated>

using Quantum;
using UnityEngine;


public partial class SimulationConfigAsset  {
  public Quantum.SimulationConfig Settings;

  public override Quantum.AssetObject AssetObject => Settings;
  
}

public static partial class SimulationConfigAssetExts {
  public static SimulationConfigAsset GetUnityAsset(this SimulationConfig data) {
    return data == null ? null : UnityDB.FindAsset<SimulationConfigAsset>(data);
  }
}
