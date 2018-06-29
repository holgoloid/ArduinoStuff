#include "Light.h"


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

Light::Light() {
  _baseColor = rgb(0,0,0);
  _blinkEffectColor = rgb(0,0,0);
}

rgb Light::Loop() {

  if (_blinkDuration > 0) {
    _blinkTic++;
  }
  
  if (_glowCycleDuration > 0) {
    _glowTic++;
  }

  if (_blinkEffectCount > 0) {
    _blinkEffectTic++;
    if (_blinkEffectTic == _blinkEffectDuration ) {
      _blinkEffectTic = 0;
      _blinkEffectState = !_blinkEffectState;
      if (!_blinkEffectState) {
        _blinkEffectCount--;
      }
    }
    if (_blinkEffectState) {
      return rgb(0, 0, 0);
    } else {
      return _blinkEffectColor;
    }
  }
  
  if (_blinkDuration > 0) {
    if (_blinkTic == _blinkDuration ) {
      _blinkTic = 0;
      _blinkState = !_blinkState;
    }
    if (!_blinkState) {
      return rgb(0, 0, 0);
    }
  }

  if (_glowCycleDuration > 0) {
    if (_glowTic >= _glowCycleDuration) {
      _glowTic = 0;
    }
    
    int t = _glowTic;
    if (_glowTic > (_glowCycleDuration / 2)) {
      t = _glowCycleDuration - _glowTic;
    }

    float k = (_glowCycleDuration - (t * (_glowRange * 0.01 * 2))) / _glowCycleDuration;

    int r = _baseColor.r * k;
    int g = _baseColor.g * k;
    int b = _baseColor.b * k;

    return rgb(r, g, b);
  }

  return _baseColor;
}

void Light::ResetTic() {
  _glowTic = 0;
  _blinkTic = 0;
}

void Light::SetBaseColor(int r, int g, int b) {
  _baseColor = rgb(r,g,b);
}

void Light::SetGlow(int cycleDuration, int glowRange) {
  _glowCycleDuration = cycleDuration;
  _glowRange = glowRange;
  _glowTic = 0;
}

void Light::SetBlink(int blinkDuration) {  
  _blinkDuration = blinkDuration;
  _blinkState = true;
  _blinkTic = 0;
}

void Light::SetBlinkEffect(int r, int g, int b, int blinkDuration, int blinkCount) {
  _blinkEffectColor = rgb(r, g, b);
  _blinkEffectCount = blinkCount + 1;
  _blinkEffectDuration = blinkDuration;
  _blinkEffectState = true;

  _blinkEffectTic = 0;
}