using System.Collections.Generic;
using LevelEditor.Engine.Mesh;

namespace LevelEditor.Engine
{
    /// <summary>
    /// The Scene class is used to represent a scene. This is useful
    /// if we want many actors in one place e.g for a level.
    /// </summary>
    internal sealed class Scene
    {

        public readonly List<ActorBatch> mActorBatches;
        public readonly List<Light> mLights;
        public readonly PostProcessing mPostProcessing;

        /// <summary>
        /// Constructs a <see cref="Scene"/>.
        /// </summary>
        public Scene()
        {

            mActorBatches = new List<ActorBatch>();
            mLights = new List<Light>();

            mPostProcessing = new PostProcessing();

        }

        /// <summary>
        /// Adds an actor to the scene.
        /// </summary>
        /// <param name="actor"> The actor which you want to add to the scene</param>
        public void Add(Actor actor)
        {

            // Search for the ActorBatch
            var actorBatch = mActorBatches.Find(ele => ele.mMesh == actor.mMesh);

            // If there is no ActorBatch which already uses the mesh of the Actor we
            // need to create a new ActorBatch and add it to mActorBatches
            if (actorBatch == null)
            {
                actorBatch = new ActorBatch(actor.mMesh);
                mActorBatches.Add(actorBatch);
            }

            actorBatch.Add(actor);

        }

        /// <summary>
        /// Adds an actor to the scene.
        /// </summary>
        /// <param name="interfaceActor">The object which you want to add to the scene</param>
        public void Add(IActor interfaceActor)
        {
            var actor = interfaceActor.MeshActor;

            // Search for the ActorBatch
            var actorBatch = mActorBatches.Find(ele => ele.mMesh == actor.mMesh);

            // If there is no ActorBatch which already uses the mesh of the Actor we
            // need to create a new ActorBatch and add it to mActorBatches
            if (actorBatch == null)
            {
                actorBatch = new ActorBatch(actor.mMesh);
                mActorBatches.Add(actorBatch);
            }

            actorBatch.Add(actor);
        }


        /// <summary>
        /// Removes an actor from the scene.
        /// </summary>
        /// <param name="actor">The actor which you want to remove from the scene.</param>
        /// <returns>A boolean whether the actor was found in the scene.
        /// If there is no ActorBatch existing the return value will be null</returns>
        public bool? Remove(Actor actor)
        {

            var actorBatch = mActorBatches.Find(ele => ele.mMesh == actor.mMesh);

            return actorBatch?.Remove(actor);

        }

        /// <summary>
        /// Removes an actor from the scene.
        /// </summary>
        /// <param name="interfaceActor">The object which you want remove from the scene.</param>
        /// <returns>A boolean whether the object wasn't found in the scene.
        /// If there is no ActorBatch existing the return value will be null</returns>
        public bool? Remove(IActor interfaceActor)
        {
            var actor = interfaceActor.MeshActor;

            var actorBatch = mActorBatches.Find(ele => ele.mMesh == actor.mMesh);

            return actorBatch?.Remove(actor);

        }


        /// <summary>
        /// Adds a light to the scene.
        /// </summary>
        /// <param name="light">The light which you want to add to the scene</param>
        public void Add(Light light)
        {

           mLights.Add(light);

        }

        /// <summary>
        /// Removes a light from the scene.
        /// </summary>
        /// <param name="light"></param>
        /// <returns>A boolean whether the light was found in the scene.</returns>
        public bool Remove(Light light)
        {

            return mLights.Remove(light);

        }

    }
}