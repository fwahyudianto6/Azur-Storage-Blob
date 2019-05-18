using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

#region Trademark

// This software, all associated documentation, and all copies are CONFIDENTIAL INFORMATION of Kalpawreska Teknologi Indonesia 
// http://www.fwahyudianto.id
// ® Wahyudianto, Fajar
// Email 	: fwahyudi06@gmail.com 

#endregion

namespace Azur.Consol
{
    public class Program
    {
        #region Main Method

        static void Main(string[] args)
        {
            Console.WriteLine("Microsoft Azure - Blob Storage For .NET");
            Console.WriteLine();

            ExecuteAsync().GetAwaiter().GetResult();
            Console.WriteLine();

            Console.WriteLine("Press any key to exit Azur Application!");
            Console.ReadLine();
        }

        #endregion // end of Main Method

        #region Method

        /// <summary>
        /// Execute Azur Application
        /// </summary>
        private static async Task ExecuteAsync()
        {
            //  Microsoft Azure Storage Account
            CloudStorageAccount oCloudStorageAccount = null;
            string strSourceFile = string.Empty;

            /* 
             * Retrieve the connection string for use with the application. 
             * The storage connection string is stored in an environment variable on the machine running the application called "storageconnectionstring".
             */
            string strStorageConnectionString = Environment.GetEnvironmentVariable("storageconnectionstring");

            //  Check whether the connection string can be parsed.
            if (CloudStorageAccount.TryParse(strStorageConnectionString, out oCloudStorageAccount))
            {
                try
                {
                    #region Create Container

                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    CloudBlobClient oCloudBlobClient = oCloudStorageAccount.CreateCloudBlobClient();

                    // Create a container called 'kti_' and append a GUID value to it to make the name unique.
                    CloudBlobContainer oCloudBlobContainer = oCloudBlobClient.GetContainerReference("kti");

                    if (!oCloudBlobContainer.Exists())
                    {
                        Console.WriteLine("Creating container .....");
                        await oCloudBlobContainer.CreateIfNotExistsAsync();
                        Console.WriteLine("Created container '{0}'", oCloudBlobContainer.Name);
                        Console.WriteLine();

                        // Set the permissions so the blobs are public.
                        BlobContainerPermissions oBlobContainerPermissions = new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        };
                        await oCloudBlobContainer.SetPermissionsAsync(oBlobContainerPermissions);
                    }

                    #endregion // end of Create Container 

                    #region Upload blobs to the Container

                    // Create a file in your local folder by parameterized to upload to a blob.
                    string strPath = ConfigurationManager.AppSettings.Get("UploadPath").Trim();
                    string strFileName = "QuickStart_" + Guid.NewGuid().ToString() + ".txt";
                    strSourceFile = Path.Combine(strPath, strFileName);

                    // Write text to the file.
                    File.WriteAllText(strSourceFile,
                        string.Format("Demonstrate upload blob - {0}", DateTime.Now.ToString("dd MMM yyyy HH:mm:ss:fffffff")));

                    Console.WriteLine("Temp file = {0}", strSourceFile);
                    Console.WriteLine("Uploading to Blob storage as blob \"{0}\"", strFileName);
                    Console.WriteLine();

                    // Get a reference to the blob address, then upload the file to the blob.
                    // Use the value of strFileName for the blob name.
                    CloudBlockBlob oCloudBlockBlob = oCloudBlobContainer.GetBlockBlobReference(strFileName);
                    await oCloudBlockBlob.UploadFromFileAsync(strSourceFile);

                    #endregion // end of Upload blobs to the Container 

                    #region Blob List

                    Console.WriteLine("Blobs List");
                    Console.WriteLine("--------------------");
                    BlobContinuationToken oBlobContinuationToken = null;

                    do
                    {
                        var resultSegment = await oCloudBlobContainer.ListBlobsSegmentedAsync(null, oBlobContinuationToken);
                        // Get the value of the continuation token returned by the listing call.
                        oBlobContinuationToken = resultSegment.ContinuationToken;
                        foreach (IListBlobItem item in resultSegment.Results)
                        {
                            Console.WriteLine(item.Uri);
                        }
                    } while (oBlobContinuationToken != null); // Loop while the continuation token is not null.
                    Console.WriteLine();

                    #endregion // end of Blob List
                }
                catch (Exception oException)
                {
                    Console.WriteLine("Error : {0}.", oException.Message);
                }
            }
            else
            {
                Console.WriteLine(
                    "A connection string has not been defined in the system environment variables. " +
                    "Add a environment variable named \"storageconnectionstring\" with your storage " +
                    "connection string as a value.");
            }
        }

        #endregion // end of Method         
    }
}



#region Notes

/// <summary>
/// Microsoft Azure - Blob Storage For .NET 
/// Demonstrate how to upload, list, download, and delete blobs.
/// 
///     <source>
///     - https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet?tabs=windows
///     </source>
/// </summary>

#endregion // end of Notes   