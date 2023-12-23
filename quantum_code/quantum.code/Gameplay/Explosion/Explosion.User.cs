namespace Quantum
{
	public partial struct Explosion
	{
		public bool HasReachedEnd => AllDirectionsBlocked || CurrentReach >= MaxReach;
	}
}