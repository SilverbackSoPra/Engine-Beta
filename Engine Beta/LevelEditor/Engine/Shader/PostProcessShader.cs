using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor.Engine.Shader
{
    internal sealed class PostProcessShader : Shader
    {

        private readonly EffectParameter mAlbedoMapParameter;
        private readonly EffectParameter mSaturationParameter;

        public Texture2D mAlbedoMap;
        public float mSaturation;

        public PostProcessShader(ContentManager content, string shaderPath) : base(content, shaderPath)
        {

            mAlbedoMapParameter = mShader.Parameters["albedoMap"];
            mSaturationParameter = mShader.Parameters["saturation"];

        }

        public override void Apply()
        {

            mAlbedoMapParameter.SetValue(mAlbedoMap);
            mSaturationParameter.SetValue(mSaturation);

            base.Apply();
        }

    }
}
