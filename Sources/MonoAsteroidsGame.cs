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

namespace MonoAsteroids;

/// <summary>
/// The root class for the MonoGame framework, intended to configure and run the game.
/// </summary>
public class MonoAsteroidsGame : Game
{
    private const int DefaultBackBufferWidth = 1280;
    private const int DefaultBackBufferHeight = 720;

    private readonly GraphicsDeviceManager _graphics;

    public MonoAsteroidsGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = DefaultBackBufferWidth;
        _graphics.PreferredBackBufferHeight = DefaultBackBufferHeight;
        _graphics.ApplyChanges();

        var model = new Model(this);
        var view = new View(this, model);
        var inputController = new InputController(this, model);

        Components.Add(model);
        Components.Add(view);
        Components.Add(inputController);

        base.Initialize();
    }
}