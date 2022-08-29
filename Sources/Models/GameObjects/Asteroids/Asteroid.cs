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

using System;
using MonoGame.Extended;
using tainicom.Aether.Physics2D.Common;

namespace MonoAsteroids;

public class Asteroid : PoolableGameObject, IBreakable
{
    public event GameEventHandler Broken;

    public Func<Asteroid> ShardSupplier { get; set; }

    public int MinShardCount { get; set; } = 2;

    public int MaxShardCount { get; set; } = 4;

    public float MinShardAngularVelocity { get; set; } = -0.5f;

    public float MaxShardAngularVelocity { get; set; } = 0.5f;

    public float MinShardAngularOffset { get; set; } = -0.75f;

    public float MaxShardAngularOffset { get; set; } = 0.75f;

    public float ShardAcceleration { get; set; } = 1.25f;

    private void ScatterShards()
    {
        if (ShardSupplier == null)
        {
            return;
        }

        var scatterDistance = Size.X * 0.5f;
        for (int i = 0, n = RandomUtils.Random.Next(MinShardCount, MaxShardCount + 1); i < n; ++i)
        {
            var angularOffset = Complex.FromAngle(RandomUtils.Random.NextSingle(MinShardAngularOffset,
                    MaxShardAngularOffset));

            var shard = ShardSupplier();
            shard.LinearVelocity = Complex.Multiply(LinearVelocity, ref angularOffset) * ShardAcceleration;
            shard.AngularVelocity = RandomUtils.Random.NextSingle(MinShardAngularVelocity, MaxShardAngularVelocity);
            shard.Position = Position + RandomUtils.Random.NextVector(0, scatterDistance);
            Model.Add(shard);
        }
    }

    public virtual void Break(GameObject sender)
    {
        ScatterShards();
        OnBroken();
        Remove();
    }

    protected virtual void OnBroken()
    {
        Broken?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnRemoved()
    {
        base.OnRemoved();

        ReturnToPool();
    }

    public override void Reset()
    {
        Broken = null;
    }
}