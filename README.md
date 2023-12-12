# EWU-CSCD396-2023-Fall

this was part of my final for this dev ops class
the goal was to create a service that used 5 different azure resources
I used a blob storage container, azure queue storage, a funtion app, an event grid system topic and a web app
the service would read from a txt file uploaded to blob storage and read the file line by line into a queue which would be accessed by the web app
the goal was to make a trivia game 
this repo only has the code and CI/CD for the web app

i have since deleted my azure resource group for this project

if i were to redo this the goals i have include
-branch on github for the fn app with CI/CD
-bicep infrastructure as code deployment of all resources
-proper use of github secrets for connection strings, or use of managed identity to avoid connection strings
-name the resources and files better
-create a better website ui
-make it so that more questions would be loaded into queue if it was found to be empty
-actually restrict file types with the event grid system topic, so that the blob trigger only triggers on certain files being uploaded
-implement the rest of the trivia game that i have the rules for.
