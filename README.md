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
- Visual Studio
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
| Column       | Type              | Description                               |
|--------------|-------------------|-------------------------------------------|
| `Id`         | SERIAL PRIMARY KEY| Unique identifier for each book           |
| `Title`      | CHARACTER VARYING | The title of the book                     |
| `Pages `     | INTEGER           | Number of pages the book has              |
| `Publisher`  | CHARACTER VARYING | The publisher of the book                 |
| `ReleaseYear`| INTEGER           | The year the book was released            |
| `Summary`    | CHARACTER VARYING | A short summary of the book's plot        |
| `ISBN`       | TEXT              | The international standard book number of the book|
| `Rating`     | DOUBLE PRECISION  | The rating of the book                    |


### Author Table:
| Column       | Type              | Description                               |
|--------------|-------------------|-------------------------------------------|
| `Id`         | SERIAL PRIMARY KEY| Unique identifier for each author         |
| `FirstName`  | CHARACTER VARYING | The first name of the author              |
| `LastName`   | CHARACTER VARYING | The last name of the author               |


### AuthorBook Table:
-> connects the book(s) with the author(s)

| Column       | Type              | Description                               |
|--------------|-------------------|-------------------------------------------|
| `Id`         | SERIAL PRIMARY KEY| Unique identifier for each book           |
| `AuthorId`   | INTEGER           | The Id of the author                      |
| `BookId `    | INTEGER           | The Id of the book              |


## Impressions
[![BookCatalogueGUI](https://www.youtube.com/watch?v=faRJLwTrd4o)

![Main Menu](https://github.com/user-attachments/assets/7f2b9300-3bc4-48bc-a630-edf491c4a69f)

![Main Menu with data](https://github.com/user-attachments/assets/4bc0cb1e-c38b-4944-b8a9-bc16349b3ea8)

![Add a book](https://github.com/user-attachments/assets/8cfcf947-e509-476f-a583-669f3c5f72f2)

![Rate a book](https://github.com/user-attachments/assets/bc14c44c-a041-4b61-a74b-33349f923fc6)


## Notes
**Rating System:** The ratings are based on multiple categories such as plot, characters, writin, etc. The system calculates the average of these ratings to assign an overall rating to the book. This makes it easier to compare the ratings of different books.

**Error Handling:** I implemented methods for error handling that notify the user when they input invalid data.
