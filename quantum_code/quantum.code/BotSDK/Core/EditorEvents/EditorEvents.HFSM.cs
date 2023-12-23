using System;
using System.Runtime.CompilerServices;

namespace Quantum
{
  public static partial class BotSDKEditorEvents
  {
    public static class HFSM
    {
      private static event Action<EntityRef, long, string> _stateChanged;
      public static event Action<EntityRef, long, string> StateChanged
      {
        add => _stateChanged += value;
        remove => _stateChanged -= value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static void InvokeStateChanged(EntityRef entityRef, long newStateGuid, string transitionId)
      {
        try
        {
          _stateChanged?.Invoke(entityRef, newStateGuid, transitionId);
        }
        catch (Exception e)
        {
          Log.Exception(e);
        }
      }
    }
  }
}