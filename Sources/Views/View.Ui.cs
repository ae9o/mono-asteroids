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

using System.Reflection;
using System.Resources;
using System.IO;
using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D.UI;
using FontStashSharp;

namespace MonoAsteroids;

/// <summary>
/// This part of the view is responsible for drawing the user interface.
/// </summary>
public partial class View : DrawableGameComponent
{
    private readonly ResourceManager _rm = new ResourceManager("MonoAsteroids.View", Assembly.GetExecutingAssembly());

    private Desktop _desktop;

    private FontSystem _fontSystem;
    private DynamicSpriteFont _largeFont;
    private DynamicSpriteFont _defaultFont;

    private Label _welcomeLabel;
    private Label _scoreLabel;
    private ProgressBar _laserAmmoBar;
    private ProgressBar _laserCooldownBar;
    private Label _positionLabel;
    private Label _angleLabel;
    private Label _speedLabel;
    private Panel _finalStatsPanel;
    private Label _finalScoreLabel;

    private void LoadUi()
    {
        MyraEnvironment.Game = Game;

        CreateFonts();
        CreateDesktop();
    }

    private void UnloadUi()
    {
        _fontSystem.Dispose();
    }

    private void CreateFonts()
    {
        _fontSystem = new FontSystem();
        _fontSystem.AddFont(File.ReadAllBytes(@"Content/Fonts/DejaVuSans.ttf"));
        _largeFont = _fontSystem.GetFont(64);
        _defaultFont = _fontSystem.GetFont(20);
    }

    private void CreateDesktop()
    {
        _desktop = new Desktop();
        _desktop.Root = CreateRootPanel();
    }

    /// <summary>
    /// Creates a panel that spans the entire screen and contains all the UI widgets.
    /// </summary>
    /// <returns>Root panel with all the UI widgets.</returns>
    private Panel CreateRootPanel()
    {
        _welcomeLabel = new Label();
        _welcomeLabel.Text = _rm.GetString("Welcome");
        _welcomeLabel.Font = _largeFont;
        _welcomeLabel.HorizontalAlignment = HorizontalAlignment.Center;
        _welcomeLabel.VerticalAlignment = VerticalAlignment.Center;

        var statsPanel = CreateStatsPanel();
        statsPanel.Left = 10;
        statsPanel.Top = 10;

        _finalStatsPanel = CreateFinalStatsPanel();
        _finalStatsPanel.Visible = false;

        var faqLabel = new Label();
        faqLabel.Text = _rm.GetString("Faq");
        faqLabel.Font = _defaultFont;
        faqLabel.VerticalAlignment = VerticalAlignment.Bottom;
        faqLabel.HorizontalAlignment = HorizontalAlignment.Right;

        var rootPanel = new Panel();
        rootPanel.Widgets.Add(_welcomeLabel);
        rootPanel.Widgets.Add(statsPanel);
        rootPanel.Widgets.Add(_finalStatsPanel);
        rootPanel.Widgets.Add(faqLabel);

        return rootPanel;
    }

    /// <summary>
    /// Creates a panel with information about the player's spaceship.
    /// </summary>
    /// <returns>Panel with information about the player's spaceship.</returns>
    private VerticalStackPanel CreateStatsPanel()
    {
        _scoreLabel = new Label();
        _scoreLabel.Font = _defaultFont;
        _scoreLabel.Text = string.Format(_rm.GetString("Score"), 0);

        var laserStatsPanel = CreateLaserStatsPanel();

        _positionLabel = new Label();
        _positionLabel.Font = _defaultFont;
        _positionLabel.Text = string.Format(_rm.GetString("Position"), 0, 0);

        _angleLabel = new Label();
        _angleLabel.Font = _defaultFont;
        _angleLabel.Text = string.Format(_rm.GetString("Angle"), 0);

        _speedLabel = new Label();
        _speedLabel.Font = _defaultFont;
        _speedLabel.Text = string.Format(_rm.GetString("Speed"), 0);

        var statsPanel = new VerticalStackPanel();
        statsPanel.Width = 120;
        statsPanel.Spacing = 10;
        statsPanel.Widgets.Add(_scoreLabel);
        statsPanel.Widgets.Add(_positionLabel);
        statsPanel.Widgets.Add(_angleLabel);
        statsPanel.Widgets.Add(_speedLabel);
        statsPanel.Widgets.Add(laserStatsPanel);

        return statsPanel;
    }

    /// <summary>
    /// Creates a panel with information about the charge and cooldown of the spaceship's laser.
    /// </summary>
    /// <returns>Panel with information about the spaceship's laser.</returns>
    private VerticalStackPanel CreateLaserStatsPanel()
    {
        var laserAmmoLabel = new Label();
        laserAmmoLabel.Text = _rm.GetString("LaserAmmo");
        laserAmmoLabel.Font = _defaultFont;

        _laserAmmoBar = new HorizontalProgressBar();

        _laserCooldownBar = new HorizontalProgressBar();
        _laserCooldownBar.Maximum = 1f;
        _laserCooldownBar.Visible = false;

        var laserStatsPanel = new VerticalStackPanel();
        laserStatsPanel.Widgets.Add(laserAmmoLabel);
        laserStatsPanel.Widgets.Add(_laserAmmoBar);
        laserStatsPanel.Widgets.Add(_laserCooldownBar);

        return laserStatsPanel;
    }

    /// <summary>
    /// Creates a panel that is displayed at the end of a game round.
    /// </summary>
    /// <returns>Panel with final stats.</returns>
    private Panel CreateFinalStatsPanel()
    {
        var bangLabel = new Label();
        bangLabel.Text = _rm.GetString("Bang");
        bangLabel.Font = _largeFont;
        bangLabel.HorizontalAlignment = HorizontalAlignment.Center;

        _finalScoreLabel = new Label();
        _finalScoreLabel.Text = string.Format(_rm.GetString("FinalScore"), 0);
        _finalScoreLabel.Font = _largeFont;
        _finalScoreLabel.HorizontalAlignment = HorizontalAlignment.Center;

        var finalScorePanel = new VerticalStackPanel();
        finalScorePanel.Spacing = 10;
        finalScorePanel.Widgets.Add(bangLabel);
        finalScorePanel.Widgets.Add(_finalScoreLabel);
        finalScorePanel.HorizontalAlignment = HorizontalAlignment.Center;
        finalScorePanel.VerticalAlignment = VerticalAlignment.Center;

        var restartTipLabel = new Label();
        restartTipLabel.Text = _rm.GetString("RestartTip");
        restartTipLabel.Font = _defaultFont;
        restartTipLabel.Top = -50;
        restartTipLabel.HorizontalAlignment = HorizontalAlignment.Center;
        restartTipLabel.VerticalAlignment = VerticalAlignment.Bottom;

        var finalStatsPanel = new Panel();
        finalStatsPanel.Widgets.Add(finalScorePanel);
        finalStatsPanel.Widgets.Add(restartTipLabel);

        return finalStatsPanel;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        switch (_model.State)
        {
            case ModelState.RoundStarted:
                HideWelcomePanels();
                UpdateScore();
                UpdateLaserAmmo();
                UpdateLaserCooldown();
                UpdateCoords();
                UpdateAngle();
                UpdateSpeed();
                break;

            case ModelState.RoundFinished:
                ShowFinalStats();
                break;

            default:
                // Ignore
                break;
        }
    }

    private void HideWelcomePanels()
    {
        _welcomeLabel.Visible = false;
        _finalStatsPanel.Visible = false;
    }

    private void ShowFinalStats()
    {
        _finalStatsPanel.Visible = true;
        _finalScoreLabel.Text = string.Format(_rm.GetString("FinalScore"), _model.Score);
    }

    private void UpdateScore()
    {
        _scoreLabel.Text = string.Format(_rm.GetString("Score"), _model.Score);
    }

    private void UpdateLaserAmmo()
    {
        var laserGun = _model.Starship.LaserGun;
        _laserAmmoBar.Maximum = laserGun.MaxCharge;
        _laserAmmoBar.Value = laserGun.CurrentCharge;
    }

    private void UpdateLaserCooldown()
    {
        var laserGun = _model.Starship.LaserGun;
        _laserCooldownBar.Value = (float) laserGun.ChargingPercent;
        _laserCooldownBar.Visible = laserGun.CurrentCharge < laserGun.MaxCharge;
    }

    private void UpdateCoords()
    {
        var coords = _model.Starship.Position;
        _positionLabel.Text = string.Format(_rm.GetString("Position"), coords.X * _scaleX, coords.Y * _scaleY);
    }

    private void UpdateAngle()
    {
        var starship = _model.Starship;
        var angle = -MathHelper.ToDegrees(MathHelper.WrapAngle(starship.Rotation - MathHelper.PiOver2));
        _angleLabel.Text = string.Format(_rm.GetString("Angle"), angle);
    }

    private void UpdateSpeed()
    {
        var starship = _model.Starship;
        var velocity = starship.LinearVelocity;
        velocity.X *= _scaleX;
        velocity.Y *= _scaleY;
        _speedLabel.Text = string.Format(_rm.GetString("Speed"), velocity.Length());
    }

    public void DrawUi()
    {
        _desktop.Render();
    }
}