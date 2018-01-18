using BlinkManager.Controls;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BlinkManager {

  public sealed partial class MainPage : Page {
    private Blinker blinker = new Blinker();
    public MainPage() {
      InitializeComponent();
    }

    private async void LightColorPicker_ColorChanged(LightColorPicker sender, CustomColorChangedEventArgs args) {
      var color = args.Color;
      await blinker.WriteBlink(BlinkBuilder.Pos(args.LightIndex).Glow().SetSpeed(100).SetDuration(100).Color(color.R, color.G, color.B));
    }

    private async void Button_Click(object sender, RoutedEventArgs e) {
      await blinker.SyncGlow();
    }
  }
}
