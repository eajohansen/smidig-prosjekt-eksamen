using agile_dev.Models;
using agile_dev.Repo;
using Microsoft.EntityFrameworkCore;

namespace agile_dev.Service;

public class ImageService {
    private readonly InitContext _dbCon;
    private readonly OrganizationService _organizationService;

    public ImageService(InitContext context) {
        _dbCon = context;
    }

    #region GET

    public async Task<ICollection<Image>> FetchAllImages() {
        try {
            ICollection<Image> foundImages = await _dbCon.Image.ToListAsync();
            ICollection<Image> newImages = await AddRelationToImage(foundImages.ToList());

            return newImages;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching images.", exception);
        }
    }

    public async Task<Image?> FetchImageById(int id) {
        try {
            Image? image = await _dbCon.Image.FindAsync(id);
            if (image != null) {
                List<Image> foundImage = [image];
                foundImage = await AddRelationToImage(foundImage);
                return foundImage[0];
            } else {
                return image;
            }
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while fetching image.", exception);
        }
    }

    #endregion
    
    #region POST

    public async Task<bool> AddImage(int userId, int organizationId, Image image) {
        try {
            if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return false;
            }

            await _dbCon.Image.AddAsync(image);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while adding image to database.", exception);
        }
    }

    #endregion
    
    #region PUT

    public async Task<bool> UpdateImage(int userId, int organizationId, Image image) {
        try {
            if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return false;
            }

            Image? databaseImage = await FetchImageById(image.ImageId);
            if (databaseImage == null) {
                return false;
            }
            
            _dbCon.Image.Update(image);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while updating image.", exception);
        }
    }

    #endregion
    
    #region DELETE

    public async Task<bool> DeleteImage(int userId, int organizationId, Image image) {
        try {
            if (!_organizationService.CheckValidation(userId, organizationId).Result) {
                return false;
            }
            
            _dbCon.Image.Remove(image);
            await _dbCon.SaveChangesAsync();
            return true;
        }
        catch (Exception exception) {
            throw new Exception("An error occurred while trying to delete image.", exception);
        }
    }

    #endregion
    
    #region MISCELLANEOUS

    private async Task<List<Image>> AddRelationToImage(List<Image> images) {
        List<int> imageIds = images.Select(image => image.ImageId).ToList();

        List<Image> newImages = await _dbCon.Image
            .Where(image => imageIds.Contains(image.ImageId))
            .Include(image => image.Events)
            .Include(image => image.Organizations)
            .ToListAsync();

        return newImages;
    }

    #endregion
}