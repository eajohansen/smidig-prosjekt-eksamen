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

    public async Task<bool> AddEvent(int userId, Event eEvent, int organizationId) {
        try {
            if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return false;
            }
            
            Event tempEvent = (await _dbCon.Event.AddAsync(eEvent)).Entity;
            
            foreach (EventCustomField eEventCustomField in eEvent.EventCustomFields) {
                eEventCustomField.EventId = tempEvent.EventId;
                _dbCon.EventCustomField.Update(eEventCustomField);
            }
            
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding event to database.", exception);
        }
    }
    
    public async Task<int> AddPlace(int userId, Place place, int organizationId) {
        try {
            if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return 0;
            }

            await _dbCon.Place.AddAsync(place);
            await _dbCon.SaveChangesAsync();
            return _dbCon.Place.LastAsync().Result.PlaceId;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding eventDateTime to database.", exception);
        }
    }
    
    public async Task<int> AddContactPerson(int userId, ContactPerson contactPerson, int organizationId) {
        try {
            if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return 0;
            }

            await _dbCon.ContactPerson.AddAsync(contactPerson);
            await _dbCon.SaveChangesAsync();
            return _dbCon.ContactPerson.LastAsync().Result.ContactPersonId;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding eventDateTime to database.", exception);
        }
    }

    public async Task<List<EventCustomField>> AddCustomFields(int userId, int organizationId,
        List<CustomField> customFields) {
        try {
            List<EventCustomField> eventCustomFields = [];
            if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return eventCustomFields;
            }

            foreach (CustomField customField in customFields) {
                
                CustomField tempCustomField = (await _dbCon.CustomField.AddAsync(customField)).Entity;
                EventCustomField newEventCustomField = new EventCustomField(tempCustomField.CustomFieldId, 0);
                
                newEventCustomField = (await _dbCon.EventCustomField.AddAsync(newEventCustomField)).Entity;
                eventCustomFields.Add(newEventCustomField);
            }
            
            await _dbCon.SaveChangesAsync();
            return eventCustomFields;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding customFields to database.", exception);
        }
    }
    
    #endregion
    
    #region PUT

    public async Task<bool> UpdateEvent(int userId, int organizationId, Event eEvent) {
        try {
            if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return false;
            }

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

    #endregion
}