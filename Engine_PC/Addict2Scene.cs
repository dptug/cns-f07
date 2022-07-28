using Microsoft.Xna.Framework.Graphics;

namespace Engine_PC;

public class Addict2Scene
{
	public int ID;

	public bool ColorDiscard;

	public Addict2Object[] ObjectList;

	public int ObjectNum;

	public int ObjectCapacity;

	public int ZmaskList;

	public int NoZmaskList;

	public int ObjDummyCount;

	private OBJDUMMY[] ObjDummies;

	public void RenderList(bool ZMask)
	{
		for (int i = 0; i < ObjectNum; i++)
		{
			Addict2Object addict2Object = ObjectList[i];
			if (addict2Object.ZMask != ZMask)
			{
				continue;
			}
			if (!ColorDiscard)
			{
				Addict2RenderingTools.SetColor(addict2Object.Color);
			}
			if (addict2Object.Backface)
			{
				if (addict2Object.Backfront)
				{
					Addict2Engine.device.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
				}
				else
				{
					Addict2Engine.device.RenderState.CullMode = CullMode.CullClockwiseFace;
				}
			}
			else
			{
				Addict2Engine.device.RenderState.CullMode = CullMode.None;
			}
			Addict2Engine.device.RenderState.CullMode = CullMode.None;
			if (addict2Object.EnvMapped)
			{
				if (Addict2RenderingTools.FogEnable)
				{
					Addict2Engine.device.PixelShader = Addict2RenderingTools.pshTexEnvColorFog;
				}
				else
				{
					Addict2Engine.device.PixelShader = Addict2RenderingTools.pshTexEnvColor;
				}
				Addict2Engine.device.Textures[0] = addict2Object.Material.TextureHandle;
				Addict2Engine.device.Textures[1] = addict2Object.EnvMap.TextureHandle;
			}
			else if (addict2Object.Textured)
			{
				Addict2Engine.device.PixelShader = Addict2RenderingTools.pshTexColor;
				Addict2Engine.device.Textures[0] = addict2Object.Material.TextureHandle;
				Addict2Engine.device.Textures[1] = null;
			}
			else
			{
				Addict2Engine.device.PixelShader = Addict2RenderingTools.pshColor;
			}
			Addict2Engine.device.RenderState.AlphaBlendEnable = true;
			Addict2Engine.device.RenderState.SourceBlend = addict2Object.SRCBlend;
			Addict2Engine.device.RenderState.DestinationBlend = addict2Object.DSTBlend;
			Addict2Engine.device.RenderState.FillMode = (addict2Object.Wireframe ? FillMode.WireFrame : FillMode.Solid);
			Addict2Engine.device.VertexDeclaration = Addict2Engine.decl;
			Addict2Engine.device.VertexShader = Addict2RenderingTools.vshPosNormalTex;
			Addict2Engine.device.Indices = addict2Object.ib;
			Addict2Engine.device.Vertices[0].SetSource(addict2Object.vb, 0, VertexPositionNormalTexture.SizeInBytes);
			Addict2Engine.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, addict2Object.VertexNum, 0, addict2Object.PolygonNum);
		}
	}
}
