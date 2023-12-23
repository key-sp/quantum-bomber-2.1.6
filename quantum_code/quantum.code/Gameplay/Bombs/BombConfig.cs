using Photon.Deterministic;
using Quantum.Inspector;

namespace Quantum
{
	public partial class BombConfig
	{
		[Tooltip("In Seconds")]
		public FP TimeToLiveInSeconds = FP._3;
	}
}