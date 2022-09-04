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
/// Implements the key logic of the game.
/// </summary>
public partial class Model : GameComponent
{
    // The app has only one model, which is used by different subsystems.
    // It is advisable to make it a singleton.
    #region Singleton
    private static readonly Model _instance = new Model(MonoAsteroidsGame.Instance);
    public static Model Instance => _instance;
    static Model() {}
    #endregion

    public const float AsteroidSpawnInterval = 10f;
    public const float UfoSpawnInterval = 5f;
    public const int InitialAsteroidCount = 2;

    // The same objects are constantly created and destroyed on the stage.
    // It is advisable to use pooling for them.
    private readonly Pool<Asteroid> _largeAsteroidPool = new Pool<Asteroid>(GameObjectFactory.NewLargeAsteroid);
    private readonly Pool<Asteroid> _mediumAsteroidPool = new Pool<Asteroid>(GameObjectFactory.NewMediumAsteroid);
    private readonly Pool<Asteroid> _smallAsteroidPool = new Pool<Asteroid>(GameObjectFactory.NewSmallAsteroid);
    private readonly Pool<Ufo> _ufoPool = new Pool<Ufo>(GameObjectFactory.NewDefaultUfo);

    private ModelState _state;
    private Stage _stage;
    private Timers _timers;
    private Starship _starship;
    private int _score;

    private Model(Game game)
        : base(game)
    {
    }

    public ModelState State => _state;

    public int Score => _score;

    public Stage Stage => _stage;

    public Starship Starship => _starship;

    public override void Initialize()
    {
        base.Initialize();

        _stage = new Stage();
        _timers = new Timers();

        _timers.Add(OnAsteroidTimerTick, AsteroidSpawnInterval);
        _timers.Add(OnUfoTimerTick, UfoSpawnInterval);

        ShowDemo();
    }

    /// <summary>
    /// When the app starts, displays the demo activity on the stage.
    /// </summary>
    private void ShowDemo()
    {
        _starship = GameObjectFactory.NewDemoStarship();
        _stage.Add(_starship);

        SpawnInitialAsteroids();
    }

    public override void Update(GameTime gameTime)
    {
        _timers.Update(gameTime);
        _stage.Update(gameTime);
    }

    public void StartRound()
    {
        if (_state == ModelState.RoundStarted)
        {
            throw new InvalidOperationException("Round has already been started.");
        }

        _state = ModelState.RoundStarted;
        _score = 0;

        _stage.Clear();

        _starship = GameObjectFactory.NewDefaultStarship();
        _starship.Position = Stage.StageCenter;
        _starship.Broken += OnStarshipBroken;
        _stage.Add(_starship);

        _timers.Restart();
        SpawnInitialAsteroids();
    }

    public void FinishRound()
    {
        if (_state != ModelState.RoundStarted)
        {
            return;
        }

        _state = ModelState.RoundFinished;
        _timers.Stop();
    }

    private void SpawnInitialAsteroids()
    {
        for (int i = 0; i < InitialAsteroidCount; ++i)
        {
            SpawnLargeAsteroid();
        }
    }

    private void SpawnLargeAsteroid()
    {
        _stage.Add(ObtainLargeAsteroid());
    }

    private void SpawnUfo()
    {
        _stage.Add(ObtainUfo());
    }

    private Asteroid ObtainLargeAsteroid()
    {
        var asteroid = _largeAsteroidPool.Obtain();
        asteroid.Position = RandomUtils.Random.NextPositionOutsideWorld(Stage.StageWidth, Stage.StageHeight,
                asteroid.Size);
        asteroid.ShardSupplier = ObtainMediumAsteroid;
        asteroid.Broken += OnScorableObjectBroken;
        return asteroid;
    }

    private Asteroid ObtainMediumAsteroid()
    {
        var asteroid = _mediumAsteroidPool.Obtain();
        asteroid.ShardSupplier = ObtainSmallAsteroid;
        asteroid.Broken += OnScorableObjectBroken;
        return asteroid;
    }

    private Asteroid ObtainSmallAsteroid()
    {
        var asteroid = _smallAsteroidPool.Obtain();
        asteroid.Broken += OnScorableObjectBroken;
        return asteroid;
    }

    private Ufo ObtainUfo()
    {
        var ufo = _ufoPool.Obtain();
        ufo.Position = RandomUtils.Random.NextPositionOutsideWorld(Stage.StageWidth, Stage.StageHeight, ufo.Size);
        ufo.Target = _starship;
        ufo.Broken += OnScorableObjectBroken;
        return ufo;
    }

    private void OnAsteroidTimerTick(object sender, EventArgs e)
    {
        SpawnLargeAsteroid();
    }

    private void OnUfoTimerTick(object sender, EventArgs e)
    {
        SpawnUfo();
    }

    private void OnStarshipBroken(GameObject sender, EventArgs e)
    {
        FinishRound();
    }

    private void OnScorableObjectBroken(GameObject sender, EventArgs e)
    {
        _score += ((IScorable)sender).ScorePoints;
    }
}