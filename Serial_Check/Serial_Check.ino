#include <SoftwareSerial.h>

int incomingByte = 0;   // for incoming serial data, supposed ot be 0

void setup() {
        Serial.begin(115200);     // opens serial port, sets data rate to 115200 bps
        /*for(int pin = 8; pin < 13; pin = pin += 2)//works but you have to give it a 10 second head start
        {
          pinMode(pin,OUTPUT);
          Serial.print(pin);
          Serial.print("\n");
        }*/
        
        pinMode(12,OUTPUT);
        pinMode(10,OUTPUT);
        pinMode(8,OUTPUT); 
}

int window_position = 0; //0 is closed and 1 is open

void loop() {//work on something that allows the user to choose whether they want to start in open or closed position
        int counter = 0;
                
      if(Serial.available() > 0) {
        
        incomingByte = Serial.read();
        
        switch(incomingByte){
          case 65:
          //Serial.print("A\n");
          digitalWrite(10,HIGH);//Chooses direction (HIGH = forward)
          digitalWrite(8,LOW);//Enables steppers
          
          if(window_position == 0)//if window position is closed
          {
            Serial.print("Opening Window Shade (or at least trying to)\n");
            while(counter < 8500) //open window
            {
              //Serial.print(counter);
              //Serial.print("\n");
              counter++;
              digitalWrite(12,HIGH);
              delay(1);
              digitalWrite(12,LOW);
              delay(1);
            }
            
            Serial.print("Window in up position! (A)\n");
            window_position = 1;//window is open
          }
          else
          {
           Serial.print("Yo dawg! I'm in the up position! Can't go up twice!\n"); 
          }
           break;
           
          case 66:
          //Serial.print("B\n");
          
          digitalWrite(10,LOW);//Chooses direction (LOW = backward)
          digitalWrite(8,LOW);
          if(window_position == 1)//if window is open
          {
            Serial.print("Closing Window Shade (or at least trying to)\n");
            while(counter < 8500)//close window
            {
              //Serial.print(counter);
              //Serial.print("\n");
              counter++;
              digitalWrite(12,HIGH);
              delay(1);
              digitalWrite(12,LOW);
              delay(1);
            }
            window_position = 0;
            Serial.print("Window in closed position! (B)\n");
          }
          else
          {
            Serial.print("Dude, I'm closed. Either open me or leave me alone.\n");  
          }
          
          break;
           
          case 67:
          Serial.print("Yeah, I don't do anything yet\n");
           break;
          case 68:
          Serial.print("D\n");
           break;
          case 69:
          Serial.print("E\n");
           break;
          case 70:
          Serial.print("F\n");
           break;
          case 71:
          Serial.print("G\n");
           break;
          case 72:
          Serial.print("H\n");
           break;
          case 73:
          Serial.print("I\n");
           break;
          case 74:
          Serial.print("J\n");
           break;
          case 75:
          Serial.print("K\n");
           break;
          case 76:
          Serial.print("L\n");
           break;
          case 77:
          Serial.print("M\n");
           break;
          
          case 78:
          Serial.print("N\n");
           break;
          
          case 79:
          Serial.print("O\n");
           break;
          
          case 80:
          Serial.print("P\n");
           break;
          case 81:
          Serial.print("Q\n");
           break;
          case 82:
          Serial.print("R\n");
           break;
          case 83:
          Serial.print("S\n");
           break;
          case 84:
          Serial.print("T\n");
           break;
          case 85:
          Serial.print("U\n");
           break;
          case 86:
          Serial.print("V\n");
           break;
          case 87:
          Serial.print("W\n");
           break;
          case 88:
          Serial.print("X\n");
           break;
          case 89:
          Serial.print("Y\n");
           break;
          case 90:
          Serial.print("Z\n");
           break;
        }
    }   
}



