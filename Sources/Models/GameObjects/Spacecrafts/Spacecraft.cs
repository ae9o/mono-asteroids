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
using Microsoft.Xna.Framework;

namespace MonoAsteroids;

/// <summary>
///
/// <para>Base class for spacecrafts. Implements movement and rotation capabilities.</para>
///
/// <para>Objects of this class are intended for auto pooling. When they are removed from the world, they are
/// automatically returned to the pool that spawned them.</para>
///
/// </summary>
public class Spacecraft : PoolableGameObject, IBreakable
{
    public event GameEventHandler Broken;

    public float EngageImpulse { get; set; } = 0.001f;

    public float RotationSpeed { get; set; } = 5f;

    public Func<GameObject> BlastSupplier { get; set; }

    public void Engage()
    {
        Engage(LookDirection);
    }

    public void Engage(Vector2 direction)
    {
        ApplyLinearImpulse(direction * EngageImpulse);
    }

    public void RotateLeft(float delta)
    {
        Rotation -= RotationSpeed * delta;
    }

    public void RotateRight(float delta)
    {
        Rotation += RotationSpeed * delta;
    }

    public virtual void Break(GameObject sender)
    {
        if (BlastSupplier != null)
        {
            var blast = BlastSupplier();
            blast.Position = Position;
            blast.Size = Size;
            Model.Add(blast);
        }

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