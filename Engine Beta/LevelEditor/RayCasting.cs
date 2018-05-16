using LevelEditor.Engine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LevelEditor.Engine.Mesh;
using Microsoft.Xna.Framework.Input;

namespace LevelEditor
{
    internal sealed class RayCasting
    {
        
        public static Actor CastRayFromMousePosition(Viewport viewport, Camera camera, Scene scene)
        {

            Actor nearestActor = null;

            float minDistance = camera.mFarPlane;

            var mouseState = Mouse.GetState();

            var mousePosition = new Vector2(mouseState.X, mouseState.Y);

            var ray = CalculateRay(viewport, camera, mousePosition);

            foreach (var actorBatch in scene.mActorBatches)
            {

                foreach (var actor in actorBatch.mActors)
                {

                    if (actor.mRender)
                    {

                        float? distance = IntersectDistance(actor, ray);

                        if (distance != null)
                        {

                            if (minDistance > distance.Value)
                            {
                                nearestActor = actor;
                                minDistance = distance.Value;
                            }

                        }

                    }

                }

            }

            return nearestActor;

        }

        private static Ray CalculateRay(Viewport viewport, Camera camera, Vector2 mouseLocation)
        {

            var nearPoint = viewport.Unproject(new Vector3(mouseLocation.X,
                mouseLocation.Y, 0.0f),
                camera.mProjectionMatrix,
                camera.mViewMatrix,
                Matrix.Identity);

            var farPoint = viewport.Unproject(new Vector3(mouseLocation.X,
                mouseLocation.Y, 1.0f),
                camera.mProjectionMatrix,
                camera.mViewMatrix,
                Matrix.Identity);

            var direction = farPoint - nearPoint;

            direction.Normalize();

            return new Ray(nearPoint, direction);

        }

        private static float? IntersectDistance(Actor actor, Ray ray)
        {

            var boundingSphere = new BoundingSphere(actor.mModelMatrix.Translation + actor.mMesh.mMeshData.mBoundingSphere.Center, actor.mMesh.mMeshData.mBoundingSphere.Radius);

            return ray.Intersects(boundingSphere);
        }

    }

}
