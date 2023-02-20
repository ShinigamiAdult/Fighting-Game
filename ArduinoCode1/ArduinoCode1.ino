#include <Keyboard.h>

int startPin = 2;
int endPin = 19;

void setup() 
{
  delay(3000);

  for(int i = startPin; i < endPin; i++)
  {
    pinMode(i, INPUT_PULLUP);
  }

  Keyboard.begin();
}

void loop() 
{
  //Action Buttons
  //Controller 1
  checkPush(2, 'z');
  checkPush(3, 'x');
  checkPush(4, 'c');
  checkPush(5, 'v');
  //Controller 2
  checkPush(6, 'h');
  checkPush(7, 'j');
  checkPush(8, 'k');
  checkPush(10, 'l');

  //Joystick
  //Controller 1
  checkPush(11, 'a');
  checkPush(12, 'w');
  checkPush(13, 'd');
  checkPush(14, 's');
  //Controlle 2
  checkArrowPush(15, KEY_LEFT_ARROW);
  checkArrowPush(16, KEY_UP_ARROW);
  checkArrowPush(17, KEY_RIGHT_ARROW);
  checkArrowPush(18, KEY_DOWN_ARROW);

  //Pause / Start
  checkPush(9, 'p');

  delay(50);
}

void checkPush(int pinNumber, char press) 
{
  int buttonPushed = digitalRead(pinNumber);
  if (buttonPushed == 0)
    Keyboard.press(press);
}

void checkArrowPush(int pinNumber, uint8_t press) 
{
  int buttonPushed = digitalRead(pinNumber);
  if (buttonPushed == 0)
    Keyboard.press(press);
}