using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_PC;

public class Addict2Logger
{
	private SpriteFont font;

	private SpriteBatch batch;

	private static string Addict2LoggerString;

	private Color c = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);

	public Addict2Logger(ContentManager content, GraphicsDevice device)
	{
		font = content.Load<SpriteFont>("data\\SpriteFont1");
		batch = new SpriteBatch(device);
		Addict2LoggerString = "";
	}

	public static void Append(string a)
	{
		char[] trimChars = new char[2] { '\n', '\r' };
		Addict2LoggerString = Addict2LoggerString + a.TrimEnd(trimChars) + "\n";
	}

	public void PrintLog()
	{
		Addict2Engine.device.RenderState.FillMode = FillMode.Solid;
		batch.Begin();
		batch.DrawString(font, Addict2LoggerString, new Vector2(5f, 5f), c);
		batch.End();
		Addict2Engine.device.RenderState.AlphaTestEnable = false;
		Addict2Engine.device.RenderState.AlphaFunction = CompareFunction.Always;
		Addict2Engine.device.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
		Addict2Engine.device.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
		Addict2LoggerString = "";
	}
}
