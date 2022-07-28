using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_PC;

public class Addict2Project
{
	public Blend[] SRCBlends = new Blend[9]
	{
		Blend.Zero,
		Blend.One,
		Blend.DestinationColor,
		Blend.InverseDestinationColor,
		Blend.DestinationAlpha,
		Blend.InverseDestinationAlpha,
		Blend.SourceAlpha,
		Blend.InverseSourceAlpha,
		Blend.SourceAlphaSaturation
	};

	public Blend[] DSTBlends = new Blend[8]
	{
		Blend.Zero,
		Blend.One,
		Blend.SourceColor,
		Blend.InverseSourceColor,
		Blend.DestinationAlpha,
		Blend.InverseDestinationAlpha,
		Blend.SourceAlpha,
		Blend.InverseSourceAlpha
	};

	public int nTextures;

	public int nMaterials;

	public Addict2Material[] pMaterials;

	public int nPolySelections;

	public int nScenes;

	public Addict2Scene[] pScenes;

	public int nWorlds;

	public Addict2World[] pWorlds;

	public int nEvents;

	public Addict2Event[] pEvents;

	public float AspectRatio = 1.7777778f;

	public bool CropAspect;

	public int XRes;

	public int YRes;

	public float IntroAspect = 1.7777778f;

	public int IntroXRes;

	public int IntroYRes;

	public int IntroXOff;

	public int IntroYOff;

	private DEFAULTEVENTDATA d;

	public void LoadTextures(Addict2BinaryReader stream)
	{
		for (int i = 0; i < 4; i++)
		{
			Addict2Engine.device.SamplerStates[i].MinFilter = TextureFilter.Linear;
			Addict2Engine.device.SamplerStates[i].MagFilter = TextureFilter.Linear;
			Addict2Engine.device.SamplerStates[i].MipFilter = TextureFilter.Linear;
			Addict2Engine.device.SamplerStates[i].AddressU = TextureAddressMode.Wrap;
			Addict2Engine.device.SamplerStates[i].AddressV = TextureAddressMode.Wrap;
		}
		nMaterials = 0;
		nTextures = stream.ReadByte();
		pMaterials = new Addict2Material[nTextures * 8];
		for (int j = 0; j < nTextures; j++)
		{
			int num = stream.ReadByte();
			int num2 = 32;
			for (int k = 0; k < num; k++)
			{
				num2 <<= 1;
			}
			int num3 = stream.ReadByte();
			int textureID = stream.ReadByte();
			for (int l = 0; l < num3; l++)
			{
				int num4 = stream.ReadByte();
				stream.ReadByte();
				stream.ReadByte();
				int[] array = new int[37]
				{
					1, 2, 6, 2, 3, 7, 2, 4, 9, 2,
					1, 1, 3, 1, 4, 2, 3, 3, 2, 1,
					2, 0, 0, 3, 2, 2, 2, 4, 4, 1,
					1, 16, 6, 5, 8, 4, 1
				};
				switch (num4)
				{
				case 5:
					stream.ReadBytes(7);
					while (stream.ReadByte() != 0)
					{
					}
					break;
				case 35:
				{
					uint count = stream.ReadUInt32();
					stream.ReadBytes((int)count);
					break;
				}
				default:
					stream.ReadBytes(array[num4]);
					break;
				case 30:
				{
					byte slotNumber = stream.ReadByte();
					stream.ReadBoolean();
					Addict2Material addict2Material = new Addict2Material();
					addict2Material.Size = num2;
					addict2Material.TextureID = textureID;
					addict2Material.Data = new uint[addict2Material.Size * addict2Material.Size];
					for (int m = 0; m < addict2Material.Size * addict2Material.Size; m++)
					{
						RGBA rGBA = new RGBA(stream.ReadUInt32());
						RGBA rGBA2 = new RGBA(rGBA.B, rGBA.G, rGBA.R, rGBA.A);
						addict2Material.Data[m] = rGBA2.AsDword;
					}
					addict2Material.SlotNumber = slotNumber;
					int num5 = 8;
					for (int n = 0; n < 32; n++)
					{
						if (((1 << n) & addict2Material.Size) > 0)
						{
							num5 = n;
							break;
						}
					}
					addict2Material.TextureHandle = new Texture2D(Addict2Engine.device, addict2Material.Size, addict2Material.Size, num5, (ResourceUsage)0, SurfaceFormat.Color, (ResourceManagementMode)1);
					for (int num6 = 0; num6 < num5; num6++)
					{
						int num7 = addict2Material.Size >> num6;
						int num8 = addict2Material.Size >> num6;
						uint[] array2 = new uint[num7 * num8];
						for (int num9 = 0; num9 < num7; num9++)
						{
							for (int num10 = 0; num10 < num8; num10++)
							{
								array2[num10 + num8 * num9] = addict2Material.Data[num9 * (1 << num6) * addict2Material.Size + num10 * (1 << num6)];
							}
						}
						addict2Material.TextureHandle.SetData(num6, null, array2, 0, num7 * num8, SetDataOptions.None);
						array2 = null;
					}
					addict2Material.Data = null;
					pMaterials[nMaterials++] = addict2Material;
					break;
				}
				}
			}
		}
	}

	private Addict2Material FindMaterial(int TextureID, int SlotNumber)
	{
		for (int i = 0; i < nMaterials; i++)
		{
			if (pMaterials[i].TextureID == TextureID && pMaterials[i].SlotNumber == SlotNumber)
			{
				return pMaterials[i];
			}
		}
		return null;
	}

	public void LoadPolyselections(Addict2BinaryReader stream)
	{
		nPolySelections = stream.ReadByte();
	}

	private Addict2Scene FindScene(int ID)
	{
		for (int i = 0; i < nScenes; i++)
		{
			if (pScenes[i].ID == ID)
			{
				return pScenes[i];
			}
		}
		return null;
	}

	public void LoadScenes(Addict2BinaryReader stream)
	{
		nScenes = stream.ReadByte();
		pScenes = new Addict2Scene[nScenes];
		for (int i = 0; i < nScenes; i++)
		{
			pScenes[i] = new Addict2Scene();
			pScenes[i].ID = stream.ReadByte();
			pScenes[i].ColorDiscard = stream.ReadByte() > 0;
			pScenes[i].ObjectNum = stream.ReadUInt16();
			pScenes[i].ObjectList = new Addict2Object[pScenes[i].ObjectNum];
			for (int j = 0; j < pScenes[i].ObjectNum; j++)
			{
				Addict2Object addict2Object = new Addict2Object();
				addict2Object.Primitive = (Addict2ObjectPrimitiveType)stream.ReadByte();
				if (addict2Object.Primitive == Addict2ObjectPrimitiveType.STORED)
				{
					uint num = stream.ReadUInt32();
					for (int k = 0; k < num; k++)
					{
						Vector3 vector = default(Vector3);
						vector.X = stream.ReadCompactFloat(3);
						vector.Y = stream.ReadCompactFloat(3);
						vector.Z = stream.ReadCompactFloat(3);
						Vector3 currentNormal = default(Vector3);
						currentNormal.X = (float)stream.ReadSByte() / 127f;
						currentNormal.Y = (float)stream.ReadSByte() / 127f;
						currentNormal.Z = (float)stream.ReadSByte() / 127f;
						currentNormal.Normalize();
						Vector2 vector2 = default(Vector2);
						vector2.X = (float)stream.ReadInt16() / 32767f;
						vector2.Y = (float)stream.ReadInt16() / 32767f;
						addict2Object.AddVertex(vector.X, vector.Y, vector.Z, vector2.X, vector2.Y);
						addict2Object.VertexList[addict2Object.VertexNum - 1].Normal = (addict2Object.VertexList[addict2Object.VertexNum - 1].CurrentNormal = currentNormal);
					}
					bool flag = stream.ReadByte() > 0;
					num = stream.ReadUInt32();
					for (int l = 0; l < num; l++)
					{
						int x = stream.ReadInt32();
						int y = stream.ReadInt32();
						int z = stream.ReadInt32();
						Vector3 currentNormal2 = default(Vector3);
						currentNormal2.X = (float)stream.ReadSByte() / 127f;
						currentNormal2.Y = (float)stream.ReadSByte() / 127f;
						currentNormal2.Z = (float)stream.ReadSByte() / 127f;
						addict2Object.AddPolygon(x, y, z, Addict2Shading.GOURAUDSHADE, e1: true, e2: true, e3: true);
						POLYGON pOLYGON = addict2Object.PolygonList[addict2Object.PolygonNum - 1];
						pOLYGON.Normal = (pOLYGON.CurrentNormal = currentNormal2);
						ref Vector2 reference = ref pOLYGON.ct[0];
						ref Vector2 reference2 = ref pOLYGON.t[0];
						reference = (reference2 = addict2Object.VertexList[pOLYGON.v[0]].TextureCoordinate);
						ref Vector2 reference3 = ref pOLYGON.ct[1];
						ref Vector2 reference4 = ref pOLYGON.t[1];
						reference3 = (reference4 = addict2Object.VertexList[pOLYGON.v[1]].TextureCoordinate);
						ref Vector2 reference5 = ref pOLYGON.ct[2];
						ref Vector2 reference6 = ref pOLYGON.t[2];
						reference5 = (reference6 = addict2Object.VertexList[pOLYGON.v[2]].TextureCoordinate);
						if (flag)
						{
							pOLYGON.ct[0].X = (float)stream.ReadSByte() / 127f;
							pOLYGON.ct[0].Y = (float)stream.ReadSByte() / 127f;
							pOLYGON.ct[1].X = (float)stream.ReadSByte() / 127f;
							pOLYGON.ct[1].Y = (float)stream.ReadSByte() / 127f;
							pOLYGON.ct[2].X = (float)stream.ReadSByte() / 127f;
							pOLYGON.ct[2].Y = (float)stream.ReadSByte() / 127f;
						}
						addict2Object.PolygonList[addict2Object.PolygonNum - 1] = pOLYGON;
					}
					addict2Object.Xtile = (addict2Object.Ytile = 1);
					addict2Object.OffsetX = (addict2Object.OffsetY = 0f);
					addict2Object.Color = new RGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
					_ = addict2Object.Primitive;
					addict2Object.ID = stream.ReadUInt16();
					Addict2ObjectData addict2ObjectData = new Addict2ObjectData(stream.ReadUInt32());
					Addict2ObjectData2 addict2ObjectData2 = new Addict2ObjectData2(stream.ReadByte());
					addict2Object.Backface = addict2ObjectData2.BackFace;
					addict2Object.Backfront = addict2ObjectData2.BackFront;
					addict2Object.AEpsilon = stream.ReadByte();
					addict2Object.ModelView = stream.ReadMatrix(2, addict2ObjectData2.Orientation, addict2ObjectData2.Position);
					addict2Object.Wireframe = addict2ObjectData.Wireframe != 0;
					addict2Object.Textured = addict2ObjectData.Textured != 0;
					addict2Object.EnvMapped = addict2ObjectData.Envmapped != 0;
					addict2Object.NormalsInverted = addict2ObjectData.NormalsInverted != 0;
					addict2Object.XSwap = addict2ObjectData.XSwap != 0;
					addict2Object.YSwap = addict2ObjectData.YSwap != 0;
					addict2Object.Swap = addict2ObjectData.Swap != 0;
					addict2Object.ZMask = addict2ObjectData.Zmask != 0;
					addict2Object.Shading = (Addict2Shading)addict2ObjectData.Shading;
					addict2Object.AEpsilon = (byte)addict2ObjectData.AEpsilon;
					addict2Object.SRCBlend = SRCBlends[addict2ObjectData.SRCBlend];
					addict2Object.DSTBlend = DSTBlends[addict2ObjectData.DSTBlend];
					if (addict2ObjectData.XTile != 0)
					{
						addict2Object.Xtile = stream.ReadByte();
					}
					if (addict2ObjectData.YTile != 0)
					{
						addict2Object.Ytile = stream.ReadByte();
					}
					if (addict2ObjectData.Offx != 0)
					{
						addict2Object.OffsetX = (int)stream.ReadByte();
						addict2Object.OffsetX /= 255f;
					}
					if (addict2ObjectData.Offy != 0)
					{
						stream.ReadByte();
						addict2Object.OffsetY /= 255f;
					}
					if (addict2ObjectData.Red != 0)
					{
						addict2Object.Color.R = stream.ReadByte();
					}
					if (addict2ObjectData.Green != 0)
					{
						addict2Object.Color.G = stream.ReadByte();
					}
					if (addict2ObjectData.Blue != 0)
					{
						addict2Object.Color.B = stream.ReadByte();
					}
					if (addict2ObjectData.Alpha != 0)
					{
						addict2Object.Color.A = stream.ReadByte();
					}
					if (addict2ObjectData.Textured != 0)
					{
						int textureID = stream.ReadByte();
						addict2Object.Material = FindMaterial(textureID, (int)addict2ObjectData.TexSlot);
					}
					if (addict2ObjectData.Envmapped != 0)
					{
						int textureID2 = stream.ReadByte();
						addict2Object.EnvMap = FindMaterial(textureID2, (int)addict2ObjectData.EnvSlot);
					}
					Matrix modelView = addict2Object.ModelView;
					modelView = Matrix.Invert(modelView);
					modelView = Matrix.Transpose(modelView);
					for (int m = 0; m < addict2Object.VertexNum; m++)
					{
						addict2Object.VertexList[m].Position = Vector3.Transform(addict2Object.VertexList[m].Position, addict2Object.ModelView);
						addict2Object.VertexList[m].Normal = Vector3.TransformNormal(addict2Object.VertexList[m].Normal, addict2Object.ModelView);
					}
					for (int n = 0; n < addict2Object.PolygonNum; n++)
					{
						addict2Object.PolygonList[n].Normal = Vector3.TransformNormal(addict2Object.PolygonList[n].Normal, addict2Object.ModelView);
					}
					if (addict2Object.Shading == Addict2Shading.FLATSHADE)
					{
						addict2Object.vb = new VertexBuffer(Addict2Engine.device, addict2Object.PolygonNum * 3 * VertexPositionNormalTexture.SizeInBytes, (ResourceUsage)0, (ResourceManagementMode)1);
						VertexPositionNormalTexture[] array = new VertexPositionNormalTexture[addict2Object.PolygonNum * 3];
						for (int num2 = 0; num2 < addict2Object.PolygonNum; num2++)
						{
							for (int num3 = 0; num3 < 3; num3++)
							{
								array[num2 * 3 + num3].Position = addict2Object.VertexList[addict2Object.PolygonList[num2].v[num3]].Position;
								array[num2 * 3 + num3].Normal = addict2Object.PolygonList[num2].Normal;
								array[num2 * 3 + num3].TextureCoordinate = addict2Object.VertexList[addict2Object.PolygonList[num2].v[num3]].TextureCoordinate;
							}
						}
						addict2Object.VertexNum = addict2Object.PolygonNum * 3;
						addict2Object.vb.SetData(array);
						addict2Object.ib = new IndexBuffer(Addict2Engine.device, addict2Object.PolygonNum * 4 * 3, (ResourceUsage)0, IndexElementSize.ThirtyTwoBits);
						uint[] array2 = new uint[addict2Object.PolygonNum * 3];
						for (int num4 = 0; num4 < addict2Object.PolygonNum * 3; num4++)
						{
							array2[num4] = (uint)num4;
						}
						addict2Object.ib.SetData(array2);
						addict2Object.VertexList = null;
						addict2Object.PolygonList = null;
					}
					else
					{
						addict2Object.vb = new VertexBuffer(Addict2Engine.device, addict2Object.VertexNum * VertexPositionNormalTexture.SizeInBytes, (ResourceUsage)0, (ResourceManagementMode)1);
						VertexPositionNormalTexture[] array3 = new VertexPositionNormalTexture[addict2Object.VertexNum];
						for (int num5 = 0; num5 < addict2Object.VertexNum; num5++)
						{
							array3[num5].Position = addict2Object.VertexList[num5].Position;
							array3[num5].Normal = addict2Object.VertexList[num5].Normal;
							array3[num5].TextureCoordinate = addict2Object.VertexList[num5].TextureCoordinate;
						}
						addict2Object.vb.SetData(array3);
						addict2Object.ib = new IndexBuffer(Addict2Engine.device, addict2Object.PolygonNum * 2 * 3, (ResourceUsage)0, IndexElementSize.SixteenBits);
						ushort[] array4 = new ushort[addict2Object.PolygonNum * 3];
						for (int num6 = 0; num6 < addict2Object.PolygonNum; num6++)
						{
							array4[num6 * 3] = (ushort)addict2Object.PolygonList[num6].v[0];
							array4[num6 * 3 + 1] = (ushort)addict2Object.PolygonList[num6].v[1];
							array4[num6 * 3 + 2] = (ushort)addict2Object.PolygonList[num6].v[2];
						}
						addict2Object.ib.SetData(array4);
						addict2Object.VertexList = null;
						addict2Object.PolygonList = null;
					}
					pScenes[i].ObjectList[j] = addict2Object;
					continue;
				}
				throw new NotImplementedException("Primitive = " + addict2Object.Primitive);
			}
		}
	}

	public Addict2Spline ImportSplineFloat(Addict2BinaryReader stream)
	{
		Addict2Spline addict2Spline = new Addict2Spline();
		int num = stream.ReadByte();
		addict2Spline.Interpolation = (Addict2SplineInterpolationType)stream.ReadByte();
		addict2Spline.Keys = new Key[num + 1];
		addict2Spline.KeyNum = num;
		for (int i = 0; i < num; i++)
		{
			addict2Spline.Keys[i].Time = stream.ReadCompactFloat(2);
			addict2Spline.Keys[i].Value = stream.ReadCompactFloat(2);
		}
		addict2Spline.InitVectors();
		return addict2Spline;
	}

	public Addict2Spline ImportSplineColor(Addict2BinaryReader stream)
	{
		Addict2Spline addict2Spline = new Addict2Spline();
		int num = stream.ReadByte();
		addict2Spline.Interpolation = (Addict2SplineInterpolationType)stream.ReadByte();
		addict2Spline.Keys = new Key[num + 1];
		addict2Spline.KeyNum = num;
		for (int i = 0; i < num; i++)
		{
			addict2Spline.Keys[i].Time = stream.ReadCompactFloat(2);
			addict2Spline.Keys[i].Value = (int)stream.ReadByte();
		}
		addict2Spline.InitVectors();
		return addict2Spline;
	}

	private Addict2World FindWorld(int ID)
	{
		for (int i = 0; i < nWorlds; i++)
		{
			if (pWorlds[i].ID == ID)
			{
				return pWorlds[i];
			}
		}
		return null;
	}

	public void LoadWorlds(Addict2BinaryReader stream)
	{
		Addict2ParticleEngine.MaxParticleNum = 0;
		nWorlds = stream.ReadByte();
		pWorlds = new Addict2World[nWorlds];
		for (int i = 0; i < nWorlds; i++)
		{
			Addict2World addict2World = new Addict2World();
			addict2World.ID = stream.ReadInt16();
			addict2World.Fog = stream.ReadBoolean();
			if (addict2World.Fog)
			{
				addict2World.FogCol = new RGBA(stream.ReadByte(), stream.ReadByte(), stream.ReadByte(), stream.ReadByte());
				addict2World.FogStart = stream.ReadCompactFloat(2);
				addict2World.FogEnd = stream.ReadCompactFloat(2);
			}
			addict2World.Lighting = stream.ReadBoolean();
			if (addict2World.Lighting)
			{
				addict2World.Lights = new LIGHT[8];
				byte b = stream.ReadByte();
				for (int j = 0; j < b; j++)
				{
					addict2World.Lights[j].Lit = true;
					addict2World.Lights[j].Identifier = stream.ReadByte();
					addict2World.Lights[j].Linear_Attenuation = 1f;
					stream.ReadByte();
					addict2World.Lights[j].Ambient = new float[4];
					for (int k = 0; k < 3; k++)
					{
						int num = stream.ReadByte();
						addict2World.Lights[j].Ambient[k] = (float)num / 255f;
					}
					addict2World.Lights[j].Color = new float[4];
					for (int k = 0; k < 3; k++)
					{
						int num2 = stream.ReadByte();
						addict2World.Lights[j].Color[k] = (float)num2 / 255f;
					}
					addict2World.Lights[j].Position = new float[4];
					for (int k = 0; k < 4; k++)
					{
						addict2World.Lights[j].Position[k] = stream.ReadCompactFloat(2);
					}
					if (addict2World.Lights[j].Position[3] != 0f)
					{
						addict2World.Lights[j].Constant_Attenuation = stream.ReadCompactFloat(2);
						addict2World.Lights[j].Linear_Attenuation = stream.ReadCompactFloat(2);
						addict2World.Lights[j].Quadratic_Attenuation = stream.ReadCompactFloat(2);
					}
					addict2World.Lights[j].Spot_Direction = new float[4];
					addict2World.Lights[j].Spot_Cutoff = stream.ReadCompactFloat(2);
					if (addict2World.Lights[j].Spot_Cutoff != 180f)
					{
						for (int k = 0; k < 3; k++)
						{
							addict2World.Lights[j].Spot_Direction[k] = stream.ReadCompactFloat(2);
						}
						addict2World.Lights[j].Spot_Exponent = stream.ReadCompactFloat(2);
					}
				}
			}
			addict2World.CamNum = stream.ReadByte();
			addict2World.CamCapacity = stream.ReadByte();
			addict2World.CamAnims = new CAMSPLINES[addict2World.CamCapacity];
			for (int l = 0; l < addict2World.CamNum; l++)
			{
				addict2World.CamAnims[l].CamID = stream.ReadByte();
				addict2World.CamAnims[l].Eyex = ImportSplineFloat(stream);
				addict2World.CamAnims[l].Eyey = ImportSplineFloat(stream);
				addict2World.CamAnims[l].Eyez = ImportSplineFloat(stream);
				addict2World.CamAnims[l].Trgx = ImportSplineFloat(stream);
				addict2World.CamAnims[l].Trgy = ImportSplineFloat(stream);
				addict2World.CamAnims[l].Trgz = ImportSplineFloat(stream);
				addict2World.CamAnims[l].Roll = ImportSplineFloat(stream);
				addict2World.CamAnims[l].Fov = ImportSplineFloat(stream);
			}
			addict2World.AnimNum = stream.ReadByte();
			addict2World.AnimCapacity = stream.ReadByte();
			addict2World.Animations = new ANIMDATA[addict2World.AnimCapacity];
			addict2World.ObjectNum = stream.ReadUInt16();
			addict2World.ObjectList = new WORLDOBJECT[addict2World.ObjectNum];
			for (int m = 0; m < addict2World.ObjectNum; m++)
			{
				WORLDOBJECT wORLDOBJECT = new WORLDOBJECT();
				wORLDOBJECT.Color = new RGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				wORLDOBJECT.ID = stream.ReadUInt16();
				wORLDOBJECT.ParentID = stream.ReadInt16();
				wORLDOBJECT.Parent = addict2World.FindObjectByID(wORLDOBJECT.ParentID);
				wORLDOBJECT.Primitive = (Addict2WorldType)stream.ReadByte();
				wORLDOBJECT.DataScene = FindScene(wORLDOBJECT.TargetID = stream.ReadByte());
				Addict2WorldObjectData addict2WorldObjectData = new Addict2WorldObjectData(stream.ReadUInt32());
				wORLDOBJECT.Textured = addict2WorldObjectData.Textured != 0;
				wORLDOBJECT.PosData.Scale = new Vector3(1f, 1f, 1f);
				wORLDOBJECT.PosData.Quaternion.W = 1f;
				if (addict2WorldObjectData.Textured != 0)
				{
					int textureID = stream.ReadByte();
					wORLDOBJECT.Material = FindMaterial(textureID, (int)addict2WorldObjectData.TexSlot);
				}
				if (addict2WorldObjectData.Posx != 0)
				{
					wORLDOBJECT.PosData.Pos.X = stream.ReadCompactFloat(2);
				}
				if (addict2WorldObjectData.Posy != 0)
				{
					wORLDOBJECT.PosData.Pos.Y = stream.ReadCompactFloat(2);
				}
				if (addict2WorldObjectData.Posz != 0)
				{
					wORLDOBJECT.PosData.Pos.Z = stream.ReadCompactFloat(2);
				}
				if (addict2WorldObjectData.Sclx != 0)
				{
					wORLDOBJECT.PosData.Scale.X = stream.ReadCompactFloat(2);
				}
				if (addict2WorldObjectData.Scly != 0)
				{
					wORLDOBJECT.PosData.Scale.Y = stream.ReadCompactFloat(2);
				}
				if (addict2WorldObjectData.Sclz != 0)
				{
					wORLDOBJECT.PosData.Scale.Z = stream.ReadCompactFloat(2);
				}
				if (addict2WorldObjectData.Quat != 0)
				{
					wORLDOBJECT.PosData.Quaternion.W = stream.ReadCompactFloat(2);
					wORLDOBJECT.PosData.Quaternion.X = stream.ReadCompactFloat(2);
					wORLDOBJECT.PosData.Quaternion.Y = stream.ReadCompactFloat(2);
					wORLDOBJECT.PosData.Quaternion.Z = stream.ReadCompactFloat(2);
				}
				switch (wORLDOBJECT.Primitive)
				{
				case Addict2WorldType.SPHEREEMITTER:
				case Addict2WorldType.CUBEEMITTER:
				case Addict2WorldType.CYLINDEREMITTER:
				{
					wORLDOBJECT.EmitterData = new EMITTERDATA();
					wORLDOBJECT.EmitterData.DefaultAge = stream.ReadUInt16();
					wORLDOBJECT.EmitterData.AgeChaos = stream.ReadUInt16();
					wORLDOBJECT.EmitterData.ParticlesPerFrame = stream.ReadCompactFloat(2);
					wORLDOBJECT.EmitterData.MaxParticles = stream.ReadUInt16();
					wORLDOBJECT.EmitterData.FixedUp = stream.ReadBoolean();
					wORLDOBJECT.ZMask = stream.ReadBoolean();
					wORLDOBJECT.AEpsilon = stream.ReadByte();
					wORLDOBJECT.EmitterData.Particles = new PARTICLE[wORLDOBJECT.EmitterData.MaxParticles];
					wORLDOBJECT.EmitterData.ParticleNumBuffer = wORLDOBJECT.EmitterData.ParticlesPerFrame;
					Addict2ParticleEngine.MaxParticleNum = Math.Max(Addict2ParticleEngine.MaxParticleNum, wORLDOBJECT.EmitterData.MaxParticles);
					wORLDOBJECT.SRCBlend = SRCBlends[addict2WorldObjectData.SRCBlend];
					wORLDOBJECT.DSTBlend = DSTBlends[addict2WorldObjectData.DSTBlend];
					wORLDOBJECT.EmitterData.Tail = addict2WorldObjectData.Tail != 0;
					wORLDOBJECT.EmitterData.Head = addict2WorldObjectData.Head != 0;
					wORLDOBJECT.EmitterData.TailLength = (byte)(addict2WorldObjectData.TailRes1 + 1);
					wORLDOBJECT.EmitterData.TailLength2 = (byte)(addict2WorldObjectData.TailRes2 + 1);
					for (int n = 0; n < wORLDOBJECT.EmitterData.MaxParticles; n++)
					{
						if (addict2WorldObjectData.Tail != 0)
						{
							wORLDOBJECT.EmitterData.Particles[n].Tail = new Vector3[256];
						}
						wORLDOBJECT.EmitterData.Particles[n].Color1 = new byte[4];
						wORLDOBJECT.EmitterData.Particles[n].Color2 = new byte[4];
					}
					wORLDOBJECT.EmitterData.Color1 = new float[5];
					wORLDOBJECT.EmitterData.Color2 = new float[5];
					for (int num3 = 0; num3 < 5; num3++)
					{
						wORLDOBJECT.EmitterData.Color1[num3] = (int)stream.ReadByte();
					}
					for (int num3 = 0; num3 < 5; num3++)
					{
						wORLDOBJECT.EmitterData.Color2[num3] = (int)stream.ReadByte();
					}
					wORLDOBJECT.EmitterData.Param1 = stream.ReadCompactFloat(2);
					wORLDOBJECT.EmitterData.Param2 = stream.ReadCompactFloat(2);
					wORLDOBJECT.EmitterData.Size = stream.ReadCompactFloat(2);
					wORLDOBJECT.EmitterData.Rotspeed = stream.ReadCompactFloat(2);
					wORLDOBJECT.EmitterData.RotChaos = stream.ReadCompactFloat(2);
					wORLDOBJECT.EmitterData.CamFade = stream.ReadCompactFloat(2);
					wORLDOBJECT.EmitterData.Speed = stream.ReadCompactFloat(2);
					wORLDOBJECT.EmitterData.SpeedChaos = stream.ReadCompactFloat(2);
					wORLDOBJECT.EmitterData.DirChaos = stream.ReadCompactFloat(2);
					wORLDOBJECT.EmitterData.Dir.X = stream.ReadCompactFloat(2);
					wORLDOBJECT.EmitterData.Dir.Y = stream.ReadCompactFloat(2);
					wORLDOBJECT.EmitterData.Dir.Z = stream.ReadCompactFloat(2);
					wORLDOBJECT.EmitterData.RandRotate = stream.ReadBoolean();
					break;
				}
				case Addict2WorldType.PLANEDEFLECTOR:
				case Addict2WorldType.SPHEREDEFLECTOR:
				case Addict2WorldType.GRAVITY:
					wORLDOBJECT.EmitterData = new EMITTERDATA();
					wORLDOBJECT.EmitterData.Param1 = stream.ReadCompactFloat(2);
					wORLDOBJECT.EmitterData.Param2 = stream.ReadCompactFloat(2);
					break;
				}
				wORLDOBJECT.Animations = new DEFAULTOBJECTSPLINES[addict2World.AnimCapacity];
				wORLDOBJECT.AnimNum = addict2World.AnimNum;
				wORLDOBJECT.AnimCapacity = addict2World.AnimCapacity;
				for (int num4 = 0; num4 < addict2World.AnimNum; num4++)
				{
					switch (wORLDOBJECT.Primitive)
					{
					case Addict2WorldType.SPHEREEMITTER:
					case Addict2WorldType.CUBEEMITTER:
					case Addict2WorldType.CYLINDEREMITTER:
						wORLDOBJECT.Animations[num4].Prtfrme = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Size = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Red = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Green = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Blue = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Alpha = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Posx = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Posy = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Posz = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Sclx = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Scly = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Sclz = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Rotx = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Roty = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Rotz = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Rotw = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].AnimID = ImportSplineColor(stream);
						wORLDOBJECT.Animations[num4].AnimTime = ImportSplineFloat(stream);
						Addict2RenderingTools.Spline_QuaternionGetVectors(ref wORLDOBJECT.Animations[num4].Rotx, ref wORLDOBJECT.Animations[num4].Roty, ref wORLDOBJECT.Animations[num4].Rotz, ref wORLDOBJECT.Animations[num4].Rotw);
						break;
					default:
						wORLDOBJECT.Animations[num4].Red = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Green = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Blue = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Alpha = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Posx = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Posy = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Posz = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Sclx = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Scly = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Sclz = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Rotx = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Roty = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Rotz = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].Rotw = ImportSplineFloat(stream);
						wORLDOBJECT.Animations[num4].AnimID = ImportSplineColor(stream);
						wORLDOBJECT.Animations[num4].AnimTime = ImportSplineFloat(stream);
						Addict2RenderingTools.Spline_QuaternionGetVectors(ref wORLDOBJECT.Animations[num4].Rotx, ref wORLDOBJECT.Animations[num4].Roty, ref wORLDOBJECT.Animations[num4].Rotz, ref wORLDOBJECT.Animations[num4].Rotw);
						break;
					}
				}
				addict2World.ObjectList[m] = wORLDOBJECT;
			}
			for (int num5 = 0; num5 < addict2World.AnimNum; num5++)
			{
				addict2World.Animations[num5].AnimID = stream.ReadByte();
			}
			pWorlds[i] = addict2World;
		}
		for (int num6 = 0; num6 < nWorlds; num6++)
		{
			Addict2World addict2World2 = pWorlds[num6];
			for (int num7 = 0; num7 < addict2World2.ObjectNum; num7++)
			{
				if (addict2World2.ObjectList[num7].Primitive == Addict2WorldType.SUBSCENE)
				{
					addict2World2.ObjectList[num7].DataWorld = FindWorld(addict2World2.ObjectList[num7].TargetID);
				}
			}
		}
	}

	private Blend ConvertGLToXNABlend(uint b)
	{
		return b switch
		{
			0u => Blend.Zero, 
			1u => Blend.One, 
			768u => Blend.SourceColor, 
			769u => Blend.InverseSourceColor, 
			770u => Blend.SourceAlpha, 
			771u => Blend.InverseSourceAlpha, 
			772u => Blend.DestinationAlpha, 
			773u => Blend.InverseDestinationAlpha, 
			774u => Blend.DestinationColor, 
			775u => Blend.InverseDestinationColor, 
			776u => Blend.SourceAlphaSaturation, 
			_ => Blend.One, 
		};
	}

	public void LoadEvents(Addict2BinaryReader stream)
	{
		nEvents = stream.ReadUInt16();
		pEvents = new Addict2Event[nEvents];
		for (int i = 0; i < nEvents; i++)
		{
			ushort startFrame = stream.ReadUInt16();
			ushort endFrame = stream.ReadUInt16();
			byte pass = stream.ReadByte();
			Addict2EventType addict2EventType = (Addict2EventType)stream.ReadByte();
			short ex = stream.ReadInt16();
			short ex2 = stream.ReadInt16();
			short ey = stream.ReadInt16();
			short ey2 = stream.ReadInt16();
			short sx = stream.ReadInt16();
			short sx2 = stream.ReadInt16();
			short sy = stream.ReadInt16();
			short sy2 = stream.ReadInt16();
			Addict2Event addict2Event;
			switch (addict2EventType)
			{
			case Addict2EventType.NOEFFECT:
				addict2Event = new Addict2Event();
				break;
			case Addict2EventType.LAYER2D:
			{
				Addict2Event_Layer2D addict2Event_Layer2D = new Addict2Event_Layer2D();
				addict2Event_Layer2D.StartCol = new RGBA(stream.ReadUInt32());
				addict2Event_Layer2D.EndCol = new RGBA(stream.ReadUInt32());
				addict2Event_Layer2D.SrcBlend = ConvertGLToXNABlend(stream.ReadUInt32());
				addict2Event_Layer2D.DstBlend = ConvertGLToXNABlend(stream.ReadUInt32());
				addict2Event_Layer2D.Textured = stream.ReadBoolean();
				uint num = stream.ReadUInt32();
				uint slotNumber = num >> 24;
				uint textureID = num & 0xFFFFFFu;
				addict2Event_Layer2D.Texture = FindMaterial((int)textureID, (int)slotNumber);
				addict2Event = addict2Event_Layer2D;
				break;
			}
			case Addict2EventType.DISPLAY3D:
			{
				Addict2Event_Display3D addict2Event_Display3D = new Addict2Event_Display3D();
				int iD2 = stream.ReadByte();
				addict2Event_Display3D.World = FindWorld(iD2);
				addict2Event_Display3D.CamID = stream.ReadByte();
				addict2Event_Display3D.AnimID = stream.ReadByte();
				addict2Event_Display3D.CamStart = stream.ReadCompactFloat(2);
				addict2Event_Display3D.CamEnd = stream.ReadCompactFloat(2);
				addict2Event_Display3D.AnimStart = stream.ReadCompactFloat(2);
				addict2Event_Display3D.AnimEnd = stream.ReadCompactFloat(2);
				addict2Event_Display3D.DontSaveCam = stream.ReadBoolean();
				addict2Event = addict2Event_Display3D;
				break;
			}
			case Addict2EventType.R2TXT:
			{
				Addict2Event_RenderToTexture addict2Event_RenderToTexture = new Addict2Event_RenderToTexture();
				addict2Event_RenderToTexture.RenderTexture = stream.ReadSByte();
				addict2Event = addict2Event_RenderToTexture;
				break;
			}
			case Addict2EventType.BLUR:
			{
				Addict2Event_Blur addict2Event_Blur = new Addict2Event_Blur();
				addict2Event_Blur.StartCol = new RGBA(stream.ReadUInt32());
				addict2Event_Blur.EndCol = new RGBA(stream.ReadUInt32());
				addict2Event_Blur.SrcBlend = ConvertGLToXNABlend(stream.ReadUInt32());
				addict2Event_Blur.DstBlend = ConvertGLToXNABlend(stream.ReadUInt32());
				addict2Event_Blur.RenderTexture = stream.ReadSByte();
				addict2Event_Blur.LayerNum = stream.ReadByte();
				addict2Event_Blur.LayerZoom = stream.ReadByte();
				addict2Event = addict2Event_Blur;
				break;
			}
			case Addict2EventType.FEEDBACK:
			{
				Addict2Event_Feedback addict2Event_Feedback = new Addict2Event_Feedback();
				addict2Event_Feedback.StartCol = new RGBA(stream.ReadUInt32());
				addict2Event_Feedback.EndCol = new RGBA(stream.ReadUInt32());
				addict2Event_Feedback.SrcBlend = ConvertGLToXNABlend(stream.ReadUInt32());
				addict2Event_Feedback.DstBlend = ConvertGLToXNABlend(stream.ReadUInt32());
				addict2Event_Feedback.RenderTexture = stream.ReadSByte();
				addict2Event_Feedback.LayerNum = stream.ReadByte();
				addict2Event_Feedback.LayerZoom = stream.ReadByte();
				addict2Event = addict2Event_Feedback;
				break;
			}
			case Addict2EventType.FEEDBACK2:
			{
				Addict2Event_OldFeedback addict2Event_OldFeedback = new Addict2Event_OldFeedback();
				addict2Event_OldFeedback.StartCol = new RGBA(stream.ReadUInt32());
				addict2Event_OldFeedback.EndCol = new RGBA(stream.ReadUInt32());
				addict2Event_OldFeedback.SrcBlend = ConvertGLToXNABlend(stream.ReadUInt32());
				addict2Event_OldFeedback.DstBlend = ConvertGLToXNABlend(stream.ReadUInt32());
				addict2Event_OldFeedback.RenderTexture = stream.ReadSByte();
				addict2Event_OldFeedback.LayerNum = stream.ReadByte();
				addict2Event_OldFeedback.LayerZoom = stream.ReadByte();
				addict2Event = addict2Event_OldFeedback;
				break;
			}
			case Addict2EventType.CONTRAST:
			{
				Addict2Event_Contrast addict2Event_Contrast = new Addict2Event_Contrast();
				addict2Event_Contrast.StartCol = new RGBA(stream.ReadUInt32());
				addict2Event_Contrast.EndCol = new RGBA(stream.ReadUInt32());
				addict2Event_Contrast.SrcBlend = ConvertGLToXNABlend(stream.ReadUInt32());
				addict2Event_Contrast.DstBlend = ConvertGLToXNABlend(stream.ReadUInt32());
				addict2Event_Contrast.RenderTexture = stream.ReadSByte();
				addict2Event_Contrast.MulAmount = stream.ReadByte();
				addict2Event_Contrast.AddAmount = stream.ReadByte();
				addict2Event = addict2Event_Contrast;
				break;
			}
			case Addict2EventType.SHAKE:
			{
				Addict2Event_CameraShake addict2Event_CameraShake = new Addict2Event_CameraShake();
				addict2Event_CameraShake.Eye = stream.ReadSingle();
				addict2Event_CameraShake.Target = stream.ReadSingle();
				addict2Event_CameraShake.Start = stream.ReadSingle();
				addict2Event_CameraShake.End = stream.ReadSingle();
				addict2Event_CameraShake.Freq = stream.ReadByte();
				addict2Event = addict2Event_CameraShake;
				break;
			}
			case Addict2EventType.PARTICLE:
			{
				Addict2Event_ParticleCalc addict2Event_ParticleCalc = new Addict2Event_ParticleCalc();
				int iD = stream.ReadByte();
				addict2Event_ParticleCalc.World = FindWorld(iD);
				addict2Event_ParticleCalc.CamID = stream.ReadByte();
				addict2Event_ParticleCalc.AnimID = stream.ReadByte();
				addict2Event_ParticleCalc.AnimStart = stream.ReadCompactFloat(2);
				addict2Event_ParticleCalc.AnimEnd = stream.ReadCompactFloat(2);
				addict2Event = addict2Event_ParticleCalc;
				break;
			}
			case Addict2EventType.CLEAR:
			{
				Addict2Event_Clear addict2Event_Clear = new Addict2Event_Clear();
				addict2Event_Clear.Col = new RGBA(stream.ReadUInt32());
				addict2Event_Clear.Screen = stream.ReadBoolean();
				addict2Event_Clear.Zbuffer = stream.ReadBoolean();
				addict2Event = addict2Event_Clear;
				break;
			}
			default:
				throw new NotImplementedException("Event = " + addict2EventType);
			}
			addict2Event.StartFrame = startFrame;
			addict2Event.EndFrame = endFrame;
			addict2Event.Pass = pass;
			addict2Event.EventType = addict2EventType;
			addict2Event.ex1 = ex;
			addict2Event.ex2 = ex2;
			addict2Event.ey1 = ey;
			addict2Event.ey2 = ey2;
			addict2Event.sx1 = sx;
			addict2Event.sx2 = sx2;
			addict2Event.sy1 = sy;
			addict2Event.sy2 = sy2;
			pEvents[i] = addict2Event;
		}
	}

	public void Load(Addict2BinaryReader stream)
	{
		LoadTextures(stream);
		LoadPolyselections(stream);
		LoadScenes(stream);
		LoadWorlds(stream);
		LoadEvents(stream);
		Addict2ParticleEngine.Init();
		Addict2RenderingTools.Init();
	}

	public void DisplayFrame(int Frame)
	{
		int num = (int)((600f - 800f / IntroAspect) / 2f);
		Addict2RenderingTools.EyeShake.X = 0f;
		Addict2RenderingTools.EyeShake.Y = 0f;
		Addict2RenderingTools.EyeShake.Z = 0f;
		Addict2RenderingTools.TargetShake.X = 0f;
		Addict2RenderingTools.TargetShake.Y = 0f;
		Addict2RenderingTools.TargetShake.Z = 0f;
		int num2 = 0;
		for (int i = 0; i < nEvents; i++)
		{
			Addict2Event addict2Event = pEvents[i];
			if (Frame >= addict2Event.StartFrame && Frame <= addict2Event.EndFrame)
			{
				num2++;
				d.TimePos = (float)(Frame - addict2Event.StartFrame) / (float)(addict2Event.EndFrame - addict2Event.StartFrame);
				d.StartFrame = addict2Event.StartFrame;
				float num3 = Addict2RenderingTools.LinearInterpolate(addict2Event.sx1, addict2Event.ex1, d.TimePos);
				float num4 = Addict2RenderingTools.LinearInterpolate(addict2Event.sx2, addict2Event.ex2, d.TimePos);
				float num5 = Addict2RenderingTools.LinearInterpolate(addict2Event.sy1, addict2Event.ey1, d.TimePos) - (float)num + 1f;
				float num6 = Addict2RenderingTools.LinearInterpolate(addict2Event.sy2, addict2Event.ey2, d.TimePos) - (float)num + 1f;
				float num7 = 1f;
				float num8 = 1f;
				float num9 = 0f;
				float num10 = 0f;
				if (AspectRatio > IntroAspect)
				{
					num7 = IntroAspect / AspectRatio;
					num9 = ((float)XRes - (float)XRes * num7) / 2f;
				}
				else
				{
					num8 = AspectRatio / IntroAspect;
					num10 = ((float)YRes - (float)YRes * num8) / 2f;
				}
				d.x1 = (int)(num3 / 799f * (float)XRes * num7 + num9);
				d.x2 = (int)(num4 / 799f * (float)XRes * num7 + num9);
				d.y1 = (int)(num5 / (800f / IntroAspect) * (float)YRes * num8 + num10);
				d.y2 = (int)(num6 / (800f / IntroAspect) * (float)YRes * num8 + num10);
				d.OnScreenInLastFrame = addict2Event.OnScreenInLastFrame;
				d.CurrentFrame = Frame * 10;
				d.Aspect = (num4 - num3) / (num6 - num5);
				addict2Event.Render(d);
				addict2Event.OnScreenInLastFrame = true;
			}
			else
			{
				addict2Event.OnScreenInLastFrame = false;
			}
		}
	}
}
