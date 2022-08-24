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

public class Starship : GameObject, IGameObjectsVisitor
{
    private static readonly Vector2 UP_DIRECTION = -Vector2.UnitY;

    public Starship()
    {
        BodyType = BodyType.Dynamic;
        OnCollision += HandleCollision;
    }

    public float EngageImpulse { get; set; }

    public float RotationSpeed { get; set; }

    public void Engage()
    {
        ApplyLinearImpulse(GetWorldVector(UP_DIRECTION) * EngageImpulse);
    }

    public void RotateLeft(float delta)
    {
        Rotation -= RotationSpeed * delta;
    }

    public void RotateRight(float delta)
    {
        Rotation += RotationSpeed * delta;
    }

    private bool HandleCollision(Fixture sender, Fixture other, Contact contact)
    {
        ((GameObject)other.Body).Visit(this);
        return true;
    }

    public void Visit(Asteroid asteroid)
    {
        System.Console.WriteLine("Collide");
    }

    public void Visit(Ufo ufo)
    {
    }

    public void Visit(Starship starship)
    {
    }

    public override void Visit(IGameObjectsVisitor visitor)
    {
        visitor.Visit(this);
    }
}