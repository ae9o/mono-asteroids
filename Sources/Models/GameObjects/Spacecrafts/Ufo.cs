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

public class Ufo : Spacecraft
{
    public GameObject Target { get; set; }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        PursueTarget();
    }

    public void PursueTarget()
    {
        Engage(GetTargetDirection());
    }

    private Vector2 GetTargetDirection()
    {
        if (Target == null)
        {
            return Vector2.Zero;
        }

        var direction = Target.Position - Position;
        direction.Normalize();
        return direction;
    }

    public override void Reset()
    {
        base.Reset();

        Target = null;
    }
}