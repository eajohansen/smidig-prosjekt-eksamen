# API Endpoints Documentation

## Table of Contents
- [General info](#general-info)


- [User Controller](#user-controller)
  - [GET /api/user](#get-apiuser)
    - [GET /api/user/fetchAll](#get-apiUser-fetchAll)
    - [GET /api/user/fetch/id/{userId}](#get-apiUser-fetchById)
    - [GET /api/user/fetch/email/{email}](#get-apiUser-fetchByEmail)
  - [POST /api/user](#post-apiuser)
    - [POST /api/user/add/organizer/{organizationId}](#post-apiuser-addOrganizer)
    - [POST /api/user/add/follower/{organizationId}](#post-apiuser-addFollower)
    - [POST /api/user/add/event/{eventId}](#post-apiuser-addEventToUser)
  - [PUT /api/user](#put-apiuserid)
    - [PUT /api/user/update](#put-apiuserid-updateUser)
    - [PUT /api/user/add/admin](#put-apiuserid-addAdmin)
  - [DELETE /api/user](#delete-apiuserid)
    - [DELETE /api/user/delete](#delete-apiuserid-deleteUser)
    

- [Event Controller](#event-controller)
  - [GET /api/event](#get-apievent)
    - [GET /api/event/fetchAll](#get-apievent-fetchAll)
    - [GET /api/event/fetchAll/attending](#get-apievent-fetchAll-attending)
    - [GET /api/event/fetchAll/not/attending](#get-apievent-fetchAll-not-attending)
    - [GET /api/event/fetchAll/organization/{organizationId}](#get-apievent-fetchAll-organization)
    - [GET /api/event/fetchAll/not/organization/{organizationId}](#get-apievent-fetchAll-not-organization)
    - [GET /api/event/fetch/id/{userId}](#get-apievent-fetch-id)
    - [GET /api/event/customField/fetchAll](#get-apievent-customField-fetchAll)
  - [POST /api/event](#post-apievent)
    - [POST /api/event/create](#post-apievent-create)
  - [PUT /api/event](#put-apieventid)
    - [PUT /api/event/update](#put-apieventid-update)
  - [DELETE /api/event](#delete-apieventid)
    - [DELETE /api/event/delete](#delete-apieventid-deleteEvent)


- [Organization Controller](#organization-controller)
  - [GET /api/organization](#get-apiorganization)
    - [GET /api/organization/fetchAll](#get-apiorganization-fetchAll)
    - - [GET /api/organization/fetch/id/{organizationId}](#get-apiorganization-fetchOrganization)
  - [POST /api/organization](#post-apiorganization)
    - [POST /api/organization/create](#post-apiorganization-AddOrganization)
  - [PUT /api/organization](#put-apiorganizationid)
    - [PUT /api/organization/update](#put-apiorganizationid-update)
  - [DELETE /api/organization](#delete-apiorganizationid)
    - [DELETE /api/organization/delete](#delete-apiorganizationid-deleteOrganization)

---

## User Controller


### GET /api/user

#### GET /api/user/fetchAll

#### Description
Fetches all the users in the database.

#### Restriction
Has to be Admin or Organizer.

#### URL
`GET /api/user/fetchAll`

#### Example Request
```http
GET /api/user/fetchAll
Authorization: Bearer yourtoken
```

#### Response
##### Success (200)
```json
[
  {
    "Id": "id",
    "email": "admin@test.com",
    "firstName": "name",
    "lastName": "name",
    "birthdate": "12-12-2020 00:00:00",
    "extraInfo": "extra text",
    "followOrganization": ["Follower Object"],
    "organizerOrganization": ["Organizer Object"],
    "userEvents": ["UserEvent Object"],
    "notices": ["Notice Object"],
    "allergies": ["Allergy Object"]
  },
  {
  "more user objects": ""
  }
]
```

##### NoContent (204)
```json
{
  
}
```

##### Error (500)
```json
{
  "error": "Description of the error"
}
```

#### GET /api/user/fetch/id/{userId}

#### Description
Fetches a user from the database by Id.

#### Restriction
Has to be logged in.

#### URL
`GET /api/user/fetch/id/{userId}`

#### Parameters
| Parameter | Type   | Required | Description |
|-----------|--------|----------|-------------|
| userId    | string | Yes      | User's id   |

#### Example Request
```http
GET /api/user/fetc/id/{userId}
Authorization: Bearer yourtoken
```

#### Response
##### Success (200)
```json
{
  "Id": "id",
  "email": "admin@test.com",
  "firstName": "name",
  "lastName": "name",
  "birthdate": "12-12-2020 00:00:00",
  "extraInfo": "extra text",
  "followOrganization": ["Follower Object"],
  "organizerOrganization": ["Organizer Object"],
  "userEvents": ["UserEvent Object"],
  "notices": ["Notice Object"],
  "allergies": ["Allergy Object"]
}
```

##### NoContent (204)
```json
{
  
}
```

##### Error (500)
```json
{
  "error": "Description of the error"
}
```

#### GET /api/user/fetch/email/{email}

#### Description
Fetches a user from the database by email.

#### Restriction
Has to be logged in.

#### URL
`GET /api/user/fetch/email/{email}`

#### Parameters
| Parameter | Type   | Required | Description  |
|-----------|--------|----------|--------------|
| email     | string | Yes      | User's email |

#### Example Request
```http
GET /api/user/fetch/email/{email}
Authorization: Bearer yourtoken
```

#### Response
##### Success (200)
```json
{
  "Id": "id",
  "email": "admin@test.com",
  "firstName": "name",
  "lastName": "name",
  "birthdate": "12-12-2020 00:00:00",
  "extraInfo": "extra text",
  "followOrganization": ["Follower Object"],
  "organizerOrganization": ["Organizer Object"],
  "userEvents": ["UserEvent Object"],
  "notices": ["Notice Object"],
  "allergies": ["Allergy Object"]
}
```

##### NoContent (204)
```json
{
  
}
```

##### Error (500)
```json
{
  "error": "Description of the error"
}
```


### POST /api/user

#### POST /api/user/add/organizer/{organizationId}

#### Description
Makes a connection between user and organization as organizer.
This is for when an admin wants to make a user into an organizer.

#### Restriction
Has to be logged in.

#### URL
`POST /api/user/add/organizer/{organizationId}`

#### Request Body
| Field          | Type   | Required | Description       |
|----------------|--------|----------|-------------------|
| email          | string | Yes      | User's email      |

#### Example Request
```http
POST /api/user/add/organizer/{organizationId}
Content-Type: application/json
Authorization: Bearer yourtoken

{
  "email": "some@thing.com",
}
```

#### Response
##### Success (201)
```json
{
  "stuff": "works"
}
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

#### POST /api/user/add/follower/{organizationId}

#### Description
Makes a connection between user and organization as follower.
This is for when a user wants to follow an organization.

#### Restriction
Has to be logged in.

#### URL
`POST /api/user/add/follower/{organizationId}`

#### Example Request
```http
POST /api/user/add/follower/{organizationId}
Content-Type: application/json
Authorization: Bearer yourtoken
```

#### Response
##### Success (201)
```json
{
  "stuff": "works"
}
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

#### POST /api/user/add/event/{eventId}

#### Description
Makes a connection between user and event.
This is for when a user is going to attend to an event.

#### Restriction
Has to be Admin.

#### URL
`POST /api/user/add/event/{eventId}`

#### Example Request
```http
POST /api/user/add/event/{eventId}
Content-Type: application/json
Authorization: Bearer yourtoken
```

#### Response
##### Success (201)
```json
{
  "stuff": "works"
}
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```


### PUT /api/user

#### PUT /api/user/update

#### Description
Updates a user.
This is what you want to use after registering a user.

#### Restriction
Has to be logged in.

#### URL
`PUT /api/user/update`

#### Request Body
| Field                 | Type                   | Required | Description                  |
|-----------------------|------------------------|----------|------------------------------|
| email                 | string                 | Yes      | user's email                 |
| firstName             | string                 | No       | user's firstName             |
| lastName              | string                 | No       | user's lastName              |
| birthdate             | DateType               | No       | user's birthdate             |
| extraInfo             | string                 | No       | user's extraInfo             |
| followOrganization    | ICollection<Follower>  | No       | user's followOrganization    |
| organizerOrganization | ICollection<Organizer> | No       | user's organizerOrganization |
| userEvents            | ICollection<userEvent> | No       | user's userEvents            |
| notices               | ICollection<Notice>    | No       | user's notices               |
| allergies             | ICollection<Allergy>   | No       | user's allergies             |


#### Example Request
```http
PUT /api/user/update
Content-Type: application/json
Authorization: Bearer yourtoken
```

#### Response
##### Success (200)
```json
{
  "Id": "id",
  "email": "admin@test.com",
  "firstName": "name",
  "lastName": "name",
  "birthdate": "12-12-2020 00:00:00",
  "extraInfo": "extra text",
  "followOrganization": ["Follower Object"],
  "organizerOrganization": ["Organizer Object"],
  "userEvents": ["UserEvent Object"],
  "notices": ["Notice Object"],
  "allergies": ["Allergy Object"]
}
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

#### PUT /api/user/add/admin

#### Description
Makes another user to an admin.
The user doing this action makes another one to an admin

#### Restriction
Has to be admin.

#### URL
`PUT /api/user/add/admin`

#### Request Body
| Field | Type   | Required | Description  |
|-------|--------|----------|--------------|
| email | string | Yes      | user's email |

#### Example Request
```http
PUT /api/useradd/admin
Content-Type: application/json
Authorization: Bearer yourtoken
```

#### Response
##### Success (200)
```json
{
  "true": true
}
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

### DELETE /api/user

#### DELETE /api/user/delete

#### Description
Deletes a user.

#### Restriction
Has to be logged in.

#### URL
`DELETE /api/user/delete`

#### Parameters
| Parameter | Type   | Required | Description |
|-----------|--------|----------|-------------|

#### Example Request
```http
DELETE /api/user/delete
Authorization: Bearer yourtoken
```

#### Response
##### Success (204)
```
(No content)
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

---

## Event Controller

### GET /api/event

#### Description
Briefly describe what this endpoint does.

#### URL
`GET /api/event`

#### Parameters
| Parameter | Type   | Required | Description           |
|-----------|--------|----------|-----------------------|
| param1    | string | Yes      | Description of param1 |
| param2    | int    | No       | Description of param2 |

#### Example Request
```http
GET /api/event?param1=value1&param2=value2 HTTP/1.1
Host: yourapi.com
Authorization: Bearer yourtoken
```

#### Response
##### Success (200)
```json
{
  "key1": "value1",
  "key2": "value2"
}
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

### POST /api/event

#### Description
Briefly describe what this endpoint does.

#### URL
`POST /api/event`

#### Request Body
| Field   | Type   | Required | Description         |
|---------|--------|----------|---------------------|
| field1  | string | Yes      | Description of field1 |
| field2  | int    | Yes      | Description of field2 |

#### Example Request
```http
POST /api/event HTTP/1.1
Host: yourapi.com
Content-Type: application/json
Authorization: Bearer yourtoken

{
  "field1": "value1",
  "field2": value2
}
```

#### Response
##### Success (201)
```json
{
  "id": "new-event-id",
  "field1": "value1",
  "field2": value2
}
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

### PUT /api/event/{id}

#### Description
Briefly describe what this endpoint does.

#### URL
`PUT /api/event/{id}`

#### Request Body
| Field   | Type   | Required | Description         |
|---------|--------|----------|---------------------|
| field1  | string | Yes      | Description of field1 |
| field2  | int    | Yes      | Description of field2 |

#### Example Request
```http
PUT /api/event/{id} HTTP/1.1
Host: yourapi.com
Content-Type: application/json
Authorization: Bearer yourtoken

{
  "field1": "value1",
  "field2": value2
}
```

#### Response
##### Success (200)
```json
{
  "id": "event-id",
  "field1": "updated value1",
  "field2": updated value2
}
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

### DELETE /api/event/{id}

#### Description
Briefly describe what this endpoint does.

#### URL
`DELETE /api/event/{id}`

#### Parameters
| Parameter | Type | Required | Description         |
|-----------|------|----------|---------------------|
| id        | string | Yes    | ID of the event  |

#### Example Request
```http
DELETE /api/event/{id} HTTP/1.1
Host: yourapi.com
Authorization: Bearer yourtoken
```

#### Response
##### Success (204)
```
(No content)
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

---

## Organization Controller

### GET /api/organization

#### Description
Briefly describe what this endpoint does.

#### URL
`GET /api/organization`

#### Parameters
| Parameter | Type   | Required | Description           |
|-----------|--------|----------|-----------------------|
| param1    | string | Yes      | Description of param1 |
| param2    | int    | No       | Description of param2 |

#### Example Request
```http
GET /api/organization?param1=value1&param2=value2 HTTP/1.1
Host: yourapi.com
Authorization: Bearer yourtoken
```

#### Response
##### Success (200)
```json
{
  "key1": "value1",
  "key2": "value2"
}
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

### POST /api/organization

#### Description
Briefly describe what this endpoint does.

#### URL
`POST /api/organization`

#### Request Body
| Field   | Type   | Required | Description         |
|---------|--------|----------|---------------------|
| field1  | string | Yes      | Description of field1 |
| field2  | int    | Yes      | Description of field2 |

#### Example Request
```http
POST /api/organization HTTP/

1.1
Host: yourapi.com
Content-Type: application/json
Authorization: Bearer yourtoken

{
  "field1": "value1",
  "field2": value2
}
```

#### Response
##### Success (201)
```json
{
  "id": "new-organization-id",
  "field1": "value1",
  "field2": value2
}
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

### PUT /api/organization/{id}

#### Description
Briefly describe what this endpoint does.

#### URL
`PUT /api/organization/{id}`

#### Request Body
| Field   | Type   | Required | Description         |
|---------|--------|----------|---------------------|
| field1  | string | Yes      | Description of field1 |
| field2  | int    | Yes      | Description of field2 |

#### Example Request
```http
PUT /api/organization/{id} HTTP/1.1
Host: yourapi.com
Content-Type: application/json
Authorization: Bearer yourtoken

{
  "field1": "value1",
  "field2": value2
}
```

#### Response
##### Success (200)
```json
{
  "id": "organization-id",
  "field1": "updated value1",
  "field2": updated value2
}
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

### DELETE /api/organization/{id}

#### Description
Briefly describe what this endpoint does.

#### URL
`DELETE /api/organization/{id}`

#### Parameters
| Parameter | Type | Required | Description         |
|-----------|------|----------|---------------------|
| id        | string | Yes    | ID of the organization  |

#### Example Request
```http
DELETE /api/organization/{id} HTTP/1.1
Host: yourapi.com
Authorization: Bearer yourtoken
```

#### Response
##### Success (204)
```
(No content)
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

---

### Notes
- Ensure you replace placeholder texts like `yourapi.com`, `yourtoken`, and `{id}` with actual values.
- Adjust the fields and types according to your specific API requirements.
- Add any additional headers or authentication requirements if applicable.


This updated template provides a structured way to document the endpoints for your User, Event, and Organization controllers. You can replicate and fill out each section for every endpoint you have in your project.