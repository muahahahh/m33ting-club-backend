using Amazon.S3;
using M33tingClub.Application.Utilities.ImageStorageServiceInterfaces;
using M33tingClub.Infrastructure.ImageStorageServiceImplementations;

namespace M33tingClub.Web.Registrations;

public static class AwsRegistration
{
    public static IServiceCollection AddAWSS3Access(this IServiceCollection services, M33tingClubConfiguration configuration)
    {
        services.AddScoped<IBackgroundImageStorageService, BackgroundImageStorageService>(x =>
            ActivatorUtilities.CreateInstance<BackgroundImageStorageService>(x, x.GetRequiredService<IAmazonS3>(), 
                configuration.S3BackgroundImageBucketName, configuration.S3BackgroundImageFolderName));
        services.AddScoped<IUserImageStorageService, UserImageStorageService>(x =>
            ActivatorUtilities.CreateInstance<UserImageStorageService>(x, x.GetRequiredService<IAmazonS3>(), 
                configuration.S3UserImageBucketName, configuration.S3UserImageFolderName));

        return services;
    }
}