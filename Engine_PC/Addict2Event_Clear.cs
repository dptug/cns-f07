using Microsoft.Xna.Framework.Graphics;

namespace Engine_PC;

public class Addict2Event_Clear : Addict2Event
{
	public RGBA Col;

	public bool Screen;

	public bool Zbuffer;

	public override void Render(DEFAULTEVENTDATA d)
	{
		Addict2Logger.Append("Rendering Clear at " + d.TimePos + "\n");
		ClearOptions clearOptions = (ClearOptions)0;
		if (Screen)
		{
			clearOptions |= ClearOptions.Target;
		}
		if (Zbuffer)
		{
			clearOptions |= ClearOptions.DepthBuffer;
		}
		Addict2Engine.device.Clear(clearOptions, new Color(Col.R, Col.G, Col.B, Col.A), 1f, 0);
	}
}
