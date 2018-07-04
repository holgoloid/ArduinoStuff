using System;
using TogglSnusProxy.Snus;
using TogglSnusProxy.Util;

namespace TogglSnusProxy.Toggl {
  public class Project {
    public int id { get; set; }
    public int wid { get; set; }
    public int cid { get; set; }
    public string name { get; set; }
    public bool billable { get; set; }
    public bool active { get; set; }
    public DateTime at { get; set; }
    public bool template { get; set; }
    public string color { get; set; }
    public string hex_color { get; set; }

    public Color GetColor() 
    {
      var c = ColorManager.FromHex(hex_color);
      return new Color {
        r = c.R,
        g = c.G,
        b = c.B
      };
    }
  }

}
