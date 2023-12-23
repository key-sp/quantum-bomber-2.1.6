using Photon.Deterministic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum {
  public static class SystemSetup {
    public static SystemBase[] CreateSystems(RuntimeConfig gameConfig, SimulationConfig simulationConfig) {
      return new SystemBase[] {
        // pre-defined core systems
        new Core.CullingSystem2D(), 
        new Core.CullingSystem3D(),
        
        new Core.PhysicsSystem2D(),
        new Core.PhysicsSystem3D(),

        Core.DebugCommand.CreateSystem(),

        new Core.NavigationSystem(),
        new Core.EntityPrototypeSystem(),
        new Core.PlayerConnectedSystem(),

        // user systems go here
        new SpawnPointSystem(),
        new PlayerSetupSystem(),

        // --- Sets Grid Cell Data for this tick
        new SetBroadphaseSystem(),
        new BlockDestroyableSystem(),
        
        // --- Game Systems
        new InputSystem(),
        
        // ------ Bomber Systems
        new MovementSystem(),
        new AbilityPlaceBombSystem(),
        new BomberSystem(),
        
        // ------ Level Environment Systems
        new PowerUpSystem(),
        new PowerUpManagerSystem(),
        
        // ------ Damage Systems
        new BombSystem(),
        new ExplosionSystem(),

        // --- Clears Grid Cell Data of expired entities
        new ClearBroadphaseSystem(),
        new GameSessionStateSystem(),
      };
    }
  }
}
