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

public partial class View : DrawableGameComponent, IGameObjectsVisitor
{
    private readonly Model _model;
    private Matrix _viewportScaleMatrix;
    private SpriteBatch _spriteBatch;

    public View(Game game, Model Model) : base(game)
    {
        _model = Model;
    }

    public override void Initialize()
    {
        var viewport = Game.GraphicsDevice.Viewport;
        var scaleX = viewport.Width / Model.WORLD_WIDTH;
        var scaleY = viewport.Height / Model.WORLD_HEIGHT;
        _viewportScaleMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

        LoadStarship();
        LoadAsteroid();
        LoadUfo();
    }

    protected override void UnloadContent()
    {
        UnloadStarship();
        UnloadAsteroid();
        UnloadUfo();

        _spriteBatch.Dispose();
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(transformMatrix: _viewportScaleMatrix);
        _model.Visit(this);
        _spriteBatch.End();
    }
}