using System.Collections.Generic;

namespace LevelEditor.Engine.Mesh
{

    /// <summary>
    /// 
    /// </summary>
    internal sealed class ActorBatch
    {
        public readonly List<Actor> mActors;

        public readonly Mesh mMesh;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mesh"></param>
        public ActorBatch(Mesh mesh)
        {
            mMesh = mesh;
            mActors = new List<Actor>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        public void Add(Actor actor)
        {
            mActors.Add(actor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public bool Remove(Actor actor)
        {
            return mActors.Remove(actor);
        }
    }
}