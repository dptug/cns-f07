using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_PC;

public class Addict2World
{
	public int ID;

	public WORLDOBJECT[] ObjectList;

	public int ObjectNum;

	public int ObjectCapacity;

	public int ObjectCount;

	public int ParticleCount;

	public int SubWorldCount;

	public bool Lighting;

	public LIGHT[] Lights;

	public bool Fog;

	public RGBA FogCol;

	public float FogStart;

	public float FogEnd;

	public int AnimNum;

	public int AnimCapacity;

	public ANIMDATA[] Animations;

	public CAMSPLINES[] CamAnims;

	public int CamNum;

	public int CamCapacity;

	private byte[] bcol = new byte[4];

	private Random randc = new Random();

	private int RAND_MAX = int.MaxValue;

	public WORLDOBJECT FindObjectByID(int ID)
	{
		for (int i = 0; i < ObjectNum; i++)
		{
			if (ObjectList[i] != null && ObjectList[i].ID == ID)
			{
				return ObjectList[i];
			}
		}
		return null;
	}

	public void CalculateObjectMatrices()
	{
		for (int i = 0; i < ObjectNum; i++)
		{
			Addict2RenderingTools.RST2Matrix(ObjectList[i].PosData, out ObjectList[i].ModelView);
		}
	}

	private void DrawChildren(int Parent, bool ZMask)
	{
		for (int i = 0; i < ObjectNum; i++)
		{
			if (ObjectList[i].ParentID != Parent)
			{
				continue;
			}
			Addict2RenderingTools.PushMatrix();
			Addict2RenderingTools.MultiplyStackMatrix(ObjectList[i].ModelView);
			switch (ObjectList[i].Primitive)
			{
			case Addict2WorldType.SCENE:
				if (ObjectList[i].DataScene.ColorDiscard)
				{
					Addict2RenderingTools.SetColor(ObjectList[i].Color);
				}
				Addict2RenderingTools.SetWorldMatrix();
				ObjectList[i].DataScene.RenderList(ZMask);
				break;
			case Addict2WorldType.SUBSCENE:
				ObjectList[i].DataWorld.SetToAnimPos(ObjectList[i].APosData);
				ObjectList[i].DataWorld.Render(SubWorld: true, ZMask);
				break;
			}
			DrawChildren(ObjectList[i].ID, ZMask);
			Addict2RenderingTools.PopMatrix();
		}
	}

	private void SetToAnimPos(ANIMPOS a)
	{
		if (a.AnimID >= AnimNum || a.AnimID < 0)
		{
			return;
		}
		float time = a.AnimPos;
		if (a.AnimPos - (float)Math.Floor(a.AnimPos) != 0f)
		{
			time = a.AnimPos - (float)Math.Floor(a.AnimPos);
		}
		for (int i = 0; i < ObjectNum; i++)
		{
			DEFAULTOBJECTSPLINES dEFAULTOBJECTSPLINES = ObjectList[i].Animations[a.AnimID];
			if (dEFAULTOBJECTSPLINES.Posx.KeyNum > 0)
			{
				ObjectList[i].PosData.Pos.X = dEFAULTOBJECTSPLINES.Posx.GetInterpolatedValue(time);
			}
			if (dEFAULTOBJECTSPLINES.Posy.KeyNum > 0)
			{
				ObjectList[i].PosData.Pos.Y = dEFAULTOBJECTSPLINES.Posy.GetInterpolatedValue(time);
			}
			if (dEFAULTOBJECTSPLINES.Posz.KeyNum > 0)
			{
				ObjectList[i].PosData.Pos.Z = dEFAULTOBJECTSPLINES.Posz.GetInterpolatedValue(time);
			}
			if (dEFAULTOBJECTSPLINES.Sclx.KeyNum > 0)
			{
				ObjectList[i].PosData.Scale.X = dEFAULTOBJECTSPLINES.Sclx.GetInterpolatedValue(time);
			}
			if (dEFAULTOBJECTSPLINES.Scly.KeyNum > 0)
			{
				ObjectList[i].PosData.Scale.Y = dEFAULTOBJECTSPLINES.Scly.GetInterpolatedValue(time);
			}
			if (dEFAULTOBJECTSPLINES.Sclz.KeyNum > 0)
			{
				ObjectList[i].PosData.Scale.Z = dEFAULTOBJECTSPLINES.Sclz.GetInterpolatedValue(time);
			}
			if (dEFAULTOBJECTSPLINES.Red.KeyNum > 0)
			{
				ObjectList[i].Color.R = (byte)Math.Min(255f, Math.Max(0f, dEFAULTOBJECTSPLINES.Red.GetInterpolatedValue(time)));
			}
			if (dEFAULTOBJECTSPLINES.Green.KeyNum > 0)
			{
				ObjectList[i].Color.G = (byte)Math.Min(255f, Math.Max(0f, dEFAULTOBJECTSPLINES.Green.GetInterpolatedValue(time)));
			}
			if (dEFAULTOBJECTSPLINES.Blue.KeyNum > 0)
			{
				ObjectList[i].Color.B = (byte)Math.Min(255f, Math.Max(0f, dEFAULTOBJECTSPLINES.Blue.GetInterpolatedValue(time)));
			}
			if (dEFAULTOBJECTSPLINES.Alpha.KeyNum > 0)
			{
				ObjectList[i].Color.A = (byte)Math.Min(255f, Math.Max(0f, dEFAULTOBJECTSPLINES.Alpha.GetInterpolatedValue(time)));
			}
			if (dEFAULTOBJECTSPLINES.AnimID.KeyNum > 0)
			{
				ObjectList[i].APosData.AnimID = (int)dEFAULTOBJECTSPLINES.AnimID.GetInterpolatedValue(time);
			}
			if (dEFAULTOBJECTSPLINES.AnimTime.KeyNum > 0)
			{
				ObjectList[i].APosData.AnimPos = dEFAULTOBJECTSPLINES.AnimTime.GetInterpolatedValue(time);
			}
			if (dEFAULTOBJECTSPLINES.Rotx.KeyNum > 0)
			{
				ObjectList[i].PosData.Quaternion = Addict2RenderingTools.SplineSlerp(dEFAULTOBJECTSPLINES.Rotw, dEFAULTOBJECTSPLINES.Rotx, dEFAULTOBJECTSPLINES.Roty, dEFAULTOBJECTSPLINES.Rotz, time);
			}
		}
		CalculateObjectMatrices();
	}

	public void Render(bool SubWorld, bool Zmask)
	{
		Addict2Engine.device.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
		Addict2Engine.device.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
		Addict2RenderingTools.SetTextureMatrix(Matrix.Identity);
		Addict2Engine.device.RenderState.DepthBufferEnable = true;
		if (!SubWorld)
		{
			if (Fog)
			{
				Addict2RenderingTools.FogStart = FogStart;
				Addict2RenderingTools.FogEnd = FogEnd;
				Addict2RenderingTools.FogColor = FogCol;
				Addict2RenderingTools.FogEnable = true;
			}
			else
			{
				Addict2RenderingTools.FogEnable = false;
			}
			Addict2Engine.device.RenderState.DepthBufferWriteEnable = true;
			DrawChildren(-1, ZMask: true);
			Addict2Engine.device.RenderState.DepthBufferWriteEnable = false;
			DrawChildren(-1, ZMask: false);
		}
		else
		{
			DrawChildren(-1, Zmask);
		}
		Addict2Engine.device.RenderState.FillMode = FillMode.Solid;
	}

	public void RenderParticles(CAMERA Cam, int ActualAnim, ref Matrix WorldData)
	{
		Cam.Normal = Vector3.Subtract(Cam.Target, Cam.Eye);
		Cam.Normal.Normalize();
		Cam.d = Cam.Eye.X * Cam.Normal.X + Cam.Eye.Y * Cam.Normal.Y + Cam.Eye.Z * Cam.Normal.Z;
		for (int i = 0; i < ObjectNum; i++)
		{
			if (ObjectList[i].Primitive != Addict2WorldType.CUBEEMITTER && ObjectList[i].Primitive != Addict2WorldType.CYLINDEREMITTER && ObjectList[i].Primitive != Addict2WorldType.SPHEREEMITTER)
			{
				continue;
			}
			Addict2Engine.device.RenderState.DepthBufferEnable = true;
			Addict2Engine.device.RenderState.DepthBufferWriteEnable = ObjectList[i].ZMask;
			Addict2Engine.device.RenderState.AlphaBlendEnable = true;
			Addict2Engine.device.RenderState.SourceBlend = ObjectList[i].SRCBlend;
			Addict2Engine.device.RenderState.DestinationBlend = ObjectList[i].DSTBlend;
			if (ObjectList[i].EmitterData.Tail)
			{
				Addict2Engine.device.VertexShader = Addict2RenderingTools.vshPosColor;
				Addict2Engine.device.PixelShader = Addict2RenderingTools.pshDiffuse;
				Addict2Engine.device.VertexDeclaration = Addict2ParticleEngine.linedecl;
				Addict2RenderingTools.PushMatrix();
				Addict2RenderingTools.MultiplyStackMatrix(WorldData);
				int num = 0;
				float[] array = new float[4];
				for (int j = 0; j < ObjectList[i].EmitterData.MaxParticles; j++)
				{
					if (!ObjectList[i].EmitterData.Particles[j].Active)
					{
						continue;
					}
					float num2 = 1f - (float)ObjectList[i].EmitterData.Particles[j].Age / (float)ObjectList[i].EmitterData.Particles[j].StartAge;
					if (ObjectList[i].Animations[ActualAnim].Red.KeyNum == 0)
					{
						array[0] = Addict2RenderingTools.LinearInterpolate((int)ObjectList[i].EmitterData.Particles[j].Color1[0], (int)ObjectList[i].EmitterData.Particles[j].Color2[0], num2) / 255f;
					}
					else
					{
						array[0] = ObjectList[i].Animations[ActualAnim].Red.GetInterpolatedValue(num2) / 255f;
					}
					if (ObjectList[i].Animations[ActualAnim].Green.KeyNum == 0)
					{
						array[1] = Addict2RenderingTools.LinearInterpolate((int)ObjectList[i].EmitterData.Particles[j].Color1[1], (int)ObjectList[i].EmitterData.Particles[j].Color2[1], num2) / 255f;
					}
					else
					{
						array[1] = ObjectList[i].Animations[ActualAnim].Green.GetInterpolatedValue(num2) / 255f;
					}
					if (ObjectList[i].Animations[ActualAnim].Blue.KeyNum == 0)
					{
						array[2] = Addict2RenderingTools.LinearInterpolate((int)ObjectList[i].EmitterData.Particles[j].Color1[2], (int)ObjectList[i].EmitterData.Particles[j].Color2[2], num2) / 255f;
					}
					else
					{
						array[2] = ObjectList[i].Animations[ActualAnim].Blue.GetInterpolatedValue(num2) / 255f;
					}
					if (ObjectList[i].Animations[ActualAnim].Alpha.KeyNum == 0)
					{
						array[3] = Addict2RenderingTools.LinearInterpolate((int)ObjectList[i].EmitterData.Particles[j].Color1[3], (int)ObjectList[i].EmitterData.Particles[j].Color2[3], num2) / 255f;
					}
					else
					{
						array[3] = ObjectList[i].Animations[ActualAnim].Alpha.GetInterpolatedValue(num2) / 255f;
					}
					VertexPositionColor[] array2 = new VertexPositionColor[ObjectList[i].EmitterData.TailLength];
					for (int k = 0; k < ObjectList[i].EmitterData.TailLength; k++)
					{
						float num3 = 1f - (float)k / (float)(int)ObjectList[i].EmitterData.TailLength;
						int num4 = ObjectList[i].EmitterData.Particles[j].TailCnt - k * ObjectList[i].EmitterData.TailLength2;
						num4 %= ObjectList[i].EmitterData.TailLength * ObjectList[i].EmitterData.TailLength2;
						if (num4 < 0)
						{
							num4 += ObjectList[i].EmitterData.TailLength * ObjectList[i].EmitterData.TailLength2;
						}
						Vector3 position = ObjectList[i].EmitterData.Particles[j].Tail[num4];
						array2[k].Color = new Color((byte)(num3 * array[0] * 255f), (byte)(num3 * array[1] * 255f), (byte)(num3 * array[2] * 255f), (byte)(num3 * array[3] * 255f));
						array2[k].Position = position;
					}
					Addict2RenderingTools.SetWorldMatrix();
					Addict2Engine.device.DrawUserPrimitives(PrimitiveType.LineStrip, array2, 0, ObjectList[i].EmitterData.TailLength - 1);
					array2 = null;
					num++;
				}
				array = null;
				Addict2Logger.Append("Tails.ActiveCount = " + num);
				Addict2RenderingTools.PopMatrix();
			}
			if (!ObjectList[i].EmitterData.Head)
			{
				continue;
			}
			Addict2Engine.device.VertexShader = Addict2RenderingTools.vshPosTexColor;
			Addict2Engine.device.PixelShader = Addict2RenderingTools.pshTexDiffuse;
			Addict2Engine.device.VertexDeclaration = Addict2ParticleEngine.decl;
			if (!ObjectList[i].Textured)
			{
				continue;
			}
			Addict2RenderingTools.PushMatrix();
			Addict2RenderingTools.MultiplyStackMatrix(WorldData);
			int num5 = 0;
			for (int l = 0; l < ObjectList[i].EmitterData.MaxParticles; l++)
			{
				if (ObjectList[i].EmitterData.Particles[l].Active)
				{
					num5++;
				}
			}
			Vector3 vector = Vector3.Normalize(Vector3.Subtract(Cam.Target, Cam.Eye));
			Addict2Logger.Append("Heads.ActiveCount = " + num5);
			if (num5 > 0)
			{
				int num6 = 0;
				PARTICLESORTINFO[] array3 = new PARTICLESORTINFO[num5];
				int l;
				for (l = 0; l < ObjectList[i].EmitterData.MaxParticles; l++)
				{
					if (ObjectList[i].EmitterData.Particles[l].Active)
					{
						array3[num6].ParticleID = l;
						ObjectList[i].EmitterData.Particles[l].DisplayPos2 = Vector3.Transform(ObjectList[i].EmitterData.Particles[l].DisplayPos, WorldData);
						Vector3 vector2 = Vector3.Subtract(ObjectList[i].EmitterData.Particles[l].DisplayPos2, Cam.Eye);
						array3[num6].Distance = Vector3.Dot(vector, vector2);
						num6++;
					}
				}
				for (int m = 0; m < num5; m++)
				{
					float distance = array3[m].Distance;
					int num7 = m;
					int num8 = m;
					while (l < num5)
					{
						if (array3[num8].Distance > distance)
						{
							distance = array3[num8].Distance;
							num7 = l;
						}
						num8++;
					}
					PARTICLESORTINFO pARTICLESORTINFO = array3[m];
					ref PARTICLESORTINFO reference = ref array3[m];
					reference = array3[num7];
					array3[num7] = pARTICLESORTINFO;
				}
				Addict2Engine.device.Textures[0] = ObjectList[i].Material.TextureHandle;
				int num9 = 0;
				for (l = 0; l < num5; l++)
				{
					float num10 = SetParticleColor(ref ObjectList[i], array3[l].ParticleID, ActualAnim, ref Addict2ParticleEngine.ParticleVertices[num9 * 6], ref Cam, array3[l].Distance);
					for (int n = 0; n < 5; n++)
					{
						Addict2ParticleEngine.ParticleVertices[num9 * 6 + n + 1].Color = Addict2ParticleEngine.ParticleVertices[num9 * 6].Color;
					}
					Matrix matrix = Matrix.CreateFromAxisAngle(Cam.Normal, ObjectList[i].EmitterData.Particles[array3[l].ParticleID].Rotation / 180f * 3.1415f);
					Vector3 normal = Vector3.Zero;
					Vector3 normal2 = Vector3.Multiply(Cam.i, num10);
					if (!ObjectList[i].EmitterData.FixedUp)
					{
						normal = Vector3.Multiply(Cam.j, num10);
					}
					else
					{
						normal.Y = num10;
					}
					Vector3 value = Vector3.TransformNormal(normal2, matrix);
					Vector3 value2 = Vector3.TransformNormal(normal, matrix);
					Vector3 position2 = Vector3.Add(Vector3.Add(ObjectList[i].EmitterData.Particles[array3[l].ParticleID].DisplayPos2, value), value2);
					Vector3 position3 = Vector3.Add(Vector3.Subtract(ObjectList[i].EmitterData.Particles[array3[l].ParticleID].DisplayPos2, value), value2);
					Vector3 position4 = Vector3.Subtract(Vector3.Subtract(ObjectList[i].EmitterData.Particles[array3[l].ParticleID].DisplayPos2, value), value2);
					Vector3 position5 = Vector3.Subtract(Vector3.Add(ObjectList[i].EmitterData.Particles[array3[l].ParticleID].DisplayPos2, value), value2);
					Addict2ParticleEngine.ParticleVertices[num9 * 6].Position = position4;
					Addict2ParticleEngine.ParticleVertices[num9 * 6 + 1].Position = position5;
					Addict2ParticleEngine.ParticleVertices[num9 * 6 + 2].Position = position2;
					Addict2ParticleEngine.ParticleVertices[num9 * 6 + 3].Position = position4;
					Addict2ParticleEngine.ParticleVertices[num9 * 6 + 4].Position = position2;
					Addict2ParticleEngine.ParticleVertices[num9 * 6 + 5].Position = position3;
					num9++;
				}
				Addict2RenderingTools.SetWorldMatrix();
				Addict2Engine.device.DrawUserPrimitives(PrimitiveType.TriangleList, Addict2ParticleEngine.ParticleVertices, 0, num9 * 2);
				array3 = null;
			}
			Addict2RenderingTools.PopMatrix();
		}
	}

	private float SetParticleColor(ref WORLDOBJECT o, int Particle, int ActualAnim, ref VertexPositionColorTexture k, ref CAMERA c, float Distance)
	{
		byte b = byte.MaxValue;
		if (o.EmitterData.CamFade != 0f)
		{
			b = (byte)(Math.Min(1f, Distance / o.EmitterData.CamFade) * 255f);
		}
		float num = 1f - (float)o.EmitterData.Particles[Particle].Age / (float)o.EmitterData.Particles[Particle].StartAge;
		if (o.AnimNum == 0 || o.Animations[ActualAnim].Red.KeyNum == 0)
		{
			bcol[0] = (byte)Addict2RenderingTools.LinearInterpolate((int)o.EmitterData.Particles[Particle].Color1[0], (int)o.EmitterData.Particles[Particle].Color2[0], num);
		}
		else
		{
			bcol[0] = (byte)Math.Min(255f, Math.Max(0f, o.Animations[ActualAnim].Red.GetInterpolatedValue(num)));
		}
		if (o.AnimNum == 0 || o.Animations[ActualAnim].Green.KeyNum == 0)
		{
			bcol[1] = (byte)Addict2RenderingTools.LinearInterpolate((int)o.EmitterData.Particles[Particle].Color1[1], (int)o.EmitterData.Particles[Particle].Color2[1], num);
		}
		else
		{
			bcol[1] = (byte)Math.Min(255f, Math.Max(0f, o.Animations[ActualAnim].Green.GetInterpolatedValue(num)));
		}
		if (o.AnimNum == 0 || o.Animations[ActualAnim].Blue.KeyNum == 0)
		{
			bcol[2] = (byte)Addict2RenderingTools.LinearInterpolate((int)o.EmitterData.Particles[Particle].Color1[2], (int)o.EmitterData.Particles[Particle].Color2[2], num);
		}
		else
		{
			bcol[2] = (byte)Math.Min(255f, Math.Max(0f, o.Animations[ActualAnim].Blue.GetInterpolatedValue(num)));
		}
		if (o.AnimNum == 0 || o.Animations[ActualAnim].Alpha.KeyNum == 0)
		{
			bcol[3] = (byte)Addict2RenderingTools.LinearInterpolate((int)o.EmitterData.Particles[Particle].Color1[3], (int)o.EmitterData.Particles[Particle].Color2[3], num);
		}
		else
		{
			bcol[3] = (byte)Math.Min(255f, Math.Max(0f, o.Animations[ActualAnim].Alpha.GetInterpolatedValue(num)));
		}
		float result = ((o.AnimNum != 0 && o.Animations[ActualAnim].Size.KeyNum != 0) ? o.Animations[ActualAnim].Size.GetInterpolatedValue(num) : o.EmitterData.Particles[Particle].Size);
		k.Color = new Color((byte)(bcol[0] * b / 255), (byte)(bcol[1] * b / 255), (byte)(bcol[2] * b / 255), (byte)(bcol[3] * b / 255));
		return result;
	}

	public void RenderParticleTree(CAMERA Cam, int ActualAnim, ref Matrix WorldData)
	{
		RenderParticles(Cam, Math.Max(ActualAnim, 0), ref WorldData);
		for (int i = 0; i < ObjectNum; i++)
		{
			if (ObjectList[i].Primitive == Addict2WorldType.SUBSCENE)
			{
				Matrix identity = Matrix.Identity;
				int num = ObjectList[i].ID;
				while (num != -1)
				{
					WORLDOBJECT wORLDOBJECT = FindObjectByID(num);
					identity *= wORLDOBJECT.ModelView;
					num = wORLDOBJECT.ParentID;
				}
				Matrix WorldData2 = WorldData * identity;
				ObjectList[i].DataWorld.RenderParticleTree(Cam, Math.Max(ActualAnim, 0), ref WorldData2);
			}
		}
	}

	public void CalculateParticleObjectMatrices()
	{
		for (int i = 0; i < ObjectNum; i++)
		{
			if (ObjectList[i].Primitive == Addict2WorldType.CUBEEMITTER || ObjectList[i].Primitive == Addict2WorldType.SPHEREEMITTER || ObjectList[i].Primitive == Addict2WorldType.CYLINDEREMITTER || ObjectList[i].Primitive == Addict2WorldType.GRAVITY || ObjectList[i].Primitive == Addict2WorldType.SPHEREDEFLECTOR || ObjectList[i].Primitive == Addict2WorldType.PLANEDEFLECTOR)
			{
				Addict2RenderingTools.RST2Matrix(ObjectList[i].PosData, out ObjectList[i].ModelView);
			}
		}
	}

	private float Random()
	{
		return (float)randc.Next() / 2.1474836E+09f - 0.5f;
	}

	private int rand()
	{
		return randc.Next();
	}

	public void CalculateParticles(int CurrentFrame)
	{
		for (int i = 0; i < ObjectNum; i++)
		{
			switch (ObjectList[i].Primitive)
			{
			case Addict2WorldType.PLANEDEFLECTOR:
			{
				Vector3 up = Vector3.Up;
				ObjectList[i].Inverted = ObjectList[i].ModelView;
				ObjectList[i].Inverted = Matrix.Invert(ObjectList[i].Inverted);
				ObjectList[i].IT = ObjectList[i].Inverted;
				ObjectList[i].IT = Matrix.Transpose(ObjectList[i].IT);
				up = Vector3.Transform(up, ObjectList[i].IT);
				up.Normalize();
				ObjectList[i].EmitterData.n = up;
				ObjectList[i].EmitterData.d = ObjectList[i].ModelView.M41 * up.X + ObjectList[i].ModelView.M42 * up.Y + ObjectList[i].ModelView.M43 * up.Z;
				break;
			}
			case Addict2WorldType.SPHEREDEFLECTOR:
				ObjectList[i].Inverted = ObjectList[i].ModelView;
				ObjectList[i].Inverted = Matrix.Invert(ObjectList[i].Inverted);
				ObjectList[i].IT = ObjectList[i].Inverted;
				ObjectList[i].IT = Matrix.Transpose(ObjectList[i].IT);
				break;
			}
		}
		for (int i = 0; i < ObjectNum; i++)
		{
			switch (ObjectList[i].Primitive)
			{
			case Addict2WorldType.SPHEREEMITTER:
			case Addict2WorldType.CUBEEMITTER:
			case Addict2WorldType.CYLINDEREMITTER:
			{
				if (ObjectList[i].EmitterData.LastFrameChecked > CurrentFrame)
				{
					float pos = Math.Min(1f, (float)(CurrentFrame - (ObjectList[i].EmitterData.LastFrameChecked - 25)) / 25f);
					for (int j = 0; j < ObjectList[i].EmitterData.MaxParticles; j++)
					{
						ObjectList[i].EmitterData.Particles[j].DisplayPos.X = Addict2RenderingTools.LinearInterpolate(ObjectList[i].EmitterData.Particles[j].LastPos.X, ObjectList[i].EmitterData.Particles[j].Position.X, pos);
						ObjectList[i].EmitterData.Particles[j].DisplayPos.Y = Addict2RenderingTools.LinearInterpolate(ObjectList[i].EmitterData.Particles[j].LastPos.Y, ObjectList[i].EmitterData.Particles[j].Position.Y, pos);
						ObjectList[i].EmitterData.Particles[j].DisplayPos.Z = Addict2RenderingTools.LinearInterpolate(ObjectList[i].EmitterData.Particles[j].LastPos.Z, ObjectList[i].EmitterData.Particles[j].Position.Z, pos);
					}
					break;
				}
				Matrix identity = Matrix.Identity;
				int num = ObjectList[i].ID;
				while (num != -1)
				{
					WORLDOBJECT wORLDOBJECT = FindObjectByID(num);
					identity *= wORLDOBJECT.ModelView;
					num = wORLDOBJECT.ParentID;
				}
				Vector3 vector = Vector3.Zero;
				if (ObjectList[i].EmitterData.Dir.X != 0f || ObjectList[i].EmitterData.Dir.Y != 0f || ObjectList[i].EmitterData.Dir.Z != 0f)
				{
					vector = Vector3.Normalize(ObjectList[i].EmitterData.Dir);
				}
				int k;
				for (k = ObjectList[i].EmitterData.LastFrameChecked; k <= CurrentFrame; k += 25)
				{
					float num2 = Math.Min(1f, (float)(CurrentFrame - k) / 25f);
					if (ObjectList[i].EmitterData.DefaultAge > 0)
					{
						ObjectList[i].EmitterData.ParticleNumBuffer += Math.Max(0f, ObjectList[i].EmitterData.ParticlesPerFrame);
					}
					while (ObjectList[i].EmitterData.ParticleNumBuffer >= 1f)
					{
						float num3 = 1f;
						int num4 = 0;
						for (int l = 0; l < ObjectList[i].EmitterData.MaxParticles; l++)
						{
							if ((float)ObjectList[i].EmitterData.Particles[l].Age / (float)ObjectList[i].EmitterData.Particles[l].StartAge < num3)
							{
								num3 = (float)ObjectList[i].EmitterData.Particles[l].Age / (float)ObjectList[i].EmitterData.Particles[l].StartAge;
								num4 = l;
							}
							if (!ObjectList[i].EmitterData.Particles[l].Active)
							{
								num4 = l;
								l = ObjectList[i].EmitterData.MaxParticles;
							}
						}
						ObjectList[i].EmitterData.Particles[num4].Active = true;
						ObjectList[i].EmitterData.Particles[num4].Aging = ObjectList[i].EmitterData.DefaultAge != 0;
						ObjectList[i].EmitterData.Particles[num4].StartAge = (ObjectList[i].EmitterData.Particles[num4].Age = ObjectList[i].EmitterData.DefaultAge + (int)(Random() * (float)ObjectList[i].EmitterData.AgeChaos) + ((!ObjectList[i].EmitterData.Particles[num4].Aging) ? 1 : 0));
						ObjectList[i].EmitterData.Particles[num4].Rotation = 0f;
						if (ObjectList[i].EmitterData.RandRotate)
						{
							ObjectList[i].EmitterData.Particles[num4].Rotation = (Random() + 0.5f) * 360f;
						}
						ObjectList[i].EmitterData.Particles[num4].RotChaos = (Random() + 0.5f) * ObjectList[i].EmitterData.RotChaos;
						ObjectList[i].EmitterData.Particles[num4].Size = ObjectList[i].EmitterData.Size * (1f + ((float)rand() / (float)RAND_MAX * 2f - 1f) * ObjectList[i].EmitterData.Color2[4] / 255f);
						for (int m = 0; m < 4; m++)
						{
							if (m != 3 || ObjectList[i].EmitterData.Color1[3] != 0f)
							{
								ObjectList[i].EmitterData.Particles[num4].Color1[m] = (byte)Math.Max(0f, Math.Min(255f, ObjectList[i].EmitterData.Color1[m] + Random() * 2f * ObjectList[i].EmitterData.Color1[4] / 255f));
							}
							else
							{
								ObjectList[i].EmitterData.Particles[num4].Color1[3] = 0;
							}
							if (m != 3 || ObjectList[i].EmitterData.Color2[3] != 0f)
							{
								ObjectList[i].EmitterData.Particles[num4].Color2[m] = (byte)Math.Max(0f, Math.Min(255f, ObjectList[i].EmitterData.Color2[m]));
							}
							else
							{
								ObjectList[i].EmitterData.Particles[num4].Color2[3] = 0;
							}
						}
						Vector3 vector2 = Vector3.Zero;
						ObjectList[i].EmitterData.Particles[num4].Position = new Vector3(Random(), Random(), Random());
						Vector3 zero = Vector3.Zero;
						zero.X = (float)rand() / (float)RAND_MAX - 0.5f;
						zero.Y = (float)rand() / (float)RAND_MAX - 0.5f;
						zero.Z = (float)rand() / (float)RAND_MAX - 0.5f;
						zero.Normalize();
						zero = Vector3.Cross(zero, vector);
						float angle = ((float)rand() / (float)RAND_MAX - 0.5f) * ObjectList[i].EmitterData.DirChaos * 0.017452778f / 2f;
						Matrix matrix = Matrix.CreateFromAxisAngle(zero, angle);
						vector = Vector3.Transform(vector, matrix);
						ObjectList[i].EmitterData.Particles[num4].Speed = vector * (((float)rand() / (float)RAND_MAX - 0.5f) * ObjectList[i].EmitterData.SpeedChaos + ObjectList[i].EmitterData.Speed);
						ObjectList[i].EmitterData.Particles[num4].TailCnt = 0;
						if (ObjectList[i].Primitive != Addict2WorldType.CUBEEMITTER)
						{
							if (ObjectList[i].Primitive == Addict2WorldType.CYLINDEREMITTER)
							{
								ObjectList[i].EmitterData.Particles[num4].Position.Y = 0f;
							}
							vector2 = Vector3.Normalize(ObjectList[i].EmitterData.Particles[num4].Position) * 0.5f;
							ObjectList[i].EmitterData.Particles[num4].Position.X = vector2.X * ((Random() + 0.5f) * (1f - ObjectList[i].EmitterData.Param1) + ObjectList[i].EmitterData.Param1);
							ObjectList[i].EmitterData.Particles[num4].Position.Z = vector2.Z * ((Random() + 0.5f) * (1f - ObjectList[i].EmitterData.Param1) + ObjectList[i].EmitterData.Param1);
						}
						if (ObjectList[i].Primitive == Addict2WorldType.CYLINDEREMITTER)
						{
							ObjectList[i].EmitterData.Particles[num4].Position.Y = Random() + 0.5f - 0.5f;
						}
						if (ObjectList[i].Primitive == Addict2WorldType.SPHEREEMITTER)
						{
							ObjectList[i].EmitterData.Particles[num4].Position.Y = vector2.Y * ((Random() + 0.5f) * (1f - ObjectList[i].EmitterData.Param1) + ObjectList[i].EmitterData.Param1);
						}
						ObjectList[i].EmitterData.Particles[num4].Position = Vector3.Transform(ObjectList[i].EmitterData.Particles[num4].Position, identity);
						if (ObjectList[i].ParentID >= 0 && ObjectList[i].Parent != null)
						{
							ObjectList[i].EmitterData.Particles[num4].Position = Vector3.Transform(ObjectList[i].EmitterData.Particles[num4].Position, ObjectList[i].Parent.ModelView);
						}
						if (ObjectList[i].EmitterData.Tail)
						{
							for (int n = 0; n < 256; n++)
							{
								ref Vector3 reference = ref ObjectList[i].EmitterData.Particles[num4].Tail[n];
								reference = ObjectList[i].EmitterData.Particles[num4].Position;
							}
						}
						ObjectList[i].EmitterData.ParticleNumBuffer -= 1f;
					}
					for (int num5 = 0; num5 < ObjectList[i].EmitterData.MaxParticles; num5++)
					{
						if (!ObjectList[i].EmitterData.Particles[num5].Active)
						{
							continue;
						}
						for (int num6 = 0; num6 < ObjectNum; num6++)
						{
							Addict2WorldType primitive = ObjectList[num6].Primitive;
							if (primitive == Addict2WorldType.GRAVITY)
							{
								float num7 = 0f;
								Vector3 vector3;
								if (ObjectList[num6].EmitterData.Param2 != 1f)
								{
									vector3 = new Vector3(ObjectList[num6].ModelView.M41, ObjectList[num6].ModelView.M42, ObjectList[num6].ModelView.M43) - ObjectList[i].EmitterData.Particles[num5].Position;
									num7 = vector3.Length();
									num7 = ObjectList[num6].EmitterData.Param1 / (num7 * num7 * num7) * 0.01f;
								}
								else if (ObjectList[num6].ModelView.M41 != 0f || ObjectList[num6].ModelView.M42 != 0f || ObjectList[num6].ModelView.M43 != 0f)
								{
									vector3 = Vector3.Normalize(new Vector3(ObjectList[num6].ModelView.M41, ObjectList[num6].ModelView.M42, ObjectList[num6].ModelView.M43));
									num7 = ObjectList[num6].EmitterData.Param1 * 0.01f;
								}
								else
								{
									vector3 = Vector3.Zero;
								}
								ObjectList[i].EmitterData.Particles[num5].Speed = ObjectList[i].EmitterData.Particles[num5].Speed + vector3 * num7;
							}
						}
						for (int num6 = 0; num6 < ObjectNum; num6++)
						{
							switch (ObjectList[num6].Primitive)
							{
							case Addict2WorldType.PLANEDEFLECTOR:
							{
								float num14 = Vector3.Dot(ObjectList[num6].EmitterData.n, ObjectList[i].EmitterData.Particles[num5].Position) - ObjectList[num6].EmitterData.d;
								float num15 = Vector3.Dot(ObjectList[num6].EmitterData.n, ObjectList[i].EmitterData.Particles[num5].Position + ObjectList[i].EmitterData.Particles[num5].Speed) - ObjectList[num6].EmitterData.d;
								if (num14 < 0f != num15 < 0f)
								{
									float num16 = (0f - num14) / Vector3.Dot(ObjectList[num6].EmitterData.n, ObjectList[i].EmitterData.Particles[num5].Speed);
									Vector3 value = Vector3.Add(ObjectList[i].EmitterData.Particles[num5].Position, Vector3.Multiply(ObjectList[i].EmitterData.Particles[num5].Speed, num16));
									ObjectList[i].EmitterData.Particles[num5].Speed = Vector3.Multiply(Vector3.Add(ObjectList[i].EmitterData.Particles[num5].Speed, Vector3.Multiply(ObjectList[num6].EmitterData.n, -2f * Vector3.Dot(ObjectList[num6].EmitterData.n, ObjectList[i].EmitterData.Particles[num5].Speed))), ObjectList[num6].EmitterData.Param1);
									ObjectList[i].EmitterData.Particles[num5].Position = Vector3.Add(value, Vector3.Multiply(ObjectList[i].EmitterData.Particles[num5].Speed, num16 - 1f));
								}
								break;
							}
							case Addict2WorldType.SPHEREDEFLECTOR:
							{
								Vector3 vector4 = Vector3.Transform(ObjectList[i].EmitterData.Particles[num5].Position, ObjectList[num6].Inverted);
								Vector3 vector5 = Vector3.TransformNormal(ObjectList[i].EmitterData.Particles[num5].Speed, ObjectList[num6].Inverted);
								float num8 = Vector3.Dot(vector4, vector4);
								Vector3 vector6 = Vector3.Add(vector4, vector5);
								float num9 = Vector3.Dot(vector6, vector6);
								if ((double)num8 <= 0.25 != (double)num9 <= 0.25)
								{
									float num10 = Vector3.Dot(vector5, vector5);
									float num11 = 2f * Vector3.Dot(vector4, vector5);
									float num12 = (float)Math.Sqrt(num11 * num11 - 4f * num10 * (num8 - 0.25f));
									float num13 = (0f - num11 + num12) / (2f * num10);
									if (num13 >= 1f)
									{
										num13 = (0f - num11 - num12) / (2f * num10);
									}
									Vector3 position;
									Vector3 position2 = (position = Vector3.Add(vector4, Vector3.Multiply(vector5, num13)));
									position2 = Vector3.Transform(position2, ObjectList[num6].IT);
									position = Vector3.Transform(position, ObjectList[num6].ModelView);
									position2 = Vector3.Normalize(position2);
									ObjectList[i].EmitterData.Particles[num5].Speed = Vector3.Multiply(Vector3.Add(ObjectList[i].EmitterData.Particles[num5].Speed, Vector3.Multiply(position2, -2f * Vector3.Dot(position2, ObjectList[i].EmitterData.Particles[num5].Speed))), ObjectList[num6].EmitterData.Param1);
									ObjectList[i].EmitterData.Particles[num5].Position = Vector3.Add(position, Vector3.Multiply(ObjectList[i].EmitterData.Particles[num5].Speed, num2 - 1f));
									if (ObjectList[i].EmitterData.Particles[num5].Tail != null)
									{
										ObjectList[i].EmitterData.Particles[num5].TailCnt = (ObjectList[i].EmitterData.Particles[num5].TailCnt + 1) % (ObjectList[i].EmitterData.TailLength * ObjectList[i].EmitterData.TailLength2);
										ObjectList[i].EmitterData.Particles[num5].Tail[ObjectList[i].EmitterData.Particles[num5].TailCnt] = position;
										ObjectList[i].EmitterData.Particles[num5].TailCnt2 = (ObjectList[i].EmitterData.Particles[num5].TailCnt2 + 1) % (int)ObjectList[i].EmitterData.TailLength2;
									}
								}
								break;
							}
							}
						}
						ObjectList[i].EmitterData.Particles[num5].LastPos = ObjectList[i].EmitterData.Particles[num5].Position;
						ObjectList[i].EmitterData.Particles[num5].Position = Vector3.Add(ObjectList[i].EmitterData.Particles[num5].Position, Vector3.Multiply(ObjectList[i].EmitterData.Particles[num5].Speed, 1f));
						ObjectList[i].EmitterData.Particles[num5].DisplayPos.X = Addict2RenderingTools.LinearInterpolate(ObjectList[i].EmitterData.Particles[num5].LastPos.X, ObjectList[i].EmitterData.Particles[num5].Position.X, num2);
						ObjectList[i].EmitterData.Particles[num5].DisplayPos.Y = Addict2RenderingTools.LinearInterpolate(ObjectList[i].EmitterData.Particles[num5].LastPos.Y, ObjectList[i].EmitterData.Particles[num5].Position.Y, num2);
						ObjectList[i].EmitterData.Particles[num5].DisplayPos.Z = Addict2RenderingTools.LinearInterpolate(ObjectList[i].EmitterData.Particles[num5].LastPos.Z, ObjectList[i].EmitterData.Particles[num5].Position.Z, num2);
						ObjectList[i].EmitterData.Particles[num5].Rotation += ObjectList[i].EmitterData.Rotspeed + ObjectList[i].EmitterData.Particles[num5].RotChaos;
						if (ObjectList[i].EmitterData.Tail)
						{
							ObjectList[i].EmitterData.Particles[num5].TailCnt = (ObjectList[i].EmitterData.Particles[num5].TailCnt + 1) % (ObjectList[i].EmitterData.TailLength * ObjectList[i].EmitterData.TailLength2);
							ref Vector3 reference2 = ref ObjectList[i].EmitterData.Particles[num5].Tail[ObjectList[i].EmitterData.Particles[num5].TailCnt];
							reference2 = ObjectList[i].EmitterData.Particles[num5].Position;
							ObjectList[i].EmitterData.Particles[num5].TailCnt2 = (ObjectList[i].EmitterData.Particles[num5].TailCnt2 + 1) % (int)ObjectList[i].EmitterData.TailLength2;
						}
						if (ObjectList[i].EmitterData.Particles[num5].Aging)
						{
							ObjectList[i].EmitterData.Particles[num5].Age--;
							if (ObjectList[i].EmitterData.Particles[num5].Age <= 0)
							{
								ObjectList[i].EmitterData.Particles[num5].Active = false;
							}
						}
					}
				}
				ObjectList[i].EmitterData.LastFrameChecked = k;
				break;
			}
			}
		}
	}
}
