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

[CreateAssetMenu(menuName = "Quantum/HFSMDecision/TrueDecision/AnotherDecision", order = Quantum.EditorDefines.AssetMenuPriorityStart + 182)]
public partial class AnotherDecisionAsset : TrueDecisionAsset {
  public Quantum.AnotherDecision Settings_AnotherDecision;

  public override string AssetObjectPropertyPath => nameof(Settings_AnotherDecision);
  
  public override Quantum.AssetObject AssetObject => Settings_AnotherDecision;
  
  public override void Reset() {
    if (Settings_AnotherDecision == null) {
      Settings_AnotherDecision = new Quantum.AnotherDecision();
    }
    base.Reset();
  }
}

public static partial class AnotherDecisionAssetExts {
  public static AnotherDecisionAsset GetUnityAsset(this AnotherDecision data) {
    return data == null ? null : UnityDB.FindAsset<AnotherDecisionAsset>(data);
  }
}
