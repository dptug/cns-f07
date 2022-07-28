using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_PC;

public class Addict2Event_Display3D : Addict2Event
{
	public Addict2World World;

	public int CamID;

	public int AnimID;

	public float CamStart;

	public float CamEnd;

	public float AnimStart;

	public float AnimEnd;

	public bool DontSaveCam;

	private int FindCamByID(Addict2World w, int CamID)
	{
		for (int i = 0; i < w.CamNum; i++)
		{
			if (w.CamAnims[i].CamID == CamID)
			{
				return i;
			}
		}
		return -1;
	}

	private int FindAnimByID(Addict2World w, int CamID)
	{
		for (int i = 0; i < w.AnimNum; i++)
		{
			if (w.Animations[i].AnimID == CamID)
			{
				return i;
			}
		}
		return -1;
	}

	private void SetCameraView(ref CAMERA Cam, float AspectRatio)
	{
		Matrix identity = Matrix.Identity;
		Vector3 vector = Cam.Target - Cam.Eye;
		vector.Normalize();
		identity = Matrix.CreateFromAxisAngle(vector, Cam.Roll * 360f / 255f * 3.1415f / 180f);
		Vector3 vector2 = Vector3.Transform(Cam.Up, identity);
		Cam.i = Vector3.Normalize(Vector3.Cross(vector, vector2));
		Cam.j = Vector3.Normalize(Vector3.Cross(vector, Cam.i));
		identity = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(Cam.Fov), AspectRatio, 0.1f, 2000f);
		Addict2RenderingTools.SetProjectionMatrix(identity);
		identity = Matrix.CreateLookAt(Cam.Eye, Cam.Target, vector2);
		Addict2RenderingTools.SetViewMatrix(identity);
	}

	public override void Render(DEFAULTEVENTDATA DEData)
	{
		Addict2Logger.Append("Rendering Display3D at " + DEData.TimePos + "\n");
		if (World == null)
		{
			return;
		}
		float time = Addict2RenderingTools.LinearInterpolate(CamStart, CamEnd, DEData.TimePos);
		float time2 = Addict2RenderingTools.LinearInterpolate(AnimStart, AnimEnd, DEData.TimePos);
		CAMERA Cam = Addict2RenderingTools.CamBuffer;
		if (World.CamNum > 0)
		{
			int num = FindCamByID(World, CamID);
			CAMSPLINES cAMSPLINES = World.CamAnims[num];
			if (cAMSPLINES.Eyex != null && cAMSPLINES.Eyex.KeyNum > 0)
			{
				Cam.Fov = 45f;
				Cam.Up.X = 0f;
				Cam.Up.Y = 1f;
				Cam.Up.Z = 0f;
				Cam.Eye.X = cAMSPLINES.Eyex.GetInterpolatedValue(time);
				Cam.Eye.Y = cAMSPLINES.Eyey.GetInterpolatedValue(time);
				Cam.Eye.Z = cAMSPLINES.Eyez.GetInterpolatedValue(time);
				Cam.Target.X = cAMSPLINES.Trgx.GetInterpolatedValue(time);
				Cam.Target.Y = cAMSPLINES.Trgy.GetInterpolatedValue(time);
				Cam.Target.Z = cAMSPLINES.Trgz.GetInterpolatedValue(time);
				Cam.Eye += Addict2RenderingTools.EyeShake;
				Cam.Target += Addict2RenderingTools.TargetShake;
				if (cAMSPLINES.Fov.KeyNum > 0)
				{
					Cam.Fov = cAMSPLINES.Fov.GetInterpolatedValue(time);
				}
				Cam.Roll = cAMSPLINES.Roll.GetInterpolatedValue(time);
				if (!DontSaveCam)
				{
					Addict2RenderingTools.CamBuffer = Cam;
				}
			}
		}
		if (World.AnimNum > 0)
		{
			for (int i = 0; i < World.ObjectNum; i++)
			{
				int num2 = FindAnimByID(World, AnimID);
				DEFAULTOBJECTSPLINES dEFAULTOBJECTSPLINES = World.ObjectList[i].Animations[num2];
				if (dEFAULTOBJECTSPLINES.Posx.KeyNum > 0)
				{
					World.ObjectList[i].PosData.Pos.X = dEFAULTOBJECTSPLINES.Posx.GetInterpolatedValue(time2);
				}
				if (dEFAULTOBJECTSPLINES.Posy.KeyNum > 0)
				{
					World.ObjectList[i].PosData.Pos.Y = dEFAULTOBJECTSPLINES.Posy.GetInterpolatedValue(time2);
				}
				if (dEFAULTOBJECTSPLINES.Posz.KeyNum > 0)
				{
					World.ObjectList[i].PosData.Pos.Z = dEFAULTOBJECTSPLINES.Posz.GetInterpolatedValue(time2);
				}
				if (dEFAULTOBJECTSPLINES.Sclx.KeyNum > 0)
				{
					World.ObjectList[i].PosData.Scale.X = dEFAULTOBJECTSPLINES.Sclx.GetInterpolatedValue(time2);
				}
				if (dEFAULTOBJECTSPLINES.Scly.KeyNum > 0)
				{
					World.ObjectList[i].PosData.Scale.Y = dEFAULTOBJECTSPLINES.Scly.GetInterpolatedValue(time2);
				}
				if (dEFAULTOBJECTSPLINES.Sclz.KeyNum > 0)
				{
					World.ObjectList[i].PosData.Scale.Z = dEFAULTOBJECTSPLINES.Sclz.GetInterpolatedValue(time2);
				}
				if (dEFAULTOBJECTSPLINES.Rotx.KeyNum > 0)
				{
					World.ObjectList[i].PosData.Quaternion = Addict2RenderingTools.SplineSlerp(dEFAULTOBJECTSPLINES.Rotx, dEFAULTOBJECTSPLINES.Roty, dEFAULTOBJECTSPLINES.Rotz, dEFAULTOBJECTSPLINES.Rotw, time2);
				}
				if (dEFAULTOBJECTSPLINES.AnimID.KeyNum > 0)
				{
					World.ObjectList[i].APosData.AnimID = (int)dEFAULTOBJECTSPLINES.AnimID.GetInterpolatedValue(time2);
				}
				if (dEFAULTOBJECTSPLINES.AnimTime.KeyNum > 0)
				{
					World.ObjectList[i].APosData.AnimPos = dEFAULTOBJECTSPLINES.AnimTime.GetInterpolatedValue(time2);
				}
				if (dEFAULTOBJECTSPLINES.Red.KeyNum > 0)
				{
					World.ObjectList[i].Color.R = (byte)Math.Min(255f, Math.Max(0f, dEFAULTOBJECTSPLINES.Red.GetInterpolatedValue(time2)));
				}
				if (dEFAULTOBJECTSPLINES.Green.KeyNum > 0)
				{
					World.ObjectList[i].Color.G = (byte)Math.Min(255f, Math.Max(0f, dEFAULTOBJECTSPLINES.Green.GetInterpolatedValue(time2)));
				}
				if (dEFAULTOBJECTSPLINES.Blue.KeyNum > 0)
				{
					World.ObjectList[i].Color.B = (byte)Math.Min(255f, Math.Max(0f, dEFAULTOBJECTSPLINES.Blue.GetInterpolatedValue(time2)));
				}
				if (dEFAULTOBJECTSPLINES.Alpha.KeyNum > 0)
				{
					World.ObjectList[i].Color.A = (byte)Math.Min(255f, Math.Max(0f, dEFAULTOBJECTSPLINES.Alpha.GetInterpolatedValue(time2)));
				}
			}
		}
		World.CalculateObjectMatrices();
		Addict2Engine.device.ScissorRectangle = new Rectangle(DEData.x1, DEData.y1, DEData.x2 - DEData.x1, DEData.y2 - DEData.y1);
		Viewport viewport = default(Viewport);
		viewport.X = DEData.x1;
		viewport.Y = DEData.y1;
		viewport.Width = DEData.x2 - DEData.x1;
		viewport.Height = DEData.y2 - DEData.y1;
		viewport.MinDepth = 0f;
		viewport.MaxDepth = 1f;
		Addict2Engine.device.Viewport = viewport;
		Addict2Engine.device.RenderState.ScissorTestEnable = true;
		Addict2RenderingTools.SetWorldMatrix(Matrix.Identity);
		SetCameraView(ref Cam, DEData.Aspect);
		World.Render(SubWorld: false, Zmask: false);
		Matrix WorldData = Matrix.Identity;
		Addict2Engine.device.SamplerStates[0].AddressU = TextureAddressMode.Clamp;
		Addict2Engine.device.SamplerStates[0].AddressV = TextureAddressMode.Clamp;
		Addict2RenderingTools.SetTextureMatrix(Matrix.Identity);
		World.RenderParticleTree(Cam, AnimID, ref WorldData);
		Addict2Engine.device.RenderState.ScissorTestEnable = false;
		viewport.X = 0;
		viewport.Y = 0;
		viewport.Width = Addict2Engine.project.XRes;
		viewport.Height = Addict2Engine.project.YRes;
		Addict2Engine.device.Viewport = viewport;
	}
}
