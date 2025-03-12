# Youtube Convertor

This is a simple console application that allows you to convert YouTube videos to MP3 and MP4 formats. The application uses the `YoutubeExplode` library to fetch video data, and the `Xabe.FFmpeg` library to mux audio and video streams.

## Features
- Fetch video data (title, author, duration) from a given YouTube URL.
- Download the video in MP3 format.
- Download the video in MP4 format.
- Mux MP3 audio and MP4 video into a single MP4 file.

## Prerequisites

Before running the application, make sure you have the following installed:

- **.NET SDK**: Required to build and run the C# application.
- **FFmpeg**: Required for muxing audio and video streams.

## Getting Started

Follow these steps to run the application:

### 1. Clone the Repository

Clone the repository to your local machine:

```bash
git clone <repository_url>
```

### 2. Build the Application

Navigate to the project directory and build the application:

```bash
dotnet build
```

### 3. Run the Application

Run the application:

```bash
dotnet run
```

### 4. User Interface

Once the application starts, you'll be presented with the main menu with the following options:

- **Run application**: Begin processing the video URL.
- **Exit application**: Exit the application.

If you choose to run the application, you'll be prompted to insert a YouTube URL. After entering the URL, you can choose from the following options:

- **View URL data**: View information about the video, such as its title, author, and duration.
- **Download MP3**: Download the video as an MP3 file.
- **Download MP4**: Download the video as an MP4 file.
- **Back**: Go back to the previous menu to insert a new URL.
- **Exit**: Exit the application.

## Functionality

1. **Insert URL**: 
   - The user is prompted to enter a YouTube URL.
   - The application validates the URL and fetches video data.
   
2. **View URL Data**:
   - Displays the video title, author, and duration.
   
3. **Download MP3**:
   - Downloads the audio stream of the YouTube video and saves it as an MP3 file.

4. **Download MP4**:
   - Downloads both the video and audio streams separately, then muxes them into a single MP4 file using FFmpeg.

## Libraries Used

- **YoutubeExplode**: A library for fetching video data and streams from YouTube.
- **Xabe.FFmpeg**: A wrapper for FFmpeg to handle video and audio muxing.

## Error Handling

The application includes retry logic for the following operations:

- Fetching video data from YouTube.
- Downloading MP3 and MP4 files.
- Muxing audio and video streams.

It will retry up to three times before reporting an error.

## License

This project is open-source and available under the [MIT License](LICENSE).

---

This README should help you get started with the application. You can expand on it based on any additional features or specific details you may want to highlight!
