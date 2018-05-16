using System;
using Microsoft.Xna.Framework;

namespace LevelEditor.Engine
{
    /// <summary>
    /// Used to represent a camera in the engine.
    /// </summary>
    internal sealed class Camera
    {

        public Vector3 mLocation;
        public Vector2 mRotation;

        public float mFieldOfView;
        public float mAspectRatio;
        public float mNearPlane;
        public float mFarPlane;

        public Matrix mViewMatrix;
        public Matrix mProjectionMatrix;

        public Vector3 Direction { get; private set; }
        public Vector3 Up { get; private set; }

        public Vector3 Right { get; private set; }

        /// <summary>
        /// Constructs a <see cref="Camera"/>.
        /// </summary>
        /// <param name="location">The location of the camera.</param>
        /// <param name="rotation">The rotation of the camera, where .X is the horizontal and .Y the vertical rotation.</param>
        /// <param name="fieldOfView">The field of view in degrees.</param>
        /// <param name="aspectRatio">The ratio of the image width to the image height.</param>
        /// <param name="nearPlane">The plane where the camera starts to render.</param>
        /// <param name="farPlane">The plane where the camera stops to render.</param>
        public Camera(Vector3 location = default(Vector3),
            Vector2 rotation = default(Vector2),
            float fieldOfView = 45.0f,
            float aspectRatio = 2.0f,
            float nearPlane = 1.0f,
            float farPlane = 100.0f)
        {
            mViewMatrix = new Matrix();
            mProjectionMatrix = new Matrix();

            mLocation = location;
            mRotation = rotation;

            mFieldOfView = fieldOfView;
            mAspectRatio = aspectRatio;

            mNearPlane = nearPlane;
            mFarPlane = farPlane;
        }

        /// <summary>
        /// Calculates the view matrix based on the location and rotation of the camera.
        /// </summary>
        public void UpdateView()
        {
            // TODO: We need to change this to a 3rd person camera
            Direction = -Vector3.Normalize(new Vector3((float) (Math.Cos(mRotation.Y) * Math.Sin(mRotation.X)),
                (float) Math.Sin(mRotation.Y),
                (float) (Math.Cos(mRotation.Y) * Math.Cos(mRotation.X))));

            Right = -Vector3.Normalize(new Vector3((float) Math.Sin(mRotation.X - 3.14 / 2.0), 0.0f, (float) Math.Cos(mRotation.X - 3.14 / 2.0)));

            Up = Vector3.Cross(Right, Direction);

            mViewMatrix = Matrix.CreateLookAt(mLocation, mLocation + Direction, Up);
        }

        /// <summary>
        /// Calculates the perspective matrix based on the FoV, the aspect ratio and the near and far plane.
        /// </summary>
        public void UpdatePerspective()
        {
            mProjectionMatrix =
                Matrix.CreatePerspectiveFieldOfView(mFieldOfView / 180.0f * (float)Math.PI, mAspectRatio, mNearPlane, mFarPlane);
        }
    }
}