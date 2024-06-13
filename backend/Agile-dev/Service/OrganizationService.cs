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

    public async Task<HandleReturn<ICollection<Organization>>> FetchAllOrganizations() {
        try {
            ICollection<Organization> foundOrganizations = await _dbCon.Organization.ToListAsync();
            
            if (foundOrganizations.Count == 0) {
                return HandleReturn<ICollection<Organization>>.Failure(foundOrganizations, "No organizations found.");
            }
            
            ICollection<Organization> newOrganizations = await AddRelationToOrganization(foundOrganizations.ToList());
            
            return HandleReturn<ICollection<Organization>>.Success(newOrganizations);
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching organizations.", exception);
        }
    }

    public async Task<HandleReturn<Organization>> FetchOrganizationById(int id) {
        try {
            Organization? organization = await _dbCon.Organization.FindAsync(id);
            
            if (organization == null) {
                return HandleReturn<Organization>.Failure(organization, "No organization found with that id.");
            }
            
            List<Organization> foundOrganization = [organization];
            foundOrganization = await AddRelationToOrganization(foundOrganization);
            
            return HandleReturn<Organization>.Success(foundOrganization[0]);
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching organization.", exception);
        }
    }

    #endregion

    #region POST

    public async Task<HandleReturn<Organization>> AddOrganization(Organization organization) {
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
            
            
            return HandleReturn<Organization>.Success(organization);
        }
        catch (Exception exception) {
            Console.WriteLine(exception);
            throw new Exception("An error occurred while adding organization to database.", exception);
        }
    }

    #endregion
    
    #region PUT

    public async Task<HandleReturn<Organization>> UpdateOrganization(Organization organization) {
        try {
            HandleReturn<Organization> databaseOrganization = await FetchOrganizationById(organization.OrganizationId);
            if (!databaseOrganization.IsSuccess) {
                return HandleReturn<Organization>.Failure("Could not find the organization");
            }
            
            Organization updatedOrganization = databaseOrganization.Value!;
            updatedOrganization.Name = organization.Name;
            updatedOrganization.Description = organization.Description;
            updatedOrganization.ImageId = organization.ImageId;
            updatedOrganization.Image = organization.Image;

            _dbCon.Organization.Update(updatedOrganization);
            await _dbCon.SaveChangesAsync();
            return HandleReturn<Organization>.Success(updatedOrganization);
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while updating organization.", exception);
        }
    }

    #endregion

    #region DELETE

    public async Task<HandleReturn<bool>> DeleteOrganization(Organization organization) {
         try {
            _dbCon.Organization.Remove(organization);
             await _dbCon.SaveChangesAsync();
             return HandleReturn<bool>.Success();
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