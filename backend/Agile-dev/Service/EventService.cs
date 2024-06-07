using System.Globalization;
using agile_dev.Dto;
using agile_dev.Models;
using agile_dev.Repo;
using Microsoft.EntityFrameworkCore;

namespace agile_dev.Service;

public class EventService {
    private readonly InitContext _dbCon;
    private readonly UserService _userService;
    private readonly OrganizationService _organizationService;

    public EventService(InitContext context) {
        _dbCon = context;
    }

    #region GET

    public async Task<ICollection<Event>> FetchAllEvents() {
        try {
            ICollection<Event> foundEvents = await _dbCon.Event.ToListAsync();
            ICollection<Event> newEvents = AddRelationToEvent(foundEvents.ToList()).Result;

            return newEvents;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching events.", exception);
        }
    }

    public async Task<ICollection<Event>> FetchAllEventsByAttending(int userId) {
        try {
            ICollection<Event> foundEvents = await _dbCon.Event
                .Where(eEvent => eEvent.UserEvents != null && eEvent.UserEvents.Any(userEvent => userEvent.UserId == userId))
                .ToListAsync();
        
            ICollection<Event> newEvents = await AddRelationToEvent(foundEvents.ToList());
            return newEvents;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching events that the user is attending.", exception);
        }
    }
    
    public async Task<ICollection<Event>> FetchAllEventsByNotAttending(int userId) {
        try {
            ICollection<Event> foundEvents = await _dbCon.Event
                .Where(eEvent => eEvent.UserEvents != null && eEvent.UserEvents.Any(userEvent => userEvent.UserId != userId))
                .ToListAsync();
        
            ICollection<Event> newEvents = await AddRelationToEvent(foundEvents.ToList());
            return newEvents;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching events that the user is not attending.", exception);
        }
    }
    
    public async Task<ICollection<Event>> FetchAllEventsByOrganization(int organizationId) {
        try {
            ICollection<Event> foundEvents = await _dbCon.Event
                .Where(eEvent => eEvent.OrganizationId.Equals(organizationId))
                .ToListAsync();
        
            ICollection<Event> newEvents = await AddRelationToEvent(foundEvents.ToList());
            return newEvents;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching events from this organization.", exception);
        }
    }
    
    public async Task<ICollection<Event>> FetchAllEventsByOtherOrganizations(int organizationId) {
        try {
            ICollection<Event> foundEvents = await _dbCon.Event
                .Where(eEvent => eEvent.OrganizationId != organizationId)
                .ToListAsync();
        
            ICollection<Event> newEvents = await AddRelationToEvent(foundEvents.ToList());
            return newEvents;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching events not from this organization.", exception);
        }
    }


    public async Task<Event?> FetchEventById(int id) {
        try {
            Event? eEvent = await _dbCon.Event.FindAsync(id);
            if (eEvent != null) {
                List<Event> foundEvent = [eEvent];
                foundEvent = AddRelationToEvent(foundEvent).Result;
                return foundEvent[0];
            } else {
                return eEvent;
            }
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching event.", exception);
        }
    }

    public async Task<ICollection<CustomField?>> FetchAllCustomFields(int eventId) {
        try {
            List<CustomField?> customFields = [];
            Event? eEvent = await FetchEventById(eventId);
            
            if (eEvent == null) {
                return customFields;
            }

            foreach (EventCustomField eventCustomField in eEvent.EventCustomFields) {
                customFields.Add(await _dbCon.CustomField.FindAsync(eventCustomField.CustomFieldId));
            }
            
            return customFields;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching customFields.", exception);
        }
    }

    #endregion

    #region POST

    public async Task<bool> AddEvent(int userId, EventDto frontendEvent, int organizationId) {
        try {
            /*
            if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return false;
            }*/

            Event eEvent = new Event(frontendEvent.Title) {
                Title = frontendEvent.Title,
                Published = frontendEvent.Published,
                OrganizationId = frontendEvent.OrganizationId,
                CreatedAt = DateTime.Now
            };

            if (frontendEvent.Description != null) {
                eEvent.Description = frontendEvent.Description;
            }

            if (frontendEvent.Place != null) {
                eEvent.Place = frontendEvent.Place;
            }

            if (frontendEvent.ImageId != null) {
                eEvent.ImageId = frontendEvent.ImageId;
            }

            if (frontendEvent.Image != null) {
                eEvent.Image = frontendEvent.Image;
            }
            
            if (frontendEvent.ContactPerson != null) {
                eEvent.ContactPerson = frontendEvent.ContactPerson;
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
                Image? newImage = await CheckIfImageExists(eEvent.Image);
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

            if (eEvent.CustomFields != null) {
                ICollection<CustomField>? customFields = eEvent.CustomFields.ToList();
                eEvent.CustomFields.Clear();

                await _dbCon.Event.AddAsync(eEvent);
                await _dbCon.SaveChangesAsync();

                foreach (CustomField customField in customFields) {
                    CustomField? newCustomField = await CheckIfCustomFieldExists(customField);
                    if (newCustomField == null) {
                        await _dbCon.CustomField.AddAsync(customField);
                        await _dbCon.SaveChangesAsync();
                        newCustomField = customField;
                    }

                    await _dbCon.EventCustomField.AddAsync(new EventCustomField(newCustomField.CustomFieldId, eEvent.EventId));
                }
            }

            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding event to database.", exception);
        }
    }
    
    #endregion
    
    #region PUT

    public async Task<bool> UpdateEvent(int userId, int organizationId, Event eEvent) {
        try {
            /*
             if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return false;
            }*/

            Event? databaseEvent = await FetchEventById(eEvent.EventId);
            if (databaseEvent == null) {
                return false;
            }
            
            

            _dbCon.Event.Update(eEvent);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while updating event.", exception);
        }
    }
    
    public async Task<bool> UpdatePlace(int userId, int organizationId, int eventId, Place place) {
        try {
            if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return false;
            }
            
            Event? eEvent = await _dbCon.Event.FindAsync(eventId);

            if (eEvent == null) {
                return false;
            }

            Place? databasePlace = await _dbCon.Place.FindAsync(eEvent.PlaceId);
            if (databasePlace == null) {
                return false;
            }

            _dbCon.Place.Update(databasePlace);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while updating place.", exception);
        }
    }
    
    public async Task<bool> UpdateContactPerson(int userId, int organizationId, int eventId, ContactPerson contactPerson) {
        try {
            if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return false;
            }
            
            Event? eEvent = await _dbCon.Event.FindAsync(eventId);

            if (eEvent == null) {
                return false;
            }

            ContactPerson? databaseContactPerson = await _dbCon.ContactPerson.FindAsync(eEvent.ContactPersonId);
            if (databaseContactPerson == null) {
                return false;
            }

            _dbCon.ContactPerson.Update(databaseContactPerson);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while updating contactPerson.", exception);
        }
    }

    public async Task<bool> UpdateCustomField(int userId, int organizationId, List<CustomField> customFields) {
        try {
            if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return false;
            }

            foreach (CustomField customField in customFields) {
                CustomField? databaseCustomField = await _dbCon.CustomField.FindAsync(customField.CustomFieldId);
                if (databaseCustomField != null) {
                    _dbCon.CustomField.Update(databaseCustomField);
                }
            }

            await _dbCon.SaveChangesAsync();
            return true;

        }
        catch (Exception exception) {
            throw new Exception("An error occurred while updating customField.", exception);
        }
    }

    #endregion
    
    #region DELETE

    public async Task<bool> DeleteEvent(int userId, Event eEvent, int organizationId) {
        try {
            if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return false;
            }

            _dbCon.Event.Remove(eEvent);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while trying to delete event.", exception);
        }
    }

    #endregion
    
    #region MISCELLANEOUS

    private async Task<List<Event>> AddRelationToEvent(List<Event> events) {
        List<int> eventsId = events.Select(eEvent => eEvent.EventId).ToList();

        List<Event> newEvents = await _dbCon.Event
            .Where(eEvent => eventsId.Contains(eEvent.EventId))
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
    
    public async Task<Image?> CheckIfImageExists(Image newImage) {
        Image? image;
        if (newImage.ImageDescription == null) {
            image = await _dbCon.Image
                .Where(loopImage => newImage.Link.Equals(loopImage.Link) && loopImage.ImageDescription == null)
                .FirstOrDefaultAsync();
        } else {
            image = await _dbCon.Image
                .Where(loopImage => newImage.Link.Equals(loopImage.Link) && newImage.ImageDescription.Equals(loopImage.ImageDescription))
                .FirstOrDefaultAsync();
        }
        
        return image;
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

    private async Task<CustomField?> CheckIfCustomFieldExists(CustomField newCustomField) {
        CustomField? customField = await _dbCon.CustomField
            .Where(customField => newCustomField.Value.Equals(customField.Value) && newCustomField.Description.Equals(customField.Description))
            .FirstOrDefaultAsync();

        return customField;
    }
    
    private DateTime CombineDateTime(string date, string time) {
        return DateTime.ParseExact($"{date} {time}", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
    }


    #endregion
}