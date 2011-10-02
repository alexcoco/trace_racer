using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;


namespace GCA2
{
    public class GameStateManagementGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        ScreenFactory screenFactory;
        SoundEffect gameMusic;
        SoundEffectInstance gameMusicI;

        public GameStateManagementGame()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            //TargetElapsedTime = TimeSpan.FromTicks(333333);
            TargetElapsedTime = TimeSpan.FromTicks(333);

#if WINDOWS_PHONE
            graphics.IsFullScreen = true;
            InitializeLandscapeGraphics();
#endif

            screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

#if WINDOWS_PHONE
            // hook
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Launching +=
                new EventHandler<Microsoft.Phone.Shell.LaunchingEventArgs>(GameLaunching);
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Activated +=
                new EventHandler<Microsoft.Phone.Shell.ActivatedEventArgs>(GameActivated);
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Deactivated +=
                new EventHandler<Microsoft.Phone.Shell.DeactivatedEventArgs>(GameDeactivated);
#else
            AddInitialScreens();
#endif
            gameMusic = Content.Load<SoundEffect>("sounds/GamePlayHalfBit");
            gameMusicI = gameMusic.CreateInstance();
            gameMusicI.IsLooped = true;
            gameMusicI.Play();
        }

        private void AddInitialScreens()
        {
            BackgroundScreen backgroundScreen = new BackgroundScreen();
            screenManager.AddScreen(backgroundScreen, null);

#if WINDOWS_PHONE
            screenManager.AddScreen(new PhoneMainMenuScreen(), null);
#else
            screenManager.AddScreen(new MainMenuScreen(), null);
#endif
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

#if WINDOWS_PHONE
        private void InitializePortraitGraphics()
        {
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
        }

        private void InitializeLandscapeGraphics()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
        }

        void GameLaunching(object sender, Microsoft.Phone.Shell.LaunchingEventArgs e)
        {
            AddInitialScreens();
        }

        void GameActivated(object sender, Microsoft.Phone.Shell.ActivatedEventArgs e)
        {
            if (!screenManager.Activate(e.IsApplicationInstancePreserved))
            {
                AddInitialScreens();
            }
        }

        void GameDeactivated(object sender, Microsoft.Phone.Shell.DeactivatedEventArgs e)
        {
            screenManager.Deactivate();
        }
#endif
    }
}
