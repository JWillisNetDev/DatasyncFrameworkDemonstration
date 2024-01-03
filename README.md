## Installation and Usage
- Ensure you have SQL Server Express installed.

- I recommend using Multiple Startup Projects to run both the API and the client at the same time
	- Right-click on the solution and select Configure Startup Projects.
	- Select Multiple Startup Projects.
	- Set the Action for both the API and MAUI to Start. Data is a DLL and cannot be started.
	- Click OK.