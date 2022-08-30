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

/// <summary>
/// This part of the model contains the key logic of the game. Starts a new game round. Manages the calculation of score
/// points. Tracks the destruction of the player's spaceship.
/// </summary>
public partial class Model : GameComponent, IEnumerable<GameObject>
{
    private ModelState _state;
    private Starship _starship;
    private int _score;

    public Model(Game game)
        : base(game)
    {
    }

    public ModelState State => _state;

    public int Score => _score;

    public Starship Starship => _starship;

    public override void Initialize()
    {
        base.Initialize();

        _world = new World();
        _world.Gravity = Vector2.Zero;

        _starship = GameObjectFactory.NewDemoStarship();
        Add(_starship);

        InitializeTimers();
        SpawnInitialAsteroids();
    }

    public override void Update(GameTime gameTime)
    {
        Lock();
        UpdateTimers(gameTime);

        foreach (var obj in _gameObjects)
        {
            obj.Update(gameTime);
        }

        _world.Step(gameTime.ElapsedGameTime);
        Unlock();
    }

    public void StartRound()
    {
        if (_state == ModelState.RoundStarted)
        {
            throw new InvalidOperationException("Round has already been started.");
        }

        _state = ModelState.RoundStarted;
        _score = 0;

        Clear();

        _starship = GameObjectFactory.NewDefaultStarship();
        _starship.Position = WorldCenter;
        _starship.BlastSupplier = ObtainBlast;
        _starship.Broken += OnStarshipBroken;
        Add(_starship);

        RestartTimers();
        SpawnInitialAsteroids();
    }

    public void FinishRound()
    {
        if (_state != ModelState.RoundStarted)
        {
            return;
        }

        _state = ModelState.RoundFinished;

        StopTimers();
    }

    private void OnStarshipBroken(GameObject sender, EventArgs e)
    {
        FinishRound();
    }

    private void OnScorableObjectBroken(GameObject sender, EventArgs e)
    {
        _score += sender.ScorePoints;
    }
}