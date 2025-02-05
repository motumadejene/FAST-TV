FastTV App
A Unity-based Android application for exploring movies using The Movie Database (TMDb) API. The app allows users to search for movies, view detailed information, and browse trending, now playing, top-rated, and upcoming movies. The project is designed with best practices, including SOLID principles and unit tests.

You can download the latest APK from this Google Drive link.
https://drive.google.com/file/d/1L9rZnd7_tClp4QZWVBNZqdA5knW9FtSB/view?usp=sharing

Features
- Movie Search: Search for movies by name and view results with posters.
- Detailed View: View detailed information about a movie, including the title, overview, release year, genres, and ratings..
- Categorized Browsing: Explore trending, now playing, top-rated, and upcoming movies.

Getting Started
  Prerequisites
  - Unity (Ensure Android Build Support is installed)
  - TMDb API Key (Sign up and generate your API key)
  - Git

Setup Instructions
 - Clone the Repository:
  git clone https://github.com/motumadejene/FAST-TV.git
    1. cd FAST-TV
    2. Open in Unity
      - Open the Unity Editor.
      - Load the FAST-TV project folder.
    3. Set API Key
      - On first launch, the app will prompt you to input your TMDb API key.
    4. Run the Application
      - Set the platform to Android in Build Settings.
      - Test the app in the Unity Editor or deploy it to an Android device.

Architecture
 - Code Structure
    - Scripts:
      - MovieDetailsUI.cs: Handles the detailed view of a movie.
      - FeaturedMoviesUI.cs: Manages the featured movies and categories.
      - APIKeyManager.cs: Handles the TMDb API key storage and validation.
      - GenreManager.cs: Maps genre IDs to genre names.
    - Design Patterns
      - Singletons: Used for managing UI components like MovieDetailsUI.
      - Coroutines: For handling asynchronous API calls and image loading.
      - Separation of Concerns: Dedicated managers for API, genres, and movie details.
     
Future Improvements

- Offline Mode: Cache movie data for offline access.
- Localization: Add support for multiple languages.
- Advanced Filtering: Allow filtering by genres, release year, etc.
