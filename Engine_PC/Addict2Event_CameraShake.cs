using System;
using Microsoft.Xna.Framework;

namespace Engine_PC;

public class Addict2Event_CameraShake : Addict2Event
{
	public float Eye;

	public float Target;

	public float Start;

	public float End;

	public byte Freq;

	private Random randc = new Random();

	private int RAND_MAX = int.MaxValue;

	private int rand()
	{
		return randc.Next();
	}

	private void srand(int i)
	{
		randc = new Random(i);
	}

	public override void Render(DEFAULTEVENTDATA DEData)
	{
		float num = Addict2RenderingTools.LinearInterpolate(Start, End, DEData.TimePos);
		int num2 = DEData.CurrentFrame - DEData.CurrentFrame % (Freq * 10);
		int i = num2 + Freq * 10;
		float[,] array = new float[2, 3];
		float[,] array2 = new float[2, 3];
		srand(num2);
		for (int j = 0; j < 2; j++)
		{
			for (int k = 0; k < 3; k++)
			{
				array[j, k] = ((float)rand() / (float)RAND_MAX - 0.5f) * 2f * num;
			}
		}
		srand(i);
		for (int j = 0; j < 2; j++)
		{
			for (int l = 0; l < 3; l++)
			{
				array2[j, l] = ((float)rand() / (float)RAND_MAX - 0.5f) * 2f * num;
			}
		}
		float pos = (float)(DEData.CurrentFrame - num2) / (float)(Freq * 10);
		Addict2RenderingTools.EyeShake = Vector3.Multiply(new Vector3(Eye, Eye, Eye), 0.1f);
		Addict2RenderingTools.TargetShake = Vector3.Multiply(new Vector3(Target, Target, Target), 0.1f);
		Addict2RenderingTools.EyeShake.X *= Addict2RenderingTools.LinearInterpolate(array[0, 0], array2[0, 0], pos);
		Addict2RenderingTools.EyeShake.Y *= Addict2RenderingTools.LinearInterpolate(array[0, 1], array2[0, 1], pos);
		Addict2RenderingTools.EyeShake.Z *= Addict2RenderingTools.LinearInterpolate(array[0, 2], array2[0, 2], pos);
		Addict2RenderingTools.TargetShake.X *= Addict2RenderingTools.LinearInterpolate(array[1, 0], array2[1, 0], pos);
		Addict2RenderingTools.TargetShake.Y *= Addict2RenderingTools.LinearInterpolate(array[1, 1], array2[1, 1], pos);
		Addict2RenderingTools.TargetShake.Z *= Addict2RenderingTools.LinearInterpolate(array[1, 2], array2[1, 2], pos);
	}
}
