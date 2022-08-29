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
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace MonoAsteroids;

public class Spacecraft : PoolableGameObject
{
    public float EngageImpulse { get; set; } = 0.001f;

    public float RotationSpeed { get; set; } = 5f;

    public Func<GameObject> BlowSupplier { get; set; }

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

    protected override bool OnCollisionValidating(Fixture sender, Fixture other, Contact contact)
    {
        Blow();

        return base.OnCollisionValidating(sender, other, contact);
    }

    protected virtual void Blow()
    {
        if (BlowSupplier != null)
        {
            var blow = BlowSupplier();
            blow.Position = Position;
            blow.Size = Size;
            Model.Add(blow);
        }

        Remove();
    }

    public override void Reset() {}
}