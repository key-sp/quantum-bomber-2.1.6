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


public partial class HFSMNotDecisionAsset : HFSMLogicalDecisionAsset {
  public Quantum.HFSMNotDecision Settings;

  public override Quantum.AssetObject AssetObject => Settings;
  
}

public static partial class HFSMNotDecisionAssetExts {
  public static HFSMNotDecisionAsset GetUnityAsset(this HFSMNotDecision data) {
    return data == null ? null : UnityDB.FindAsset<HFSMNotDecisionAsset>(data);
  }
}
