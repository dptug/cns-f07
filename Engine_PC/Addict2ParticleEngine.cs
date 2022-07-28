using Microsoft.Xna.Framework.Graphics;

namespace Engine_PC;

public class Addict2ParticleEngine
{
	public static int[] ParticleIndexBuffer;

	public static int MaxParticleNum;

	public static VertexPositionColorTexture[] ParticleVertices;

	public static VertexDeclaration decl;

	public static VertexDeclaration linedecl;

	public static void Init()
	{
		decl = new VertexDeclaration(Addict2Engine.device, VertexPositionColorTexture.VertexElements);
		linedecl = new VertexDeclaration(Addict2Engine.device, VertexPositionColor.VertexElements);
		ParticleIndexBuffer = new int[MaxParticleNum * 6];
		for (int i = 0; i < MaxParticleNum * 6; i++)
		{
			ParticleIndexBuffer[i] = i;
		}
		ParticleVertices = new VertexPositionColorTexture[MaxParticleNum * 6];
		for (int i = 0; i < MaxParticleNum; i++)
		{
			ParticleVertices[i * 6 + 1].TextureCoordinate.X = 1f;
			ParticleVertices[i * 6 + 2].TextureCoordinate.X = 1f;
			ParticleVertices[i * 6 + 4].TextureCoordinate.X = 1f;
			ParticleVertices[i * 6 + 2].TextureCoordinate.Y = 1f;
			ParticleVertices[i * 6 + 4].TextureCoordinate.Y = 1f;
			ParticleVertices[i * 6 + 5].TextureCoordinate.Y = 1f;
		}
	}
}
