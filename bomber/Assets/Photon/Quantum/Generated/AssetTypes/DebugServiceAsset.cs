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

[CreateAssetMenu(menuName = "Quantum/BTService/DebugService", order = Quantum.EditorDefines.AssetMenuPriorityStart + 29)]
public partial class DebugServiceAsset : BTServiceAsset {
  public Quantum.DebugService Settings;

  public override Quantum.AssetObject AssetObject => Settings;
  
  public override void Reset() {
    if (Settings == null) {
      Settings = new Quantum.DebugService();
    }
    base.Reset();
  }
}

public static partial class DebugServiceAssetExts {
  public static DebugServiceAsset GetUnityAsset(this DebugService data) {
    return data == null ? null : UnityDB.FindAsset<DebugServiceAsset>(data);
  }
}
