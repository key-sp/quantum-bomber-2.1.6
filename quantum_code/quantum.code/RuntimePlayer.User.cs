using Photon.Deterministic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum {
  partial class RuntimePlayer
  {
    public ColorRGBA Color;
    
    partial void SerializeUserData(BitStream stream)
    {
      // implementation
      stream.Serialize(ref Color.R);
      stream.Serialize(ref Color.G);
      stream.Serialize(ref Color.B);
      stream.Serialize(ref Color.A);
    }
  }
}
