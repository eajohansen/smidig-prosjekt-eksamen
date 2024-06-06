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

    #endregion

    #region POST

    public async Task<bool> AddEvent(int userId, Event eEvent, int organizationId) {
        try {
            if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return false;
            }
            
            await _dbCon.Event.AddAsync(eEvent);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding event to database.", exception);
        }
    }

    public async Task<int> AddEventDateTime(int userId, EventDateTime eventDateTime, int organizationId) {
        try {
            if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return 0;
            }

            await _dbCon.EventDateTime.AddAsync(eventDateTime);
            await _dbCon.SaveChangesAsync();
            return _dbCon.EventDateTime.LastAsync().Result.EventDateTimeId;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding eventDateTime to database.", exception);
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

    public async Task<bool> UpdateEventDateTime(int userId, int organizationId, int eventId, EventDateTime eventDateTime) {
        try {
            if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return false;
            }

            Event? eEvent = await _dbCon.Event.FindAsync(eventId);

            if (eEvent == null) {
                return false;
            }
            
            EventDateTime? databaseEventDateTime = await _dbCon.EventDateTime.FindAsync(eEvent.EventDateTimeId);
            if (databaseEventDateTime == null) {
                return false;
            }

            _dbCon.EventDateTime.Update(eventDateTime);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while updating eventDateTime.", exception);
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