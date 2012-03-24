namespace Speech
{
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

    public class Program
    {
        public static SerialPort port;
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

            #region Port Checking
            SerialPort.GetPortNames().Count(); //set this as a name somewhere
            #endregion

            sensor.Start();

            // Obtain the KinectAudioSource to do audio capture
            KinectAudioSource source = sensor.AudioSource;
            source.EchoCancellationMode = EchoCancellationMode.None; // No AEC :(
            source.AutomaticGainControlEnabled = false; // Important to turn this off for speech recognition

            //RecognizerInfo ri = GetKinectRecognizer();
            #region Check for Audio SDK
            if (GetKinectRecognizer() == null)
            {
                Console.WriteLine("Could not find Kinect speech recognizer! You should probably install the Audio SDK for Kinect (released by Microsoft)"); //Put a download link here to get the audio sdk from microsoft for kinect
                return;
            }
            #endregion

            int wait = 2;
            while (wait > -1)//stops printing at 0 seconds
            {
                Console.Write("Device will be ready for speech recognition in {0} second(s).\r", wait--);//overwrite last printed statement
                Thread.Sleep(1000);
            }
            
            using (var sre = new SpeechRecognitionEngine(GetKinectRecognizer().Id))
            {                
                var colors = new Choices(); //Change this variable from colors to Commands

                colors.Add("Dim plus far window shades"); 
                colors.Add("Dim minus far window shades");
                colors.Add("Dim plus computer window shades");
                colors.Add("Dim minus computer window shades");
                colors.Add("Open far window shades");
                colors.Add("Close far window shades");
                colors.Add("Close computer window shades");
                colors.Add("Open computer window shades");

                var gb = new GrammarBuilder { Culture = GetKinectRecognizer().Culture };

                // Specify the culture to match the recognizer in case we are running in a different culture.                                 
                gb.Append(colors);
                                    
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

        public static int SerialCount { get; set; }
    }
}
