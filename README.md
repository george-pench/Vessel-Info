# Vessel Info

Vessel Info is a simple ASP.NET Core app for creating and storing tanker ships. 

# Project Description

The project is divided into two main sections:
- **An admin user** is responsible for managing basic CRUD operations as well as scraping a large amount of tankers related data.
- **A user** can look through more than 8000 tankers. All important information related to a specific vessel is shown on a dedicated page.

---

Apart from doing some basic CRUD operations on a given ship when logged as an admin you have an option for data scraping. Once the whole data is scrapped, it would automatically be persisted into the project's database. This process usually lasts quite some time due to a significant amount of information being processed. 

A user is granted access to detailed information about ship specifications like:

- *IMO (identification number)*
- *DWT (deadweight tonnage)*
- *LOA (length overall)*
- *Call Sign (a unique identifier)*
- *Flag and Port of Registry*
- *Type of Vessel*
- *Type of Hull*
- *Classification Society*
- *Owner*
- *Operator*

The data is being scrapped from: *https://www.q88.com/Ships.aspx?letter=A&v=list*

# Roles

- Admin
- Authorized User
- Non-Authorized User

# Admin Credentials

| Email            | Password |
| ---------------- | -------- |
| Admin@vessel.com | admin12  |

# Technologies Used

- ASP.NET Core 5.0
- Entity Framework Core 5.0
- MS SQL Server
- Bootstrap 4
- HTML5
- CSS
- MS Visual Studio 2019
- MS SQL Server Management Studio 2018
- AngleSharp 0.14.0
- AutoMapper
