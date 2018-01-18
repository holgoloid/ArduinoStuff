using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BlinkManager.Controls {
  public sealed partial class LightColorPicker : UserControl {

    public int LightIndex { get; set; }

    public Color Color {
      get { return picker.Color; }
      set { picker.Color = value; }
    }

    public event TypedEventHandler<LightColorPicker, CustomColorChangedEventArgs> ColorChanged 
      = new TypedEventHandler<LightColorPicker, CustomColorChangedEventArgs>((s, e) => { });

    public LightColorPicker() {
      this.InitializeComponent();

      picker.ColorChanged += (s,e) 
        => ColorChanged.Invoke(this, new CustomColorChangedEventArgs {
            Color = e.NewColor,
            LightIndex = LightIndex
        }); 
    }
  }

  public class CustomColorChangedEventArgs {
    public Color Color { get; set; }
    public int LightIndex { get; set; }
  }
}
