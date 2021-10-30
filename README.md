
# khinsider-gui
A GUI for khinsider to download soundtracks (because of a paywall...)

![khi](https://user-images.githubusercontent.com/93412725/139517301-db1bbad9-660f-4842-a1f6-2cceb0e2c59b.jpg)

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

 1. Download latest version from the [Release Page](https://github.com/skynetDE/khinsider-gui/releases)
 2. Adjust the configuration to your needs
 3. Load the whole catalog via `File -> Refresh Catalog`
 4. Scroll through the albums or use the full text search field above the list
 5. Drag and Drop albums from the left to the right list
 6. Click on "Download" and choose a destination

In order to clear the list of selected albums you can either use the Delete-Key on your keyboard or use the "Clear" button.
