# MovieTheatre
## How to use
1. Start a Windows build or Open the project in Unity and Play
1. Click "Select movies" and open movies list
1. Select a movie and open seats reservation screen
1. Select seats and click "Reserve" button -> Reservations screen opens
1. On Reservations screen you can change user and see its reservations, the user will be used to save next reservations
1. Reservations screen could be opened from the Home screen as well

## Time spent for the task
* Making UI (preparing scene and prefabs) - 1h
* Client logic with test data - 4h
* Back-end implementation options investigating - 2h
* Back-end implementation (SDK integration and data processing logic) - 3h
* Bugs fixing and cleanup - 2h
* Documenting and preparing sources - 2h

## Known bugs
1. Updated Data is being saved only for the current session. Updated Data isn't loading to the storage because of issues with authentication:
> The service sheets has thrown an exception. HttpStatusCode is Forbidden. Request had insufficient authentication scopes.

## Notes
1. There could be a more common structure to work with dynamically filled UI lists
1. Dependency Invertion is implemented by Singletons
1. Some simple buttons listeners are set up in Editor (I prefer to have all listeners in code, but it was a faster way)
1. UI mostly is responsive
1. Back-end is implemented with [Google Sheets](https://docs.google.com/spreadsheets/d/1BQDcoWa32zPNk-UhabLPRHtaQif1VKnPoObl6b93CH8/edit#gid=925917952). 
It's not very convenient way to store user data, but I have some recent experience with it, 
and it's kind of common way for games in Unity to store not so sensitive data like configurations.
