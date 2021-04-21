# Ascii-Art Converter

## About The Project
Ascii-Art Converter is a converter that converts images to ASCII-based images for Windows.

### Download
You can download the executable [here](https://github.com/cauemarcovich/AsciiArtConverter/releases/tag/v1.0).

### Prerequisites

[.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1), for non-standalone version.

### Usage

Run the executable via the command prompt.

Parameters:
```sh
-file -> the image file path that you want to convert. [Required]  
-maxWidth -> maximum width to the ASCII image that will be created.
```

E.g.:
```sh
.\AsciiArtConverter.exe -file C:\some_path\generic_image.jpg
```