# Authentication API

This API is used for user authentication actions: 
- Signing up
- Logging in
- Refreshing Users' access tokens
- Logging out

## Endpoints

### POST: api/auth/signup

This endpoint allows a new user to sign up.

* Access: Public

**Request Body:**

```json
{
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "password": "string"
}
```

**Response:**

```json
{
  "UserId": "string",
  "AccessToken": "string",
  "RefreshToken": "string"
}
```
This endpoint returns **400 Bad Request** response if:
- The user with given email already exists
- At least one request field failed validation

### POST: api/auth/login

This endpoint allows an existing user to log in.

* Access: Public

**Request Body:**

```json
{
  "email": "string",
  "password": "string"
}
```

**Response:**

```json
{
  "UserId": "string",
  "AccessToken": "string",
  "RefreshToken": "string"
}
```

This endpoint returns **400 Bad Request** response if:
- The user with given email does not exist
- Provided password is incorrect
- At least one request field failed validation

### POST: api/auth/refresh

This endpoint allows an existing user refresh their expired access token.

* Access: Public

**Request Body:**

```json
{
  "refreshToken": "string"
}
```

**Response:**

```json
{
  "UserId": "string",
  "AccessToken": "string",
  "RefreshToken": "string"
}
```
This endpoint returns **404 Not Found** response if:
- Provided refresh token does not exist

**403 Unauthorized** response if:
- Provided refresh token is itself expired

**401 Bad Request** response if:
- Provided refresh token is invalid or invalidated

### POST: api/auth/logout

This endpoint allows an existing user to invalidate their refresh token.

* Access: Requires a valid access token

**Response:**

**204: No Content**

This endpoint returns **404 Not Found** response if:
- The refresh token does not exist