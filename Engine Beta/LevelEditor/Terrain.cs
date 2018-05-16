using System;
using LevelEditor.Engine.Mesh;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    internal class Terrain : IActor
    {

        private const float TerrainScale = 1.0f;
        private const float TerrainMaxHeight = 40.0f;
        private const float TextureRepitions = 1.0f;

        private readonly int mHeight;
        private readonly int mWidth;

        public Actor MeshActor { get; }

        public Terrain(ContentManager content, string heightMapPath, string texturePath, GraphicsDevice device)
        {

            var meshData = new MeshData();

            var heightmap = content.Load<Texture2D>(heightMapPath);

            mWidth = heightmap.Width;
            mHeight = heightmap.Height;

            meshData.mTexture = content.Load<Texture2D>(texturePath);
            meshData.mPrimitiveType = PrimitiveType.TriangleStrip;
            meshData.mPrimitiveCount = (heightmap.Width * 2 + 2) * (heightmap.Height - 1);
            meshData.mBoundingSphere.Radius = (float)Math.Sqrt(Math.Pow(heightmap.Width /  2.0f, 2) + Math.Pow(heightmap.Height / 2.0f, 2));

            meshData.mVertices = new VertexPositionTexture[mWidth * mHeight];
            meshData.mIndices = new int[(mWidth * 2 + 2) * (mHeight - 1)];
            
            var heightVal = new Color[mWidth * mHeight];
            heightmap.GetData(heightVal);

            for (int z = 0; z < mHeight; z++)
            {
                for (int x = 0; x < mWidth; x++)
                {
                    //Position
                    meshData.mVertices[x + z * mWidth].Position.X = (x - mWidth / 2.0f) * TerrainScale;
                    meshData.mVertices[x + z * mWidth].Position.Y = heightVal[x + z * mWidth].G / 255.0f * TerrainMaxHeight;
                    meshData.mVertices[x + z * mWidth].Position.Z = (z - mHeight / 2.0f) * TerrainScale;

                    //Texture
                    meshData.mVertices[x + z * mWidth].TextureCoordinate.X = (float)x / mWidth * TextureRepitions;
                    meshData.mVertices[x + z * mWidth].TextureCoordinate.Y = (float)z / mHeight * TextureRepitions;
                }
            }

            //Calculates the triangle strip
            var i = 0;

            for (int z = 0; z < mHeight - 1; z++)
            {

                meshData.mIndices[i++] = z * mWidth;

                for (int x = 0; x < mWidth; x++)
                {
                    meshData.mIndices[i++] = z * mWidth + x;
                    meshData.mIndices[i++] = (z + 1) * mWidth + x;
                    
                }

                meshData.mIndices[i++] = (z + 1) * mWidth + (mWidth - 1);


            }

            MeshActor = new Actor(new Mesh(device, meshData));

        }

        private float GetHeightFromData(VertexPositionTexture[] vertices, int x, int z)
        {

            if (x < mWidth && z < mHeight && z >= 0 && x >= 0)
            {
                return vertices[z * mWidth + x].Position.Y;
            }
            else
            {
                return vertices[0].Position.Y;
            }

        }

        static float BarryCentric(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 pos)
        {
            var det = (p2.Z - p3.Z) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Z - p3.Z);
            var l1 = ((p2.Z - p3.Z) * (pos.X - p3.X) + (p3.X - p2.X) * (pos.Y - p3.Z)) / det;
            var l2 = ((p3.Z - p1.Z) * (pos.X - p3.X) + (p1.X - p3.X) * (pos.Y - p3.Z)) / det;
            var l3 = 1.0f - l1 - l2;
            return l1 * p1.Y + l2 * p2.Y + l3 * p3.Y;
        }

        public float GetHeight(Vector3 location)
        {
            
			float height;
            
            var x = mWidth / 2f + location.X;
            var z = mHeight / 2f + location.Z;

            // Console.WriteLine(x.ToString() + " " + z.ToString());

            if (x < 0 || z < 0 || x >= mWidth * TerrainScale || z >= mHeight * TerrainScale)
            {
                return 0.0f;
            }

            var position = new Vector2((int)Math.Floor(x / TerrainScale), (int)Math.Floor(z / TerrainScale));

            if (position.X < 0 || position.Y < 0 ||
                (int)position.X >= mWidth * TerrainScale || (int)position.Y >= mHeight * TerrainScale)
            {
                return 0.0f;
            }

            var coord = new Vector2((x % TerrainScale) / TerrainScale, (z % TerrainScale) / TerrainScale);

            if (coord.X > coord.Y)
            {
                height = BarryCentric(new Vector3(0.0f,
                        MeshActor.mMesh.mMeshData.mVertices[(int)(position.X + mWidth * position.Y)].Position.Y,
                        0.0f),
                    new Vector3(1.0f, MeshActor.mMesh.mMeshData.mVertices[(int)(position.X + 1 + mWidth * position.Y)].Position.Y, 0.0f),
                    new Vector3(1.0f, MeshActor.mMesh.mMeshData.mVertices[(int)(position.X + 1 + mWidth * (position.Y + 1))].Position.Y, 1.0f),
                    coord);
            }
            else
            {
                height = BarryCentric(new Vector3(0.0f, MeshActor.mMesh.mMeshData.mVertices[(int)(position.X + mWidth * position.Y)].Position.Y, 0.0f),
                    new Vector3(1.0f, MeshActor.mMesh.mMeshData.mVertices[(int)(position.X + 1 + mWidth * (position.Y + 1))].Position.Y, 1.0f),
                    new Vector3(0.0f, MeshActor.mMesh.mMeshData.mVertices[(int)(position.X + mWidth * (position.Y + 1))].Position.Y, 1.0f),
                    coord);
            }

            return height;

        }


    }
}
