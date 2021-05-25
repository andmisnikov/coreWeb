#Project specification
Use ASP.net core + HTML theme UI

Create a simple web application that consist:

Login Page
o   When username and password match with the data in database, redirect to the User Management Page
o   If user key in wrong password, prompt an alert

User Management Page
o   Contain all the registered user information
-  In table layout
-  Admin allowed to delete/edit the data
-  Data must be able to export into Excel file
-  Generate graph (No of user register Vs. Date)
o   Add filtering features
-  Ex: Filter based on date registration

Add New User Page
o   Add new user forms which allow user to add new data

# Web App structure
.Net Core 3.1 MVC Web App with Identity

The solution contains 3 projects:
1) BL - Business Logic layer
2) DAL - Data Access Layer (EF)
3) WebApplication - .Net core MVC application + default identity with scaffolding

Get started guide:
1) Open appsettings.json file and change Database Connection (DefaultConnection) value if it is needed
2) Update database via "update-database" command 

Note to readers: If you haven't installed dotnet ef, you need to install it first: dotnet tool install --global dotnet-ef
