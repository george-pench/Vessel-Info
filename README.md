# Vessel Info

Vessel Info is a simple ASP.NET Core app for creating and storing tanker ships. 

# Project Description

The project is divided into two main sections:
- **An admin user** is responsible for managing basic CRUD operations as well as scraping a large amount of tankers related data.
- **A user** can look through more than 8000 tankers. All important information related to a specific vessel is shown on a dedicated page.

---

Apart from doing some basic CRUD operations on a given ship when logged as an admin you have an option for data scraping. Once the whole data is scrapped, it would automatically be persisted into the project's database. This process usually takes quite some time due to a significant amount of information being processed. 

A user can sort all vessels by different criteria such as Registrations, Owners, Types, Class Societies and is granted access to detailed information about ship specifications like:

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

The data has been scrapped from: *https://www.q88.com/Ships.aspx?letter=A&v=list*

![Q88Sample](https://user-images.githubusercontent.com/97052397/199532689-5f47cf6e-6155-4a53-b998-5e805833418c.PNG)

The database's backup file can be found here: *https://github.com/george-pench/Vessel-Info/tree/main/Data/Vassel-Info.Data.Backup*

# Database Diagram

![DatabaseDiagram](https://user-images.githubusercontent.com/97052397/199499995-f137e550-b8e5-4967-9526-bf81d2422ad2.png)

# Project Layout

![Capture](https://user-images.githubusercontent.com/97052397/201772885-bfe3f210-897a-48b5-8d73-15be48f70c90.PNG)

# Roles

- Admin
- Authorized User
- Non-Authorized User

# Admin Credentials

| Email            | Password |
| ---------------- | -------- |
| Admin@vessel.com | admin12  |

# Technologies Used

- *ASP.NET Core 5.0*
- *Entity Framework Core 5.0*
- *MS SQL Server*
- *Bootstrap 4*
- *Bootstrap Lazy Kit*
- *HTML5*
- *CSS*
- *MS Visual Studio 2019*
- *MS SQL Server Management Studio 2018*
- *AngleSharp 0.14.0*
- *AutoMapper*
- *xUnit*
- *Fluent Assertions*
- *Moq*

## API Reference

```http
  GET /api/CountsApi - returns the count of vessels related data

  {"vesselsCount":8146,"registrationsCount":99,"ownersCount":6448,"typesCount":265,"classSocietiesCount":97,"operatorsCount":1531}
```

```http
  GET /api/VesselsApi - shows all vessels by basic specifications

  {"loa":"29.26","cubic":"\u00A0","beam":"9.14","draft":"3.05","hullType":"\u00A0","callSign":"WDI8684","id":"d7f5495a-7cfe-4ef5-8ebe-ec01a23eb22a","name":"A J","imo":"8890310","built":"1995","summerDwt":"109"}
```

```http
  GET /api/VesselsApi/{id} - returns a vessel by given id 

  {"loa":"49.90","cubic":"\u00A0","beam":"14.00","draft":"4.16","hullType":"DH","callSign":"5BMM5","id":"007d004e-8bf5-4015-81ec-e19b29ff0c4d","name":"Iris","imo":"9594119","built":"2010","summerDwt":"1,838"}
```

```http
  DELETE /api/VesselsApi/{id} - deletes a vessel by given id
```

# Images

- Register/Log in

![RegisterPNG](https://user-images.githubusercontent.com/97052397/199501388-3a6eb0ce-7d0e-42aa-b6d6-3a815af2f7e4.PNG)

![LogIn](https://user-images.githubusercontent.com/97052397/199501432-d67fafa2-9a79-495b-8fd9-4c9e077c9121.PNG)

- Homepage

![Homepage](https://user-images.githubusercontent.com/97052397/199531996-7245eb0d-23ff-4bfa-8458-99a84c3771df.PNG)

- Admin Create Page

![Create](https://user-images.githubusercontent.com/97052397/199533347-a8711fbd-c9ae-457a-8bba-4e4dd435b8d3.PNG)

- Details Page

![Details](https://user-images.githubusercontent.com/97052397/199534664-2ea33a93-7b9f-45a3-9077-d9cb5b1d5fc0.PNG)

- All Page

![All](https://user-images.githubusercontent.com/97052397/199534727-77a2903b-dfc8-4342-b3f3-f21dcc12a405.PNG)

- Delete page

![Capture](https://user-images.githubusercontent.com/97052397/200939915-9874372a-0002-4c02-bdbd-fa606af16e1e.PNG)

- Get All Vessels By Most Class Societies Page

![Capture](https://user-images.githubusercontent.com/97052397/209227085-a82bff74-875e-474a-9b25-2953728ac110.PNG)
