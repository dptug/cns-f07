using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_PC;

public class Addict2Event_OldFeedback : Addict2Event
{
	public RGBA StartCol;

	public RGBA EndCol;

	public Blend SrcBlend;

	public Blend DstBlend;

	public sbyte RenderTexture;

	public byte LayerNum;

	public byte LayerZoom;

	private VertexBuffer vb;

	private VertexDeclaration decl;

	public Addict2Event_OldFeedback()
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

	public override void Render(DEFAULTEVENTDATA DEData)
	{
		Addict2Logger.Append("Rendering OldFeedback at " + DEData.TimePos + "\n");
		Addict2RenderingTools.SwitchTo2D();
		Addict2Engine.device.RenderState.DepthBufferEnable = false;
		Addict2RenderingTools.SetColor((byte)Addict2RenderingTools.LinearInterpolate((int)StartCol.R, (int)EndCol.R, DEData.TimePos), (byte)Addict2RenderingTools.LinearInterpolate((int)StartCol.G, (int)EndCol.G, DEData.TimePos), (byte)Addict2RenderingTools.LinearInterpolate((int)StartCol.B, (int)EndCol.B, DEData.TimePos), (byte)Addict2RenderingTools.LinearInterpolate((int)StartCol.A, (int)EndCol.A, DEData.TimePos));
		Addict2Engine.device.Textures[0] = Addict2RenderingTools.RenderTextures[RenderTexture].TexImage;
		Addict2Engine.device.PixelShader = Addict2RenderingTools.pshTexColor;
		Addict2Engine.device.SamplerStates[0].AddressU = TextureAddressMode.Clamp;
		Addict2Engine.device.SamplerStates[0].AddressV = TextureAddressMode.Clamp;
		float x = Addict2RenderingTools.RenderTextures[RenderTexture].x1;
		float x2 = Addict2RenderingTools.RenderTextures[RenderTexture].x2;
		float y = Addict2RenderingTools.RenderTextures[RenderTexture].y1;
		float y2 = Addict2RenderingTools.RenderTextures[RenderTexture].y2;
		Matrix identity = Matrix.Identity;
		identity.M11 = x2 - x;
		identity.M22 = y - y2;
		identity.M31 = x;
		identity.M32 = 1f - y;
		Addict2RenderingTools.SetTextureMatrix(identity);
		Matrix identity2 = Matrix.Identity;
		identity2.M11 = DEData.x2 - DEData.x1;
		identity2.M22 = DEData.y2 - DEData.y1;
		identity2.M41 = DEData.x1;
		identity2.M42 = DEData.y1;
		Addict2RenderingTools.SetWorldMatrix(identity2);
		Addict2Engine.device.RenderState.AlphaBlendEnable = true;
		Addict2Engine.device.RenderState.SourceBlend = SrcBlend;
		Addict2Engine.device.RenderState.DestinationBlend = DstBlend;
		Addict2Engine.device.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
		Addict2Engine.device.VertexDeclaration = Addict2Engine.decl;
		Addict2Engine.device.VertexShader = Addict2RenderingTools.vshPosNormalTex;
		Addict2Engine.device.Vertices[0].SetSource(vb, 0, VertexPositionNormalTexture.SizeInBytes);
		float num = (float)(DEData.x2 - DEData.x1) / (float)(DEData.y2 - DEData.y1) * (float)Addict2RenderingTools.TYR / (float)Addict2RenderingTools.TXR;
		float num2 = (float)Addict2Engine.project.XRes / 800f * 1024f / (float)Addict2RenderingTools.TXR;
		float num3 = (float)Addict2Engine.project.YRes / 600f * 1024f / (float)Addict2RenderingTools.TXR;
		for (int i = 0; i < LayerNum; i++)
		{
			identity = Matrix.Identity;
			float num4 = (float)(int)LayerZoom / 255f / 100f * (float)Addict2RenderingTools.TXR / (float)Addict2RenderingTools.TYR;
			float num5 = x + (float)i * num4 * num * num2;
			float num6 = x2 - (float)i * num4 * num * num2;
			float num7 = y - (float)i * num4 * num3;
			float num8 = y2 + (float)i * num4 * num3;
			identity.M11 = num6 - num5;
			identity.M22 = num7 - num8;
			identity.M31 = num5;
			identity.M32 = 1f - num7;
			Addict2RenderingTools.SetTextureMatrix(identity);
			Addict2Engine.device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
		}
		identity2 = Matrix.Identity;
		Addict2RenderingTools.SetTextureMatrix(identity2);
	}
}
