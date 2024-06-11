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

    public async Task<HandleReturn<ICollection<EventDtoBackend>>> FetchAllEvents() {
        try {
            ICollection<Event> foundEvents = await _dbCon.Event
                .Where(eEvent => eEvent.Private.Equals(false) && eEvent.Published.Equals(true))
                .ToListAsync();
            
            if (foundEvents.Count == 0) {
                return HandleReturn<ICollection<EventDtoBackend>>.Failure("Could not find any events");
            }
            
            List<int> eventIds = foundEvents.Select(userEvent => userEvent.EventId).ToList();
            List<EventDtoBackend> fetchedEvents = ConvertEventsToEventDtoBackend(eventIds);
            return HandleReturn<ICollection<EventDtoBackend>>.Success(fetchedEvents);
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching events.", exception);
        }
    }

    // Fetch all the events that the user is attending
    public async Task<HandleReturn<ICollection<EventDtoBackend>>>? FetchAllEventsByAttending(string userName) {
        try {
            HandleReturn<UserFrontendDto> user = await _organizationService._userService.FetchUserByEmail(userName);

            if (!user.IsSuccess) {
                return HandleReturn<ICollection<EventDtoBackend>>.Failure("Could not find user by email");
            }

            ICollection<UserEvent> userEvents = _dbCon.UserEvent
                .Where(userEvent => userEvent.Id.Equals(user.Value.Id))
                .ToList();

            List<int> eventIds = userEvents.Select(userEvent => userEvent.EventId).ToList();
            List<EventDtoBackend> foundEvents = ConvertEventsToEventDtoBackend(eventIds);

            return HandleReturn<ICollection<EventDtoBackend>>.Success(foundEvents);
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching events that the user is attending.", exception);
        }
    }

    // Fetch all events that the user is not attending
    public async Task<HandleReturn<ICollection<EventDtoBackend>>> FetchAllEventsByNotAttending(string userName) {
        try {
            HandleReturn<UserFrontendDto> user = await _organizationService._userService.FetchUserByEmail(userName);

            if (!user.IsSuccess) {
                return HandleReturn<ICollection<EventDtoBackend>>.Failure("Could not find user by email");
            }
        
            List<int> attendedEventIds = _dbCon.UserEvent
                .Where(userEvent => userEvent.Id == user.Value.Id)
                .Select(userEvent => userEvent.EventId)
                .ToList();
        
            List<Event> allEvents = _dbCon.Event.ToList();

            List<int> notAttendingEventIds = allEvents
                .Where(eEvent => !attendedEventIds.Contains(eEvent.EventId))
                .ToList().Select(userEvent => userEvent.EventId).ToList();
            
            List<EventDtoBackend> foundEvents = ConvertEventsToEventDtoBackend(notAttendingEventIds);
        
            return HandleReturn<ICollection<EventDtoBackend>>.Success();
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching events that the user is not attending.", exception);
        }
    }
    
    // Fetch for organizer to see all events that the organization has
    public async Task<HandleReturn<ICollection<EventDtoBackend>>> FetchAllEventsByOrganization(int organizationId) {
        try {
            ICollection<Event> foundEvents = await _dbCon.Event
                .Where(eEvent => eEvent.OrganizationId.Equals(organizationId) && eEvent.Private.Equals(false))
                .ToListAsync();
            
            if (foundEvents.Count == 0) {
                return HandleReturn<ICollection<EventDtoBackend>>.Failure("Could not find any events for this organization");
            }
        
            List<int> eventIds = foundEvents.Select(userEvent => userEvent.EventId).ToList();
            List<EventDtoBackend> fetchedEvents = ConvertEventsToEventDtoBackend(eventIds);
            return HandleReturn<ICollection<EventDtoBackend>>.Success(fetchedEvents);
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

    public async Task<HandleReturn<EventDtoBackend>> FetchEventById(int id) {
        try {
            Event? eEvent = await _dbCon.Event.FindAsync(id);
            if (eEvent != null) {
                List<EventDtoBackend> foundEvents = ConvertEventsToEventDtoBackend([id]);
                return HandleReturn<EventDtoBackend>.Success(foundEvents[0]);
            }

            return HandleReturn<EventDtoBackend>.Failure("Could not find event");
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
    
    public async Task<HandleReturn<Event>> AddEvent(string userName, EventDtoFrontend frontendEvent) {
        try {
            HandleReturn<UserFrontendDto> user = await _organizationService._userService.FetchUserByEmail(userName);
            if (!user.IsSuccess) {
                return HandleReturn<Event>.Failure("Could not find user by email");
            }
            
            Event eEvent = new Event {
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
                try {
                    eEvent.StartTime = CombineDateTime(frontendEvent.Start, frontendEvent.StartTime);
                }
                catch (Exception exception) {
                    throw new Exception("An error occurred while combining date and time.", exception);
                }
            }
            
            if (frontendEvent is { End: not null, EndTime: not null }) {
                try {
                    eEvent.EndTime = CombineDateTime(frontendEvent.End, frontendEvent.EndTime);
                }
                catch (Exception exception) {
                    throw new Exception("An error occurred while combining date and time.", exception);
                }
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

            return HandleReturn<Event>.Success(eEvent);
        }
        catch (Exception exception) {
            Console.WriteLine(exception);
            throw new Exception("An error occurred while adding event to database.", exception);
        }
    }
    
    #endregion
    
    #region PUT

    public async Task<HandleReturn<bool>> UpdateEvent(Event eEvent) {
        try {
            Event? databaseEvent = await _dbCon.Event.Where(vEvent => vEvent.EventId.Equals(eEvent.EventId))
                .Include(eEvent => eEvent.EventCustomFields).FirstOrDefaultAsync();
            if (databaseEvent == null) {
                return HandleReturn<bool>.Failure("Could not find event in database");
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

            databaseEvent.PublishedAt = databaseEvent.Published ? DateTime.Now : null;

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
            
            return HandleReturn<bool>.Success();
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while updating event.", exception);
        }
    }

    #endregion
    
    #region DELETE

    public async Task<HandleReturn<bool>> DeleteEvent(Event eEvent) {
        try {
            /*
            Event? deleteEvent = await _dbCon.Event.FindAsync(eEvent.EventId);
            if (deleteEvent == null) {
                return false;
            }
            _dbCon.Event.Remove(deleteEvent); */
            _dbCon.Event.Remove(eEvent);
            await _dbCon.SaveChangesAsync();
            return HandleReturn<bool>.Success();
        }
        catch (Exception exception) {
            Console.WriteLine(exception);
            throw new Exception("An error occurred while trying to delete event.", exception);
        }
    }

    #endregion
    
    #region MISCELLANEOUS
    
    private async Task<ContactPerson?> CheckIfContactPersonExists(ContactPerson newContactPerson) {
        ContactPerson? contactPerson;
        
        if (newContactPerson.Email == null && newContactPerson.Number == null) {
            // @Eirik
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
        
        try {
            string correctDate = DateTime.ParseExact(date, "yyyy-MM-dd", null).ToString("dd-MM-yyyy");
            return DateTime.ParseExact($"{correctDate} {time}", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while combining date and time.", exception);
        }

    }
    
    private List<EventDtoBackend> ConvertEventsToEventDtoBackend(List<int> eventIds) {
        List<EventDtoBackend> foundEvents = _dbCon.Event
            .Where(eEvent => eventIds.Contains(eEvent.EventId))
            .Select(eEvent => new EventDtoBackend {
                EventId = eEvent.EventId,
                Title = eEvent.Title,
                Description = eEvent.Description,
                Capacity = eEvent.Capacity,
                AgeLimit = eEvent.AgeLimit,
                Private = eEvent.Private,
                Published = eEvent.Published,
                AvailableCapacity = eEvent.Capacity - eEvent.UserEvents.Count,
                PlaceLocation = eEvent.Place.Location,
                PlaceUrl = eEvent.Place.Url,
                ImageLink = eEvent.Image.Link,
                ImageDescription = eEvent.Image.ImageDescription,
                ContactPersonName = eEvent.ContactPerson.Name,
                ContactPersonEmail = eEvent.ContactPerson.Email,
                ContactPersonNumber = eEvent.ContactPerson.Number,
                OrganizationName = eEvent.Organization.Name,
                EventCustomFields = eEvent.EventCustomFields
                    .Select(ecf => new EventCustomField {
                        EventCustomFieldId = ecf.EventCustomFieldId,
                        CustomFieldId = ecf.CustomFieldId,
                        EventId = ecf.EventId,
                        CustomField = new CustomField {
                            CustomFieldId = ecf.CustomField.CustomFieldId,
                            Description = ecf.CustomField.Description,
                            Value = ecf.CustomField.Value
                        }
                    }).ToList(),
                StartTime = eEvent.StartTime,
                EndTime = eEvent.EndTime,
                // Include other properties as needed
            })
            .ToList();
        return foundEvents;
    }

    #endregion
}