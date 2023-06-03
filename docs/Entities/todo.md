# Todo
The Todo entity represents a todo record created by a user.
## Fields
| Field       | Type     | Description                              |
|-------------|----------|------------------------------------------|
| Id          | Guid     | A globally unique identifier of the todo |
| UserId      | Guid     | Id of the user who created the todo      |
| Title       | string   | Title of the todo                        |
| Description | string   | Description of the todo                  |
| DueDate     | DateTime | Due date of the todo                     |

Todos may not have a description or a due date, so these fields can be left null.