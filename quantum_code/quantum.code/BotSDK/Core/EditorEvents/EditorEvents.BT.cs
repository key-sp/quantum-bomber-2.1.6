using System;
using System.Runtime.CompilerServices;

namespace Quantum
{
  public static partial class BotSDKEditorEvents
  {
    public static class BT
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static void InvokeAction(Action<EntityRef, long, bool> action, EntityRef entityRefParam, long longParam, bool boolParam)
      {
        try
        {
          action?.Invoke(entityRefParam, longParam, boolParam);
        }
        catch (Exception e)
        {
          Log.Exception(e);
        }
      }

      // --------------------
      // ON SETUP DEBUGGER
      // --------------------
      private static event Action<EntityRef, string, bool> _onSetupDebugger;
      public static event Action<EntityRef, string, bool> OnSetupDebugger
      {
        add => _onSetupDebugger += value;
        remove => _onSetupDebugger -= value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static void InvokeOnSetupDebugger(EntityRef entityRef, string treePath, bool isCompound)
      {
        try
        {
          _onSetupDebugger?.Invoke(entityRef, treePath, isCompound);
        }
        catch (Exception e)
        {
          Log.Exception(e);
        }
      }

      // --------------------
      // ON TREE COMPLETED
      // --------------------
      private static event Action<EntityRef, bool> _onTreeCompleted;
      public static event Action<EntityRef, bool> OnTreeCompleted
      {
        add => _onTreeCompleted += value;
        remove => _onTreeCompleted -= value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static void InvokeOnTreeCompleted(EntityRef entityRef, bool isCompound)
      {
        try
        {
          _onTreeCompleted?.Invoke(entityRef, isCompound);
        }
        catch (Exception e)
        {
          Log.Exception(e);
        }
      }

      // --------------------
      // ON NODE ENTER
      // --------------------
      private static event Action<EntityRef, long, bool> _onNodeEnter;
      public static event Action<EntityRef, long, bool> OnNodeEnter
      {
        add => _onNodeEnter += value;
        remove => _onNodeEnter -= value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static void InvokeOnNodeEnter(EntityRef entityRef, long nodeId, bool isCompound)
      {
        InvokeAction(_onNodeEnter, entityRef, nodeId, isCompound);
      }

      // --------------------
      // ON NODE EXIT
      // --------------------
      private static event Action<EntityRef, long, bool> _onNodeExit;
      public static event Action<EntityRef, long, bool> OnNodeExit
      {
        add => _onNodeExit += value;
        remove => _onNodeExit -= value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static void InvokeOnNodeExit(EntityRef entityRef, long nodeId, bool isCompound)
      {
        InvokeAction(_onNodeExit, entityRef, nodeId, isCompound);
      }

      // --------------------
      // ON NODE SUCCESS
      // --------------------
      private static event Action<EntityRef, long, bool> _onNodeSuccess;
      public static event Action<EntityRef, long, bool> OnNodeSuccess
      {
        add => _onNodeSuccess += value;
        remove => _onNodeSuccess -= value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static void InvokeOnNodeSuccess(EntityRef entityRef, long nodeId, bool isCompound)
      {
        InvokeAction(_onNodeSuccess, entityRef, nodeId, isCompound);
      }

      // --------------------
      // ON NODE FAILURE
      // --------------------
      private static event Action<EntityRef, long, bool> _onNodeFailure;
      public static event Action<EntityRef, long, bool> OnNodeFailure
      {
        add => _onNodeFailure += value;
        remove => _onNodeFailure -= value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static void InvokeOnNodeFailure(EntityRef entityRef, long nodeId, bool isCompound)
      {
        InvokeAction(_onNodeFailure, entityRef, nodeId, isCompound);
      }

      // --------------------
      // ON DECORATOR CHECKED
      // --------------------
      private static event Action<EntityRef, long, bool, bool> _onDecoratorChecked;
      public static event Action<EntityRef, long, bool, bool> OnDecoratorChecked
      {
        add => _onDecoratorChecked += value;
        remove => _onDecoratorChecked -= value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static void InvokeOnDecoratorChecked(EntityRef entityRef, long nodeId, bool success, bool isCompound)
      {
        try
        {
          _onDecoratorChecked?.Invoke(entityRef, nodeId, success, isCompound);
        }
        catch (Exception e)
        {
          Log.Exception(e);
        }
      }

      // --------------------
      // ON DECORATOR RESET
      // --------------------
      private static event Action<EntityRef, long, bool> _onDecoratorReset;
      public static event Action<EntityRef, long, bool> OnDecoratorReset
      {
        add => _onDecoratorReset += value;
        remove => _onDecoratorReset -= value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static void InvokeOnDecoratorReset(EntityRef entityRef, long nodeId, bool isCompound)
      {
        InvokeAction(_onDecoratorReset, entityRef, nodeId, isCompound);
      }
    }
  }
}
