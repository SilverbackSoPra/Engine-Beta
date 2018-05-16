using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor.Engine.Shader
{

    internal sealed class ForwardShader : Shader
    {
        private readonly EffectParameter mAlbedoMapParameter;
        private readonly EffectParameter mModelMatrixParameter;
        private readonly EffectParameter mViewMatrixParameter;
        private readonly EffectParameter mProjectionMatrixParameter;
        private readonly EffectParameter mGlobalLightLocationParameter;
        private readonly EffectParameter mGlobalLightColorParameter;
        private readonly EffectParameter mGlobalLightAmbientParameter;
        private readonly EffectParameter mLightSpaceMatrixParameter;
        private readonly EffectParameter mShadowMapParameter;

        public Matrix mViewMatrix;
        public Matrix mProjectionMatrix;

        public Vector3 mGlobalLightLocation;
        public Vector3 mGlobalLightColor;
        public float mGlobalLightAmbient;

        public Matrix mLightSpaceMatrix;
        public Texture2D mShadowMap;

        public ForwardShader(ContentManager content, string shaderPath) : base(content, shaderPath)
        { 

            mModelMatrixParameter = mShader.Parameters["modelMatrix"];
            mViewMatrixParameter = mShader.Parameters["viewMatrix"];
            mProjectionMatrixParameter = mShader.Parameters["projectionMatrix"];

            mAlbedoMapParameter = mShader.Parameters["albedoMap"];
            mGlobalLightLocationParameter = mShader.Parameters["lightLocation"];
            mGlobalLightColorParameter = mShader.Parameters["lightColor"];
            mGlobalLightAmbientParameter = mShader.Parameters["lightAmbient"];

            mLightSpaceMatrixParameter = mShader.Parameters["lightSpaceMatrix"];
            mShadowMapParameter = mShader.Parameters["shadowMap"];

        }

        
        public override void Apply()
        {

            // TODO: We should check whether the matrices are not null
            mViewMatrixParameter.SetValue(mViewMatrix);
            mProjectionMatrixParameter.SetValue(mProjectionMatrix);

            mGlobalLightLocationParameter.SetValue(mGlobalLightLocation);
            mGlobalLightColorParameter.SetValue(mGlobalLightColor);
            mGlobalLightAmbientParameter.SetValue(mGlobalLightAmbient);

            mLightSpaceMatrixParameter.SetValue(mLightSpaceMatrix);
            mShadowMapParameter.SetValue(mShadowMap);

            base.Apply();
        }
        
        public void ApplyMaterial(Texture2D albedoMap)
        {
            mAlbedoMapParameter.SetValue(albedoMap);
            base.Apply();
        }

        public void ApplyModelMatrix(Matrix modelMatrix)
        {
            mModelMatrixParameter.SetValue(modelMatrix);
            base.Apply();
        }

    }
}