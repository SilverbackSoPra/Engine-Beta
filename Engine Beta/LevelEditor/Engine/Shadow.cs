using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor.Engine
{
    class Shadow
    {

        public float mDistance;
        public float mBias;

        public Matrix mViewMatrix;
        public Matrix mProjectionMatrix;

        public Shadow()
        {

            mDistance = 100.0f;
            mBias = 0.001f;

            mProjectionMatrix = Matrix.CreateOrthographicOffCenter(-50.0f, 50.0f, -50.0f, 50.0f, -100.0f, 100.0f);
            mViewMatrix = Matrix.Identity;

        }

        public void Update(Light light, Vector3 location)
        {

            Vector3 direction = Vector3.Normalize(-light.mLocation);

            mViewMatrix = Matrix.CreateLookAt(location, location + direction, Vector3.Up);

        }

    }
}
