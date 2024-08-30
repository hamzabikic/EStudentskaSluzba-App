# EStudentskaSluzba Application

# Description 

The University Management System is a comprehensive application built with C# ASP.NET Core Web API for the backend and Angular with Bootstrap for the frontend. The application is localized in Bosnian language. The system is designed to facilitate the management of university operations, catering to the needs of various user groups, including administrative staff, professors, and students. Additionally, an SMTP service is utilized for sending emails when adding new professors and students. Upon registration, an email containing their username and password is sent to their email address. Similarly, when generating a new password by administrative staff to these users, the new password is sent to their email.

<h5>Key Features</h5>

Role-Based Access Control: The system implements role-based access control, providing different levels of permissions to three main user groups: administrative staff (with full permissions), professors, and students (with partial permissions).

Enhanced Security Measures: Stringent security measures are in place at both the frontend and backend to mitigate the risk of unauthorized access and misuse. This ensures the safety of sensitive data and prevents potential security breaches.

Real-world Usability: The application is designed to be deployed and used in real-world educational environments. It can seamlessly integrate into university workflows, assisting administrative staff in efficiently managing various administrative tasks.

<h5>Getting Started</h5>

Start the Angular project, and navigate to the login page.

Use the following credentials to access the system as an administrative staff member:

Username: admin

Password: admin

Student:

Username: test.student

Password: test

Profesor:

Username: test.profesor

Password: test

<h5>Future Enhancements</h5>
The University Management System is continually evolving. Future enhancements may include:

- Integration of additional features to cater to the specific needs of professors and students.
- Improved user interface and user experience enhancements.
- Implementation of advanced analytics and reporting capabilities for better insights into university operations.


# Instructions for Running the Web Application
Instructions for Running the Web Application
This repository contains an Angular web application developed as part of the project. Follow these steps to successfully run the application on your local machine.

Prerequisites Before running the application, make sure you have the following installed:

Node.js (version 12.x or newer)

npm (Node.js package manager, usually installed alongside Node.js)

Installation

Clone this repository to your local machine using the following command:

git clone https://github.com/hamzabikic/EStudentskaSluzba-App.git

Navigate to the application directory in your terminal:

cd repository-name

Install all the necessary dependencies by running:

npm install

Running Once you have successfully installed all the required dependencies, you can run the application using the following command:

npm start

This command starts a development server. Once the server is up and running, the application will be available at http://localhost:4200/ in your web browser.

# Instructions for Running the Web Api

Prerequisites: Before running the application, make sure you have installed the .NET Core SDK, which is required for building and running ASP.NET Core applications.

Clone the Repository: Clone the repository to your local machine using the provided URL.

Configuration: Configure your application settings in the appsettings.json file, including settings for the database, access keys, and other configuration parameters.

Database Setup: Before starting the web API, ensure that the SQL Server is installed and running on your local machine. Then, follow these steps:

- Open the NuGet Package Console in Visual Studio.
- Run the following command to add a migration (replace "name-of-migration" with a suitable migration name): add-migration name-of-migration
- After adding the migration, run the following command to update the database with the changes: update-database

Build the Application: Open a terminal or command prompt in the application directory and execute the following command to build the application:
dotnet build

Run the Application: After a successful build, run the application from the terminal or command prompt with the following command:
dotnet run

After starting the web API, the you need to manually call the "TestniPodaci/GenerisiTestnePodatke" endpoint. This step is necessary to initialize the database by creating basic objects needed for testing or demonstration purposes. Once the test data is generated, the application will be ready for use.

The application will start on a local server and will be accessible at http://localhost:44351 (check which port it is listening on in your application)

Testing API Endpoints: Utilize a tool like Postman or Swagger UI to test the API endpoints.

Swagger Integration: Swagger has been integrated into the application to provide interactive API documentation. Once the application is running, navigate to the Swagger endpoint (e.g., http://localhost:44351/swagger) to access the Swagger UI and explore the API endpoints.

# Screenshots

<img src="/Screenshots/login.JPG">

<h3>Administrative staff </h3>

<img src="/Screenshots/studenti.JPG">
<img src="/Screenshots/dodavanje-studenta.JPG">
<img src="/Screenshots/opstine.JPG">
<img src="/Screenshots/editovanje-studenta-by-referent.JPG">
<img src="/Screenshots/profesori.JPG">
<img src="/Screenshots/upisi.JPG">
<img src="/Screenshots/rate.JPG">
<img src="/Screenshots/ocjene.JPG">
<img src="/Screenshots/uredjivanje-profila.JPG">

<h3>Professors</h3>

<img src="/Screenshots/profesor-ocjene.JPG">
<p>Here you can see the restriction that the professor can only add, edit and delete grades from his subjects.</p>

<h3>Students</h3>

<img src="/Screenshots/student-edit-profila.JPG">
<img src="/Screenshots/student-ocjene.JPG">
<p>Here you can see the restriction that the student can only review his grades and change the password for his account. All other permissions are given to administrative staff.</p>

# License
This application is available under the MIT License. See the LICENSE file for more information.





