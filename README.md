![logo](logo.gif)

[![Latest release](https://img.shields.io/github/v/release/ae9o/mono-asteroids?display_name=tag)](https://github.com/ae9o/mono-asteroids/releases)

# MonoAsteroids game

This app is an analogue of the well-known game in which you have to shoot at asteroids and dodge UFOs while flying on a
starship. The development of this game was started as part of a test task, and its code is distributed under the Apache
License 2.0. Feel free to use it for any purpose.

## Implementation details

### Test task conditions

As stated above, the player flies and shoots everything.

The spaceship can only accelerate forward and also rotate left and right. Another important detail: the screen borders
do not impede movement, but work like portals that move game objects to the opposite side of the game field.

Anything that touches the spaceship destroys it. And UFOs are chasing the player.

There are two types of weapons to counter it:

- Bullets that shatter asteroids into smaller fragments with greater speed.
- A laser that destroys everything in its path. The number of laser shots is limited. This ability has a cooldown.
Shots recover over time.

In such conditions it is necessary to score as many points as possible.

All of the previous restrictions have been in terms of game design, but there is also an important engineering
restriction: the logic of the game must stay independent from the presentation layer.

### Used approaches

So what do we have. Very limited time, a tiny set of interacting game objects, the need to strictly separate logic and
presentation, as well as the requirement to do everything in a certain coding language (C#).

It was decided to use the classic MVC, isolating the game logic in the model layer, rendering in the view layer, and
handling user input in the controllers layer. The model layer is completely independent and does not use other app
layers. Model classes do not contain graphical information. They store and process only those data that describe the
logic of the gameplay. The view layer uses the model in read mode. It independently decides how the game objects should
look. The controller layer takes user input, interprets it, and calls the appropriate methods on the model. The model
itself does not interact with the outside world in any way.

There is no ECS here. A tiny inheritance tree of game objects is simply formed in the model. This has reduced
development time, but of course, it seriously hampers the future expansion of the game's capabilities.

The engineering constraint of the task is clearly executed. The model is completely independent from other parts of the
game. The app can function without a graphical component.

The game itself is based on the `MonoGame` framework. The `Aether.Physics2D` library is used to handle collisions of
game objects, which is a C# port of the `Box2d` physics engine. The user interface is built using the `Myra UI` library.
Excellent assets from the free `Kenney` library are used for texturing. The `DejaVu` font is used to render text. Links
to all materials are listed below.

## References

This section contains links to all the libraries, frameworks, and assets used to build the app.

1. [MonoGame framework](https://www.monogame.net/)
2. [MonoGame.Extended libraries](https://github.com/craftworkgames/MonoGame.Extended)
3. [Aether.Physics2D library](https://github.com/tainicom/Aether.Physics2D)
4. [Myra UI library](https://github.com/rds1983/Myra)
5. [Kenney game assets](https://www.kenney.nl/)
6. [DejaVu fonts](https://dejavu-fonts.github.io/)