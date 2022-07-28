namespace Engine_PC;

public struct LIGHT
{
	public int Identifier;

	public bool Lit;

	public float[] Ambient;

	public float[] Color;

	public float[] Position;

	public float[] Spot_Direction;

	public float Spot_Exponent;

	public float Spot_Cutoff;

	public float Constant_Attenuation;

	public float Linear_Attenuation;

	public float Quadratic_Attenuation;

	public bool CastShadow;
}
