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

[CreateAssetMenu(menuName = "Quantum/GOAPGoal/GOAPDefaultGoal", order = Quantum.EditorDefines.AssetMenuPriorityStart + 162)]
public partial class GOAPDefaultGoalAsset : GOAPGoalAsset {
  public Quantum.GOAPDefaultGoal Settings;

  public override Quantum.AssetObject AssetObject => Settings;
  
  public override void Reset() {
    if (Settings == null) {
      Settings = new Quantum.GOAPDefaultGoal();
    }
    base.Reset();
  }
}

public static partial class GOAPDefaultGoalAssetExts {
  public static GOAPDefaultGoalAsset GetUnityAsset(this GOAPDefaultGoal data) {
    return data == null ? null : UnityDB.FindAsset<GOAPDefaultGoalAsset>(data);
  }
}
