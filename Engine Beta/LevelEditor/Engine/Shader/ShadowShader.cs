using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor.Engine.Shader
{
    class ShadowShader : Shader
    {

        private readonly EffectParameter mModelMatrixParameter;
        private readonly EffectParameter mLightSpaceMatrixParameter;

        public Matrix mLightSpaceMatrix;

        public ShadowShader(ContentManager content, string shaderPath) : base(content, shaderPath)
        {

            mModelMatrixParameter = mShader.Parameters["modelMatrix"];
            mLightSpaceMatrixParameter = mShader.Parameters["lightSpaceMatrix"];

        }

        public override void Apply()
        {

            mLightSpaceMatrixParameter.SetValue(mLightSpaceMatrix);

            base.Apply();

        }

        public void ApplyModelMatrix(Matrix modelMatrix)
        {
            mModelMatrixParameter.SetValue(modelMatrix);
            base.Apply();
        }

    }

}
