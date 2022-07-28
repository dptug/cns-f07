namespace Engine_PC;

public class Addict2Event_ParticleCalc : Addict2Event
{
	public Addict2World World;

	public int CamID;

	public int AnimID;

	public float AnimStart;

	public float AnimEnd;

	public override void Render(DEFAULTEVENTDATA DEData)
	{
		Addict2Logger.Append("Rendering ParticleCalc at " + DEData.TimePos + "\n");
		if (World == null)
		{
			return;
		}
		float time = Addict2RenderingTools.LinearInterpolate(AnimStart, AnimEnd, DEData.TimePos);
		if (World.AnimNum > AnimID)
		{
			for (int i = 0; i < World.ObjectNum; i++)
			{
				DEFAULTOBJECTSPLINES dEFAULTOBJECTSPLINES = World.ObjectList[i].Animations[AnimID];
				if (dEFAULTOBJECTSPLINES.Posx.KeyNum > 0)
				{
					World.ObjectList[i].PosData.Pos.X = dEFAULTOBJECTSPLINES.Posx.GetInterpolatedValue(time);
				}
				if (dEFAULTOBJECTSPLINES.Posy.KeyNum > 0)
				{
					World.ObjectList[i].PosData.Pos.Y = dEFAULTOBJECTSPLINES.Posy.GetInterpolatedValue(time);
				}
				if (dEFAULTOBJECTSPLINES.Posz.KeyNum > 0)
				{
					World.ObjectList[i].PosData.Pos.Z = dEFAULTOBJECTSPLINES.Posz.GetInterpolatedValue(time);
				}
				if (dEFAULTOBJECTSPLINES.Sclx.KeyNum > 0)
				{
					World.ObjectList[i].PosData.Scale.X = dEFAULTOBJECTSPLINES.Sclx.GetInterpolatedValue(time);
				}
				if (dEFAULTOBJECTSPLINES.Scly.KeyNum > 0)
				{
					World.ObjectList[i].PosData.Scale.Y = dEFAULTOBJECTSPLINES.Scly.GetInterpolatedValue(time);
				}
				if (dEFAULTOBJECTSPLINES.Sclz.KeyNum > 0)
				{
					World.ObjectList[i].PosData.Scale.Z = dEFAULTOBJECTSPLINES.Sclz.GetInterpolatedValue(time);
				}
				if (dEFAULTOBJECTSPLINES.Prtfrme != null && dEFAULTOBJECTSPLINES.Prtfrme.KeyNum > 0)
				{
					World.ObjectList[i].EmitterData.ParticlesPerFrame = dEFAULTOBJECTSPLINES.Prtfrme.GetInterpolatedValue(time);
				}
				if (dEFAULTOBJECTSPLINES.Rotx.KeyNum > 0)
				{
					World.ObjectList[i].PosData.Quaternion = Addict2RenderingTools.SplineSlerp(dEFAULTOBJECTSPLINES.Rotx, dEFAULTOBJECTSPLINES.Roty, dEFAULTOBJECTSPLINES.Rotz, dEFAULTOBJECTSPLINES.Rotw, time);
				}
			}
		}
		if (!DEData.OnScreenInLastFrame)
		{
			for (int j = 0; j < World.ObjectNum; j++)
			{
				if (World.ObjectList[j].EmitterData != null)
				{
					World.ObjectList[j].EmitterData.LastFrameChecked = DEData.CurrentFrame;
				}
			}
		}
		World.CalculateObjectMatrices();
		World.CalculateParticles(DEData.CurrentFrame);
	}
}
