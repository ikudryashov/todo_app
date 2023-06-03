# Todos API

This API is used for managing todos in the app:
- Getting all todos of a user
- Getting a specific todo of a user
- Creating new todos
- Updating todos
- Deleting todos

## Endpoints

### GET: api/todos/

This endpoint retrieves all todos of a user.

* Access: Requires a valid access token.

**Response:**

```json
[
    {
      "id": "string",
      "title": "string",
      "description": "string",
      "dueDate": "string",
      "isComplete": "bool"
    }
]
```
This endpoint returns an empty list if a user does not have any todos.

**401 Unauthorized** response if:
- Access token is invalid

### GET: api/todos/{id}

This endpoint retrieves a todo with specified Id.

* Access: Requires a valid access token.

**Response:**

```json
{
  "id": "string",
  "title": "string",
  "description": "string",
  "dueDate": "string",
  "isComplete": "bool"
}
```
This endpoint returns **404 Not Found** response if:
- Todo with given id does not exist

**401 Unauthorized** response if:
- Access token Sub claim does not match user Id associated with requested todo.

### POST: api/todos/

This endpoint allows user to create a new todo.

* Access: Requires a valid access token.

**Request Body:**

```json
{
  "title": "string",
  "description": "string",
  "dueDate": "string",
  "isComplete": "bool"
}
```

**Response:**

**204 Created**

```json
{
  "id": "string",
  "title": "string",
  "description": "string",
  "dueDate": "string",
  "isComplete": "bool"
}
```
This endpoint returns **400 Bad Request** response if:
- At least one request field has failed validation.

### PUT: api/todos/{id}

This endpoint allows to edit a todo with specified id.

* Access: Requires a valid access token with matching user id.

**Request Body:**

```json
{
  "title": "string",
  "description": "string",
  "dueDate": "string",
  "isComplete": "bool"
}
```

**Response:**

**204 No Content**

This endpoint returns **404 Not Found** response if:
- The user with given id does not exist
- The todo with given id does not exist

**400 Bad Request** response if:
- At least one request field failed validation

**401 Unauthorized** response if:
- Access token Sub claim does not match user Id associated with requested todo.

### DELETE: api/todos/{id}

This endpoint allows to delete a todo.

* Access: Requires a valid access token with matching user id.

**Response:**

**204 No Content**

This endpoint returns **404 Not Found** response if:
- The user with given id does not exist
- The todo with given id does not exist

**401 Unauthorized** response if:
- Access token Sub claim does not match user Id associated with requested todo.