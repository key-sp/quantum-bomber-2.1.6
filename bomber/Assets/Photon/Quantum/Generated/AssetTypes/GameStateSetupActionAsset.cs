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


public partial class GameStateSetupActionAsset : AIActionAsset {
  public Quantum.GameStateSetupAction Settings;

  public override Quantum.AssetObject AssetObject => Settings;
  
}

public static partial class GameStateSetupActionAssetExts {
  public static GameStateSetupActionAsset GetUnityAsset(this GameStateSetupAction data) {
    return data == null ? null : UnityDB.FindAsset<GameStateSetupActionAsset>(data);
  }
}
