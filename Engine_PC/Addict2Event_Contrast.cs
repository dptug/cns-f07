using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_PC;

public class Addict2Event_Contrast : Addict2Event
{
	public RGBA StartCol;

	public RGBA EndCol;

	public Blend SrcBlend;

	public Blend DstBlend;

	public sbyte RenderTexture;

	public byte MulAmount;

	public byte AddAmount;

	private VertexBuffer vb;

	private VertexDeclaration decl;

	public Addict2Event_Contrast()
	{
		vb = new VertexBuffer(Addict2Engine.device, 4 * VertexPositionTexture.SizeInBytes, (ResourceUsage)0);
		VertexPositionTexture[] data = new VertexPositionTexture[4]
		{
			new VertexPositionTexture(new Vector3(0f, 0f, 0f), new Vector2(0f, 0f)),
			new VertexPositionTexture(new Vector3(1f, 0f, 0f), new Vector2(1f, 0f)),
			new VertexPositionTexture(new Vector3(0f, 1f, 0f), new Vector2(0f, 1f)),
			new VertexPositionTexture(new Vector3(1f, 1f, 0f), new Vector2(1f, 1f))
		};
		vb.SetData(data);
		decl = new VertexDeclaration(Addict2Engine.device, VertexPositionTexture.VertexElements);
	}

	public override void Render(DEFAULTEVENTDATA DEData)
	{
		Addict2Logger.Append("Rendering Contrast at " + DEData.TimePos + "\n");
		Addict2Logger.Append("Target is " + RenderTexture);
		Addict2RenderingTools.SwitchTo2D();
		Addict2Engine.device.RenderState.DepthBufferEnable = false;
		Addict2RenderingTools.SetColor((byte)Addict2RenderingTools.LinearInterpolate((int)StartCol.R, (int)EndCol.R, DEData.TimePos), (byte)Addict2RenderingTools.LinearInterpolate((int)StartCol.G, (int)EndCol.G, DEData.TimePos), (byte)Addict2RenderingTools.LinearInterpolate((int)StartCol.B, (int)EndCol.B, DEData.TimePos), (byte)Addict2RenderingTools.LinearInterpolate((int)StartCol.A, (int)EndCol.A, DEData.TimePos));
		Addict2Engine.device.Textures[0] = Addict2RenderingTools.RenderTextures[RenderTexture].TexImage;
		Addict2Engine.device.PixelShader = Addict2RenderingTools.pshContrast;
		Addict2Engine.device.SamplerStates[0].AddressU = TextureAddressMode.Clamp;
		Addict2Engine.device.SamplerStates[0].AddressV = TextureAddressMode.Clamp;
		Matrix identity = Matrix.Identity;
		float x = Addict2RenderingTools.RenderTextures[RenderTexture].x1;
		float x2 = Addict2RenderingTools.RenderTextures[RenderTexture].x2;
		float y = Addict2RenderingTools.RenderTextures[RenderTexture].y1;
		float y2 = Addict2RenderingTools.RenderTextures[RenderTexture].y2;
		identity.M11 = x2 - x;
		identity.M22 = y - y2;
		float num = (float)Addict2RenderingTools.RenderTextures[RenderTexture].TexImage.Height / (float)Addict2RenderingTools.RenderTextures[RenderTexture].TexImage.Width;
		identity.M31 = 0f + x - 1f / (float)(DEData.x2 - DEData.x1);
		identity.M32 = 1f - y - num / (float)(DEData.y2 - DEData.y1);
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
		Addict2Engine.device.VertexDeclaration = decl;
		Addict2Engine.device.VertexShader = Addict2RenderingTools.vshPosTex;
		Addict2Engine.device.Vertices[0].SetSource(vb, 0, VertexPositionTexture.SizeInBytes);
		Vector4 zero = Vector4.Zero;
		zero.X = MulAmount + 1;
		zero.Y = AddAmount + 1;
		Addict2Engine.device.SetPixelShaderConstant(1, zero);
		Addict2Engine.device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
		identity2 = Matrix.Identity;
		Addict2RenderingTools.SetTextureMatrix(identity2);
	}
}
