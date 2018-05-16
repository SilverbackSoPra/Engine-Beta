using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using LevelEditor.Engine.Shader;

namespace LevelEditor.Engine.Renderer
{
    class ShadowRenderer : IRenderer
    {

        private readonly ShadowShader mShader;
        private readonly GraphicsDevice mGraphicsDevice;

        public ShadowRenderer(GraphicsDevice device, ContentManager content, string shaderPath)
        {

            mShader = new ShadowShader(content, shaderPath);

            mGraphicsDevice = device;

        }

        public void Render(RenderTarget target, Camera camera, Scene scene)
        {

            Light globalLight = scene.mLights[0];

            mShader.mLightSpaceMatrix = globalLight.mShadow.mViewMatrix * globalLight.mShadow.mProjectionMatrix;

            mShader.Apply();

            // Now render our actor in batched mode
            foreach (var actorBatch in scene.mActorBatches)
            {

                var meshData = actorBatch.mMesh.mMeshData;

                mGraphicsDevice.SetVertexBuffer(actorBatch.mMesh.VertexBuffer);
                mGraphicsDevice.Indices = (actorBatch.mMesh.IndexBuffer);

                foreach (var actor in actorBatch.mActors)
                {

                    mShader.ApplyModelMatrix(actor.mModelMatrix);

                    mGraphicsDevice.DrawIndexedPrimitives(meshData.mPrimitiveType,
                        0,
                        0,
                        meshData.mPrimitiveCount);

                }

            }

        }
    }
}
