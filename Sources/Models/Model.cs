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
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoAsteroids;

public class Model : GameComponent
{
    public const float WORLD_WIDTH = 1.66f;
    public const float WORLD_HEIGHT = 1f;

    private readonly List<GameObject> _gameObjects = new List<GameObject>();
    private World _world;
    private Starship _starship;

    public Model(Game game) : base(game)
    {
    }

    public Starship Starship => _starship;

    public void Add(GameObject obj)
    {
        _gameObjects.Add(obj);
        _world.Add(obj);
    }

    public void Remove(GameObject obj)
    {
        _gameObjects.Remove(obj);
        _world.Remove(obj);
    }

    public override void Initialize()
    {
        _world = new World();
        _world.Gravity = Vector2.Zero;

        base.Initialize();

        StartRound();
    }

    public void StartRound()
    {
        _starship = GameObjectFactory.NewStarship();
        _starship.Position = new Vector2(WORLD_WIDTH * 0.5f, WORLD_HEIGHT * 0.5f);
        Add(_starship);

        var random = new Random();
        for (int i = 0; i < 5; ++i)
        {
            Asteroid asteroid = GameObjectFactory.NewLargeAsteroid();
            asteroid.Position = new Vector2(random.NextSingle(), random.NextSingle());
            asteroid.LinearVelocity = new Vector2(random.Next(-1, 1), random.Next(-1, 1));
            Add(asteroid);
        }
    }

    public void FinishRound()
    {
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var obj in _gameObjects)
        {
            obj.Update(gameTime);
            ClipToWorld(obj);
        }

        _world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    private void ClipToWorld(GameObject obj)
    {
        Vector2 halfSize = obj.Size * 0.5f;
        Vector2 pos = obj.Position;

        if (pos.X < -halfSize.X)
        {
            pos.X = WORLD_WIDTH + halfSize.X;
        }
        else if (pos.X > WORLD_WIDTH + halfSize.X)
        {
            pos.X = -halfSize.X;
        }

        if (pos.Y < -halfSize.Y)
        {
            pos.Y = WORLD_HEIGHT + halfSize.Y;
        }
        else if (pos.Y > WORLD_HEIGHT + halfSize.Y)
        {
            pos.Y = -halfSize.Y;
        }

        obj.Position = pos;
    }

    public void Visit(IGameObjectsVisitor visitor)
    {
        foreach (var obj in _gameObjects)
        {
            obj.Visit(visitor);
        }
    }
}