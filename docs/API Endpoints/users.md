# Users API

This API is used for user account management actions:
- Getting user details
- Updating account information
- Deleting an account

## Endpoints

### GET: api/users/{id}

This endpoint retrieves a user with specified Id.

* Access: Requires a valid access token with matching user id.

**Response:**

```json
{
  "Id": "string",
  "FirstName": "string",
  "LastName": "string",
  "Email": "string"
}
```
This endpoint returns **404 Not Found** response if:
- The user with given id does not exist

**401 Unauthorized** response if:
- Access token Sub claim does not match specified user Id

### PUT: api/users/{id}

This endpoint allows to edit user's data.

* Access: Requires a valid access token with matching user id.

**Request Body:**

```json
{
  "firstName": "string",
  "lastName": "string",
  "email": "string"
}
```

**Response:**

```json
{
  "Id": "string",
  "FirstName": "string",
  "LastName": "string",
  "Email": "string"
}
```
This endpoint returns **404 Not Found** response if:
- The user with given id does not exist

**400 Bad Request** response if:
- At least one request field failed validation

**401 Unauthorized** response if:
- Access token Sub claim does not match specified user Id

### DELETE: api/users/{id}

This endpoint allows to delete a user.

* Access: Requires a valid access token with matching user id.

**Response:**

**204 No Content**

This endpoint returns **404 Not Found** response if:
- The user with given id does not exist

**401 Unauthorized** response if:
- Access token Sub claim does not match specified user Id