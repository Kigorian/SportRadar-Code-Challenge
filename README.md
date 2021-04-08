# Sportradar Code Challenge
	A demonstration of an ETL pipeline based on NHL APIs.

 Purpose:
	As a Data Analyst, I want to be able to quickly generate CSV files for a given
	hockey team or player. The data for these CSV files should come from the API at
	https://statsapi.web.nhl.com/api/v1/.

 Expected Behavior:
	1. Begin a console instance in Visual Studio.
	2. User is prompted to select Team or Player.
	3. User is prompted for an ID.
	4. User is prompted for a year.
	5. The application sends a request to the appropriate API endpoint.
	6. The API returns a JSON response with the requested data.
	7. The application parses the JSON and generates a CSV file with the data.
	8. User is prompted for a directory path and a file path.
	9. CSV file is saved to that path, if it is valid.

 Instructions to run:
	1. Checkout Git repo in Visual Studio 2019.
	2. Right-click the NHL_API project.
	3. Choose Debug >> Start New Instance.

 Highlights:
	- Resources/Enums/PipelineType.cs is an enum of pipeline types, and the code is set up
	to use switch statements in most places. This would make it easy to expand this application
	to support other pipeline types, such as a Tournament pipeline.
	- Resources/JsonConverters provides a library of custom JSON converters. These are used in
	this project to serialize the API JSON responses into usable objects. This provides flexibility
	to handle complex JSON from multiple responses, including things like nested objects as in the
	case of SeasonJsonConverter, which has a nested serializer for Game entities.
	- All required functions are neatly organized into their own Service classes. Not only does this
	make it easier to find a logical area of the code, it also keeps the main program loop clean and
	free of minutia. This helps future developers, who are maintaining the code, to quickly and intuitively
	understand what's happening in the code. It also has the benefit of eliminating code duplication,
	as these service functions can be re-used in many places.
	- Comments. Comments are very near and dear to me, as I feel they break up the monotany of the code
	and help give a more human-readable flow to the logic. You'll find both function summaries and common
	comments everywhere in this project.
	- Error handling. Try/Catch blocks to print helpful messages and allow the program to gracefully
	continue in the event of an error.

 Areas to Improve:
	- If this were a real project for production, I would spend more time on the JSON Converters
	and their associated Models. They could be fully fleshed out to handle every facet of the API.
	But for the sake of time and sanity, I just went with the key entities.
	- Testing. If this were going to production, I would absolutely write a test for every logical
	part of this project. But a lot of that is just tedious, and for the sake of this demo, the skills
	can be demonstrated with just a few tests.

 Test Strategy:
