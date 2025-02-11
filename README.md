# BookCatalogueGUI
The BookCatalogueGUI is a Windows Forms application of my bookCatalogue (which you can find here: https://github.com/H0ckeyKeks/bookCatalogue), designed to manage a collection of books. It allows users to **view**, **add**, **delete** and **rate** books in a personal catalogue. This application uses PostgreSQL as the backend database to store the books and their details (Title, Author, Publisher, etc.).

## Features
**View Books**: Display a list of books with details such as title, author, number of pages, publisher, isbn, release year and a summary.
**Add Books:** Add new books to the catalogue with the above mentioned details.
**Rate Books:** Rate books based on different categories (plot, characters, writing, etc.) and add the calculated average rating to the database.
**Delete Books:** Remove books from the catalogue.


## Requirements
Before running the application, make sure to have the following installed:
- Microsoft Visual Studio (with Windows Forms support)
- PostgreSQL (for backend database)
- Npgsql (PostgreSQL .NET data provider)


## Technologies used
- C# (.Net8)
- SQL Server (PostgreSQL) -> database storage
- Npgsql -> database operations
- Environment variables -> secure database credentials


## Security
This project uses an environmental variable to store the database password (sensitive data).

## Windows (Command prompt)
```bash
setx DATABASE_PASSWORD "your_secure_password"
```


## Database Schema
### Book Table:




### Author Table:



### AuthorBook Table:



## Look
![Main Menu](https://github.com/user-attachments/assets/7f2b9300-3bc4-48bc-a630-edf491c4a69f)
