using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;

namespace Engine_PC;

public static class Addict2RenderingTools
{
	public static CAMERA CamBuffer;

	public static Vector3 EyeShake;

	public static Vector3 TargetShake;

	public static VertexShader vshPosNormalTex;

	public static VertexShader vshPosTex;

	public static VertexShader vshPosTexColor;

	public static VertexShader vshPosColor;

	public static PixelShader pshTexEnvColorFog;

	public static PixelShader pshTexEnvColor;

	public static PixelShader pshTexColor;

	public static PixelShader pshTexDiffuse;

	public static PixelShader pshDiffuse;

	public static PixelShader pshColor;

	public static PixelShader pshBlur;

	public static PixelShader pshFeedback;

	public static PixelShader pshContrast;

	private static VertexBuffer vb;

	private static VertexDeclaration decl;

	public static RENDERTEXTURESLOT[] RenderTextures = new RENDERTEXTURESLOT[6];

	public static int TXR = 256;

	public static int TYR = 256;

	public static Stack<Matrix> MatrixStack = new Stack<Matrix>();

	public static bool FogEnabled = false;

	public static float FogStart = 0f;

	public static float FogEnd = 0f;

	public static RGBA FogColor;

	public static bool FogEnable
	{
		get
		{
			return FogEnabled;
		}
		set
		{
			FogEnabled = value;
			if (value)
			{
				Vector2 zero = Vector2.Zero;
				zero.X = FogStart;
				zero.Y = FogEnd;
				Addict2Engine.device.SetPixelShaderConstant(1, new Vector4((float)(int)FogColor.R / 256f, (float)(int)FogColor.G / 256f, (float)(int)FogColor.B / 256f, (float)(int)FogColor.A / 256f));
				Addict2Engine.device.SetPixelShaderConstant(2, zero);
			}
		}
	}

	public static byte[] LoadShader(string filename)
	{
		string path = Path.Combine(StorageContainer.TitleLocation, filename);
		FileStream fileStream = File.Open(path, FileMode.Open);
		byte[] array = new byte[fileStream.Length];
		fileStream.Read(array, 0, (int)fileStream.Length);
		return array;
	}

	public static void Init()
	{
		string text = ".xbox";
		vshPosNormalTex = new VertexShader(Addict2Engine.device, LoadShader("shaderz\\posnormaltex.vso" + text));
		vshPosTex = new VertexShader(Addict2Engine.device, LoadShader("shaderz\\postex.vso" + text));
		vshPosTexColor = new VertexShader(Addict2Engine.device, LoadShader("shaderz\\postexcolor.vso" + text));
		vshPosColor = new VertexShader(Addict2Engine.device, LoadShader("shaderz\\poscolor.vso" + text));
		pshTexEnvColorFog = new PixelShader(Addict2Engine.device, LoadShader("shaderz\\texturecolorenvmapfog.pso" + text));
		pshTexEnvColor = new PixelShader(Addict2Engine.device, LoadShader("shaderz\\texturecolorenvmap.pso" + text));
		pshTexColor = new PixelShader(Addict2Engine.device, LoadShader("shaderz\\texturecolor.pso" + text));
		pshColor = new PixelShader(Addict2Engine.device, LoadShader("shaderz\\color.pso" + text));
		pshBlur = new PixelShader(Addict2Engine.device, LoadShader("shaderz\\blur.pso" + text));
		pshFeedback = new PixelShader(Addict2Engine.device, LoadShader("shaderz\\feedback.pso" + text));
		pshContrast = new PixelShader(Addict2Engine.device, LoadShader("shaderz\\contrast.pso" + text));
		pshTexDiffuse = new PixelShader(Addict2Engine.device, LoadShader("shaderz\\texturediffuse.pso" + text));
		pshDiffuse = new PixelShader(Addict2Engine.device, LoadShader("shaderz\\diffuse.pso" + text));
		TXR = 256;
		for (TYR = 256; TXR < Addict2Engine.project.XRes; TXR <<= 1)
		{
		}
		while (TYR < Addict2Engine.project.YRes)
		{
			TYR <<= 1;
		}
		for (int i = 0; i < 6; i++)
		{
			RenderTextures[i].TexImage = new Texture2D(Addict2Engine.device, Addict2Engine.project.XRes, Addict2Engine.project.YRes, 1, (ResourceUsage)1, SurfaceFormat.Color, (ResourceManagementMode)0);
		}
		TXR = Addict2Engine.project.XRes;
		TYR = Addict2Engine.project.YRes;
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

	public static void PushMatrix()
	{
		if (MatrixStack.Count == 0)
		{
			MatrixStack.Push(Matrix.Identity);
		}
		else
		{
			MatrixStack.Push(MatrixStack.Peek());
		}
	}

	public static void PopMatrix()
	{
		MatrixStack.Pop();
	}

	public static void MultiplyStackMatrix(Matrix m)
	{
		Matrix matrix = MatrixStack.Pop();
		MatrixStack.Push(matrix * m);
	}

	public static void SetProjectionMatrix(Matrix m)
	{
		Addict2Engine.device.SetVertexShaderConstant(0, m);
	}

	public static void SetViewMatrix(Matrix m)
	{
		Addict2Engine.device.SetVertexShaderConstant(4, m);
	}

	public static void SetWorldMatrix(Matrix m)
	{
		Addict2Engine.device.SetVertexShaderConstant(8, m);
	}

	public static void SetTextureMatrix(Matrix m)
	{
		Addict2Engine.device.SetVertexShaderConstant(12, m);
	}

	public static void SetWorldMatrix()
	{
		SetWorldMatrix(MatrixStack.Peek());
	}

	public static void SwitchTo2D()
	{
		Matrix identity = Matrix.Identity;
		SetViewMatrix(identity);
		SetWorldMatrix(identity);
		identity = Matrix.CreateOrthographicOffCenter(0f, Addict2Engine.project.XRes, Addict2Engine.project.YRes, 0f, -1f, 1f);
		SetProjectionMatrix(identity);
	}

	public static void SetColor(RGBA r)
	{
		SetColor((float)(int)r.R / 256f, (float)(int)r.G / 256f, (float)(int)r.B / 256f, (float)(int)r.A / 256f);
	}

	public static void SetColor(byte r, byte g, byte b, byte a)
	{
		SetColor((float)(int)r / 256f, (float)(int)g / 256f, (float)(int)b / 256f, (float)(int)a / 256f);
	}

	public static void SetColor(float r, float g, float b, float a)
	{
		Vector4 constantData = new Vector4(r, g, b, a);
		Addict2Engine.device.SetPixelShaderConstant(0, constantData);
	}

	public static float LinearInterpolate(float Min, float Max, float Pos)
	{
		return Min + (Max - Min) * Pos;
	}

	public static void RST2Matrix(RST a, out Matrix m)
	{
		m = Matrix.CreateScale(a.Scale.X, a.Scale.Y, a.Scale.Z);
		Matrix matrix = Matrix.CreateFromQuaternion(a.Quaternion);
		m *= Matrix.Transpose(matrix);
		m *= Matrix.CreateTranslation(a.Pos.X, a.Pos.Y, a.Pos.Z);
	}

	public static Quaternion quat_interp(Key[] k1, Key[] k2, float t)
	{
		Quaternion quaternion = default(Quaternion);
		Quaternion quaternion2 = default(Quaternion);
		Quaternion quaternion3 = default(Quaternion);
		Quaternion quaternion4 = default(Quaternion);
		quaternion.W = k1[0].Value;
		quaternion.X = k1[1].Value;
		quaternion.Y = k1[2].Value;
		quaternion.Z = k1[3].Value;
		quaternion.Normalize();
		quaternion2.W = k1[0].bn;
		quaternion2.X = k1[1].bn;
		quaternion2.Y = k1[2].bn;
		quaternion2.Z = k1[3].bn;
		quaternion2.Normalize();
		quaternion3.W = k2[0].Value;
		quaternion3.X = k2[1].Value;
		quaternion3.Y = k2[2].Value;
		quaternion3.Z = k2[3].Value;
		quaternion3.Normalize();
		quaternion4.W = k2[0].an;
		quaternion4.X = k2[1].an;
		quaternion4.Y = k2[2].an;
		quaternion4.Z = k2[3].an;
		quaternion4.Normalize();
		Quaternion quaternion5 = Quaternion.Slerp(quaternion, quaternion2, t);
		Quaternion quaternion6 = Quaternion.Slerp(quaternion2, quaternion4, t);
		Quaternion quaternion7 = Quaternion.Slerp(quaternion4, quaternion3, t);
		quaternion5 = Quaternion.Slerp(quaternion5, quaternion6, t);
		quaternion6 = Quaternion.Slerp(quaternion6, quaternion7, t);
		Quaternion result = Quaternion.Slerp(quaternion5, quaternion6, t);
		result.Normalize();
		return result;
	}

	public static Quaternion SplineSlerp(Addict2Spline s, Addict2Spline x, Addict2Spline y, Addict2Spline z, float Time)
	{
		int i;
		for (i = 0; s.Keys[i].Time < Time && i < s.KeyNum; i++)
		{
		}
		i--;
		if (i == -1)
		{
			i = 0;
			return new Quaternion(s.Keys[i].Value, x.Keys[i].Value, y.Keys[i].Value, z.Keys[i].Value);
		}
		if (i == s.KeyNum - 1)
		{
			return new Quaternion(s.Keys[i].Value, x.Keys[i].Value, y.Keys[i].Value, z.Keys[i].Value);
		}
		float t = (Time - s.Keys[i].Time) / (s.Keys[i + 1].Time - s.Keys[i].Time);
		return quat_interp(new Key[4]
		{
			s.Keys[i],
			x.Keys[i],
			y.Keys[i],
			z.Keys[i]
		}, new Key[4]
		{
			s.Keys[i + 1],
			x.Keys[i + 1],
			y.Keys[i + 1],
			z.Keys[i + 1]
		}, t);
	}

	public static void GetVector(byte sel, int n, ref Addict2Spline s, ref Addict2Spline x, ref Addict2Spline y, ref Addict2Spline z, ref float a, ref float b, ref float c, ref float d)
	{
		Key[] array = new Key[4];
		Key[] array2 = new Key[4];
		Key[] array3 = new Key[4];
		Quaternion quaternion = default(Quaternion);
		Quaternion quaternion2 = default(Quaternion);
		Quaternion quaternion3 = default(Quaternion);
		ref Key reference = ref array2[0];
		reference = s.Keys[n];
		ref Key reference2 = ref array2[1];
		reference2 = x.Keys[n];
		ref Key reference3 = ref array2[2];
		reference3 = y.Keys[n];
		ref Key reference4 = ref array2[3];
		reference4 = z.Keys[n];
		quaternion2.W = s.Keys[n].Value;
		quaternion2.X = x.Keys[n].Value;
		quaternion2.Y = y.Keys[n].Value;
		quaternion2.Z = z.Keys[n].Value;
		if (n == 0)
		{
			ref Key reference5 = ref array3[0];
			reference5 = s.Keys[1];
			ref Key reference6 = ref array3[1];
			reference6 = x.Keys[1];
			ref Key reference7 = ref array3[2];
			reference7 = y.Keys[1];
			ref Key reference8 = ref array3[3];
			reference8 = z.Keys[1];
			quaternion3.W = s.Keys[1].Value;
			quaternion3.X = x.Keys[1].Value;
			quaternion3.Y = y.Keys[1].Value;
			quaternion3.Z = z.Keys[1].Value;
			Quaternion quaternion4 = Quaternion.Slerp(quaternion2, quaternion3, 1f / 3f);
			a = quaternion4.W;
			b = quaternion4.X;
			c = quaternion4.Y;
			d = quaternion4.Z;
		}
		else if (n == s.KeyNum - 1)
		{
			ref Key reference9 = ref array[0];
			reference9 = s.Keys[n - 1];
			ref Key reference10 = ref array[1];
			reference10 = x.Keys[n - 1];
			ref Key reference11 = ref array[2];
			reference11 = y.Keys[n - 1];
			ref Key reference12 = ref array[3];
			reference12 = z.Keys[n - 1];
			quaternion.W = s.Keys[n - 1].Value;
			quaternion.X = x.Keys[n - 1].Value;
			quaternion.Y = y.Keys[n - 1].Value;
			quaternion.Z = z.Keys[n - 1].Value;
			Quaternion quaternion5 = Quaternion.Slerp(quaternion2, quaternion, 1f / 3f);
			a = quaternion5.W;
			b = quaternion5.X;
			c = quaternion5.Y;
			d = quaternion5.Z;
		}
		else
		{
			ref Key reference13 = ref array[0];
			reference13 = s.Keys[n - 1];
			ref Key reference14 = ref array[1];
			reference14 = x.Keys[n - 1];
			ref Key reference15 = ref array[2];
			reference15 = y.Keys[n - 1];
			ref Key reference16 = ref array[3];
			reference16 = z.Keys[n - 1];
			ref Key reference17 = ref array3[0];
			reference17 = s.Keys[n + 1];
			ref Key reference18 = ref array3[1];
			reference18 = x.Keys[n + 1];
			ref Key reference19 = ref array3[2];
			reference19 = y.Keys[n + 1];
			ref Key reference20 = ref array3[3];
			reference20 = z.Keys[n + 1];
			quaternion.W = array[0].Value;
			quaternion.X = array[1].Value;
			quaternion.Y = array[2].Value;
			quaternion.Z = array[3].Value;
			quaternion3.W = array3[0].Value;
			quaternion3.X = array3[1].Value;
			quaternion3.Y = array3[2].Value;
			quaternion3.Z = array3[3].Value;
			float num = ((sel == 0) ? 1f : (-1f));
			Quaternion quaternion6 = Quaternion.Slerp(quaternion2, quaternion, -1f / 3f);
			Quaternion quaternion7 = Quaternion.Slerp(quaternion2, quaternion3, 1f / 3f);
			Quaternion quaternion8 = Quaternion.Slerp(quaternion2, Quaternion.Slerp(quaternion6, quaternion7, 0.5f), num * -1f);
			a = quaternion8.W;
			b = quaternion8.X;
			c = quaternion8.Y;
			d = quaternion8.Z;
		}
	}

	public static void Spline_QuaternionGetVectors(ref Addict2Spline s, ref Addict2Spline x, ref Addict2Spline y, ref Addict2Spline z)
	{
		if (s.KeyNum != 1)
		{
			for (int i = 0; i < s.KeyNum; i++)
			{
				GetVector(0, i, ref s, ref x, ref y, ref z, ref s.Keys[i].an, ref x.Keys[i].an, ref y.Keys[i].an, ref z.Keys[i].an);
				GetVector(1, i, ref s, ref x, ref y, ref z, ref s.Keys[i].bn, ref x.Keys[i].bn, ref y.Keys[i].bn, ref z.Keys[i].bn);
			}
		}
	}

	public static void CopyRTBack(DEFAULTEVENTDATA DEData, int RenderTexture)
	{
		SetColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		SwitchTo2D();
		Addict2Engine.device.RenderState.DepthBufferEnable = false;
		Addict2Engine.device.Textures[0] = RenderTextures[RenderTexture].TexImage;
		Addict2Engine.device.PixelShader = pshTexColor;
		Addict2Engine.device.SamplerStates[0].AddressU = TextureAddressMode.Clamp;
		Addict2Engine.device.SamplerStates[0].AddressV = TextureAddressMode.Clamp;
		Matrix identity = Matrix.Identity;
		float x = RenderTextures[RenderTexture].x1;
		float x2 = RenderTextures[RenderTexture].x2;
		float y = RenderTextures[RenderTexture].y1;
		float y2 = RenderTextures[RenderTexture].y2;
		identity.M11 = x2 - x;
		identity.M22 = y - y2;
		identity.M31 = 0f + x;
		identity.M32 = 1f - y;
		Matrix identity2 = Matrix.Identity;
		identity2.M11 = DEData.x2 - DEData.x1;
		identity2.M22 = DEData.y2 - DEData.y1;
		identity2.M41 = DEData.x1;
		identity2.M42 = DEData.y1;
		SetTextureMatrix(identity);
		SetWorldMatrix(identity2);
		Addict2Engine.device.RenderState.AlphaBlendEnable = false;
		Addict2Engine.device.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
		Addict2Engine.device.VertexDeclaration = decl;
		Addict2Engine.device.VertexShader = vshPosTex;
		Addict2Engine.device.Vertices[0].SetSource(vb, 0, VertexPositionTexture.SizeInBytes);
		Addict2Engine.device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
		identity2 = Matrix.Identity;
		SetTextureMatrix(identity2);
	}
}
