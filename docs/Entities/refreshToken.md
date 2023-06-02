# RefreshToken
The Refresh Token entity represents a refresh token issued to a User.
## Fields
| Field      | Type     | Description                                                   |
|------------|----------|---------------------------------------------------------------|
| UserId     | Guid     | Id of the User the Refresh Token was issued to.               |
| Token      | string   | Refresh Token's value.                                        |
| Salt       | string   | Salt used for hashing the RefreshToken.                       |
| ExpiryDate | DateTime | Timestamp on which date and time the Token expires.           |
| IsValid    | Bool     | Flag representing whether the token has been revoked/expired. |

Only Refresh Token's value is accessible via the API.