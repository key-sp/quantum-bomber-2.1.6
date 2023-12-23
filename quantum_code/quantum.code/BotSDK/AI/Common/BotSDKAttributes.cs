namespace Quantum
{
	[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Class | System.AttributeTargets.Struct)]
	public class BotSDKHiddenAttribute : System.Attribute
	{
	}

	[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
	public class BotSDKCategoryAttribute : System.Attribute
	{
		public string Name;
	}
}
