﻿using System;
using Photon.Deterministic;

namespace Quantum
{
	[Serializable]
	[AssetObjectConfig(GenerateLinkingScripts = true, GenerateAssetCreateMenu = false, GenerateAssetResetMethod = false)]
	public partial class TrueDecision : HFSMDecision
	{
		public override unsafe bool Decide(Frame frame, EntityRef entity, ref AIContext aiContext)
		{
			return true;
		}
	}

  [Serializable]
  public partial class AnotherDecision : TrueDecision
  {
    public override unsafe bool Decide(Frame frame, EntityRef entity, ref AIContext aiContext)
    {
      return true;
    }
  }
}
