namespace Quantum
{
	[System.Serializable]
	public unsafe class DebugAction : AIAction
	{
		public string Message;

		public override void Update(Frame frame, EntityRef entity, ref AIContext aiContext)
		{
			Log.Info(Message);
		}
	}

  [System.Serializable]
  public unsafe class DebugAgain : DebugAction
  {
    public string Messagesss;

    public override void Update(Frame frame, EntityRef entity, ref AIContext aiContext)
    {
    }
  }
}
