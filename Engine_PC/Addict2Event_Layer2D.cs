using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_PC;

public class Addict2Event_Layer2D : Addict2Event
{
	public RGBA StartCol;

	public RGBA EndCol;

	public Blend SrcBlend;

	public Blend DstBlend;

	public bool Textured;

	public Addict2Material Texture;

	private VertexBuffer vb;

	public Addict2Event_Layer2D()
	{
		vb = new VertexBuffer(Addict2Engine.device, 4 * VertexPositionNormalTexture.SizeInBytes, (ResourceUsage)0);
		VertexPositionNormalTexture[] data = new VertexPositionNormalTexture[4]
		{
			new VertexPositionNormalTexture(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector2(0f, 0f)),
			new VertexPositionNormalTexture(new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector2(1f, 0f)),
			new VertexPositionNormalTexture(new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 0f), new Vector2(0f, 1f)),
			new VertexPositionNormalTexture(new Vector3(1f, 1f, 0f), new Vector3(0f, 0f, 0f), new Vector2(1f, 1f))
		};
		vb.SetData(data);
	}

	public override void Render(DEFAULTEVENTDATA d)
	{
		Addict2Logger.Append("Rendering Layer2D at " + d.TimePos + "\n");
		Addict2RenderingTools.SwitchTo2D();
		Addict2Engine.device.RenderState.DepthBufferEnable = false;
		Addict2RenderingTools.SetColor((byte)Addict2RenderingTools.LinearInterpolate((int)StartCol.R, (int)EndCol.R, d.TimePos), (byte)Addict2RenderingTools.LinearInterpolate((int)StartCol.G, (int)EndCol.G, d.TimePos), (byte)Addict2RenderingTools.LinearInterpolate((int)StartCol.B, (int)EndCol.B, d.TimePos), (byte)Addict2RenderingTools.LinearInterpolate((int)StartCol.A, (int)EndCol.A, d.TimePos));
		if (Textured && Texture != null)
		{
			Addict2Engine.device.Textures[0] = Texture.TextureHandle;
			Addict2Engine.device.PixelShader = Addict2RenderingTools.pshTexColor;
		}
		else
		{
			Addict2Engine.device.Textures[0] = null;
			Addict2Engine.device.PixelShader = Addict2RenderingTools.pshColor;
		}
		Matrix identity = Matrix.Identity;
		identity.M11 = d.x2 - d.x1;
		identity.M22 = d.y2 - d.y1;
		identity.M41 = d.x1;
		identity.M42 = d.y1;
		Addict2RenderingTools.SetWorldMatrix(identity);
		Addict2Engine.device.RenderState.AlphaBlendEnable = true;
		Addict2Engine.device.RenderState.SourceBlend = SrcBlend;
		Addict2Engine.device.RenderState.DestinationBlend = DstBlend;
		Addict2Engine.device.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
		Addict2Engine.device.VertexDeclaration = Addict2Engine.decl;
		Addict2Engine.device.VertexShader = Addict2RenderingTools.vshPosNormalTex;
		Addict2Engine.device.Vertices[0].SetSource(vb, 0, VertexPositionNormalTexture.SizeInBytes);
		Addict2Engine.device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
	}
}
