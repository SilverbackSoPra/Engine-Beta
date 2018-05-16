using LevelEditor.Engine.Helper;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor.Engine.Mesh
{

    /// <summary>
    /// 
    /// </summary>
    internal sealed class Mesh
    {

        public VertexBuffer VertexBuffer { get; }
        public IndexBuffer IndexBuffer { get; }

        public MeshData mMeshData;

        /// <summary>
        /// Constructs a <see cref="Mesh"/>.
        /// </summary>
        /// <param name="model">A textured model which was loaded with the content pipeline of MonoGame.</param>
        public Mesh(Model model)
        {

            // How to use animations: 
            mMeshData = new MeshData();

            if (model.Meshes.Count != 1)
            {
                throw new EngineInvalidParameterException("Model has more than one mesh");
            }
            else
            {
                if (model.Meshes[0].MeshParts.Count != 1)
                {
                    throw new EngineInvalidParameterException("Model probably has more than one material");
                }
                else
                {

                    Texture2D texture = ((BasicEffect)model.Meshes[0].MeshParts[0].Effect).Texture;

                    if (texture == null)
                    {
                        throw new EngineInvalidParameterException("Model has no texture attached");
                    }
                    else
                    {
                        mMeshData.mBoundingSphere = model.Meshes[0].BoundingSphere;

                        mMeshData.mTexture = texture;

                        mMeshData.mPrimitiveCount = model.Meshes[0].MeshParts[0].PrimitiveCount;

                        VertexBuffer = model.Meshes[0].MeshParts[0].VertexBuffer;
                        IndexBuffer = model.Meshes[0].MeshParts[0].IndexBuffer;
                    }

                }
                
            }

        }

        /// <summary>
        /// Constructs a <see cref="Mesh"/>.
        /// </summary>
        /// <param name="device">The graphics device which should already be initialized.</param>
        /// <param name="data">The data of the mesh. All the data should've already been loaded.</param>
        public Mesh(GraphicsDevice device, MeshData data)
        {

            if (data.mPrimitiveCount == 0)
            {
                throw new EngineInvalidParameterException("Invalid primitve count");
            }
            else
            {
                if (data.mBoundingSphere.Radius <= 0.0f)
                {
                    throw new EngineInvalidParameterException("Radius should be larger than zero");
                }
                else
                {
                    if (data.mVertices.Length == 0 || data.mIndices.Length == 0)
                    {
                        throw new EngineInvalidParameterException("Invalid vertices or indices count");
                    }
                    else
                    {
                        if (data.mTexture == null)
                        {
                            throw new EngineInvalidParameterException("No texture attached to data");
                        }
                        else
                        {
                            mMeshData = data;

                            VertexBuffer = new VertexBuffer(device,
                                VertexPositionTexture.VertexDeclaration,
                                mMeshData.mVertices.Length,
                                BufferUsage.WriteOnly);
                            IndexBuffer = new IndexBuffer(device,
                                typeof(int),
                                mMeshData.mIndices.Length,
                                BufferUsage.WriteOnly);

                            VertexBuffer.SetData(mMeshData.mVertices);
                            IndexBuffer.SetData(mMeshData.mIndices);
                        }
                    }
                    
                }

            }

        }

    }
}
