#include "Light.h"
#include <math.h>

Light::Light() {
  _baseColor = rgb(0,0,0);
  _glowColor = rgb(0,0,0);
  _blinkColor = rgb(0,0,0);
}

rgb Light::GetColor() {
  
  if (_blinkCount > 0) {
    _blinkTic++;
    if (_blinkTic == _blinkDuration ) {
      _blinkTic = 0;
      _blinkState = !_blinkState;
      if (_blinkState) {
        _blinkCount--;
      }
    }
    if (!_blinkState) {
      return rgb(0, 0, 0);
    } else {
      return _blinkColor;
    }
  }
     

     
  // if (tic < 128) {
  //   int cl = 155 - tic;
  //   return rgb(cl, cl, cl);
  // } else {
  //   int cl = tic - 100; 
  //   return rgb(cl, cl, cl); 
  // }   
  if (_glowCycleDuration > 0) {
    _glowTic++;
    if (_glowTic >= _glowCycleDuration) {
      _glowTic = 0;
    }
    
    int t = _glowTic;
    if (_glowTic > (_glowCycleDuration / 2)) {
      t = _glowCycleDuration - _glowTic;
    }
    float k = (_glowCycleDuration - (t * (_glowRange * 0.01 * 2))) / _glowCycleDuration;

    int r = _glowColor.r * k;
    int g = _glowColor.g * k;
    int b = _glowColor.b * k;

  // // r0  * (c   - (t * d)) / c
  //   if (_glowTic >= (_glowCycleDuration / 2)) {
  //     var adjust = (_glowCycleDuration - _glowTic);
  //     r = r - adjust;
  //     g = g - adjust;
  //     b = b - adjust;
  //   } else {      
  //     r = r - _glowTic;
  //     g = g - _glowTic;
  //     b = b - _glowTic;
  //   }

  //   if (r < 0) r = 0;
  //   if (g < 0) g = 0;      
  //   if (b < 0) b = 0;
      
    return rgb(r, g, b);
  }

  return _baseColor;
}

void Light::Sync() {
  _glowTic = 0;
}

void Light::SetBaseColor(int r, int g, int b) {
  _blinkCount = 0;
  _glowCycleDuration = 0;
  _baseColor = rgb(r,g,b);
}

void Light::SetGlow(int r, int g, int b, int cycleDuration, int glowRange) {
  _glowColor = rgb(r,g,b);
  _glowCycleDuration = cycleDuration;
  _glowRange = glowRange;
  _glowTic = 0;
}

void Light::SetBlink(int r, int g, int b, int blinkDuration, int blinkCount) {  
  _blinkColor = rgb(r, g, b);
  _blinkCount = blinkCount;
  _blinkDuration = blinkDuration;
  _blinkState = true;

  _blinkTic = 0;
}

rgb::rgb(int _r, int _g, int _b) {
  r = _r;
  g = _g;
  b = _b;
}

rgb::rgb() {
  r = 0;
  g = 0;
  b = 0;
}