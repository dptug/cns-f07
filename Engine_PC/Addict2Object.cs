using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine_PC;

public class Addict2Object
{
	public int ID;

	public Addict2ObjectPrimitiveType Primitive;

	public int Param1;

	public int Param2;

	public int Param3;

	public int Param4;

	public int Param5;

	public VERTEX[] VertexList;

	public int VertexNum;

	public int VertexCapacity;

	public POLYGON[] PolygonList;

	public int PolygonNum;

	public int PolygonCapacity;

	public int EdgeNum;

	public int EdgeCapacity;

	public Blend SRCBlend;

	public Blend DSTBlend;

	public bool Wireframe;

	public Addict2Shading Shading;

	public Matrix ModelView;

	public Matrix TransformBuffer;

	public Matrix Inverted;

	public Matrix IT;

	public int NewVertexNum;

	public bool Textured;

	public Addict2Material Material;

	public bool EnvMapped;

	public Addict2Material EnvMap;

	public bool NormalsInverted;

	public byte Xtile;

	public byte Ytile;

	public bool XSwap;

	public bool YSwap;

	public bool Swap;

	public float OffsetX;

	public float OffsetY;

	public RGBA Color;

	public bool ZMask;

	public bool Backface;

	public bool Backfront;

	public byte AEpsilon;

	public sbyte MapXformType;

	public sbyte MapXformColor;

	public int TargetObjectID;

	public VertexBuffer vb;

	public IndexBuffer ib;

	public void AddVertex(float x, float y, float z, float u, float v)
	{
		if (VertexCapacity == VertexNum)
		{
			VERTEX[] array = new VERTEX[(VertexCapacity + 10) * 2];
			if (VertexNum > 0)
			{
				VertexList.CopyTo(array, 0);
			}
			VertexList = null;
			VertexList = array;
			VertexCapacity = (VertexCapacity + 10) * 2;
		}
		VertexList[VertexNum].Position.X = x;
		VertexList[VertexNum].Position.Y = y;
		VertexList[VertexNum].Position.Z = z;
		VertexList[VertexNum].MapTransformedPosition = VertexList[VertexNum].Position;
		VertexList[VertexNum].TextureCoordinate.X = u;
		VertexList[VertexNum].TextureCoordinate.Y = v;
		VertexList[VertexNum].CurrentTextureCoordinate = VertexList[VertexNum].TextureCoordinate;
		VertexList[VertexNum].EdgeCapacity = 6;
		VertexList[VertexNum].EdgeNum = 0;
		VertexList[VertexNum].EdgeList = new int[6];
		VertexList[VertexNum].Weight = new float[4];
		VertexNum++;
	}

	public void AddPolygon(int x, int y, int z, bool e1, bool e2, bool e3)
	{
		if (PolygonCapacity == PolygonNum)
		{
			POLYGON[] array = new POLYGON[(PolygonCapacity + 10) * 2];
			if (PolygonNum > 0)
			{
				PolygonList.CopyTo(array, 0);
			}
			PolygonList = null;
			PolygonList = array;
			PolygonCapacity = (PolygonCapacity + 10) * 2;
		}
		PolygonList[PolygonNum].v = new int[3];
		PolygonList[PolygonNum].v[0] = x;
		PolygonList[PolygonNum].v[1] = y;
		PolygonList[PolygonNum].v[2] = z;
		PolygonList[PolygonNum].t = new Vector2[3];
		ref Vector2 reference = ref PolygonList[PolygonNum].t[0];
		reference = VertexList[x].TextureCoordinate;
		ref Vector2 reference2 = ref PolygonList[PolygonNum].t[1];
		reference2 = VertexList[y].TextureCoordinate;
		ref Vector2 reference3 = ref PolygonList[PolygonNum].t[2];
		reference3 = VertexList[z].TextureCoordinate;
		PolygonList[PolygonNum].ct = new Vector2[3];
		ref Vector2 reference4 = ref PolygonList[PolygonNum].ct[0];
		reference4 = PolygonList[PolygonNum].t[0];
		ref Vector2 reference5 = ref PolygonList[PolygonNum].ct[1];
		reference5 = PolygonList[PolygonNum].t[1];
		ref Vector2 reference6 = ref PolygonList[PolygonNum].ct[2];
		reference6 = PolygonList[PolygonNum].t[2];
		Vector3 vector = VertexList[y].Position - VertexList[x].Position;
		Vector3 vector2 = VertexList[z].Position - VertexList[x].Position;
		PolygonList[PolygonNum].Normal = (PolygonList[PolygonNum].CurrentNormal = Vector3.Cross(vector2, vector));
		PolygonList[PolygonNum].Normal.Normalize();
		PolygonList[PolygonNum].e = new int[3];
		PolygonNum++;
	}

	public void AddPolygon(int x, int y, int z, Addict2Shading Shading, bool e1, bool e2, bool e3)
	{
		AddPolygon(x, y, z, e1, e2, e3);
		PolygonList[PolygonNum - 1].Shading = Shading;
		PolygonList[PolygonNum - 1].CurrentShading = Shading;
	}

	public void CalculateNormals()
	{
		for (int i = 0; i < VertexNum; i++)
		{
			VertexList[i].Normal = new Vector3(0f, 0f, 0f);
		}
		for (int i = 0; i < PolygonNum; i++)
		{
			PolygonList[i].Normal = Vector3.Cross(VertexList[PolygonList[i].v[2]].Position - VertexList[PolygonList[i].v[0]].Position, VertexList[PolygonList[i].v[1]].Position - VertexList[PolygonList[i].v[0]].Position);
			if (!NormalsInverted)
			{
				PolygonList[i].Normal *= -1f;
			}
			VertexList[PolygonList[i].v[0]].Normal += PolygonList[i].Normal;
			VertexList[PolygonList[i].v[1]].Normal += PolygonList[i].Normal;
			VertexList[PolygonList[i].v[2]].Normal += PolygonList[i].Normal;
			PolygonList[i].Normal.Normalize();
		}
		for (int i = 0; i < VertexNum; i++)
		{
			VertexList[i].Normal.Normalize();
		}
	}
}
