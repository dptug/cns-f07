namespace Engine_PC;

public class Addict2Event
{
	public int StartFrame;

	public int EndFrame;

	public Addict2EventType EventType;

	public int Pass;

	public int sx1;

	public int sy1;

	public int sx2;

	public int sy2;

	public int ex1;

	public int ey1;

	public int ex2;

	public int ey2;

	public bool OnScreenInLastFrame;

	public virtual void Render(DEFAULTEVENTDATA d)
	{
	}
}
