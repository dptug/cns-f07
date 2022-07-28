using System;

namespace Engine_PC;

public class Addict2Spline
{
	public int KeyNum;

	public int KeyCapacity;

	public Key[] Keys;

	public bool Loop;

	public float LoopStart;

	public float LoopEnd;

	public Addict2SplineInterpolationType Interpolation;

	private static float TENS;

	private static bool LOOP;

	private static float BIAS;

	private static float CONT;

	public static float hermite(float p1, float p2, float r1, float r2, float t)
	{
		return p1 * (2f * (t * t * t) - 3f * (t * t) + 1f) + r1 * (t * t * t - 2f * (t * t) + t) + p2 * (-2f * (t * t * t) + 3f * (t * t)) + r2 * (t * t * t - t * t);
	}

	public static float catmull2(float v0, float v1, float v2, float v3, float t)
	{
		float num = 0f - v0 + 3f * v1 - 3f * v2 + v3;
		float num2 = 2f * v0 - 5f * v1 + 4f * v2 - v3;
		float num3 = 0f - v0 + v2;
		float num4 = 2f * v1;
		return (num * t * t * t + num2 * t * t + num3 * t + num4) * 0.5f;
	}

	public static float BSplineInterpolate(float c0, float c1, float c2, float c3, float u)
	{
		float num = u * u;
		float num2 = u * u * u;
		float num3 = num2 / 6f;
		float num4 = (1f + 3f * u + 3f * num - 3f * num2) / 6f;
		float num5 = (4f - 6f * num + 3f * num2) / 6f;
		float num6 = (1f - 3f * u + 3f * num - num2) / 6f;
		return c0 * num6 + c1 * num5 + c2 * num4 + c3 * num3;
	}

	public float GetInterpolatedValue(float Time)
	{
		int i = 0;
		float num = 1f;
		if (Interpolation == Addict2SplineInterpolationType.SINE || Interpolation == Addict2SplineInterpolationType.SAW || Interpolation == Addict2SplineInterpolationType.SQUARE)
		{
			if (Time < Keys[0].Time)
			{
				num = Addict2RenderingTools.LinearInterpolate(0f, Keys[0].Value, Time / Keys[0].Time);
			}
			if (Time > Keys[1].Time)
			{
				num = Addict2RenderingTools.LinearInterpolate(Keys[1].Value, 0f, (Time - Keys[1].Time) / (1f - Keys[1].Time));
			}
			if (Time >= Keys[0].Time && Time <= Keys[1].Time)
			{
				num = Addict2RenderingTools.LinearInterpolate(Keys[0].Value, Keys[1].Value, (Time - Keys[0].Time) / (Keys[1].Time - Keys[0].Time));
			}
		}
		else
		{
			for (; Keys[i].Time < Time && i < KeyNum; i++)
			{
			}
			i--;
			if (i == -1)
			{
				i = 0;
				return Keys[i].Value;
			}
			if (i == KeyNum - 1)
			{
				return Keys[i].Value;
			}
		}
		float num2 = (Time - Keys[i].Time) / (Keys[i + 1].Time - Keys[i].Time);
		switch (Interpolation)
		{
		case Addict2SplineInterpolationType.LINEAR:
			return Addict2RenderingTools.LinearInterpolate(Keys[i].Value, Keys[i + 1].Value, num2);
		case Addict2SplineInterpolationType.HERMITE:
			return hermite(Keys[i].Value, Keys[i + 1].Value, Keys[i].an, Keys[i + 1].bn, num2);
		case Addict2SplineInterpolationType.CATMULL:
		{
			int num6 = i - 1;
			if (i - 1 < 0)
			{
				num6 = 0;
			}
			int num7 = i;
			int num8 = i + 1;
			int num9 = i + 2;
			if (i + 1 >= KeyNum - 1)
			{
				num9 = KeyNum - 1;
			}
			return catmull2(Keys[num6].Value, Keys[num7].Value, Keys[num8].Value, Keys[num9].Value, num2);
		}
		case Addict2SplineInterpolationType.BEZIER:
		{
			int num10 = i - 1;
			if (i - 1 < 0)
			{
				num10 = 0;
			}
			int num11 = i;
			int num12 = i + 1;
			int num13 = i + 2;
			if (i + 1 >= KeyNum - 1)
			{
				num13 = KeyNum - 1;
			}
			return BSplineInterpolate(Keys[num10].Value, Keys[num11].Value, Keys[num12].Value, Keys[num13].Value, num2);
		}
		case Addict2SplineInterpolationType.SINE:
			return (float)((double)num * Math.Sin(Time / Keys[2].Time));
		case Addict2SplineInterpolationType.SAW:
		{
			float num4 = Time / Keys[2].Time / 3.1415f;
			num4 -= (float)Math.Floor(num4);
			float num5 = ((!(num2 < 0.5f)) ? (1f - 4f * (num4 - 0.5f)) : (4f * num4 - 1f));
			return num * num5;
		}
		case Addict2SplineInterpolationType.SQUARE:
		{
			float num3 = (float)Math.Sin(Time / Keys[2].Time);
			num3 /= Math.Abs(num3);
			return num * num3;
		}
		default:
			return Keys[i].Value;
		}
	}

	public void InitVectors()
	{
		if (KeyNum > 1)
		{
			for (int i = 0; i < KeyNum; i++)
			{
				Keys[i].an = GetVector(0, i);
				Keys[i].bn = GetVector(1, i);
			}
		}
	}

	public float GetVector(byte sel, int n)
	{
		Key key = Keys[n];
		float value = key.Value;
		if (sel == 2)
		{
			return value;
		}
		Key key2;
		Key key3;
		float value3;
		float value2;
		if (n == 0)
		{
			key2 = Keys[1];
			value2 = key2.Value;
			if (KeyNum == 2)
			{
				return (float)((double)(value2 - value) * (1.0 - (double)TENS));
			}
			if (!LOOP)
			{
				return (float)(((double)(value2 - value) * 1.5 - (double)GetVector(0, 1) * 0.5) * (1.0 - (double)TENS));
			}
			key3 = Keys[KeyNum - 2];
		}
		else if (n == KeyNum - 1)
		{
			key3 = Keys[n - 1];
			value3 = key3.Value;
			if (KeyNum == 2)
			{
				return (float)((double)(value - value3) * (1.0 - (double)TENS));
			}
			if (!LOOP)
			{
				return (float)(((double)(value - value3) * 1.5 - (double)GetVector(1, n - 1) * 0.5) * (1.0 - (double)TENS));
			}
			key2 = Keys[1];
		}
		else
		{
			key3 = Keys[n - 1];
			key2 = Keys[n + 1];
		}
		value3 = key3.Value;
		value2 = key2.Value;
		float num = ((sel == 0) ? 0.5f : (-0.5f));
		float num2 = (value - value3) * (1f + BIAS);
		float num3 = (value2 - value) * (1f - BIAS);
		return (num2 + (num3 - num2) * (0.5f + num * CONT)) * (1f - TENS);
	}
}
