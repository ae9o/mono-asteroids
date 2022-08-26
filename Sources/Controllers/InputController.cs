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
using Microsoft.Xna.Framework.Input;

namespace MonoAsteroids;

public class InputController : GameComponent
{
    private readonly Model _model;

    public InputController(Game game, Model model) : base(game)
    {
        _model = model;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        var keyboard = Keyboard.GetState();
        var mouse = Mouse.GetState();

        if (keyboard.IsKeyDown(Keys.Escape))
        {
            Game.Exit();
        }

        if (_model.State != ModelState.RoundStarted)
        {
            if (keyboard.IsKeyDown(Keys.Space))
            {
                _model.StartRound();
            }
            return;
        }

        if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
        {
            _model.Starship.Engage();
        }

        if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
        {
            _model.Starship.RotateLeft((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
        {
            _model.Starship.RotateRight((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        if (mouse.LeftButton == ButtonState.Pressed)
        {
            _model.Starship.FireBullet();
        }

        if (mouse.RightButton == ButtonState.Pressed)
        {
            _model.Starship.FireLaser();
        }
    }
}
