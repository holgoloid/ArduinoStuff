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
    rgb GetColor();
    void SetBaseColor(int r, int g, int b);
    void SetBlink(int r, int g, int b, int blinkDuration, int blinkCount);
    void SetGlow(int r, int g, int b, int cycleDuration, int glowRange);
    void Sync();
    
  private:
    rgb _baseColor;

    rgb _glowColor;
    int _glowCycleDuration;   
    int _glowRange; 
    int _glowTic;

    rgb _blinkColor;
    int _blinkCount;
    bool _blinkState;
    int _blinkDuration;
    int _blinkTic;
};

#endif