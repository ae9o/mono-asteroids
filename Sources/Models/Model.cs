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
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using MonoGame.Extended;
using MonoGame.Extended.Timers;
using MonoGame.Extended.Collections;

namespace MonoAsteroids;

public class Model : GameComponent, IEnumerable<GameObject>
{
    public const float WorldWidth = 1.66f;
    public const float WorldHeight = 1f;

    public static readonly Vector2 WorldCenter = new Vector2(WorldWidth * 0.5f, WorldHeight * 0.5f);
    public static readonly Vector2 WorldUpDirection = -Vector2.UnitY;

    private readonly Bag<GameObject> _gameObjects = new Bag<GameObject>();
    private readonly Bag<GameObject> _addedGameObjects = new Bag<GameObject>();
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

        _asteroidSpawnClock = new ContinuousClock(10.0);
        _asteroidSpawnClock.Tick += OnAsteroidSpawnClockTick;

        StartRound();
    }

    public void StartRound()
    {
        _starship = GameObjectFactory.NewDefaultStarship();
        _starship.Position = WorldCenter;
        _starship.OnCollision += OnStarshipCollision;
        Add(_starship);

        _asteroidSpawnClock.Restart();

        SpawnLargeAsteroid();
    }

    public void FinishRound()
    {
        _asteroidSpawnClock.Stop();
    }

    public void RestartRound()
    {
        FinishRound();
        StartRound();
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
        var smallShard1 = GameObjectFactory.NewSmallAsteroid();
        var smallShard2 = GameObjectFactory.NewSmallAsteroid();
        var smallShard3 = GameObjectFactory.NewSmallAsteroid();
        var smallShard4 = GameObjectFactory.NewSmallAsteroid();

        var mediumShard1 = GameObjectFactory.NewMediumAsteroid();
        var mediumShard2 = GameObjectFactory.NewMediumAsteroid();

        mediumShard1.Shards.Add(smallShard1);
        mediumShard1.Shards.Add(smallShard2);

        mediumShard2.Shards.Add(smallShard3);
        mediumShard2.Shards.Add(smallShard4);

        var asteroid = GameObjectFactory.NewLargeAsteroid();
        asteroid.Position = Utils.Random.NextPositionOutsideWorld(WorldWidth, WorldHeight);
        asteroid.LinearVelocity = Utils.Random.NextVector(0.1f, 0.5f);
        asteroid.AngularVelocity = Utils.Random.NextSingle(-0.5f, 0.5f);
        asteroid.Shards.Add(mediumShard1);
        asteroid.Shards.Add(mediumShard2);

        Add(asteroid);
    }

    public override void Update(GameTime gameTime)
    {
        Lock();
        _asteroidSpawnClock.Update(gameTime);

        foreach (var obj in _gameObjects)
        {
            obj.Update(gameTime);
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

        foreach (var obj in _addedGameObjects)
        {
            Add(obj);
        }
        _addedGameObjects.Clear();
    }

    public void Add(GameObject obj)
    {
        if (obj.Model != null)
        {
            return;
        }

        if (_locked)
        {
            _addedGameObjects.Add(obj);
        }
        else
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

    IEnumerator<GameObject> IEnumerable<GameObject>.GetEnumerator()
    {
        return ((IEnumerable<GameObject>)_gameObjects).GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return ((System.Collections.IEnumerable)_gameObjects).GetEnumerator();
    }
}