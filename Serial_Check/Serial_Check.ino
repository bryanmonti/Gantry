int incomingByte = 0;   // for incoming serial data

void setup() {
        Serial.begin(115200);     // opens serial port, sets data rate to 9600 bps
}

void loop() {

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
          Serial.print("A");
           break;
          case 66:
          Serial.print("B");
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
