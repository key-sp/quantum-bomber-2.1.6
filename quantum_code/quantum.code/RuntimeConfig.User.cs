using Photon.Deterministic;
using Quantum.Inspector;

namespace Quantum {
  partial class RuntimeConfig
  {
    public bool IsOnePlayerTest = false;
    public bool IsInvincible = false;
    public bool AutoAdjustToPlayerCount = false;
    
    // TODO: Rework to encapsulated all runtime accessible parameters to the Grid available in the frame.
    // Having them accessible from the RuntimeConfig is too error prone.
    [Header("Grid Parameters")]
    // Grid Parameters
    // public byte GridWidth = 17;
    // public byte GridHeight = 17;
    // public GridType GridType = GridType.Square;
    public byte GridSize = 17;
    public byte CellSize = 1;
    public byte MinDistanceBetweenSpawnPoints = 3;
    
    [Range(0, 100)] [Tooltip("in %")] 
    public byte AmountOfDestroyableCells = 70;
    
    [Header("Default Prototypes")]
    public AssetRefEntityPrototype DefaultBomberPrototype;
    public AssetRefEntityPrototype DefaultBombPrototype;
    public AssetRefEntityPrototype DefaultExplosionPrototype;
    public AssetRefEntityPrototype DefaultBlockDestroyablePrototype;

    public AssetRefEntityPrototype PowerUpManagerPrototype;
    public AssetRefEntityPrototype GameStatePrototype;
    
    partial void SerializeUserData(BitStream stream)
    {
      stream.Serialize(ref IsOnePlayerTest);
      stream.Serialize(ref IsInvincible);
      stream.Serialize(ref AutoAdjustToPlayerCount);
      
      // stream.Serialize(ref GridWidth);
      // stream.Serialize(ref GridHeight);
      // stream.Serialize(ref GridType);
      stream.Serialize(ref GridSize);
      stream.Serialize(ref CellSize);
      stream.Serialize(ref MinDistanceBetweenSpawnPoints);
      
      stream.Serialize(ref AmountOfDestroyableCells);

      stream.Serialize(ref DefaultBomberPrototype);
      stream.Serialize(ref DefaultBombPrototype);
      stream.Serialize(ref DefaultExplosionPrototype);
      stream.Serialize(ref DefaultBlockDestroyablePrototype);
      
      stream.Serialize(ref PowerUpManagerPrototype);
      stream.Serialize(ref GameStatePrototype);
    }
  }
}