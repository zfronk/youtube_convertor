using System;
using System.IO;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using System.Linq;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using System.Text.RegularExpressions;


namespace YoutubeConvertorNamespace
{
    public class YoutubeConvertor
    {
        //load user interface
        public static async Task LoadMainMenu()
        {
            
            await MainUserInterface(); // call main interface

        }

        //user Interface - Method for main interface
        private static async Task MainUserInterface()
        {
            //Main interface - - 
            Console.WriteLine("Z - Youtube Convertor");
            Console.WriteLine();

            Console.WriteLine("Main User Interface");
            Console.WriteLine("- - - - - - - - -  ");
            Console.WriteLine("1.Run application");
            Console.WriteLine("2.Exit application");

            Console.WriteLine();

            while (true)
            {

                Console.WriteLine("Choose an option from the main UI:");
                string? mainUIOption = Console.ReadLine();

                switch (mainUIOption)
                {
                    case "1":
                        
                        await InsertUrl();
                        break;

                    case "2":
                        Console.WriteLine();
                        Console.WriteLine("Initilized exit command!");
                        Console.WriteLine("- - - - - - - -");
                        Console.WriteLine("Exiting application...");
                        Environment.Exit(0);
                        return;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Invalid option!");
                        Console.WriteLine();
                        break;

                }
            }



        }


        //method to insert url
        private static async Task InsertUrl()
        {

            await Task.Delay(1000);
            Console.WriteLine();
            Console.WriteLine("Welcome to the application ");
            Console.WriteLine("- - - - - - ");
            Console.WriteLine();


            while (true)
                {
                //prompt for user url

                Console.WriteLine($"Insert youtube url:");
                string? urlInserted = Console.ReadLine();


                //condition - - - if not null

                 if (!string.IsNullOrWhiteSpace(urlInserted))
                 {

                    Console.WriteLine();
                    Console.WriteLine("Url submitted!");
                    Console.WriteLine();

                    await OnUrlSubmission(urlInserted); // call method on valid url
                    Console.WriteLine();

                    
                 }

                    else
                    {

                        Console.WriteLine("No url inserted!");
                        Console.WriteLine();
                    }

                }

        }
           

        
        
        

        //Method on url submission - - - Parameter to get url inserted

        private static async Task OnUrlSubmission(string url)
        {
            //method to display Menu
            

            //display Menu
            DisplayMainMenu();

            

            while (true)
            {

                

                //prompt for a navigation option
                Console.WriteLine();
                Console.WriteLine("Choose an option from the Main - Menu:");

                string? userOption = Console.ReadLine();



                switch (userOption)
                {
                    case "1":

                        await FetchUrldata(url);
                        Console.WriteLine();
                        break;

                    case "2": 
                        Console.WriteLine();
                        Console.WriteLine("Downloading mp3");
                        Console.WriteLine("- - - - -");
                        await DownloadMp3(url);
                        Console.WriteLine();
                        break;

                    case "3":
                        Console.WriteLine();
                        Console.WriteLine("Downloading mp4");
                        Console.WriteLine("- - - - -");
                        await DownloadMp4(url);
                        Console.WriteLine();
                        break;

                    case "4":
                        Console.WriteLine();
                        Console.WriteLine("Returned to application UI");
                        
                        Console.WriteLine();
                        await InsertUrl();
                        break;

                    case "5":
                        Console.WriteLine();
                        Console.WriteLine("Initilized exit command!");
                        Console.WriteLine("- - - - - - - -");
                        Console.WriteLine("Exiting application");
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Invalid option!");
                        
                        break;
                }
            }
        }

        //method to fetch url data 

        private static async Task FetchUrldata(string url)
        {
            

            Console.WriteLine();
            Console.WriteLine("Displaying url Data");
            Console.WriteLine("- - - - - - - -  ");

            //retry three times when request is called if cache aint saved
            int numberOfTries = 0;
            bool success = false;

            //loop
            while (!success && numberOfTries < 3)
            {
                try
                {
                    //using YoutubeExplode
                    Console.WriteLine("Please wait!");
                    var youtube = new YoutubeClient();
                    var video = await youtube.Videos.GetAsync(url);


                    //display the data on Console
                    Console.WriteLine();
                    Console.WriteLine("Video Data");
                    Console.WriteLine("- - - - - -");
                    Console.WriteLine($"Title: {video.Title}");
                    Console.WriteLine($"Author: {video.Author}");
                    Console.WriteLine($"Length: {video.Duration}");
                    success = true;

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Opps couldn't fetch url data: {e.Message}");
                    Console.WriteLine($"Trial {numberOfTries + 1} failed!");
                    Console.WriteLine();
                    numberOfTries++;

                    if (numberOfTries < 3)
                    {

                        Console.WriteLine("Retrying...");
                        await Task.Delay(1000);

                    }
                }


            }











        }


        //method to download url - - Mp3

        private static async Task DownloadMp3(string url)
        {

            bool success = false;
            int numberOfTries = 0;

            while (!success && numberOfTries < 3)
            {

                try
                {

                    //initiate instance class
                    var youtube = new YoutubeClient();
                    var youtubeVideo = await youtube.Videos.GetAsync(url);
                    var streamManifest = await youtube.Videos.Streams.GetManifestAsync(url); //await stream manifest

                    //get the highest bitrate
                    var audioStreamInfo = streamManifest.GetAudioStreams().GetWithHighestBitrate();

                    //filename
                    var fileName = $"{youtubeVideo.Title}.mp3";
                    fileName = SantizeFileName(fileName);


                    //filepath
                    var downloadFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                    var filePath = Path.Combine(downloadFolder, fileName);

                    //report progress
                    

                    //download stream
                    Console.WriteLine("Please wait!");
                    Console.WriteLine();
                    await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, filePath);

                    Console.WriteLine($"{youtubeVideo.Title} successfully downloaded!");
                    Console.WriteLine();
                    Console.WriteLine("File Location");
                    Console.WriteLine("- - - - - - - ");
                    Console.WriteLine($"Location: {filePath}!");
                    success = true;




                }
                catch (Exception error)
                {

                    Console.WriteLine($"Opps couldn't download mp3: {error.Message}");
                    Console.WriteLine($"Trial {numberOfTries + 1} failed!");
                    numberOfTries++;

                    if (numberOfTries < 3)
                    {
                        Console.WriteLine("Retrying...");
                        Console.WriteLine();
                        await Task.Delay(1500);
                    }
                }

            }



        }
       
        //method to download Mp4

        private static async Task DownloadMp4(string url)
        {

            int numberOfTries = 0;
            bool success = false;



            while (!success && numberOfTries < 3)
            {
                try
                {

                    //initiate instance to the request
                    var youtube = new YoutubeClient();
                    var video = await youtube.Videos.GetAsync(url);

                    //call manifest - - display streams
                    var streamManifest = await youtube.Videos.Streams.GetManifestAsync(url);

                    //audio stream and video stream
                    var audioStreamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                    var videoStreamInfo = streamManifest.GetVideoOnlyStreams().GetWithHighestBitrate();

                    //return if no stream found
                    if (audioStreamInfo == null || videoStreamInfo == null)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Url lacks either video stream or audio stream!");
                        return;
                    }

                    //pc download folder
                    var downloadFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

                    //sanitize filenames
                    var audioFileName = $"{video.Title}.mp3";
                    audioFileName = SantizeFileName(audioFileName);

                    var videoFileName = $"{video.Title}.mp4";
                    videoFileName = SantizeFileName(videoFileName);

                    var outputFileName = $"{video.Title}_Muxed.mp4";
                    outputFileName = SantizeFileName(outputFileName);


                    //filepath

                    var audioFilePath = Path.Combine(downloadFolder, audioFileName);
                    var videoFilePath = Path.Combine(downloadFolder, videoFileName);
                    var outputPath = Path.Combine(downloadFolder, outputFileName);

                    //download Mp3
                    Console.WriteLine("Downloading mp3...");
                    Console.WriteLine("Please wait!");
                    Console.WriteLine();


                    //progress bar audio



                    await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, audioFilePath );
                    Console.WriteLine("Mp3 download complete!");
                    Console.WriteLine();

                    //downaload mp4
                    Console.WriteLine("Downloading mp4...");
                    Console.WriteLine("Please wait!");
                    Console.WriteLine();

                    //progress bar video
                    
                    await youtube.Videos.Streams.DownloadAsync(videoStreamInfo, videoFilePath );
                    Console.WriteLine("Mp4 download complete!");
                    Console.WriteLine();

                    //mux the two downloads
                    Console.WriteLine("Muxing video and audio...");
                    Console.WriteLine();
                    await MuxVideoAndAudio(videoFilePath, audioFilePath, outputPath);

                    Console.WriteLine();
                    Console.WriteLine($"Download complete \nFile location: {outputPath}");
                    success = true;

                }

                catch (Exception error)
                {

                    Console.WriteLine();
                    Console.WriteLine($"Opps couldn't download mp4: {error.Message}");
                    numberOfTries++;

                    if (numberOfTries < 3)
                    {
                        Console.WriteLine("Retrying...");
                        Console.WriteLine();
                        await Task.Delay(1500);
                    }

                }

            }
            
           
           

        }
        

        //method to mux streams --  using Xabe.FFmpeg 
        private static async Task MuxVideoAndAudio(string videoPath , string audioPath , string outputPath)
        {
            bool success = false;
            int numberOfTries = 0;

            while (!success && numberOfTries < 3)
            {
                try
                {

                    //get info - - Metadata
                    var videoInfo = await FFmpeg.GetMediaInfo(videoPath);
                    var audioInfo = await FFmpeg.GetMediaInfo(audioPath);

                    //get stream
                    var videoStream = videoInfo.VideoStreams.FirstOrDefault();
                    var audioStream = audioInfo.AudioStreams.FirstOrDefault();

                    //call process
                    var conversionProcess = FFmpeg.Conversions.New()
                        .AddStream(videoStream)
                        .AddStream(audioStream)
                        .SetOutput(outputPath);

                    //Progress

                    

                    //event handler

                    

                    Console.WriteLine("Starting muxing process!");
                    Console.WriteLine("- - - - - - - ");
                    Console.WriteLine("Please wait!");
                    await conversionProcess.Start();
                    Console.WriteLine();
                    Console.WriteLine("Muxing Complete!");
                    success = true;





                }
                catch (Exception error)
                {
                    Console.WriteLine($"Opps failed to mux streams: {error.Message}");
                    Console.WriteLine($"Trial {numberOfTries + 1} failed!");
                    numberOfTries++;

                    if (numberOfTries < 3)
                    {
                        Console.WriteLine("Retrying...");
                        Console.WriteLine();
                        await Task.Delay(1500);
                    }
                    
                }

            }
            
        }

        //sanitize filename
       private static string SantizeFileName(string fileName)
        {
            foreach(var invalidCharacter in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(invalidCharacter, '_');
            }

            return fileName;
        }
        

        //method to display Main menu
        private static void DisplayMainMenu()
        {

            //display menu
            Console.WriteLine("Main - Menu");
            Console.WriteLine("- - - - - - - ");
            Console.WriteLine("1.View url data");
            Console.WriteLine("2.Download mp3");
            Console.WriteLine("3.Download mp4");
            Console.WriteLine("4.Back");
            Console.WriteLine("5.Exit");

        }
    } 
    //excetuble class
    class Program
    {
        static async Task Main(string[] args)
        {

            await YoutubeConvertor.LoadMainMenu();

        }
    }
}