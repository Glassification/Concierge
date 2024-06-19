// <copyright file="SoundService.cs" company="Thomas Beckett">
// Copyright (c) Thomas Beckett. All rights reserved.
// </copyright>

namespace Concierge.Services
{
    using System;
    using System.Windows.Media;

    using Concierge.Common;
    using Concierge.Configuration;

    /// <summary>
    /// Provides functionality for managing sounds in the application.
    /// </summary>
    public static class SoundService
    {
        private static readonly MediaPlayer updateValueSound = new ();
        private static readonly MediaPlayer navigationSound = new ();
        private static readonly MediaPlayer warningSound = new ();

        private static int skipClickCount;

        static SoundService()
        {
            try
            {
                updateValueSound.Open(new Uri(@$"{ConciergeFiles.ExecutingDirectory}\Properties\Resources\Sounds\HighPitchTapSoundLoud.wav"));
                navigationSound.Open(new Uri(@$"{ConciergeFiles.ExecutingDirectory}\Properties\Resources\Sounds\GenericTapSoundLoud.wav"));
                warningSound.Open(new Uri(@$"{ConciergeFiles.ExecutingDirectory}\Properties\Resources\Sounds\GenericWarning.wav"));
            }
            catch (Exception)
            {
                // We are currently handling this error. Just swallow it.
            }
        }

        /// <summary>
        /// Gets a value indicating whether to skip the sound when clicked.
        /// </summary>
        public static bool SkipClick
        {
            get
            {
                if (skipClickCount == 0)
                {
                    skipClickCount++;
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Resets the skip click counter.
        /// </summary>
        public static void ResetSkipClick()
        {
            skipClickCount = 0;
        }

        /// <summary>
        /// Plays the navigation sound.
        /// </summary>
        public static void PlayNavigation()
        {
            if (ShouldPlay(navigationSound, SkipClick))
            {
                Play(navigationSound);
            }
        }

        /// <summary>
        /// Plays the update value sound.
        /// </summary>
        public static void PlayUpdateValue()
        {
            if (ShouldPlay(updateValueSound))
            {
                Play(updateValueSound);
            }
        }

        /// <summary>
        /// Plays the warning sound.
        /// </summary>
        public static void PlayWarning()
        {
            if (ShouldPlay(warningSound))
            {
                Play(warningSound);
            }
        }

        /// <summary>
        /// Stops and closes all sound players.
        /// </summary>
        public static void CloseAll()
        {
            updateValueSound.Stop();
            navigationSound.Stop();
            warningSound.Stop();

            updateValueSound.Close();
            navigationSound.Close();
            warningSound.Close();
        }

        /// <summary>
        /// Sets the volume of all sound players.
        /// </summary>
        public static void SetVolume()
        {
            var volume = AppSettingsManager.UserSettings.Volume;

            updateValueSound.Volume = volume / 100f;
            navigationSound.Volume = volume / 100f;
            warningSound.Volume = volume / 100f;
        }

        private static void Play(MediaPlayer mediaPlayer)
        {
            mediaPlayer.Stop();
            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Play();
        }

        private static bool ShouldPlay(MediaPlayer mediaPlayer, bool skipClick = false)
        {
            return
                !AppSettingsManager.UserSettings.MuteSounds &&
                !skipClick &&
                !Program.IsDrawing &&
                mediaPlayer is not null;
        }
    }
}
