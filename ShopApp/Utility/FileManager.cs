using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ShopApp.Utility
{
	public class FileManager
	{
		//static readonly string[] imageValidExtensions = new string[] { "jpg", "png", "jpeg, blob" };
		static readonly string[] documentValidFileExtensions = new string[] { "pdf", "doc", "docx" };

		public static async Task<string> UploadAvatar(HttpPostedFileBase file, int userID)
		{
			string imageFullPath = null;

			if (file == null || file.ContentLength == 0)
			{
				return null;
			}

			try
			{
				string fileName = string.Empty;
				string fileExtension = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1);

				//if (!imageValidExtensions.Contains(fileExtension.ToLower()))
				//	throw new Exception("The file extension is invalid!");


				CloudStorageAccount cloudStorageAccount = ConnectionString.GetConnectionString();
				CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
				CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("avatars");


				StringBuilder fileNameBuilder = new StringBuilder("Avatar");
				fileNameBuilder.Append('_');
				fileNameBuilder.Append(userID);
				fileNameBuilder.Append('.');
				fileNameBuilder.Append(fileExtension);

				fileName = fileNameBuilder.ToString();


				if (await cloudBlobContainer.CreateIfNotExistsAsync())
				{
					await cloudBlobContainer.SetPermissionsAsync(
						new BlobContainerPermissions
						{
							PublicAccess = BlobContainerPublicAccessType.Container
						}
						);
				}

				CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
				cloudBlockBlob.Properties.ContentType = file.ContentType;

				await cloudBlockBlob.UploadFromStreamAsync(file.InputStream);

				imageFullPath = cloudBlockBlob.Uri.ToString();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
			return imageFullPath;
		}

		public static async Task<string> UploadOfferImage(HttpPostedFileBase file, int offerID, int pictureNumber)
		{
			string imageFullPath = null;

			if (file == null || file.ContentLength == 0)
			{
				return null;
			}


			try
			{
				string fileName = string.Empty;
				string fileExtenstion = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1);

				//if (!imageValidExtensions.Contains(fileExtenstion.ToLower()))
				//	throw new Exception("The file extension is invalid!");


				CloudStorageAccount cloudStorageAccount = ConnectionString.GetConnectionString();
				CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
				CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("offerpictures");


				StringBuilder fileNameBuilder = new StringBuilder("OfferPicture");
				fileNameBuilder.Append('_');
				fileNameBuilder.Append(offerID);
				fileNameBuilder.Append("_PictureNo_");
				fileNameBuilder.Append(pictureNumber);
				fileNameBuilder.Append('.');
				fileNameBuilder.Append(fileExtenstion);

				fileName = fileNameBuilder.ToString();


				if (await cloudBlobContainer.CreateIfNotExistsAsync())
				{
					await cloudBlobContainer.SetPermissionsAsync(
						new BlobContainerPermissions
						{
							PublicAccess = BlobContainerPublicAccessType.Container
						}
					 );
				}

				CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
				cloudBlockBlob.Properties.ContentType = file.ContentType;


				await cloudBlockBlob.UploadFromStreamAsync(file.InputStream);


				imageFullPath = cloudBlockBlob.Uri.ToString();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				return null;
			}
			return imageFullPath;
		}

		public static async Task<string> UploadBundlePicture(HttpPostedFileBase file, int bundleID)
		{
			string imageFullPath = null;

			if (file == null || file.ContentLength == 0)
			{
				return null;
			}

			try
			{
				string fileName = string.Empty;
				string fileExtenstion = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1);

				//if (!imageValidExtensions.Contains(fileExtenstion.ToLower()))
				//	throw new Exception("The file extension is invalid!");


				CloudStorageAccount cloudStorageAccount = ConnectionString.GetConnectionString();
				CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
				CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("bundlepictures");


				StringBuilder fileNameBuilder = new StringBuilder("BundlePicture");
				fileNameBuilder.Append('_');
				fileNameBuilder.Append(bundleID);
				fileNameBuilder.Append('.');
				fileNameBuilder.Append(fileExtenstion);

				fileName = fileNameBuilder.ToString();


				if (await cloudBlobContainer.CreateIfNotExistsAsync())
				{
					await cloudBlobContainer.SetPermissionsAsync(
						new BlobContainerPermissions
						{
							PublicAccess = BlobContainerPublicAccessType.Container
						}
					 );
				}

				CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
				cloudBlockBlob.Properties.ContentType = file.ContentType;

				await cloudBlockBlob.UploadFromStreamAsync(file.InputStream);

				imageFullPath = cloudBlockBlob.Uri.ToString();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
			return imageFullPath;
		}

		public static async Task<string> UploadDocument(HttpPostedFileBase file, int userID, int documentID)
		{
			string imageFullPath = null;

			if (file == null || file.ContentLength == 0)
			{
				return null;
			}

			try
			{
				string fileName = string.Empty;
				string fileExtenstion = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1);

				if (!documentValidFileExtensions.Contains(fileExtenstion))
					throw new Exception("The file extension is invalid!");


				CloudStorageAccount cloudStorageAccount = ConnectionString.GetConnectionString();
				CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
				CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("documents");

				// DO POPRAWY BY KAŻDY DOKUMENT MIAŁ WŁASNE ID
				StringBuilder fileNameBuilder = new StringBuilder("Document");
				fileNameBuilder.Append('_');
				fileNameBuilder.Append(userID);
				fileNameBuilder.Append('_');
				fileNameBuilder.Append(documentID);
				fileNameBuilder.Append('.');
				fileNameBuilder.Append(fileExtenstion);

				fileName = fileNameBuilder.ToString();


				if (await cloudBlobContainer.CreateIfNotExistsAsync())
				{
					await cloudBlobContainer.SetPermissionsAsync(
						new BlobContainerPermissions
						{
							PublicAccess = BlobContainerPublicAccessType.Container
						}
						);
				}

				CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
				cloudBlockBlob.Properties.ContentType = file.ContentType;

				await cloudBlockBlob.UploadFromStreamAsync(file.InputStream);

				imageFullPath = cloudBlockBlob.Uri.ToString();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
			return imageFullPath;
		}

	}
}