
DotNet Core 3.x Template Solution
=================================

This template demonstrates concept of separation of concerns (SOC) and is
a simple implementation of the Onion Architecture.

## Core Project
The Core project contains all domain entities 

### Security
Password hashing functionality added via the Security.Hasher class. This is used in the Data project DataService to hash the user password before storing in database.

## Data Project
The Data project encapsulates all data related concerns. Internally it
uses the Repository pattern implemented by EntityFramework Core to handle
data storage/retrieval. It defaults to using Sqlite for portability across
platforms.

The DataService is the only element exposed from this project
and consumers of this project simply need reference it to access its functionalty.

## Test Project
The Test project references the Data project and should implement unit tests to
test the functionalty of the DataService. The tests should be extended to fully
exercise the functionality of your DataService.

## Web Project
The Web project uses the MVC pattern to implement a web application. It references
the Data project and uses the DataService and Models to access data management
functionality. This allows the Web project to be completely independent of
EntityFrameworkCore (as used in this template) or any other persistence framework 
used in the data project.

The project also uses cookie based user Identity without using the boilerplate 
template used in the Visual Studio Web MVC project. This allows the developer to 
gain a better appreciation of how Identity is implemented. The data project
implements a User model and the DataService provides user management functionality
such as Authenticate, Register etc. The Web project implements a UserController
with actions for Login/Register/NotAuthorized/NotAuthenticated etc. 

The only element required to connect the User to Identity, is the private
method in the UserController that builds a ClaimsPrincipal. This is used in 
in the CookieBased authentication enabled in the Startup.cs and accessed in the
UserController Login method.

### Additional Functionality
Any Controller that inherits from the Web project BaseController, can utilise 
the Alert functionality. Alerts can be used to display alert messages following 
controller actions. 

`Alert("The User Was Registered Successfully", AlertType.info);`

Review the UserController for an example using alerts.

Two custom TagHelpers are included that provide 

1. Authentication and authorisation Tags

* `<p asp-authorized>Only displayed if the user is authenticated</p>`

* `<p asp-roles="Admin,Manager">Only displayed if the user has one of specified roles</p>`

2. Conditional Display Tag

* `<p asp-condtion="@some_boolean_expression">Only displayed if the condition is true</p>`


## Dependency Injection (DI)
The project template doesn't use DI to keep the template simple, however this is
something that is easy to turn on as you gain more appreciation of the benefits
of DI and how to use it in the solution.