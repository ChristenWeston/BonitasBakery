# Bonita's :smile_cat: Bakery :croissant: :bagel: :cake: :cupcake: :doughnut:

### Created by Christen Weston

#### This project display Bonita's Bakery's Flavors and Treats! Ensures that only users that are logged in can edit, delete, or add flavors and treats. All users can view the flavors and treats.

## Technologies Used :woman_technologist:

* Git
* C#
* .NET 5.0
* MVC
* Razor
* Entity
* CSS
* SQL

## Setup Installation Requirements :scroll:

1. :white_medium_square:  Clone the BonitasBakery repository
2. :white_medium_square:  Open in your favorite IDE
3. :white_medium_square:  CD into bonitas-bakery
4. :white_medium_square:  Ensure MySql workbench is installed on your machine. If needed, please visit this site to download and install: [MySQLWorkBench]("https://www.mysql.com/products/workbench/")

5. :white_medium_square:  Create an appsettings.json
6. :white_medium_square: :keyboard: Add the following to your appsettings.json replacing the ```[YourPasswordHere]``` with your password:
```
{
  "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Port=3306;database=bonitas_bakery;uid=root;pwd=[YourPasswordHere];"
  }
}
```
7. :white_medium_square: :keyboard: Install dotnet globally by running "``dotnet tool install --global dotnet-ef``"
8. :white_medium_square: :keyboard: run "``dotnet ef database update``"
9. :white_medium_square: :keyboard: run "``dotnet build``"
10. :white_medium_square: :keyboard: run "``dotnet run``" to use the program
11. :white_medium_square:  Enjoy! :partying_face:

# Known Bugs
None

## Date Published
>April 2022

## License Info
[MIT License](https://opensource.org/licenses/MIT)