A program made for UVS Task.

Uses localhost as the server, need to change that if using a proper database server.

The database should be automatically created when launching the program for the first time, if it doesn't, you would need to create the database manually or apply a new migration from the Package manager console inside the code (I used SSMS)

Console for monitoring thread generation, the WPF application on the right shows the last 20 rows from the database when the process is stopped or the program is launched.
![UVS1](https://github.com/PaulJur/UVSUzduotis/assets/97526083/b226c0c4-957a-44e2-8785-76b11402cd46)

Database data is saved with ID's, DateTime.Now and the symbol data.
![UVS2](https://github.com/PaulJur/UVSUzduotis/assets/97526083/23f9299e-6da2-4608-a8d6-3bd3cb1d2487)


Made by Paulius Jurgelis
