using System;

namespace Engine_PC;

public class Addict2Timer
{
	private long Start;

	public Addict2Timer()
	{
		Reset();
	}

	public void Reset()
	{
		Start = DateTime.UtcNow.Ticks;
	}

	public int GetTime()
	{
		return (int)((DateTime.UtcNow.Ticks - Start) / 10000);
	}
}
