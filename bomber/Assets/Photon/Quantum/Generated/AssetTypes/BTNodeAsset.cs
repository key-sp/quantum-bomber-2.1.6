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

public abstract partial class BTNodeAsset : AssetBase {

}

public static partial class BTNodeAssetExts {
  public static BTNodeAsset GetUnityAsset(this BTNode data) {
    return data == null ? null : UnityDB.FindAsset<BTNodeAsset>(data);
  }
}