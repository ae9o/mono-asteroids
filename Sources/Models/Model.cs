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
using MonoGame.Extended.Timers;
using MonoGame.Extended.Collections;

namespace MonoAsteroids;

public class Model : GameComponent
{
    public const float WorldWidth = 1.66f;
    public const float WorldHeight = 1f;

    public static readonly Vector2 WorldCenter = new Vector2(WorldWidth * 0.5f, WorldHeight * 0.5f);
    public static readonly Vector2 WorldUpDirection = -Vector2.UnitY;

    private readonly Bag<GameObject> _gameObjects = new Bag<GameObject>();
    private readonly Bag<GameObject> _removedGameObjects = new Bag<GameObject>();

    private bool _locked;

    private World _world;
    private Starship _starship;
    private ContinuousClock _asteroidSpawnClock;

    public Model(Game game) : base(game)
    {
    }

    public Starship Starship => _starship;

    public override void Initialize()
    {
        base.Initialize();

        _world = new World();
        _world.Gravity = Vector2.Zero;

        _asteroidSpawnClock = new ContinuousClock(5.0);
        _asteroidSpawnClock.Tick += OnAsteroidSpawnClockTick;

        StartRound();
    }

    public void StartRound()
    {
        _starship = GameObjectFactory.NewDefaultStarship();
        _starship.Position = WorldCenter;
        _starship.OnCollision += OnStarshipCollision;
        Add(_starship);

        _asteroidSpawnClock.Start();

        SpawnLargeAsteroid();
    }

    public void FinishRound()
    {
        _asteroidSpawnClock.Stop();
    }

    private bool OnStarshipCollision(Fixture sender, Fixture other, Contact contact)
    {
        System.Console.WriteLine("Starship collision!");
        return true;
    }

    private void OnAsteroidSpawnClockTick(object sender, EventArgs e)
    {
        SpawnLargeAsteroid();
    }

    private void SpawnLargeAsteroid()
    {
        var asteroid = GameObjectFactory.NewLargeAsteroid();
        asteroid.Position = Utils.GetRandomPositionOutsideWorld(WorldWidth, WorldHeight);
        asteroid.LinearVelocity = Utils.GetRandomVector(0.1f, 0.5f);
        Add(asteroid);
    }

    public override void Update(GameTime gameTime)
    {
        Lock();
        _asteroidSpawnClock.Update(gameTime);

        foreach (var obj in _gameObjects)
        {
            obj.Update(gameTime);
            ClipToWorld(obj);
        }

        _world.Step(gameTime.ElapsedGameTime);
        Unlock();
    }

    private void Lock()
    {
        _locked = true;
    }

    private void Unlock()
    {
        _locked = false;

        foreach (var obj in _removedGameObjects)
        {
            Remove(obj);
        }
        _removedGameObjects.Clear();
    }

    private void ClipToWorld(GameObject obj)
    {
        Vector2 halfSize = obj.Size * 0.5f;
        Vector2 pos = obj.Position;
        Vector2 tmp = pos;

        if (pos.X < -halfSize.X)
        {
            tmp.X = WorldWidth + halfSize.X;
        }
        else if (pos.X > WorldWidth + halfSize.X)
        {
            tmp.X = -halfSize.X;
        }

        if (pos.Y < -halfSize.Y)
        {
            tmp.Y = WorldHeight + halfSize.Y;
        }
        else if (pos.Y > WorldHeight + halfSize.Y)
        {
            tmp.Y = -halfSize.Y;
        }

        if (tmp != pos)
        {
            obj.OnFlewOutOfWorld();
            obj.Position = tmp;
        }
    }

    public void Add(GameObject obj)
    {
        if (obj.Model == null)
        {
            _gameObjects.Add(obj);
            _world.Add(obj);
            obj.Model = this;
        }
    }

    public void Remove(GameObject obj)
    {
        if (obj.Model != this)
        {
            return;
        }

        if (_locked)
        {
            _removedGameObjects.Add(obj);
        }
        else
        {
            _gameObjects.Remove(obj);
            _world.Remove(obj);
            obj.Model = null;
        }
    }

    public void Visit(IGameObjectsVisitor visitor)
    {
        foreach (var obj in _gameObjects)
        {
            obj.Visit(visitor);
        }
    }
}