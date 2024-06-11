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
Admin or Organizer

#### URL
`GET /api/user/fetchAll`

#### Parameters
| Parameter | Type   | Required | Description           |
|-----------|--------|----------|-----------------------|
| param1    | string | Yes      | Description of param1 |
| param2    | int    | No       | Description of param2 |

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
Fetches a user from the database by the Id.

#### Restriction
Admin or Organizer

#### URL
`GET /api/user/fetchAll`

#### Parameters
| Parameter | Type   | Required | Description    |
|-----------|--------|----------|----------------|
| userId    | string | Yes      | Id of the user |

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



### POST /api/user

#### Description
Briefly describe what this endpoint does.

#### URL
`POST /api/user`

#### Request Body
| Field   | Type   | Required | Description         |
|---------|--------|----------|---------------------|
| field1  | string | Yes      | Description of field1 |
| field2  | int    | Yes      | Description of field2 |

#### Example Request
```http
POST /api/user HTTP/1.1
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
  "id": "new-user-id",
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

### PUT /api/user/{id}

#### Description
Briefly describe what this endpoint does.

#### URL
`PUT /api/user/{id}`

#### Request Body
| Field   | Type   | Required | Description         |
|---------|--------|----------|---------------------|
| field1  | string | Yes      | Description of field1 |
| field2  | int    | Yes      | Description of field2 |

#### Example Request
```http
PUT /api/user/{id} HTTP/1.1
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
  "id": "user-id",
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

### DELETE /api/user/{id}

#### Description
Briefly describe what this endpoint does.

#### URL
`DELETE /api/user/{id}`

#### Parameters
| Parameter | Type | Required | Description         |
|-----------|------|----------|---------------------|
| id        | string | Yes    | ID of the user  |

#### Example Request
```http
DELETE /api/user/{id} HTTP/1.1
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