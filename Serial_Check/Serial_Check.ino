int incomingByte = 0;   // for incoming serial data

void setup() {
        Serial.begin(115200);     // opens serial port, sets data rate to 115200 bps
        pinMode(12,OUTPUT);
        pinMode(10,OUTPUT);
        pinMode(8,OUTPUT); 
}
//TODO: Add \n to everything!
void loop() {
        int counter = 0;
        //digitalWrite(8,LOW);//Enables steppers
        // send data only when you receive data:
        if (Serial.available() > 0) {
               /* // read the incoming byte:
                incomingByte = Serial.read();

                // say what you got:
                Serial.print("I received: ");
                Serial.println(incomingByte);
        }*/
        incomingByte = Serial.read();
        switch(incomingByte){
          case 65:
          Serial.print("A\n");
          Serial.print("Opening Window Shade (or at least trying to)\n");
          digitalWrite(10,HIGH);//Chooses direction (HIGH = forward)
          digitalWrite(8,LOW);
          while(counter < 10000)
          {
            Serial.print(counter);
            Serial.print("\n");
            counter++;
            digitalWrite(12,HIGH);
            delay(1);
            digitalWrite(12,LOW);
            delay(1);
          }
           break;
           
          case 66:
          Serial.print("B\n");
          Serial.print("Closing Window Shade (or at least trying to)\n");
          digitalWrite(10,LOW);//Chooses direction (LOW = backward)
          digitalWrite(8,LOW);
          while(counter < 12800)
          {
            Serial.print(counter);
            Serial.print("\n");
            counter++;
            digitalWrite(12,HIGH);
            delay(1);
            digitalWrite(12,LOW);
            delay(1);
          }
           break;
           
          case 67:
          Serial.print("C");
           break;
          case 68:
          Serial.print("D");
           break;
          case 69:
          Serial.print("E");
           break;
          case 70:
          Serial.print("F");
           break;
          case 71:
          Serial.print("G");
           break;
          case 72:
          Serial.print("H");
           break;
          case 73:
          Serial.print("I");
           break;
          case 74:
          Serial.print("J");
           break;
          case 75:
          Serial.print("K");
           break;
          case 76:
          Serial.print("L");
           break;
          case 77:
          Serial.print("M");
           break;
          case 78:
          Serial.print("N");
           break;
          case 79:
          Serial.print("O");
           break;
          case 80:
          Serial.print("P");
           break;
          case 81:
          Serial.print("Q");
           break;
          case 82:
          Serial.print("R");
           break;
          case 83:
          Serial.print("S");
           break;
          case 84:
          Serial.print("T");
           break;
          case 85:
          Serial.print("U");
           break;
          case 86:
          Serial.print("V");
           break;
          case 87:
          Serial.print("W");
           break;
          case 88:
          Serial.print("X");
           break;
          case 89:
          Serial.print("Y");
           break;
          case 90:
          Serial.print("Z");
           break;
        }
    }   
}
