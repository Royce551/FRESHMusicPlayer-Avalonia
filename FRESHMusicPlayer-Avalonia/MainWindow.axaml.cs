using ATL;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using FRESHMusicPlayer;
using FRESHMusicPlayer_Avalonia.Utils;
using LiteDB;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Linq;
using System.Collections.Generic;

namespace FRESHMusicPlayer_Avalonia
{
    public partial class MainWindow : Window
    {
        public static LiteDatabase Libraryv2;

        public Player Player = new Player();
        public ATL.Track? CurrentTrack;

        private Timer progressTimer = new Timer(1000);

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            Program.logInstance.Info("Avalonia Xaml loaded");
        }

        public async void LoadLibrary()
        {
            var trackEntries = new List<string>();
            await Task.Run(() =>
            {
                var tracks = DatabaseUtils.Read();
                
                foreach (var track in tracks)
                {
                    trackEntries.Add($"{track.Artist} - {track.Title}");
                    Program.logInstance.Info($"{track.Artist} - {track.Title} added to library");
                }
                
            });
            Tracks_TracksListBox.Items = trackEntries;
        }

        private void Player_SongException(object? sender, FRESHMusicPlayer.Handlers.PlaybackExceptionEventArgs e)
        {
            Program.logInstance.Error(e.Details);
        }

        private void Player_SongStopped(object? sender, System.EventArgs e)
        {
            Title = "FRESHMusicPlayer";
            TrackTitleTextBlock.Text = "Nothing Playing";
            TrackArtistTextBlock.Text = "Nothing Playing";
            ProgressIndicator1.Text = "--:--";
            ProgressIndicator2.Text = "--:--";
            progressTimer.Stop();
        }

        private void Player_SongChanged(object? sender, System.EventArgs e)
        {
            CurrentTrack = new ATL.Track(Player.FilePath);
            Title = $"{CurrentTrack.Artist} - {CurrentTrack.Title} | FRESHMusicPlayer";
            TrackTitleTextBlock.Text = string.IsNullOrWhiteSpace(CurrentTrack.Title) ? "Unknown Title" : CurrentTrack.Title;
            TrackArtistTextBlock.Text = string.IsNullOrWhiteSpace(CurrentTrack.Artist) ? "Unknown Artist" : CurrentTrack.Artist;
            CoverArtImage.Source = new Bitmap(new MemoryStream(CurrentTrack.EmbeddedPictures[0].PictureData));
            ProgressIndicator2.Text = Player.CurrentBackend.TotalTime.ToString(@"mm\:ss");
            ProgressSlider.Maximum = Player.CurrentBackend.TotalTime.TotalSeconds;
            progressTimer.Start();
        }
        private void ProgressTimer_Elapsed(object sender, ElapsedEventArgs e) => ProgressTick();
        public void PlayPauseMethod()
        {
            if (!Player.Playing) return;
            if (Player.Paused)
            {
                Player.ResumeMusic();
                progressTimer.Start();
            }
            else
            {
                Player.PauseMusic();
                progressTimer.Stop();
            }
        }
        public void ProgressTick()
        {
            var time = TimeSpan.FromSeconds(Math.Floor(Player.CurrentBackend.CurrentTime.TotalSeconds));
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                ProgressIndicator1.Text = time.ToString(@"mm\:ss");
                ProgressSlider.Value = time.TotalSeconds;
            });
        }
        public void StopMethod() => Player.StopMusic();
        public void NextTrackMethod() => Player.NextSong();
        public void PreviousTrackMethod() => Player.PreviousSong();
        public void UpdatePlayButtonState()
        {
            if (!Player.Paused) PlayPauseButton.Content = "Pause";
            else PlayPauseButton.Content = "Resume";
        }
        private void NextTrackButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => NextTrackMethod();

        private void RepeatOneButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => Player.RepeatOnce = RepeatOneButton.IsPressed;

        private void PlayPauseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => PlayPauseMethod();

        private void ShuffleButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => Player.Shuffle = ShuffleButton.IsPressed;

        private void PreviousTrackButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => PreviousTrackMethod();

        private bool progressSliderIsBeingDragged;
        private void ProgressSlider_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == RangeBase.ValueProperty)
            {
                if (e.NewValue is double d)
                {
                    if (Player.Playing && progressSliderIsBeingDragged)
                    {
                        Player.RepositionMusic((int)d);
                        ProgressTick();
                    }
                }
            }
        }

        private void ProgressSlider_PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
        {
            progressSliderIsBeingDragged = false;
            progressTimer.Start();
        }

        private void ProgressSlider_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            progressSliderIsBeingDragged = true;
            progressTimer.Stop();
        }

        private void VolumeSlider_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == RangeBase.ValueProperty)
            {
                if (e.NewValue is double d)
                {
                    Player.CurrentVolume = (float)d;
                    if (Player.Playing) Player.UpdateSettings();
                }
            }
        }

        private void Import_PlaySongButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Player.AddQueue(Import_PathTextBox.Text);
            Player.CurrentVolume = 0.5f;
            Player.PlayMusic();
        }
    }
}
