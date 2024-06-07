using agile_dev.Models;
using agile_dev.Repo;
using Microsoft.EntityFrameworkCore;

namespace agile_dev.Service;

public class OrganizationService {
    private readonly InitContext _dbCon;
    private readonly UserService _userService;
    private readonly EventService _eventService;

    public OrganizationService(InitContext context) {
        _dbCon = context;
    }

    #region GET

    public async Task<ICollection<Organization>> FetchAllOrganizations() {
        try {
            ICollection<Organization> foundOrganizations = await _dbCon.Organization.ToListAsync();
            ICollection<Organization> newOrganizations = await AddRelationToOrganization(foundOrganizations.ToList());
            
            return newOrganizations;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching organizations.", exception);
        }
    }

    public async Task<Organization?> FetchOrganizationById(int id) {
        try {
            Organization? organization = await _dbCon.Organization.FindAsync(id);
            if (organization != null) {
                List<Organization> foundOrganization = [organization];
                foundOrganization = await AddRelationToOrganization(foundOrganization);
                return foundOrganization[0];
            } else {
                return organization;
            }
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching organization.", exception);
        }
    }

    #endregion

    #region POST

    public async Task<bool> AddOrganization(int userId, Organization organization) {
        try {
            User? user = await _userService.FetchUserById(userId);
            if (user == null || !_userService.IsUserAdmin(user).Result) {
                return false;
            }

            if (organization.Image != null) {
                Image? newImage = await _eventService.CheckIfImageExists(organization.Image);
                if (newImage == null) {
                    await _dbCon.Image.AddAsync(organization.Image);
                    await _dbCon.SaveChangesAsync();
                    newImage = organization.Image;
                }

                organization.ImageId = newImage.ImageId;
            }
            
            
            await _dbCon.Organization.AddAsync(organization);
            await _dbCon.SaveChangesAsync();
            
            
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding organization to database.", exception);
        }
    }

    #endregion

    #region PUT

    public async Task<bool> UpdateOrganization(int userId, Organization organization) {
        try {
            if (!CheckValidation(userId, organization.OrganizationId).Result) {
                return false;
            }
            
            Organization? databaseOrganization = await FetchOrganizationById(organization.OrganizationId);
            if (databaseOrganization == null) {
                return false;
            }

            _dbCon.Organization.Update(organization);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while updating organization.", exception);
        }
    }

    #endregion

    #region DELETE

    public async Task<bool> DeleteOrganization(int userId, Organization organization) {
        try {
            if (!CheckValidation(userId, organization.OrganizationId).Result) {
                return false;
            }
            
            _dbCon.Organization.Remove(organization);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while trying to delete organization.", exception);
        }
    }

    #endregion

    #region MISCELLANEOUS

    private async Task<List<Organization>> AddRelationToOrganization(List<Organization> organizations) {
        List<int> organizationIds = organizations.Select(organizer=> organizer.OrganizationId).ToList();

        List<Organization> newOrganizations = await _dbCon.Organization
            .Where(organizer=> organizationIds.Contains(organizer.OrganizationId))
            .Include(organizer=> organizer.Followers)
            .Include(organizer=> organizer.Organizers)
            .ToListAsync();

        return newOrganizations;
    }
    
    public async Task<bool> CheckValidation(int userId, int organizationId) {
        User? user = await _userService.FetchUserById(userId);
        Organization? organization = await FetchOrganizationById(organizationId);

        if (user == null || organization == null) {
            return false;
        }
            
        bool isAdmin = await _userService.IsUserAdmin(user);
        bool isOrganizer = await _userService.IsUserOrganizerForOrganization(user, organization);
        if (!isAdmin && !isOrganizer) {
            return false;
        } else {
            return true;
        }
    }

    #endregion
}