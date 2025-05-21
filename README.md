# Ava

Welcome to Ava, short for AufgabenVerwaltungsAnwendung.
This application was created by Miran K, Tina T, and Lars P for our Secure Software Development exam.
This is a task management application where users can create organisations or get added to existing organisations. Inside each organisation the admin can create projects and assign project managers to these. Each project can hold numerous project tasks.
The project manager can add more project tasks, and add Users from the organisation as Contributors to the project. The Contributors can then move tasks between the columns: To Do, In Progress, and Done.

## Setup

To get started using the project, simply follow the following steps:

1. Setup Environment Variables
   -  Open Git Bash in Windows
   -  Run ```openssl rand -hex 64``` to generate your [secretkey]
   -  Open your CMD in administrator mode
   -  Run the following command in your CMD and replace [secretkey] with the key you got in the earlier step ```setx AVA_JWT__Secret "[secretkey]"```
   -  Now run this command: ```setx AVA_JWT__Audience "ava-client"```
   -  Then run this command: ```setx AVA_JWT__Issuer "ava-api"```
   -  And finally this command: ```setx AVA_ConnectionStrings__DBConnection "Data Source=..\\Infrastructure\\app.db"```
   -  Note: setx sets environment variables for future sessions. You must restart your terminal or IDE for changes to apply.
  
2. Setting up the Application
   -  Once you have set the environment variables, clone this repository to your pc
   -  If you have already cloned it, restart your IDE and run ```echo %AVA_JWT__Issuer%``` or ```[Environment]::GetEnvironmentVariable("AVA_JWT__Issuer")``` if you are using CMD or Powershell respectively.
   -  If the terminal returns "ava-api", then you should be setup to go. 
   -  Next in your terminal run ```dotnet ef database update --project Infrastructure``` this will create a file called app.db under Infrastructure which will act as your database.
   -  Run ```dotnet dev-certs https --trust``` to create a self-signed certificate for development purposes.
   -  Navigate to Ava.API ```cd Ava.API``` and run the application ```dotnet run```
   -  Open a new terminal and navigate to the frontend ```cd Frontend```
   -  Run `npm install` to install frontend dependencies.
   -  Finally run a ```ng serve``` or ```npx ng serve``` if you dont have angular installed globally


## Using the application
You are now ready to use the application. The database seeder provides some users, organisations, projects and project tasks which you can use to try out the application. You can also create your own users using the register function.

The following users are present in the application:
  -  Alice
  -  Bob
  -  Charlie
  -  Dana
  -  Eve
The password for each user is "Test1234!" Enjoy!
