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

[CreateAssetMenu(menuName = "Quantum/AIFunction/AIFunction<T>/DefaultAIFunctionInt", order = Quantum.EditorDefines.AssetMenuPriorityStart + 3)]
public partial class DefaultAIFunctionIntAsset : AIFunctionAsset<System.Int32> {
  public Quantum.DefaultAIFunctionInt Settings_DefaultAIFunctionInt;

  public override string AssetObjectPropertyPath => nameof(Settings_DefaultAIFunctionInt);
  
  public override Quantum.AssetObject AssetObject => Settings_DefaultAIFunctionInt;
  
  public override void Reset() {
    if (Settings_DefaultAIFunctionInt == null) {
      Settings_DefaultAIFunctionInt = new Quantum.DefaultAIFunctionInt();
    }
    base.Reset();
  }
}

public static partial class DefaultAIFunctionIntAssetExts {
  public static DefaultAIFunctionIntAsset GetUnityAsset(this DefaultAIFunctionInt data) {
    return data == null ? null : UnityDB.FindAsset<DefaultAIFunctionIntAsset>(data);
  }
}
