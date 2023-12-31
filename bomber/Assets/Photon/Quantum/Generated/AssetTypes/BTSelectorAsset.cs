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

[CreateAssetMenu(menuName = "Quantum/BTNode/BTComposite/BTSelector", order = Quantum.EditorDefines.AssetMenuPriorityStart + 27)]
public partial class BTSelectorAsset : BTCompositeAsset {
  public Quantum.BTSelector Settings;

  public override Quantum.AssetObject AssetObject => Settings;
  
  public override void Reset() {
    if (Settings == null) {
      Settings = new Quantum.BTSelector();
    }
    base.Reset();
  }
}

public static partial class BTSelectorAssetExts {
  public static BTSelectorAsset GetUnityAsset(this BTSelector data) {
    return data == null ? null : UnityDB.FindAsset<BTSelectorAsset>(data);
  }
}
