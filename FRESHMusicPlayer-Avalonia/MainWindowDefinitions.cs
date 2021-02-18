using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

namespace FRESHMusicPlayer_Avalonia
{
    public partial class MainWindow : Window
    {
        private Button Import_PlaySongButton;
        private TextBox Import_PathTextBox;

        private TextBlock TrackTitleTextBlock;
        private TextBlock TrackArtistTextBlock;
        private TextBlock ProgressIndicator1;
        private TextBlock ProgressIndicator2;
        private Slider ProgressSlider;
        private Image CoverArtImage;

        private Button PreviousTrackButton;
        private ToggleButton ShuffleButton;
        private Button PlayPauseButton;
        private ToggleButton RepeatOneButton;
        private Button NextTrackButton;
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Import_PlaySongButton = this.Find<Button>("Import_PlaySongButton");
            Import_PlaySongButton.Click += Import_PlaySongButton_Click;
            Import_PathTextBox = this.Find<TextBox>("Import_PathTextBox");

            TrackTitleTextBlock = this.Find<TextBlock>("TrackTitleTextBlock");
            TrackArtistTextBlock = this.Find<TextBlock>("TrackArtistTextBlock");
            ProgressIndicator1 = this.Find<TextBlock>("ProgressIndicator1");
            ProgressIndicator2 = this.Find<TextBlock>("ProgressIndicator2");
            ProgressSlider = this.Find<Slider>("ProgressSlider");
            CoverArtImage = this.Find<Image>("CoverArtImage");

            PreviousTrackButton = this.Find<Button>("PreviousTrackButton");
            ShuffleButton = this.Find<ToggleButton>("ShuffleButton");
            PlayPauseButton = this.Find<Button>("PlayPauseButton");
            RepeatOneButton = this.Find<ToggleButton>("RepeatOneButton");
            NextTrackButton = this.Find<Button>("NextTrackButton");

            Player.SongChanged += Player_SongChanged;
            Player.SongStopped += Player_SongStopped;
            Player.SongException += Player_SongException;

            PreviousTrackButton.Click += PreviousTrackButton_Click;
            ShuffleButton.Click += ShuffleButton_Click;
            PlayPauseButton.Click += PlayPauseButton_Click;
            RepeatOneButton.Click += RepeatOneButton_Click;
            NextTrackButton.Click += NextTrackButton_Click;

            progressTimer.Elapsed += ProgressTimer_Elapsed;
        }


    }
}
