
# khinsider-gui
A GUI for khinsider to download soundtracks (because of a paywall...)

![khi](https://user-images.githubusercontent.com/93412725/139516044-659e45c7-2606-4804-82cd-f13289b3b707.jpg)

### Features

 - Catalog
	 - Fetch and cache the whole web catalog of albums
	 - Load catalog updates on start or on demand
	 - Full text search through the whole catalog in milliseconds
 - Downloading
	 - Download any number of albums in one go
	 - Choose a preferred file type (FLAC, MP3, other...) with fallback to MP3
	 - Retries on failure
	 - Skip or overwrite files if they already exist
	 - Download all (cover) images attached
	 - Creation of sub-folders for discs

## Requirements
 - Windows 7 or newer
 - .NET Framework v4.6.1 or higher

## Usage

 1. Adjust the configuration to your needs
 2. Load the whole catalog via `File -> Refresh Catalog`
 3. Scroll through the albums or use the full text search field above the list
 4. Drag and Drop albums from the left to the right list
 5. Click on "Download" and choose a destination

In order to clear the list of selected albums you can either use the Delete-Key on your keyboard or use the "Clear" button.
