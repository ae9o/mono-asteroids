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
using MonoGame.Extended;

namespace MonoAsteroids;

public static class Utils
{
    public static readonly Random Random = new Random();

    public enum Edge
    {
        Left,
        Top,
        Right,
        Bottom,
        All
    }

    public static Edge GetRandomEdge()
    {
        return (Edge)Random.Next((int)Edge.All);
    }

    public static Vector2 GetRandomPositionOutsideWorld(float worldWidth, float worldHeight)
    {
        Vector2 pos = new Vector2();

        switch (GetRandomEdge())
        {
            case Edge.Left:
                pos.X = 0;
                pos.Y = worldHeight * Random.NextSingle();
                break;

            case Edge.Top:
                pos.X = worldWidth * Random.NextSingle();
                pos.Y = 0;
                break;

            case Edge.Right:
                pos.X = worldWidth;
                pos.Y = worldHeight * Random.NextSingle();
                break;

            case Edge.Bottom:
            default:
                pos.X = worldWidth * Random.NextSingle();
                pos.Y = worldHeight;
                break;
        }

        return pos;
    }

    public static Vector2 GetRandomVector(float minLength, float maxLength)
    {
        Random.NextUnitVector(out var vector);
        return vector * Random.NextSingle(minLength, maxLength);
    }
}