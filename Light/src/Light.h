#ifndef Light_h
#define Light_h

class rgb {  
  public: 
    rgb();
    rgb(int _r, int _g, int _b);
    int r;
    int g;
    int b;
};

class Light {
  public:
    Light();
    rgb Loop();
    void SetBaseColor(int r, int g, int b);
    void SetGlow(int cycleDuration, int glowRange);
    void SetBlink(int blinkDuration);
    void SetBlinkEffect(int r, int g, int b, int blinkDuration, int blinkCount);
    void ResetTic();
    
  private:
    rgb _baseColor;

    int _glowCycleDuration;   
    int _glowRange; 
    int _glowTic;

    bool _blinkState;
    int _blinkDuration;
    int _blinkTic;

    rgb _blinkEffectColor;
    int _blinkEffectCount;
    bool _blinkEffectState;
    int _blinkEffectDuration;
    int _blinkEffectTic;
};

#endif