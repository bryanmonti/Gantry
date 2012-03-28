#region Using
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
#endregion
namespace Speech
{ //Pissing people off with #regions since Microsoft Visual 2008.
    public class Program
    {
        public static System.IO.Ports.SerialPort port;//try using
        public static void Main(string[] args)
        {
            int baud;
            string name;

            #region Kinect Finding
            // Obtain a KinectSensor if any are available
            KinectSensor sensor = (from sensorToCheck in KinectSensor.KinectSensors where sensorToCheck.Status == KinectStatus.Connected select sensorToCheck).FirstOrDefault();
            #endregion

            #region Kinect Checking
            if (sensor == null)
            {
                Console.WriteLine(
                        "No Kinect sensors are attached to this computer or none of the ones that are\n" +
                        "attached are \"Connected\".\n" +
                        //"Attach the KinectSensor and restart this application.\n" +
                        //"If that doesn't work run SkeletonViewer-WPF to better understand the Status of\n" +
                        //"the Kinect sensors.\n\n" +
                        "Press any key to continue.\n");

                // Give a chance for user to see console output before it is dismissed
                Console.ReadKey(true);
                return;
            }
            #endregion

            #region Port Checking + Counting
            SerialPort.GetPortNames().Count(); //counts available ports (set this as a name somewhere)
            #endregion

            #region Activates Kinect Sensor
            sensor.Start();
            #endregion

            #region Obtains KinectAudioSource
            KinectAudioSource source = sensor.AudioSource;
            source.EchoCancellationMode = EchoCancellationMode.None; // No AEC :(
            source.AutomaticGainControlEnabled = false; // Important to turn this off for speech recognition
            #endregion

            #region Check for Audio SDK
            if (GetKinectRecognizer() == null)
            {
                Console.WriteLine("Could not find Kinect speech recognizer! You should probably install the Audio SDK for Kinect (released by Microsoft)"); //Put a download link here to get the audio sdk from microsoft for kinect
                return;
            }
            #endregion

            #region Writes Options
            Console.WriteLine("Enter your parameters to begin");
            Console.WriteLine(" ");
            Console.WriteLine("If no ports are displayed below, please check your connection to the serial device");
            Console.WriteLine("Available ports:");
            #endregion

            #region Available Port Printing
            if (System.IO.Ports.SerialPorts.GetPortNames())
            {
                foreach (string p in System.IO.Ports.SerialPort.GetPortNames())
                {
                    Console.WriteLine(p);
                }
            }
            else
            {
                Console.WriteLine("No Ports are available. Press any key to quit!");
                Console.ReadLine();
                return; //Quit
            }
            #endregion

            #region Gets Port Name + Baud
            Console.WriteLine("Port Name:");
            name = Console.ReadLine();
            Console.WriteLine(" ");
            Console.WriteLine("Baud rate:\n" +
                               "1. 300\n" +
                               "2. 1200\n" +
                               "3. 2400\n" +
                               "4. 4800\n" +
                               "5. 9600\n" +
                               "6. 14400\n" +
                               "7. 19200\n" +
                               "8. 28800\n" +
                               "9. 38400\n" +
                               "10. 57600\n" +
                               "11. 115200\n");
            #endregion

            int wait = 2;
            while (wait > -1)//stops printing at 0 seconds
            {
                Console.Write("Device will be ready for speech recognition in {0} second(s).\r", wait--);//overwrite last printed statement
                Thread.Sleep(1000);
            }
            
            using (var sre = new SpeechRecognitionEngine(GetKinectRecognizer().Id))
            {                
                var commands = new Choices(); //Change this variable from colors to Commands Update: DONE BITCHES

                commands.Add("Dim plus far window shades");
                commands.Add("Dim minus far window shades");
                commands.Add("Dim plus computer window shades");
                commands.Add("Dim minus computer window shades");
                commands.Add("Open far window shades");
                commands.Add("Close far window shades");
                commands.Add("Close computer window shades");
                commands.Add("Open computer window shades");

                //Might have to fix below to one line
                var gb = new GrammarBuilder
                { 
                    Culture = GetKinectRecognizer().Culture
                };

                // Specify the culture to match the recognizer in case we are running in a different culture.                                 
                gb.Append(commands);
                                    
                // Create the actual Grammar instance, and then load it into the speech recognizer.
                var g = new Grammar(gb);                    

                sre.LoadGrammar(g);
                sre.SpeechRecognized += SreSpeechRecognized;
                sre.SpeechHypothesized += SreSpeechHypothesized;
                sre.SpeechRecognitionRejected += SreSpeechRecognitionRejected;

                using (Stream s = source.Start())
                {
                    sre.SetInputToAudioStream(
                        s, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));

                    Console.WriteLine(" ");
                    Console.WriteLine("What would you like me to do?\n" +
                                        "  \n" +
                                        "1) Dim Plus Far Window Shades\n" +
                                        "  \n" +
                                        "2) Dim Minus Far Window Shades\n" +
                                        "  \n" +
                                        "3) Dim Plus Computer Window Shades\n" +
                                        "  \n" +
                                        "4) Dim Minus Computer Window Shades\n" +
                                        "  \n" +
                                        "5) Open Far Window Shades\n" +
                                        "  \n" +
                                        "6) Close Far Window Shades\n" +
                                        "  \n" +
                                        "7) Open Computer Window Shades\n" +
                                        "  \n" +
                                        "8) Close Computer Window Shades\n");

                    sre.RecognizeAsync(RecognizeMode.Multiple);
                    Console.ReadLine();
                    Console.WriteLine("Stopping everything...gimmie a sec");
                    sre.RecognizeAsyncStop();                       
                }
            }

            sensor.Stop();
        }

        private static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }

        private static void SreSpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs audio)
        {
            Console.WriteLine("\nSpeech not recognized");
            if (audio.Result != null)
            {
                DumpRecordedAudio(audio.Result.Audio);
            }
        }

        private static void SreSpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            Console.Write("\rSpeech Hypothesized: \t{0}", e.Result.Text);
        }

        private static void SreSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= 0.7)
            {
                Console.WriteLine("\nSpeech Recognized: \t{0}\tConfidence:\t{1}", e.Result.Text, e.Result.Confidence);
                //Add a line here that sends recieved audio to serial
                if(e.Result.Text == "open far window shades")
                {
                    //send to serial
                }
                if (e.Result.Text == "WHATEVER ELSE")
                {
                    //send to serial
                }
                //so on and so fourth
            }
            else
            {
                Console.WriteLine("\nSpeech Recognized but confidence was too low: \t{0}", e.Result.Confidence);
                DumpRecordedAudio(e.Result.Audio); //deletes extra audio after being analyzed
            }
        }

        private static void DumpRecordedAudio(RecognizedAudio audio)
        {
            if (audio == null)
            {
                return;
            }

            int fileId = 0;
            string filename;
            while (File.Exists((filename = "RetainedAudio_" + fileId + ".wav")))
            {
                fileId++;
            }

            Console.WriteLine("\nWriting file: {0}", filename);
            using (var file = new FileStream(filename, System.IO.FileMode.CreateNew))
            {
                audio.WriteToWaveStream(file);
            }
        }

        //public static int SerialCount { get; set; }
    }
}
