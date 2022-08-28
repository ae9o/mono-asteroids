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

public static class RandomUtils
{
    public static readonly Random Random = new Random();

    public static Edge NextEdge(this Random random)
    {
        return (Edge)random.Next((int)Edge.All);
    }

    public static Vector2 NextPositionOutsideWorld(this Random random, float worldWidth, float worldHeight)
    {
        Vector2 pos = new Vector2();

        switch (random.NextEdge())
        {
            case Edge.Left:
                pos.X = 0;
                pos.Y = worldHeight * random.NextSingle();
                break;

            case Edge.Top:
                pos.X = worldWidth * random.NextSingle();
                pos.Y = 0;
                break;

            case Edge.Right:
                pos.X = worldWidth;
                pos.Y = worldHeight * random.NextSingle();
                break;

            case Edge.Bottom:
            default:
                pos.X = worldWidth * random.NextSingle();
                pos.Y = worldHeight;
                break;
        }

        return pos;
    }

    public static Vector2 NextVector(this Random random, float min, float max)
    {
        random.NextUnitVector(out var vector);
        return random.NextSingle(min, max) * vector;
    }
}