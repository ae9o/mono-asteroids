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

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;
using MonoGame.Extended.Collections;

namespace MonoAsteroids;

/// <summary>
/// This part of the model controls the addition and removal of objects in the game world.
/// </summary>
public partial class Model : GameComponent, IEnumerable<GameObject>
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

    public void Clear()
    {
        foreach (var obj in _gameObjects)
        {
            obj.Model = null;
        }
        _gameObjects.Clear();
        _world.Clear();

        _starship = null;
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