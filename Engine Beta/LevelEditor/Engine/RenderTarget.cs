using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace LevelEditor.Engine
{
    /// <summary>
    /// 
    /// </summary>
    class RenderTarget
    {

        public RenderTarget2D mMainRenderTarget;
        public RenderTarget2D mShadowRenderTarget;

        /// <summary>
        /// Constructs a <see cref="RenderTarget"/>.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="shadowMapResolution"></param>
        public RenderTarget(GraphicsDevice device, int width, int height, int shadowMapResolution)
        {
            mMainRenderTarget = new RenderTarget2D(device, width, height, false, SurfaceFormat.HalfVector4, DepthFormat.Depth24);
            mShadowRenderTarget = new RenderTarget2D(device, shadowMapResolution, shadowMapResolution, false, SurfaceFormat.Single, DepthFormat.Depth24);

        }

    }
}
