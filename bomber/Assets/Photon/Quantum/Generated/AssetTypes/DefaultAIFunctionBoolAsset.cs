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

[CreateAssetMenu(menuName = "Quantum/AIFunction/AIFunction<T>/DefaultAIFunctionBool", order = Quantum.EditorDefines.AssetMenuPriorityStart + 3)]
public partial class DefaultAIFunctionBoolAsset : AIFunctionAsset<System.Boolean> {
  public Quantum.DefaultAIFunctionBool Settings_DefaultAIFunctionBool;

  public override string AssetObjectPropertyPath => nameof(Settings_DefaultAIFunctionBool);
  
  public override Quantum.AssetObject AssetObject => Settings_DefaultAIFunctionBool;
  
  public override void Reset() {
    if (Settings_DefaultAIFunctionBool == null) {
      Settings_DefaultAIFunctionBool = new Quantum.DefaultAIFunctionBool();
    }
    base.Reset();
  }
}

public static partial class DefaultAIFunctionBoolAssetExts {
  public static DefaultAIFunctionBoolAsset GetUnityAsset(this DefaultAIFunctionBool data) {
    return data == null ? null : UnityDB.FindAsset<DefaultAIFunctionBoolAsset>(data);
  }
}
