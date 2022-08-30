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

using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace MonoAsteroids;

/// <summary>
///
/// <para>The base class for the ammo fired by guns.</para>
///
/// <para>Objects of this class destroy on collision other game objects that implement the IBreakable interface.</para>
///
/// <para>Objects of this class are intended for auto pooling. When they are removed from the world, they are
/// automatically returned to the pool that spawned them.</para>
///
/// </summary>
public abstract class Projectile : PoolableGameObject
{
    public float Speed { get; set; }

    public bool RemoveSelfOnCollision { get; set; }

    public virtual void Fire(Vector2 direction)
    {
        LinearVelocity = direction * Speed;
    }

    protected override void OnFlewOutOfWorld()
    {
        base.OnFlewOutOfWorld();

        Remove();
    }

    protected override bool OnCollisionValidating(Fixture sender, Fixture other, Contact contact)
    {
        if (other.Body is IBreakable)
        {
            ((IBreakable)other.Body).Break(this);
        }

        if (RemoveSelfOnCollision)
        {
            Remove();
        }

        return base.OnCollisionValidating(sender, other, contact);
    }

    protected override void OnRemoved()
    {
        base.OnRemoved();

        ReturnToPool();
    }

    public override void Reset() {}
}