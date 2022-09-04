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
using Microsoft.Xna.Framework.Input;

namespace MonoAsteroids;

/// <summary>
///
/// <para>This class accepts and interprets user input. The corresponding model layer method is called after
/// successful interpretation.</para>
///
/// <para>Input handlers are collected in a dictionary, that allows to ease binding keyboard keys to different
/// model actions.</para>
///
/// </summary>
public class InputController : GameComponent
{
    public const Keys ButtonToStartRound = Keys.Space;

    private readonly Dictionary<Keys, Action<GameTime>> _keyboardHandlers = new Dictionary<Keys, Action<GameTime>>();
    private Action<GameTime> _leftButtonMouseHandler;
    private Action<GameTime> _rightButtonMouseHandler;

    public InputController(Game game)
        : base(game)
    {
    }

    public override void Initialize()
    {
        base.Initialize();

        _leftButtonMouseHandler = FireBullet;
        _rightButtonMouseHandler = FireLaser;

        _keyboardHandlers.Add(Keys.W, EngageStarship);
        _keyboardHandlers.Add(Keys.Up, EngageStarship);

        _keyboardHandlers.Add(Keys.A, RotateStarshipLeft);
        _keyboardHandlers.Add(Keys.Left, RotateStarshipLeft);

        _keyboardHandlers.Add(Keys.D, RotateStarshipRight);
        _keyboardHandlers.Add(Keys.Right, RotateStarshipRight);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        var keyboard = Keyboard.GetState();
        var mouse = Mouse.GetState();

        if (Model.Instance.State != ModelState.RoundStarted)
        {
            if (keyboard.IsKeyDown(ButtonToStartRound))
            {
                Model.Instance.StartRound();
            }
            return;
        }

        ProcessKeyboard(keyboard, gameTime);
        ProcessMouse(mouse, gameTime);
    }

    private void ProcessKeyboard(KeyboardState keyboard, GameTime gameTime)
    {
        foreach (var key in keyboard.GetPressedKeys())
        {
            if (_keyboardHandlers.TryGetValue(key, out var handler))
            {
                handler(gameTime);
            }
        }
    }

    private void ProcessMouse(MouseState mouse, GameTime gameTime)
    {
        if (mouse.LeftButton == ButtonState.Pressed)
        {
            _leftButtonMouseHandler?.Invoke(gameTime);
        }

        if (mouse.RightButton == ButtonState.Pressed)
        {
            _rightButtonMouseHandler?.Invoke(gameTime);
        }
    }

    public void EngageStarship(GameTime gameTime)
    {
        Model.Instance.Starship.Engage();
    }

    public void RotateStarshipLeft(GameTime gameTime)
    {
        Model.Instance.Starship.RotateLeft((float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    public void RotateStarshipRight(GameTime gameTime)
    {
        Model.Instance.Starship.RotateRight((float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    public void FireBullet(GameTime gameTime)
    {
        Model.Instance.Starship.MachineGun.Fire();
    }

    public void FireLaser(GameTime gameTime)
    {
        Model.Instance.Starship.LaserGun.Fire();
    }
}