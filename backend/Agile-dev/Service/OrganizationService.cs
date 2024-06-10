using agile_dev.Models;
using agile_dev.Repo;
using Microsoft.EntityFrameworkCore;

namespace agile_dev.Service;

public class OrganizationService {
    private readonly InitContext _dbCon;
    public readonly UserService _userService;

    public OrganizationService(InitContext context, UserService userService) {
        _dbCon = context;
        _userService = userService;
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
            }

            return organization;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching organization.", exception);
        }
    }

    #endregion

    #region POST

    public async Task<object> AddOrganization(Organization organization) {
        try {
            if (organization.Image != null) {
                Image? newImage = await CheckIfImageExists(organization.Image);
                if (newImage == null) {
                    await _dbCon.Image.AddAsync(organization.Image);
                    await _dbCon.SaveChangesAsync();
                    newImage = organization.Image;
                }

                organization.ImageId = newImage.ImageId;
            }
            
            await _dbCon.Organization.AddAsync(organization);
            await _dbCon.SaveChangesAsync();
            
            
            return organization;
        }
        catch (Exception exception) {
            Console.WriteLine(exception);
            throw new Exception("An error occurred while adding organization to database.", exception);
        }
    }

    #endregion
    
    #region PUT

    public async Task<bool> UpdateOrganization(Organization organization) {
        try {
            
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

    // public async Task<bool> DeleteOrganization(string userId, Organization organization) {
    //     try {
    //         
    //         _dbCon.Organization.Remove(organization);
    //         await _dbCon.SaveChangesAsync();
    //         return true;
    //     }
    //     catch (Exception exception) {
    //         throw new Exception("An error occurred while trying to delete organization.", exception);
    //     }
    // }

    #endregion

    #region MISCELLANEOUS

    private async Task<List<Organization>> AddRelationToOrganization(List<Organization> organizations) {
        List<int> organizationIds = organizations.Select(organizer=> organizer.OrganizationId).ToList();

        List<Organization> newOrganizations = await _dbCon.Organization
            .Where(organizer=> organizationIds.Contains(organizer.OrganizationId))
            .Include(organizer=> organizer.Followers)
            .Include(organizer=> organizer.Organizers)
            .Include(organizer=> organizer.Events)
            .ToListAsync();

        return newOrganizations;
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

    #endregion
}