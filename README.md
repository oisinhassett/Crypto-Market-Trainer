
DotNet Core 5 Clean Template Solution
=====================================

This template demonstrates concept of separation of concerns (SOC) and is a simple implementation of the Clean Architecture. The template provides user/authentication management for both a Web MVC project using cookies and a Web API project using JWT.

## Core Project
The Core project contains all domain entities. 

<<<<<<< HEAD
General hashing functionality is provided via the ```Template.Core.Security.Hasher``` class. 

=======
>>>>>>> 06f0bb6abd421ae23dd2e49de57fb8bd966d9527
## Data Project
The Data project encapsulates all data related concerns. Internally it provides two implementations of the IUserService

1. Using a List that is persisted to a json file.
2. Using the Repository pattern implemented by Entityframework to handle data storage/retrieval. It defaults to using Sqlite for portability across platforms.

The Service is the only element exposed from this project and consumers of this project simply need reference it to access its functionalty.

### Security
Password hashing functionality added via the ```Template.Data.Security.Hasher``` class. This is used in the Data project DataService to hash the user password before storing in database.
## Test Project
The Test project references the Data project and should implement unit tests to test the functionalty of the IUserService. The tests should be extended to fully exercise the functionality of your Service.

## API Project
The API project provides a RESTful WebApi interface to User management. It references the Core and Data projects and uses the DataService and Models to access data management functionality. 

### Security
CORS is also enabled and JWT tokens are used for authentication and are configured via a helper class ```Template.Api.Helpers.JwtHelper``` called in ```Startup.cs```.

## Web Project
The Web project uses the MVC pattern to implement a web application. It references the Core and Data projects and uses the DataService and Models to access data management functionality. This allows the Web project to be completely independent of EntityFrameworkCore (as used in this template) or any other persistence framework used in the data project.

The project also uses cookie based user Identity without using the boilerplate template used in the Visual Studio Web MVC project. This allows the developer to gain a better appreciation of how Identity is implemented. The data project implements a User model and the UserService provides user management functionality such as Authenticate, Register etc. The Web project implements a UserController with actions for Login/Register/NotAuthorized/NotAuthenticated etc.
### Security
Authentication is configured via an authorisation helper class ```Template.Web.Helpers.AuthHelper``` that utilises CookieBased authentication. To enable authentication, call the ```AuthHelper.AddAuthSimple()``` extension method in Startup.cs configure services.

### Additional Functionality
The template replaces the locally installed Bootstrap 4 with Bootstrap 5(beta2) delivered via CDN link.

Any Controller that inherits from the Web project BaseController, can utilise the Alert functionality. Alerts can be used to display alert messages following controller actions.

`Alert("The User Was Registered Successfully", AlertType.info);`

Review the UserController for an example using alerts.

Two custom TagHelpers are included that provide 

1. Authentication and authorisation Tags

* `<p asp-authorized>Only displayed if the user is authenticated</p>`

* `<p asp-roles="Admin,Manager">Only displayed if the user has one of specified roles</p>`

2. Conditional Display Tag

* `<p asp-condtion="@some_boolean_expression">Only displayed if the condition is true</p>`

## Install Template

To install this solution as a template (template name is termonclean)

1. Download current version of the template

    ``` $ git clone https://github.com/termon/DotNetTemplate.git```

2. Install the template so it can be used by ```dotnet new``` command. Use the absolute path to the cloned template directory without trailing '/'

    ``` $ dotnet new -i /absolute_path/DotNetTemplate```

3. Once installed you can create a new project using this template

    ``` $ dotnet new termonclean -o SolutionName```