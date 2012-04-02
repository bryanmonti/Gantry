## Gantry Documentation
####Project Information
- Author/ Designer/ Creator: Bryan Monti
- Contact: bsmonti@gmail.com (title the email with "Gantry")
- Date Started: 3/17/12 (March 17th, 2012)
- Date Completed: N/A

#### Files:
* Serial_Check.ino (change this name)
* Arduino_Serial.cs
* Program.cs
* README

#### Hardware:
* Xbox 360 Kinect
* A computer (Windows only ATM)
* [Arduino Mega 2560](http://arduino.cc/en/Main/ArduinoBoardMega2560)
* 1 x [Makerbot v2.3 Stepper Driver](http://reprap.org/wiki/Stepper_Motor_Driver_2.3)
* 1 x [NEMA 23 Stepper Motors](http://www.sparkfun.com/products/10847)

####Languages:
* Arduino (C++/C)
* C#
* Python - Coming soon (this is for wide support, mult OS's)

Objectives:
1. Speech Recognition (Completed - March 19th, 2012: 10:16PM)
2. Text to Serial (Completed 
3. Speech to text to serial data (Work in progress)
4. Serial Data interpreted by Arduino and processed (Work in progress; write arduino program)

TODO: Merge Arduino_Serial and Program (Speech => Text => Serial => Arduino => ??? (interface?) => Profit)

Random notes: Arduino should have a feature where it looks for current state of something (whether or not the
shades have been dimmed or not, or whether the shades are open or closed).

List of commands and definitions:
1. Dim Plus Far Window Shades - 
2. Dim Minus Far Window Shades
3. Dim Plus Computer Window Shades
4. Dim Minus Computer Window Shades
5. Open Far Window Shades
6. Close Far Window Shades
7. Open Computer Window Shades
8. Close Computer Window Shades

How it all comes together: 

###Brought to you by:
#### Aperture Laboratories
<pre>
              .,-:;//;:=,
          . :H@@@MM@M#H/.,+%;,
       ,/X+ +M@@M@MM%=,-%HMMM@X/,
     -+@MM; $M@@MH+-,;XMMMM@MMMM@+-
    ;@M@@M- XM@X;. -+XXXXXHHH@M@M#@/.
  ,%MM@@MH ,@%=             .---=-=:=,.
  =@#@@@MX.,                -%HX$$%%%:;
 =-./@M@M$                   .;@MMMM@MM:
 X@/ -$MM/                    . +MM@@@M$
,@M@H: :@:                    . =X#@@@@-
,@@@MMX, .                    /H- ;@M@M=
.H@@@@M@+,                    %MM+..%#$.
 /MMMM@MMH/.                  XM@MH; =;
  /%+%$XHH@$=              , .H@@@@MX,
   .=--------.           -%H.,@@@@@MX,
   .%MM@@@HHHXX$$$%+- .:$MMX =M@@MM%.
     =XMMM@MM@MM#H;,-+HMM@M+ /MMMX=
       =%@M@M#@$-.=$@MM@@@M; %M%=
         ,:+$+-,/H#MMMMMMM@= =,
               =++%%%%+/:-.
</pre>