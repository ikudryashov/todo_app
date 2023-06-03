# User
The User entity represents a user registered in the app.
## Fields
| Field      | Type    | Description                                              |
|------------|---------|----------------------------------------------------------|
| Id         | Guid    | A globally unique identifier of the user                 |
| FirstName  | string  | The first name of the user.                              |
| LastName   | string  | The last name of the user.                               |
| Email      | string  | The email of the user, must be unique across the system. |
| Password   | string  | The hashed password for the user.                        |
| Salt       | string  | The salt used for password hashing.                      |

All fields are required to not be empty or null for User entity.
User's password and salt are not accessible via the API.