namespace Engine_PC;

public class Addict2Event_RenderToTexture : Addict2Event
{
	public sbyte RenderTexture;

	public override void Render(DEFAULTEVENTDATA DEData)
	{
		Addict2Logger.Append("Rendering RenderToTexture at " + DEData.TimePos + "\n");
		Addict2RenderingTools.RenderTextures[RenderTexture].x1 = (float)DEData.x1 / (float)Addict2RenderingTools.TXR;
		Addict2RenderingTools.RenderTextures[RenderTexture].x2 = (float)DEData.x2 / (float)Addict2RenderingTools.TXR;
		Addict2RenderingTools.RenderTextures[RenderTexture].y1 = (float)DEData.y2 / (float)Addict2RenderingTools.TYR;
		Addict2RenderingTools.RenderTextures[RenderTexture].y2 = (float)DEData.y1 / (float)Addict2RenderingTools.TYR;
		Addict2Logger.Append("Target is " + RenderTexture);
		Addict2Engine.device.ResolveBackBuffer(Addict2RenderingTools.RenderTextures[RenderTexture].TexImage);
		Addict2RenderingTools.CopyRTBack(DEData, RenderTexture);
	}
}
