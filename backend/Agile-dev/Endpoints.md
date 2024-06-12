# API Endpoints Documentation

## Table of Contents
- [General info](#general-info)


- [Identity Controller](#identity-controller)
  - [POST /register](#post-register)
  - [POST /login](#post-login)


- [User Controller](#user-controller)
  - [GET User](#get-user)
    - [GET /api/user/fetchAll](#get-apiUserfetchAll)
    - [GET /api/user/fetch/id/{userId}](#get-apiuserfetchiduserid)
    - [GET /api/user/fetch/email](#get-apiuserfetchemail)
    - [GET /api/user/checkAdminPrivileges](#get-apiusercheckadminprivileges)
  - [POST User](#post-user)
    - [POST /api/user/add/organizer/{organizationId}](#post-apiuseraddorganizerorganizationid)
    - [POST /api/user/add/follower/{organizationId}](#post-apiuseraddfollowerorganizationid)
    - [POST /api/user/add/event/{eventId}](#post-apiuseraddeventeventid)
  - [PUT User](#put-user)
    - [PUT /api/user/update](#put-apiuserupdate)
    - [PUT /api/user/add/admin](#put-apiuseraddadmin)
  - [DELETE User](#delete-user)
    - [DELETE /api/user/delete](#delete-apiuserdelete)
    

- [Event Controller](#event-controller)
  - [GET Event](#get-event)
    - [GET /api/event/fetchAll](#get-apieventfetchall)
    - [GET /api/event/fetchAll/attending](#get-apieventfetchallattending)
    - [GET /api/event/fetchAll/not/attending](#get-apieventfetchallnotattending)
    - [GET /api/event/fetchAll/organization/{organizationId}](#get-apieventfetchallorganizationorganizationid)
    - [GET /api/event/fetch/id/{eventId}](#get-apieventfetchideventid)
  - [POST Event](#post-event)
    - [POST /api/event/create](#post-apieventcreate)
  - [PUT Event](#put-event)
    - [PUT /api/event/update](#put-apiuserupdate)
  - [Delete Event](#delete-event)
    - [DELETE /api/event/delete](#delete-apieventdelete)


- [Organization Controller](#organization-controller)
  - [GET Organization](#get-organization)
    - [GET /api/organization/fetchAll](#get-apiorganizationfetchall)
    - [GET /api/organization/fetch/id/{organizationId}](#get-apiorganizationfetchidorganizationid)
  - [POST Organization](#post-organization)
    - [POST /api/organization/create](#post-apiorganizationcreate)
  - [PUT Organization](#put-organization)
    - [PUT /api/organization/update](#put-apiorganizationupdate)
  - [DELETE Organization](#delete-organization)
    - [DELETE /api/organization/delete](#delete-apiorganizationdelete)


- [Notes](#notes)

---

## General Info

#### Data Annotations
- [Key]: data annotation for the primary key of the table.
- [Required]: data annotation for making a field necessary in the table.
- [Display(name = {name)]: data annotation for choosing how a field looks in the database.
- [StringLength({number)]: data annotation for setting the length of the string for the field.
- [ForeignKey({foreignKey)]: data annotation for choosing which field connects up to another table.
- [DataType({dataType)]: data annotation for specifying the dataType for this field.
- [EmailAddress(ErrorMessage = "{errorMessage)]: data annotation for checking that the field is an email.
- [DisplayFormat(DataFormatString = "{format)"]: data annotation for specifying the format for this field.
- [DisplayFormat(ApplyFormatInEdit = "{bool)"]: data annotation for using the format in edit mode.

---

## Identity Controller

### POST /register

#### Description
Registers a user in the database.

#### Restriction
No restrictions.

#### URL
`GET /register`

#### Request Body
| Field    | Type   | Required | Description     |
|----------|--------|----------|-----------------|
| Email    | string | Yes      | User's email    |
| Password | string | Yes      | User's password |

#### Example Request
```http
POST /api/event/create
Content-Type: application/json
Authorization: Bearer yourtoken

{
    "email": "hey@to.no",
    "Password": "1aAbc!dead"
}
```

#### Response
##### Ok (200)
```
```

##### BadRequest (400)
###### Email already in use
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "DuplicateUserName": [
      "Username 'hey@to.no' is already taken."
    ],
    "DuplicateEmail": [
      "Email 'hey@to.no' is already taken."
    ]
  }
}
```

##### BadRequest (400)
###### Email field is not a valid email
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "InvalidEmail": [
      "Email 'hey' is invalid."
    ]
  }
}
```

##### BadRequest (400)
###### Password does not fit the criteria
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "PasswordTooShort": [
      "Passwords must be at least 8 characters."
    ],
    "PasswordRequiresNonAlphanumeric": [
      "Passwords must have at least one non alphanumeric character."
    ],
    "PasswordRequiresDigit": [
      "Passwords must have at least one digit ('0'-'9')."
    ],
    "PasswordRequiresLower": [
      "Passwords must have at least one lowercase ('a'-'z')."
    ],
    "PasswordRequiresUpper": [
      "Passwords must have at least one uppercase ('A'-'Z')."
    ],
    "PasswordRequiresUniqueChars": [
      "Passwords must use at least 1 different characters."
    ]
  }
}
```

### POST /login

#### Description
Registers a user in the database.

#### Restriction
No restrictions.

#### URL
`GET /login`

#### Request Body
| Field    | Type   | Required | Description     |
|----------|--------|----------|-----------------|
| Email    | string | Yes      | User's email    |
| Password | string | Yes      | User's password |

#### Example Request
```http
POST /api/event/login
Content-Type: application/json
Authorization: Bearer yourtoken

{
    "email": "hey@to.no",
    "Password": "1aAbc!dead"
}
```

#### Response
##### Ok (200)
```json
{
    "tokenType": "Bearer",
    "accessToken": "Long access token text",
    "expiresIn": 3600,
    "refreshToken": "Long refresh token text"
}
```

##### Unauthorized (401)
###### Wrong password or wrong email
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.2",
  "title": "Unauthorized",
  "status": 401,
  "detail": "Failed"
}
```

##### BadRequest (400)
###### Empty object
```
```

##### BadRequest (400)
###### No object
```
```


## User Controller


### GET User

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

##### NotFound (404)
```
No users found.
```

##### Unauthorized (401)
###### Not Admin or Organizer
```
```

##### InternalServerError (500)
###### Not logged in or wrong privileges
```json
{
  "statusCode": 500,
  "message": "Internal server error: An error occurred while fetching users."
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

##### InternalServerError (500)
```json
{
  "error": "Don't know"
}
```

##### Unauthorized (401)
###### Wrong userId
```
```

##### Unauthorized (401)
###### Not logged in
```
  You are not authorized to view this user, you can only view your own user
```

##### NotFound (404)
###### UserId does not exist
```
  Could not find user with this id
```

#### GET /api/user/fetch/email

#### Description
Fetches a user's email

#### Restriction
Has to be logged in.

#### URL
`GET /api/user/fetch/email`

#### Parameters
| Parameter | Type   | Required | Description  |
|-----------|--------|----------|--------------|

#### Example Request
```http
GET /api/user/fetch/email
Authorization: Bearer yourtoken
```

#### Response
##### Success (200)
```json
{
  "Email": "yada@yada.com"
}
```

##### InternalServerError (500)
```json
{
  "error": "Don't know"
}
```

##### Unauthorized (401)
###### Not logged in
```
```

#### GET /api/user/checkAdminPrivileges

#### Description
Checks if user has Admin or Organizer privileges. 

#### Restriction
Has to be logged in.

#### URL
`GET /api/user/checkAdminPrivileges`

#### Parameters
| Parameter | Type   | Required | Description  |
|-----------|--------|----------|--------------|

#### Example Request
```http
GET /api/user/checkAdminPrivileges
Authorization: Bearer yourtoken
```

#### Response
##### Success (200)
```json
{
  "admin": false,
  "organizer": true
}
```

##### InternalServerError (500)
```json
{
  "error": "Don't know"
}
```

##### Unauthorized (401)
###### Not logged in
```
```


### POST User

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
  "succeeded": true,
  "errors": []
}
```

##### BadRequest (400)
```json
{
  "succeeded": false,
  "errors": [
    {
      "code": null,
      "description": "User is already organizer for this organization"
    }
  ]
}
```

##### BadRequest (400)
###### Empty object
```
No email provided in request
```

##### BadRequest (400)
###### Wrong Email
```json
{
  "succeeded": false,
  "errors": [
    {
      "code": null,
      "description": "User with email hey@dto.no was not found."
    }
  ]
}
```

##### BadRequest (400)
###### Wrong OrganizationId
```json
{
  "succeeded": false,
  "errors": [
    {
      "code": null,
      "description": "Could not find organization."
    }
  ]
}
```

##### Forbidden (403)
###### User is not Admin
```
```

##### BadRequest (400)
###### Wrong OrganizationId
```
No organizationId provided
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
```
```

##### BadRequest (400)
```
User is already following the organization
```

##### BadRequest (400)
###### Wrong OrganizationId
```
Organization not found
```

##### BadRequest (400)
###### Wrong OrganizationId
```
No organizationId provided
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

##### BadRequest (400)
```
User is already attending event
```

##### BadRequest (400)
###### Wrong EventId
```
Event not found
```


### PUT User

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

#### Response
##### Success (200)
```json
{
  "succeeded": true,
  "errors": []
}
```

##### BadRequest (400)
```
No email provided in request
```

##### Unauthorized (400)
```
You are not authorized to update this user
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
  "succeeded": true,
  "errors": []
}
```

##### BadRequest (400)
###### No object in the request body
```
AdminUser is null
```

##### BadRequest (400)
###### No Email in the Request body
```
No email provided in request
```

##### BadRequest (400)
###### User has Admin rights from before
```json
[
  {
    "code": "UserAlreadyInRole",
    "description": "User already in role 'Admin'."
  }
]
```

### DELETE User

#### DELETE /api/user/delete

#### Description
Deletes a user.

#### Restriction
Has to be logged in.
There are still some issues with this one

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

### GET Event

#### GET /api/event/fetchAll

#### Description
Fetches all the events from the database.

#### Restriction
Has to be logged in.

#### URL
`GET /api/event/fetchAll`

#### Parameters
| Parameter | Type   | Required | Description           |
|-----------|--------|----------|-----------------------|

#### Example Request
```http
GET /api/event/fetchAll
Authorization: Bearer yourtoken
```

#### Response
##### Success (200)
```json
[
  {
    "eventId": 1,
    "title": "Sample Event",
    "description": "This is a sample description for the event.",
    "capacity": 30,
    "ageLimit": 18,
    "private": false,
    "published": true,
    "placeLocation": "there",
    "placeUrl": "hye",
    "imageLink": "hey",
    "imageDescription": "fheyu",
    "contactPersonName": "tobias",
    "contactPersonEmail": "tobias@hto.no",
    "contactPersonNumber": null,
    "organizationName": "test",
    "availableCapacity": 27,
    "startTime": "2024-01-12T21:12:00",
    "endTime": "2024-02-13T23:50:00",
    "eventCustomFields": [
      {
        "EventCustomField": "Object"
      },
      {
        "EventCustomField": "Object"
      }
    ]
  },
  {
    "more event": "objects"
  }
]
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

#### GET /api/event/fetchAll/attending

#### Description
Fetches all the events that the user is attending from the database.

#### Restriction
Has to be logged in.

#### URL
`GET /api/event/fetchAll/attending`

#### Parameters
| Parameter | Type   | Required | Description           |
|-----------|--------|----------|-----------------------|

#### Example Request
```http
GET /api/event/fetchAll/attending
Authorization: Bearer yourtoken
```

#### Response
##### Success (200)
```json
[
  {
    "eventId": 1,
    "title": "Sample Event",
    "description": "This is a sample description for the event.",
    "capacity": 30,
    "ageLimit": 18,
    "private": false,
    "published": true,
    "placeLocation": "there",
    "placeUrl": "hye",
    "imageLink": "hey",
    "imageDescription": "fheyu",
    "contactPersonName": "tobias",
    "contactPersonEmail": "tobias@hto.no",
    "contactPersonNumber": null,
    "organizationName": "test",
    "availableCapacity": 27,
    "startTime": "2024-01-12T21:12:00",
    "endTime": "2024-02-13T23:50:00",
    "eventCustomFields": [
      {
        "EventCustomField": "Object"
      },
      {
        "EventCustomField": "Object"
      }
    ]
  },
  {
    "more event": "objects"
  }
]
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

#### GET /api/event/fetchAll/not/attending

#### Description
Fetches all the events that the user is not attending from the database.

#### Restriction
Has to be logged in.

#### URL
`GET /api/event/fetchAll/not/attending`

#### Parameters
| Parameter | Type   | Required | Description           |
|-----------|--------|----------|-----------------------|

#### Example Request
```http
GET /api/event/fetchAll/not/attending
Authorization: Bearer yourtoken
```

#### Response
##### Success (200)
```json
[
  {
    "eventId": 2,
    "title": "Sample Event",
    "description": "This is a sample description for the event.",
    "capacity": 30,
    "ageLimit": 18,
    "private": false,
    "published": true,
    "placeLocation": "there",
    "placeUrl": "hye",
    "imageLink": "hey",
    "imageDescription": "fheyu",
    "contactPersonName": "tobias",
    "contactPersonEmail": "tobias@hto.no",
    "contactPersonNumber": null,
    "organizationName": "test",
    "availableCapacity": 27,
    "startTime": "2024-01-12T21:12:00",
    "endTime": "2024-02-13T23:50:00",
    "eventCustomFields": [
      {
        "EventCustomField": "Object"
      },
      {
        "EventCustomField": "Object"
      }
    ]
  },
  {
    "more event": "objects"
  }
]
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

#### GET /api/event/fetchAll/organization/{organizationId}

#### Description
Fetches all the events from the database based on organizationId.

#### Restriction
Has to be logged in.

#### URL
`GET /api/event/fetchAll/organization/{organizationId}`

#### Parameters
| Parameter      | Type | Required | Description       |
|----------------|------|----------|-------------------|
| organizationId | int  | Yes      | Organization's id |

#### Example Request
```http
GET /api/event/fetchAll/organization/{organizationId}
Authorization: Bearer yourtoken
```

#### Response
##### Success (200)
```json
[
  {
    "eventId": 2,
    "title": "Sample Event",
    "description": "This is a sample description for the event.",
    "capacity": 30,
    "ageLimit": 18,
    "private": false,
    "published": true,
    "placeLocation": "there",
    "placeUrl": "hye",
    "imageLink": "hey",
    "imageDescription": "fheyu",
    "contactPersonName": "tobias",
    "contactPersonEmail": "tobias@hto.no",
    "contactPersonNumber": null,
    "organizationName": "test",
    "availableCapacity": 27,
    "startTime": "2024-01-12T21:12:00",
    "endTime": "2024-02-13T23:50:00",
    "eventCustomFields": [
      {
        "EventCustomField": "Object"
      },
      {
        "EventCustomField": "Object"
      }
    ]
  },
  {
    "more event": "objects"
  }
]
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

#### GET /api/event/fetch/id/{eventId}

#### Description
Fetches all the events from the database based on eventId.

#### Restriction
No restrictions.

#### URL
`GET /api/event/fetch/id/{eventId}`

#### Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| eventId   | int  | Yes      | Event's id  |

#### Example Request
```http
GET /api/event/fetch/id/{eventId}
Authorization: Bearer yourtoken
```

#### Response
##### Success (200)
```json
[
  {
    "eventId": 1,
    "title": "Sample Event",
    "description": "This is a sample description for the event.",
    "capacity": 30,
    "ageLimit": 18,
    "private": false,
    "published": true,
    "placeLocation": "there",
    "placeUrl": "hye",
    "imageLink": "hey",
    "imageDescription": "fheyu",
    "contactPersonName": "tobias",
    "contactPersonEmail": "tobias@hto.no",
    "contactPersonNumber": null,
    "organizationName": "test",
    "availableCapacity": 27,
    "startTime": "2024-01-12T21:12:00",
    "endTime": "2024-02-13T23:50:00",
    "eventCustomFields": [
      {
        "EventCustomField": "Object"
      },
      {
        "EventCustomField": "Object"
      }
    ]
  },
  {
    "more event": "objects"
  }
]
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```


### POST Event

#### POST /api/event/create

#### Description
Creates an event based on the EventDtoFrontend
that Backend receives from Frontend

#### Restriction
Has to be Admin or Organizer.

#### URL
`POST /api/event`

#### Request Body
| Field             | Type             | Required | Description              |
|-------------------|------------------|----------|--------------------------|
| Title             | string           | Yes      | Event's title            |
| Description       | string           | Yes      | Event's description      |
| Published         | bool             | Yes      | Event's published status |
| Private           | bool             | Yes      | Event's private status   |
| OrganizationId    | int              | Yes      | Event's organizationId   |
| Capacity          | int              | Yes      | Event's capacity         |
| AgeLimit          | int              | Yes      | Event's ageLimit         |
| Place             | Place            | Yes      | Event's place            |
| Image             | Image            | Not      | Event's image            |
| ContactPerson     | ContactPerson    | Yes      | Event's contactPerson    |
| EventCustomFields | EventCustomField | No       | Event's eventCustomField |
| Start             | string           | Yes      | Event's startDate        |
| StartTime         | string           | Yes      | Event's startTime        |
| End               | string           | Yes      | Event's endDate          |
| EndTime           | string           | Yes      | Event's endTime          |


#### Example Request
```http
POST /api/event/create
Content-Type: application/json
Authorization: Bearer yourtoken

{
    "Event": {
        "Title": "Sample Event",
        "Description": "This is a sample description for the event.",
        "Published": true,
        "Private": false,
        "OrganizationId": 1,
        "Capacity": 30,
        "AgeLimit": 18,
        "Place": {
            "Location": "there",
            "URL": "hye"
        },
        "Image": {
            "Link": "hey",
            "ImageDescription": "fheyu"
        },
        "contactPerson": {
            "Name": "tobias",
            "Email": "tobias@hto.no"
        },
        "EventCustomFields": [
            {
                "CustomField": {
                    "Description": "Dinner",
                    "Value": true
                }
            },
            {
                "CustomField": {
                    "Description": "Dinner",
                    "Value": false
                }
            }
        ]
    },
    "Start": "2024-01-12",
    "StartTime": "21:12",
    "End": "2024-02-13",
    "EndTime": "23:50"
}
```

#### Response
##### Success (201)
```json
{
  "isSuccess": true,
  "value": {
    "eventId": 6,
    "title": "Sample Event",
    "description": "This is a sample description for the event.",
    "capacity": 30,
    "ageLimit": 18,
    "private": false,
    "published": true,
    "placeId": 6,
    "place": {
      "placeId": 6,
      "location": "there",
      "url": "hye"
    },
    "imageId": 6,
    "image": {
      "imageId": 6,
      "link": "hey",
      "imageDescription": "fheyu",
      "organizations": null,
      "events": [
        null
      ]
    },
    "contactPersonId": 6,
    "contactPerson": {
      "contactPersonId": 6,
      "name": "tobias",
      "email": "tobias@hto.no",
      "number": null,
      "events": [
        null
      ]
    },
    "organizationId": 1,
    "organization": null,
    "createdAt": "2024-06-11T23:02:59.9423778+00:00",
    "publishedAt": "2024-06-11T23:02:59.9424342+00:00",
    "startTime": "2024-01-12T21:12:00",
    "endTime": "2024-02-13T23:50:00",
    "eventCustomFields": [
      {
        "eventCustomFieldId": 29,
        "customFieldId": 1,
        "customField": {
          "customFieldId": 1,
          "description": "Dinner",
          "value": true,
          "eventCustomFields": [
            null
          ]
        },
        "eventId": 6
      },
      {
        "eventCustomFieldId": 30,
        "customFieldId": 2,
        "customField": {
          "customFieldId": 2,
          "description": "Dinner",
          "value": false,
          "eventCustomFields": [
            null
          ]
        },
        "eventId": 6
      }
    ],
    "userEvents": null
  },
  "errorMessage": null
}
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

### PUT Event

#### PUT /api/event/update

#### Description
Updates the event.

#### Restriction
Has to be Admin or Organizer.

#### URL
`PUT /api/event/update`

#### Request Body
| Field             | Type             | Required | Description              |
|-------------------|------------------|----------|--------------------------|
| EventId           | int              | Yes      | Event's id               |
| Title             | string           | Yes      | Event's title            |
| Description       | string           | Yes      | Event's description      |
| Published         | bool             | Yes      | Event's published status |
| Private           | bool             | Yes      | Event's private status   |
| OrganizationId    | int              | Yes      | Event's organizationId   |
| Capacity          | int              | Yes      | Event's capacity         |
| AgeLimit          | int              | Yes      | Event's ageLimit         |
| StartTime         | DateTime         | Yes      | Event's startTime        |
| EndTime           | DateTime         | Yes      | Event's endTime          |
| Place             | Place            | Yes      | Event's place            |
| Image             | Image            | Not      | Event's image            |
| ContactPerson     | ContactPerson    | Yes      | Event's contactPerson    |
| EventCustomFields | EventCustomField | No       | Event's eventCustomField |

#### Example Request
```http
PUT /api/event/update
Content-Type: application/json
Authorization: Bearer yourtoken

{
    "EventId": 3,
    "Title": "Sample ",
    "Description": "This is a sample description for the event.",
    "Published": true,
    "Private": false,
    "OrganizationId": 1,
    "Capacity": 30,
    "AgeLimit": 18,
    "StartTime": "1998-06-13T00:00",
    "EndTime": "1998-06-14T00:00",
    "Place": {
        "Location": "there",
        "URL": "hye"
    },
    "Image": {
        "Link": "hey",
        "ImageDescription": "fheyu"
    },
    "contactPerson": {
        "Name": "tobias",
        "Email": "tobias@hto.no"
    },
    "EventCustomFields": [
        {
            "CustomField": {
                "Description": "jk",
                "Value": true
            }
        },
        {
            "CustomField": {
                "Description": "Dinner",
                "Value": false
            }
        }
    ]
}
```

#### Response
##### Success (200)
```json
{
  
}
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

### DELETE Event

#### DELETE /api/event/delete

#### Description
Deletes the event.
There are still some issues with this one

#### Restriction
Has to be Admin or Organizer.

#### URL
`DELETE /api/event/delete`

#### Parameters
| Parameter | Type | Required | Description         |
|-----------|------|----------|---------------------|

#### Example Request
```http
DELETE /api/event/delete
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

### GET Organization

#### GET /api/organization/fetchAll

#### Description
Fetches all organizations.

#### Restriction
Has to be Admin.

#### URL
`GET /api/organization/fetchAll`

#### Parameters
| Parameter | Type   | Required | Description           |
|-----------|--------|----------|-----------------------|

#### Example Request
```http
GET /api/organization/fetchAll
Authorization: Bearer yourtoken
```

#### Response
##### Success (200)
```json
[
  {
    "organizationId": 1,
    "name": "test",
    "description": null,
    "imageId": null,
    "image": null
  },
  {
    "organizationId": 2,
    "name": "test",
    "description": null,
    "imageId": null,
    "image": null
  }
]
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

#### GET /api/organization/fetch/id/{organizationId}

#### Description
Fetch an organization by its id.

#### Restriction
Has to be Admin.

#### URL
`GET /api/organization/fetch/id/{organizationId}`

#### Parameters
| Parameter      | Type | Required | Description       |
|----------------|------|----------|-------------------|
| organizationId | int  | Yes      | Organization's id |

#### Example Request
```http
GET /api/organization/fetch/id/{organizationId}
Authorization: Bearer yourtoken
```

#### Response
##### Success (200)
```json
{
  "organizationId": 1,
  "name": "test",
  "description": null,
  "imageId": null,
  "image": null
}
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```


### POST Organization

#### POST /api/organization/create

#### Description
Creates an organization.

#### Restriction
Has to be Admin.

#### URL
`POST /api/organization/create`

#### Request Body
| Field       | Type   | Required | Description                |
|-------------|--------|----------|----------------------------|
| Name        | string | Yes      | Organization's name        |
| Description | string | no       | Organization's description |
| Image       | Image  | Yes      | Organization's image       |

#### Example Request
```http
POST /api/organization/create
Content-Type: application/json
Authorization: Bearer yourtoken

{
    "Name": "test",
    "Description": "stuff",
    "Image": {
      "Link": "link",
      "ImageDescription": "description"
    }
}
```

#### Response
##### Success (201)
```json
{
  "organizationId": 6,
  "name": "test",
  "description": "stuff",
  "imageId": 7,
  "image": {
    "imageId": 7,
    "link": "link",
    "imageDescription": "description",
    "organizations": [
      null
    ],
    "events": null
  }
}
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

### PUT Organization

#### PUT /api/organization/update

#### Description
Updates the organization.

#### Restriction
Has to be Admin.

#### URL
`PUT /api/organization/update`

#### Request Body
| Field          | Type   | Required | Description                |
|----------------|--------|----------|----------------------------|
| organizationId | int    | yes      | Organization's id          |
| Name           | string | Yes      | Organization's name        |
| Description    | string | no       | Organization's description |
| Image          | Image  | Yes      | Organization's image       |

#### Example Request
```http
PUT /api/organization/update
Content-Type: application/json
Authorization: Bearer yourtoken

{
     "OrganizationId": 5,
    "Name": "test",
    "Description": "stuff",
    "Image": {
      "Link": "link",
      "ImageDescription": "description"
    }
}
```

#### Response
##### Success (200)
```json
{
  "organizationId": 5,
  "name": "test",
  "description": null,
  "imageId": null,
  "image": null
}
```

##### Error (400)
```json
{
  "error": "Description of the error"
}
```

### DELETE Organization

#### DELETE /api/organization/delete

#### Description
Deletes the organization.
There are still some issues with this one

#### Restriction
Has to be Admin.

#### URL
`DELETE /api/organization/delete`

#### Parameters
| Parameter | Type | Required | Description         |
|-----------|------|----------|---------------------|

#### Example Request
```http
DELETE /api/organization/delete
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
