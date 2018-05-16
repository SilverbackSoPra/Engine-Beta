using LevelEditor.Engine.Shader;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor.Engine.Renderer
{
    internal sealed class PostProcessRenderer : IRenderer
    {

        private readonly PostProcessShader mShader;

        private readonly GraphicsDevice mDevice;

        public PostProcessRenderer(GraphicsDevice device, ContentManager content, string shaderPath)
        {
            mDevice = device;

            mShader = new PostProcessShader(content, shaderPath);

        }

        public void Render(RenderTarget target, Camera camera, Scene scene)
        {

            /*
            We don't want use the SpriteBatch class and should change to our own vertex structure
            To improve this: http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series1/Terrain_lighting.php
            Some further resources: https://docs.microsoft.com/en-us/previous-versions/windows/silverlight/dotnet-windows-silverlight/bb199731%28v%3dxnagamestudio.35%29
            and: http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series1/VertexBuffer_and_IndexBuffer.php
            */

            mShader.mAlbedoMap = target.mMainRenderTarget;
            mShader.mSaturation = scene.mPostProcessing.mSaturation;

            mShader.Apply();

            mDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 4);

        }
    }
}
