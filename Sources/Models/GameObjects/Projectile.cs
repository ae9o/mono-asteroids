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

public abstract class Projectile : GameObject
{
    public float Speed { get; set; }

    public bool RemoveSelfOnCollision { get; set; }

    public virtual void Fire(Vector2 direction)
    {
        LinearVelocity = direction * Speed;
    }

    public override void OnFlewOutOfWorld()
    {
        base.OnFlewOutOfWorld();
        Remove();
    }

    public override void OnAdded()
    {
        base.OnAdded();
        OnCollision += OnProjectileCollision;
    }

    public bool OnProjectileCollision(Fixture sender, Fixture other, Contact contact)
    {
        ((GameObject)other.Body).Remove();

        if (RemoveSelfOnCollision)
        {
            Remove();
        }

        return true;
    }
}