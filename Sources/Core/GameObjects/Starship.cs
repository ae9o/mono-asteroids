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

public class Starship : GameObject
{
    private readonly Body _body;
    private readonly Fixture _fixture;
    private readonly EnemyCollisionHandler _enemyCollisionHandler = new EnemyCollisionHandler();

    public Body Body => _body;
    public float EngageImpulse { get; set; }
    public float TurnImpulse { get; set; }

    public override Vector2 Position
    { 
        get => _body.Position;
        set => _body.Position = value;
    }

    public Starship(World world)
    {        
        _body = world.CreateBody();
        _body.BodyType = BodyType.Dynamic;
        _body.IgnoreGravity = true;       
        _body.LinearDamping = 1;
        _body.AngularDamping = 10;
        
        _fixture = _body.CreateCircle(1, 1);
        _fixture.Tag = this;
        _fixture.OnCollision += OnCollision;

        Size = new Vector2(100, 100);
        EngageImpulse = 10;
        TurnImpulse = 10;
    }
    
    public void Engage()
    {
        var direction = Vector2.Transform(Vector2.UnitY, Matrix.CreateRotationZ(_body.Rotation));
        _body.ApplyLinearImpulse(direction * EngageImpulse);
    }

    public void TurnLeft()
    {
        _body.ApplyAngularImpulse(-TurnImpulse);
    }

    public void TurnRight()
    {
        _body.ApplyAngularImpulse(TurnImpulse);
    }

    private bool OnCollision(Fixture sender, Fixture other, Contact contact)
    {
        ((GameObject)other.Tag).Visit(_enemyCollisionHandler);
        return true;
    }

    private class EnemyCollisionHandler : GameObjectsVisitorAdapter
    {
        public override void Visit(Asteroid asteroid)
        {
        }

        public override void Visit(Ufo ufo)
        {
        }
    }

    public override void Visit(IGameObjectsVisitor visitor)
    {
        visitor.Visit(this);
    }
}
