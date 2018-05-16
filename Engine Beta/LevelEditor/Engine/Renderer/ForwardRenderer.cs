using LevelEditor.Engine.Shader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor.Engine.Renderer
{
    internal sealed class ForwardRenderer : IRenderer
    {

        private readonly ForwardShader mShader;
        private readonly GraphicsDevice mGraphicsDevice;

        public static readonly SamplerState ShadowMap = new SamplerState
        {
            AddressU = TextureAddressMode.Clamp,
            AddressV = TextureAddressMode.Clamp,
            AddressW = TextureAddressMode.Clamp,
            Filter = TextureFilter.Linear,
            ComparisonFunction = CompareFunction.LessEqual,
            FilterMode = TextureFilterMode.Comparison
        };

        public ForwardRenderer(GraphicsDevice device, ContentManager content, string shaderPath)
        {

            mShader = new ForwardShader(content, shaderPath);

            mGraphicsDevice = device;

        }

        public void Render(RenderTarget target, Camera camera, Scene scene)
        {

            Light globalLight = scene.mLights[0];

            mGraphicsDevice.SamplerStates[1] = ShadowMap;

            mShader.mViewMatrix = camera.mViewMatrix;
            mShader.mProjectionMatrix = camera.mProjectionMatrix;

            // We need the light location to be in view space (because the shader does all operations in view space)
            mShader.mGlobalLightLocation = Vector3.Transform(globalLight.mLocation, camera.mViewMatrix);

            mShader.mGlobalLightColor = globalLight.mColor;
            mShader.mGlobalLightAmbient = globalLight.mAmbient;

            mShader.mLightSpaceMatrix = Matrix.Invert(camera.mViewMatrix) * globalLight.mShadow.mViewMatrix * globalLight.mShadow.mProjectionMatrix;
            mShader.mShadowMap = target.mShadowRenderTarget;

            mShader.Apply();

            // Now render our actor in batched mode
            foreach (var actorBatch in scene.mActorBatches)
            {

                var meshData = actorBatch.mMesh.mMeshData;
                
                mGraphicsDevice.SetVertexBuffer(actorBatch.mMesh.VertexBuffer);
                mGraphicsDevice.Indices = (actorBatch.mMesh.IndexBuffer);

                mShader.ApplyMaterial(meshData.mTexture);

                foreach (var actor in actorBatch.mActors)
                {

                    if (actor.mRender)
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
}
