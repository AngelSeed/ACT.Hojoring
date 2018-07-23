using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using ACT.UltraScouter.Config;
using ACT.UltraScouter.Models;
using ACT.UltraScouter.ViewModels;
using FFXIV.Framework.WPF.Controls;
using FFXIV.Framework.WPF.Views;

namespace ACT.UltraScouter.Views
{
    /// <summary>
    /// MPTickerView.xaml の相互作用ロジック
    /// </summary>
    public partial class MPTickerView :
        Window,
        IOverlay
    {
        public MPTickerView()
        {
            this.InitializeComponent();

            // アクティブにさせないようにする
            this.ToNonActive();

            // ドラッグによる移動を設定する
            this.MouseLeftButtonDown += (s, e) => this.DragMove();

            // 初期状態は透明（非表示）にしておく
            this.Opacity = 0;
        }

        /// <summary>オーバーレイとして表示状態</summary>
        private bool overlayVisible;

        /// <summary>
        /// オーバーレイとして表示状態
        /// </summary>
        public bool OverlayVisible
        {
            get => this.overlayVisible;
            set => this.SetOverlayVisible(ref this.overlayVisible, value, Settings.Instance.Opacity);
        }

        /// <summary>ViewModel</summary>
        public MPTickerViewModel ViewModel => (MPTickerViewModel)this.DataContext;

        /// <summary>
        /// アニメーション用のMP回復間隔（TimeSpan型）
        /// </summary>
        private static readonly TimeSpan MPRecoverTimeSpan =
            TimeSpan.FromSeconds(MeInfoModel.Constants.MPRecoverySpan);

        /// <summary>
        /// アニメーション用のMP回復間隔（KeyTime型）
        /// </summary>
        private static readonly KeyTime MPRecoverKeyTime =
            KeyTime.FromTimeSpan(TimeSpan.FromSeconds(MeInfoModel.Constants.MPRecoverySpan));

        public void BeginAnimation()
        {
            this.BeginBarAnimation();
            this.BeginCountdownAnimation();
        }

        #region Countdown Animation

        private DoubleAnimation countdownAnimation = new DoubleAnimation()
        {
            From = 3,
            To = 0,
            Duration = MPRecoverTimeSpan,
            AutoReverse = false,
            RepeatBehavior = RepeatBehavior.Forever,
        };

        private void BeginCountdownAnimation()
        {
            Timeline.SetDesiredFrameRate(this.countdownAnimation, Settings.Instance.AnimationMaxFPS);

            this.CounterLabel.BeginAnimation(
                Label.WidthProperty,
                null);
            this.CounterLabel.BeginAnimation(
                Label.WidthProperty,
                this.countdownAnimation);
        }

        #endregion Countdown Animation

        #region Bar Animation

        private DoubleAnimation barAnimation = new DoubleAnimation()
        {
            From = 0,
            To = 1,
            Duration = MPRecoverTimeSpan,
            AutoReverse = false,
            RepeatBehavior = RepeatBehavior.Forever,
        };

        private void BeginBarAnimation()
        {
            Timeline.SetDesiredFrameRate(this.barAnimation, Settings.Instance.AnimationMaxFPS);

            this.Bar.BeginAnimation(
                RichProgressBar.ProgressProperty,
                null);
            this.Bar.BeginAnimation(
                RichProgressBar.ProgressProperty,
                this.barAnimation);
        }

        #endregion Bar Animation
    }
}
