using Asteroids.Actors;
using Common;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Asteroids.Systems {
    public class MovementSystem : ActorSystem<IMoveableActor>, IActorSystem {

        public void Update() {
            var position = new NativeArray<Vector3>(Actors.Count, Allocator.TempJob);
            var direction = new NativeArray<Vector3>(Actors.Count, Allocator.TempJob);
            var speed = new NativeArray<float>(Actors.Count, Allocator.TempJob);
            var delta = Time.deltaTime;

            Actors.For((i, actor) => {
                position[i] = actor.Position.Value;
                direction[i] = actor.Direction.Value;
                speed[i] = actor.Speed.Value;
            });
            
            new MovementJob() {
                    Position = position,
                    Direction = direction,
                    Speed = speed,
                    Delta = delta,
                }.Schedule(Actors.Count, 8)
                .Complete();
            
            Actors.For((i, actor) => {
                actor.Position.Value = position[i];
                actor.Direction.Value = direction[i];
                actor.Speed.Value = speed[i];
            });

            position.Dispose();
            direction.Dispose();
            speed.Dispose();
        }

        private struct MovementJob : IJobParallelFor {
            
            public NativeArray<Vector3> Position;
            
            [ReadOnly]
            public NativeArray<Vector3> Direction;
            
            [ReadOnly]
            public NativeArray<float> Speed;

            [ReadOnly]
            public float Delta;
            
            public void Execute(int index) {
                Position[index] += Quaternion.Euler(Direction[index]) * Vector3.up * (Delta * Speed[index]);
            }
        }
    }
}
