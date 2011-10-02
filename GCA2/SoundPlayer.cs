using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace GCA2
{
    public class SoundPlayer
    {
        private ContentManager content;
        private Game game;

        SoundEffect loseSound;
        SoundEffect beepSound;
        SoundEffect gateSound;
        SoundEffect menuMusic;
        SoundEffect gameMusic;
        SoundEffect scoreMusic;

        SoundEffectInstance menuMusicI;
        SoundEffectInstance gameMusicI;
        SoundEffectInstance scoreMusicI;

        public SoundPlayer(Game game)
        {
            this.game = game;
            content = new ContentManager(game.Services, "Content");

            // Load sound effects
            loseSound = content.Load<SoundEffect>("sounds/DownbeatBeep");
            beepSound = content.Load<SoundEffect>("sounds/FastBeep");
            gateSound = content.Load<SoundEffect>("sounds/GatePass");
            menuMusic = content.Load<SoundEffect>("sounds/GameStartDatahellBeta");
            gameMusic = content.Load<SoundEffect>("sounds/GamePlayHalfBit");
            scoreMusic = content.Load<SoundEffect>("sounds/GameOverLoop");

            // Get instances from sound effects that need to be looped
            menuMusicI = menuMusic.CreateInstance();
            gameMusicI = gameMusic.CreateInstance();
            scoreMusicI = scoreMusic.CreateInstance();

            // Make instances looped
            menuMusicI.IsLooped = true;
            menuMusicI.IsLooped = true;
            menuMusicI.IsLooped = true;
        }

        public void PlayLoseSound()
        {
            loseSound.Play();
        }

        public void PlayBeepSound()
        {
            beepSound.Play();
        }

        public void PlayGateSound()
        {
            gateSound.Play();
        }

        public void PlayMenuMusic()
        {
            menuMusicI.Play();
        }

        public void PlayGameMusic()
        {
            gameMusicI.Play();
        }

        public void PlayScoreMusic()
        {
            scoreMusicI.Play();
        }

        public void StopMenuMusic()
        {
            menuMusicI.Stop();
        }

        public void StopGameMusic()
        {
            gameMusicI.Stop();
        }

        public void StopScoreMusic()
        {
            scoreMusicI.Stop();
        }

        public void Dispose()
        {
            loseSound.Dispose();
            beepSound.Dispose();
            gateSound.Dispose();
            menuMusic.Dispose();
            gameMusic.Dispose();
            scoreMusic.Dispose();
            menuMusicI.Dispose();
            gameMusicI.Dispose();
            scoreMusicI.Dispose();
        }
    }
}
    