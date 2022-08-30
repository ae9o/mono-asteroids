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
using Microsoft.Xna.Framework.Graphics;

namespace MonoAsteroids;

/// <summary>
/// This part of the view is responsible for setting up the viewport and coordinating the process of rendering the game
/// world and user interface.
/// </summary>
public partial class View : DrawableGameComponent
{
    private readonly Model _model;
    private Matrix _viewportScaleMatrix;
    private SpriteBatch _spriteBatch;
    private Vector2 _viewportSize;
    private float _scaleX;
    private float _scaleY;

    public View(Game game, Model Model)
        : base(game)
    {
        _model = Model;
    }

    public override void Initialize()
    {
        base.Initialize();

        ScaleViewport();
    }

    private void ScaleViewport()
    {
        var viewport = Game.GraphicsDevice.Viewport;
        _viewportSize.X = viewport.Width;
        _viewportSize.Y = viewport.Height;

        _scaleX = viewport.Width / Model.WorldWidth;
        _scaleY = viewport.Height / Model.WorldHeight;
        _viewportScaleMatrix = Matrix.CreateScale(_scaleX, _scaleY, 1.0f);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

        LoadDrawables();
        LoadUi();
    }

    protected override void UnloadContent()
    {
        UnloadDrawables();
        UnloadUi();

        _spriteBatch.Dispose();
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(Color.Black);

        DrawWorld();
        DrawUi();
    }
}