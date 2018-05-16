using LevelEditor.Engine;
using LevelEditor.Engine.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LevelEditor
{
    class CameraHandler
    {

        private readonly Camera mCamera;

        // These vectors are the actual location and rotation of the camera
        // We use these because we want to have a smooth camera movement
        private Vector2 mRotation;
        private Vector3 mLocation;

        private bool mLeftMouseButtonPressed;
        private Vector2 mLastMousePosition;

        private float mMouseSensibility;
        private float mMovementSpeed;
        private float mReactivity;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="movementSpeed"></param>
        /// <param name="mouseSensibility"></param>
        /// <param name="reactivity"></param>
        public CameraHandler(Camera camera, float movementSpeed, float mouseSensibility, float reactivity)
        {
            mCamera = camera;

            mRotation = mCamera.mRotation;
            mLocation = mCamera.mLocation;

            mMouseSensibility = mouseSensibility;
            mMovementSpeed = movementSpeed;
            mReactivity = reactivity;

        }

        public void Update(float deltatime)
        {

            float progress = MathExtension.Clamp(mReactivity * deltatime / 16.0f, 0.0f, 1.0f);

            // Update the mouse and keyboard
            UpdateMouse(deltatime);
            UpdateKeyboard(deltatime);

            // We mix the actual vectors with the camera vector to get a smoother movement
            mCamera.mRotation = MathExtension.Mix(mCamera.mRotation, mRotation, progress);
            mCamera.mLocation = MathExtension.Mix(mCamera.mLocation, mLocation, progress);           

        }

        private void UpdateMouse(float deltatime)
        {

            var mouseState = Mouse.GetState();

            var mousePosition = new Vector2(mouseState.X, mouseState.Y);

            if (mouseState.LeftButton == ButtonState.Pressed && !mLeftMouseButtonPressed)
            {

                mLastMousePosition = mousePosition;

                mLeftMouseButtonPressed = true;

            }
            else if (mouseState.LeftButton != ButtonState.Pressed && mLeftMouseButtonPressed)
            {
                mLeftMouseButtonPressed = false;
            }

            if (mLeftMouseButtonPressed)
            {

                mRotation +=
                    new Vector2(-(mLastMousePosition.X - mousePosition.X), mLastMousePosition.Y - mousePosition.Y) *
                    mMouseSensibility * 0.001f;

                mLastMousePosition = mousePosition;

            }

        }

        private void UpdateKeyboard(float deltatime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {

                mLocation += mCamera.Direction * deltatime / 1000.0f * mMovementSpeed;

            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {

                mLocation -= mCamera.Direction * deltatime / 1000.0f * mMovementSpeed;

            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {

                mLocation -= mCamera.Right * deltatime / 1000.0f * mMovementSpeed;

            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {

                mLocation += mCamera.Right * deltatime / 1000.0f * mMovementSpeed;

            }

        }

    }
}
