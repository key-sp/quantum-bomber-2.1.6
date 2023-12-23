using Quantum.Collections;

namespace Quantum
{
	public interface IConsiderationProvider
	{
		public AssetRefConsideration GetConsideration(QList<AssetRefConsideration> sourceList, int id);
		int Count(QList<AssetRefConsideration> sourceList);
	}
}
