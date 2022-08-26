/*
 * Copyright (C) 2022 Alexei Evdokimenko
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using MonoGame.Extended;
using MonoGame.Extended.Collections;
using tainicom.Aether.Physics2D.Common;

namespace MonoAsteroids;

public class Asteroid : GameObject, IBreakable
{
    public Bag<GameObject> Shards { get; } = new Bag<GameObject>();

    public float MinShardAngularVelocity { get; set; } = -0.5f;

    public float MaxShardAngularVelocity { get; set; } = 0.5f;

    public float MinShardAngularOffset { get; set; } = -0.75f;

    public float MaxShardAngularOffset { get; set; } = 0.75f;

    public float ShardAcceleration { get; set; } = 1.25f;

    private void ScatterShards()
    {
        foreach (var shard in Shards)
        {
            var angularOffset = Complex.FromAngle(Utils.Random.NextSingle(MinShardAngularOffset, MaxShardAngularOffset));
            shard.LinearVelocity = Complex.Multiply(LinearVelocity, ref angularOffset) * ShardAcceleration;
            shard.AngularVelocity = Utils.Random.NextSingle(MinShardAngularVelocity, MaxShardAngularVelocity);
            shard.Position = Position;
            Model.Add(shard);
        }
        Shards.Clear();
    }

    public virtual void Break(GameObject sender)
    {
        ScatterShards();
        Remove();
    }
}