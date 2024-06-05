using agile_dev.Models;
using agile_dev.Repo;
using Microsoft.EntityFrameworkCore;

namespace agile_dev.Service;

public class OrganizationService {
    private readonly InitContext _dbCon;
    private readonly UserService _userService;

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

    public async Task<bool> AddOrganization(Organization organization) {
        try {
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
            User? user = await _userService.FetchUserById(userId);
            
            if (user == null) {
                return false;
            }
            
            bool userAdmin = await _userService.IsUserAdmin(user);
            bool organizer = await _userService.IsUserOrganizerForOrganization(user, organization);

            if (!userAdmin && !organizer) {
                return false;
            }
            
            Organization? dbOrganization = await FetchOrganizationById(organization.OrganizationId);
            if (dbOrganization == null) {
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

    public async Task<bool> DeleteOrganization(Organization organization) {
        try {
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
    

    #endregion
}