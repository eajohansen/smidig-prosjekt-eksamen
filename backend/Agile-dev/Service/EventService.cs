using System.Globalization;
using agile_dev.Dto;
using agile_dev.Models;
using agile_dev.Repo;
using Microsoft.EntityFrameworkCore;

namespace agile_dev.Service;

public class EventService {
    private readonly InitContext _dbCon;
    private readonly OrganizationService _organizationService;

    public EventService(InitContext context, OrganizationService organizationService) {
        _dbCon = context;
        _organizationService = organizationService;
    }


    #region GET

    public async Task<ICollection<Event>> FetchAllEvents() {
        try {
            ICollection<Event> foundEvents = await _dbCon.Event
                .Where(eEvent => eEvent.Private.Equals(false) && eEvent.Published.Equals(true))
                .ToListAsync();
            ICollection<Event> newEvents = AddRelationToEvent(foundEvents.ToList()).Result;
            
            return newEvents;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching events.", exception);
        }
    }

    // Fetch all the events that the user is attending
    public async Task<ICollection<Event>>? FetchAllEventsByAttending(string userName) {
        try {
            /*UserFrontendDto? user = await _organizationService._userService.FetchUserByEmail(userName);
            
            // Get the user's userEvents
            ICollection<UserEvent> userEvents = _dbCon.UserEvent
                .Where(userEvent => userEvent.Id.Equals(user.Id))
                .ToList();

            // Extract the EventIds from the user's userEvents
            List<int> eventIds = userEvents.Select(userEvent => userEvent.EventId).ToList();

            // Retrieve the events without the UserEvents property
            var foundEvents = _dbCon.Event
                .Where(eEvent => eventIds.Contains(eEvent.EventId))
                .Select(eEvent => new {
                    eEvent.EventId,
                    eEvent.Title,
                    eEvent.Description,
                    eEvent.Capacity,
                    eEvent.AgeLimit,
                    eEvent.Private,
                    eEvent.Published,
                    eEvent.PlaceId,
                    eEvent.ImageId,
                    eEvent.ContactPersonId,
                    eEvent.OrganizationId,
                    eEvent.CreatedAt,
                    eEvent.PublishedAt,
                    eEvent.StartTime,
                    eEvent.EndTime,
                    // Include other properties as needed
                })
                .ToList();

            
        
            //ICollection<Event> newEvents = AddRelationToEvent(attendingEvents.ToList()).Result;
            return null;*/
            
            UserFrontendDto? user = await _organizationService._userService.FetchUserByEmail(userName);
            
            ICollection<Event>? foundEvents = null;
            
            if (user == null) {
                return foundEvents;
            }
            
            foundEvents = await _dbCon.Event
                .Where(eEvent =>
                    (eEvent.UserEvents.Any(userEvent => userEvent.Id == user.Id)) && eEvent.Private.Equals(false) && eEvent.Published.Equals(true))
                .ToListAsync();
        
            ICollection<Event> newEvents = await AddRelationToEvent(foundEvents.ToList());
            return newEvents;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching events that the user is attending.", exception);
        }
    }
    
    // Fetch all events that the user is not attending
    public async Task<ICollection<Event>?> FetchAllEventsByNotAttending(string userName) {
        try {
            UserFrontendDto? user = await _organizationService._userService.FetchUserByEmail(userName);
            
            ICollection<Event>? foundEvents = null;
            
            if (user == null) {
                return foundEvents;
            }
            
            foundEvents = await _dbCon.Event
                .Where(eEvent =>
                    (eEvent.UserEvents.Count == 0 || eEvent.UserEvents.Any(userEvent => userEvent.Id != user.Id)) && eEvent.Private.Equals(false) && eEvent.Published.Equals(true))
                .ToListAsync();
        
            ICollection<Event> newEvents = await AddRelationToEvent(foundEvents.ToList());
            return newEvents;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching events that the user is not attending.", exception);
        }
    }
    
    // Fetch for organizer to see all events that the organization has
    public async Task<ICollection<Event>> FetchAllEventsByOrganization(int organizationId) {
        try {
            ICollection<Event> foundEvents = await _dbCon.Event
                .Where(eEvent => eEvent.OrganizationId.Equals(organizationId) && eEvent.Private.Equals(false))
                .ToListAsync();
        
            ICollection<Event> newEvents = await AddRelationToEvent(foundEvents.ToList());
            return newEvents;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching events from this organization.", exception);
        }
    }
    
    /*
    // Fetch all events by other organizations
    public async Task<ICollection<Event>> FetchAllEventsByOtherOrganizations(int organizationId) {
        try {
            ICollection<Event> foundEvents = await _dbCon.Event
                .Where(eEvent => eEvent.OrganizationId != organizationId && eEvent.Private.Equals(false) && eEvent.Published.Equals(true))
                .ToListAsync();
        
            ICollection<Event> newEvents = await AddRelationToEvent(foundEvents.ToList());
            return newEvents;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching events not from this organization.", exception);
        }
    }
    */

    public async Task<Event?> FetchEventById(int id) {
        try {
            Event? eEvent = await _dbCon.Event.FindAsync(id);
            if (eEvent != null) {
                List<Event> foundEvent = [eEvent];
                foundEvent = await AddRelationToEvent(foundEvent);
                return foundEvent[0];
            } else {
                return eEvent;
            }
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching event.", exception);
        }
    }

    /*
    public async Task<ICollection<CustomField?>> FetchAllCustomFields(int eventId) {
        try {
            List<CustomField?> customFields = [];
            Event? eEvent = await FetchEventById(eventId);
            
            if (eEvent == null) {
                return customFields;
            }

            if (eEvent.EventCustomFields == null) return customFields;
            foreach (EventCustomField eventCustomField in eEvent.EventCustomFields) {
                customFields.Add(await _dbCon.CustomField.FindAsync(eventCustomField.CustomFieldId));
            }

            return customFields;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching customFields.", exception);
        }
    }
    */

    #endregion

    #region POST
    
    public async Task<object> AddEvent(string userName, EventDtoFrontend frontendEvent) {
        try {
            UserFrontendDto? user = await _organizationService._userService.FetchUserByEmail(userName);
            if (user == null) {
                return "Could not find user by email";
            }

            /*string requiredFields = "";

            if (requiredFields.Length > 0 && (frontendEvent.Event.Title == null || frontendEvent.Event.Title.Length == 0)) {
                requiredFields = "the event did not have any title";
            } else if (frontendEvent.Event.Title == null || frontendEvent.Event.Title.Length == 0) {
                requiredFields += ",\n the event did not have any title";
            }
            
            if (requiredFields.Length > 0 && (frontendEvent.Event. == null || frontendEvent.Event..Length == 0)) {
                requiredFields = "";
            } else if (frontendEvent.Event. == null || frontendEvent.Event..Length == 0) {
                requiredFields += ",\n ";
            }*/
            
            Event eEvent = new Event() {
                Title = frontendEvent.Event.Title,
                Private = frontendEvent.Event.Private,
                Published = frontendEvent.Event.Published,
                OrganizationId = frontendEvent.Event.OrganizationId,
                CreatedAt = DateTime.Now
            };
            
            if (frontendEvent.Event.Description != null) {
                eEvent.Description = frontendEvent.Event.Description;
            }
            
            if (frontendEvent.Event.Capacity != null) {
                eEvent.Capacity = frontendEvent.Event.Capacity;
            }
            
            if (frontendEvent.Event.AgeLimit != null) {
                eEvent.AgeLimit = frontendEvent.Event.AgeLimit;
            }

            if (frontendEvent.Event.Place != null) {
                eEvent.Place = frontendEvent.Event.Place;
            }

            if (frontendEvent.Event.Image != null) {
                eEvent.Image = frontendEvent.Event.Image;
            }
            
            if (frontendEvent.Event.ContactPerson != null) {
                eEvent.ContactPerson = frontendEvent.Event.ContactPerson;
            }
            
            if (frontendEvent is { Start: not null, StartTime: not null }) {
                eEvent.StartTime = CombineDateTime(frontendEvent.Start, frontendEvent.StartTime);
            }
            
            if (frontendEvent is { End: not null, EndTime: not null }) {
                eEvent.EndTime = CombineDateTime(frontendEvent.End, frontendEvent.EndTime);
            }

            if (eEvent.Published) {
                eEvent.PublishedAt = DateTime.Now;
            }

            if (eEvent.ContactPerson != null) {
                ContactPerson? newContactPerson = await CheckIfContactPersonExists(eEvent.ContactPerson);
                if (newContactPerson == null) {
                    await _dbCon.ContactPerson.AddAsync(eEvent.ContactPerson);
                    await _dbCon.SaveChangesAsync();
                    newContactPerson = eEvent.ContactPerson;
                }
                
                eEvent.ContactPersonId = newContactPerson.ContactPersonId;
            }

            if (eEvent.Image != null) {
                Image? newImage = await _organizationService.CheckIfImageExists(eEvent.Image);
                if (newImage == null) {
                    await _dbCon.Image.AddAsync(eEvent.Image);
                    await _dbCon.SaveChangesAsync();
                    newImage = eEvent.Image;
                }
                
                eEvent.ImageId = newImage.ImageId;
            }

            if (eEvent.Place != null) {
                Place? newPlace = await CheckIfPlaceExists(eEvent.Place);
                if (newPlace == null) { 
                    await _dbCon.Place.AddAsync(eEvent.Place);
                    await _dbCon.SaveChangesAsync();
                    newPlace = eEvent.Place;
                }
                
                eEvent.PlaceId = newPlace.PlaceId;
            }
            
            await _dbCon.Event.AddAsync(eEvent);
            await _dbCon.SaveChangesAsync();
            
            // ReSharper disable once InvertIf
            if (frontendEvent.Event.EventCustomFields != null) {
                ICollection<EventCustomField> customFields = frontendEvent.Event.EventCustomFields.ToList();
                frontendEvent.Event.EventCustomFields.Clear();
                foreach (EventCustomField customField in customFields) {
                    CustomField? newCustomField = await CheckIfCustomFieldExists(customField);
                    EventCustomField newEventCustomField;
                    if (newCustomField == null) {
                        newCustomField = new CustomField {
                            Description = customField.CustomField!.Description,
                            Value = customField.CustomField.Value
                        };
                        await _dbCon.CustomField.AddAsync(newCustomField);
                        await _dbCon.SaveChangesAsync();
                        newEventCustomField = new EventCustomField {
                            CustomFieldId = newCustomField.CustomFieldId,
                            EventId = eEvent.EventId
                        };
                    } else {
                        newEventCustomField = new EventCustomField {
                            CustomFieldId = newCustomField.CustomFieldId,
                            EventId = eEvent.EventId
                        };
                    }

                    await _dbCon.EventCustomField.AddAsync(newEventCustomField);
                }

                await _dbCon.SaveChangesAsync();
            }
            return eEvent;
        }
        catch (Exception exception) {
            Console.WriteLine(exception);
            throw new Exception("An error occurred while adding event to database.", exception);
        }
    }
    
    #endregion
    
    #region PUT

    public async Task<bool> UpdateEvent(Event eEvent) {
        try {
            Event? databaseEvent = await FetchEventById(eEvent.EventId);
            if (databaseEvent == null) {
                return false;
            }

            databaseEvent.Title = eEvent.Title;
            databaseEvent.Private = eEvent.Private;
            databaseEvent.Published = eEvent.Published;
            databaseEvent.OrganizationId = eEvent.OrganizationId;
            databaseEvent.Description = eEvent.Description;
            databaseEvent.Capacity = eEvent.Capacity;
            databaseEvent.AgeLimit = eEvent.AgeLimit;
            databaseEvent.StartTime = eEvent.StartTime;
            databaseEvent.EndTime = eEvent.EndTime;
            
            if (eEvent.Published) {
                eEvent.PublishedAt = DateTime.Now;
            } else {
                eEvent.PublishedAt = null;
            }
            
            if (eEvent.ContactPerson != null) {
                ContactPerson? newContactPerson = await CheckIfContactPersonExists(eEvent.ContactPerson);
                if (newContactPerson == null) {
                    await _dbCon.ContactPerson.AddAsync(eEvent.ContactPerson);
                    await _dbCon.SaveChangesAsync();
                    newContactPerson = eEvent.ContactPerson;
                }

                databaseEvent.ContactPerson = newContactPerson;
                databaseEvent.ContactPersonId = newContactPerson.ContactPersonId;
            } else {
                databaseEvent.ContactPerson = null;
                databaseEvent.ContactPersonId = null;
            }
            
            if (eEvent.Image != null) {
                Image? newImage = await _organizationService.CheckIfImageExists(eEvent.Image);
                if (newImage == null) {
                    await _dbCon.Image.AddAsync(eEvent.Image);
                    await _dbCon.SaveChangesAsync();
                    newImage = eEvent.Image;
                }

                databaseEvent.Image = newImage;
                databaseEvent.ImageId = newImage.ImageId;
            } else {
                databaseEvent.Image = null;
                databaseEvent.ImageId = null;
            }

            if (eEvent.Place != null) {
                Place? newPlace = await CheckIfPlaceExists(eEvent.Place);
                if (newPlace == null) { 
                    await _dbCon.Place.AddAsync(eEvent.Place);
                    await _dbCon.SaveChangesAsync();
                    newPlace = eEvent.Place;
                }

                databaseEvent.Place = newPlace;
                databaseEvent.PlaceId = newPlace.PlaceId;
            } else {
                databaseEvent.Place = null;
                databaseEvent.PlaceId = null;
            }

            _dbCon.Event.Update(databaseEvent);
            await _dbCon.SaveChangesAsync();

            if (!Equals(eEvent.EventCustomFields, databaseEvent.EventCustomFields)) {

                if (databaseEvent.EventCustomFields == null && eEvent.EventCustomFields != null) {
                    ICollection<EventCustomField> customFields = eEvent.EventCustomFields.ToList();
                    eEvent.EventCustomFields.Clear();
                    foreach (EventCustomField customField in customFields) {
                        CustomField? newCustomField = await CheckIfCustomFieldExists(customField);
                        EventCustomField newEventCustomField;
                        if (newCustomField == null) {
                            newCustomField = new CustomField {
                                Description = customField.CustomField!.Description,
                                Value = customField.CustomField.Value
                            };
                            await _dbCon.CustomField.AddAsync(newCustomField);
                            await _dbCon.SaveChangesAsync();
                            newEventCustomField = new EventCustomField {
                                CustomFieldId = newCustomField.CustomFieldId,
                                EventId = databaseEvent.EventId
                            };
                        } else {
                            newEventCustomField = new EventCustomField {
                                CustomFieldId = newCustomField.CustomFieldId,
                                EventId = databaseEvent.EventId
                            };
                        }

                        await _dbCon.EventCustomField.AddAsync(newEventCustomField);
                    }
                } else if (databaseEvent.EventCustomFields != null && eEvent.EventCustomFields != null) {

                    ICollection<EventCustomField> eventCustomFields = eEvent.EventCustomFields.ToList();
                    eEvent.EventCustomFields.Clear();
                    
                    foreach (EventCustomField eventCustomField in databaseEvent.EventCustomFields) {
                        if (!eventCustomFields.Contains(eventCustomField)) {
                            _dbCon.EventCustomField.Remove(eventCustomField);
                        }
                    }

                    foreach (EventCustomField eventCustomField in eventCustomFields) {
                        if (!databaseEvent.EventCustomFields.Contains(eventCustomField)) {
                            CustomField? newCustomField = await CheckIfCustomFieldExists(eventCustomField);
                            EventCustomField newEventCustomField;
                            if (newCustomField == null) {
                                newCustomField = new CustomField {
                                    Description = eventCustomField.CustomField!.Description,
                                    Value = eventCustomField.CustomField.Value
                                };
                                await _dbCon.CustomField.AddAsync(newCustomField);
                                await _dbCon.SaveChangesAsync();
                                newEventCustomField = new EventCustomField {
                                    CustomFieldId = newCustomField.CustomFieldId,
                                    EventId = eEvent.EventId
                                };
                            } else {
                                newEventCustomField = new EventCustomField {
                                    CustomFieldId = newCustomField.CustomFieldId,
                                    EventId = eEvent.EventId
                                };
                            }

                            await _dbCon.EventCustomField.AddAsync(newEventCustomField);
                        }
                    }
                    await _dbCon.SaveChangesAsync();
                } else if (databaseEvent.EventCustomFields != null && eEvent.EventCustomFields == null) {
                    foreach (EventCustomField eventCustomField in databaseEvent.EventCustomFields) {
                        _dbCon.Remove(eventCustomField);
                    }

                    await _dbCon.SaveChangesAsync();
                }
            }
            
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while updating event.", exception);
        }
    }

    #endregion
    
    #region DELETE

    public async Task<bool> DeleteEvent(Event eEvent) {
        try {
            _dbCon.Event.Remove(eEvent);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            Console.WriteLine(exception);
            throw new Exception("An error occurred while trying to delete event.", exception);
        }
    }

    #endregion
    
    #region MISCELLANEOUS

    private async Task<List<Event>> AddRelationToEvent(List<Event> events) {
        List<int> eventsId = events.Select(eEvent => eEvent.EventId).ToList();

        List<Event> newEvents = await _dbCon.Event
            .Where(eEvent => eventsId.Contains(eEvent.EventId))
            .Include(eEvent => eEvent.Place)
            .Include(eEvent => eEvent.Image)
            .Include(eEvent => eEvent.ContactPerson)
            .Include(eEvent => eEvent.Organization)
            .Include(eEvent => eEvent.EventCustomFields)
            .Include(eEvent => eEvent.UserEvents)
            .ToListAsync();

        return newEvents;
    }
    
    private async Task<ContactPerson?> CheckIfContactPersonExists(ContactPerson newContactPerson) {
        ContactPerson? contactPerson;
        
        if (newContactPerson.Email == null && newContactPerson.Number == null) {
            //
            // It just needs a name, mostly for testing. Should be deleted when frontend can demand number og email
            //
            contactPerson = await _dbCon.ContactPerson
                .Where(loopContactPerson => newContactPerson.Name.Equals(loopContactPerson.Name) && 
                                            loopContactPerson.Number == null && 
                                            loopContactPerson.Email == null)
                .FirstOrDefaultAsync();
            
        } else if (newContactPerson.Email == null) {
            // Email can be null here, but not number. Frontend handles the logic that at least email or number have to exist
            contactPerson = await _dbCon.ContactPerson
                .Where(loopContactPerson => newContactPerson.Name.Equals(loopContactPerson.Name) && 
                                            newContactPerson.Number!.Equals(loopContactPerson.Number) && 
                                            loopContactPerson.Email == null)
                .FirstOrDefaultAsync();
            
        } else if (newContactPerson.Number == null) {
            // Number can be null here, but not email. Frontend handles the logic that at least email or number have to exist
            contactPerson = await _dbCon.ContactPerson
                .Where(loopContactPerson => newContactPerson.Name.Equals(loopContactPerson.Name) && 
                                            newContactPerson.Email.Equals(loopContactPerson.Email) && 
                                            loopContactPerson.Number == null)
                .FirstOrDefaultAsync();
            
        } else {
            contactPerson = await _dbCon.ContactPerson
                .Where(loopContactPerson => newContactPerson.Name.Equals(loopContactPerson.Name) && 
                                            newContactPerson.Email.Equals(loopContactPerson.Email) && 
                                            newContactPerson.Number.Equals(loopContactPerson.Number))
                .FirstOrDefaultAsync();
        }
        
        return contactPerson;
    }
    
    private async Task<Place?> CheckIfPlaceExists(Place newPlace) {
        Place? place;
        if (newPlace.Url == null) {
            place = await _dbCon.Place
                .Where(loopPlace => newPlace.Location.Equals(loopPlace.Location) && loopPlace.Url == null)
                .FirstOrDefaultAsync();
        } else {
            place = await _dbCon.Place
                .Where(loopPlace => newPlace.Location.Equals(loopPlace.Location) && newPlace.Url.Equals(loopPlace.Url))
                .FirstOrDefaultAsync();
        }
        
        return place;
    }

    private async Task<CustomField?> CheckIfCustomFieldExists(EventCustomField newEventCustomField) {
        string customFieldDescription = newEventCustomField.CustomField!.Description;
        bool customFieldValue = newEventCustomField.CustomField.Value;

        CustomField? customField = await _dbCon.CustomField
            .Where(customField => customFieldDescription.Equals(customField.Description) &&
                                  customFieldValue.Equals(customField.Value)).FirstOrDefaultAsync();
        return customField;
    }
    
    private DateTime CombineDateTime(string date, string time) {
        
        string correctDate = DateTime.ParseExact(date, "yyyy-MM-dd", null).ToString("dd-MM-yyyy");
        
        return DateTime.ParseExact($"{correctDate} {time}", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
    }

    #endregion
}