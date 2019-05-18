using System;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;

#region Trademark

// This software, all associated documentation, and all copies are CONFIDENTIAL INFORMATION of Kalpawreska Teknologi Indonesia 
// http://www.fwahyudianto.id
// ® Wahyudianto, Fajar
// Email 	: fwahyudi06@gmail.com 

#endregion

namespace Azur.Consol
{
    public static class Program
    {
        #region Main Method

        static void Main(string[] args)
        {
            Console.WriteLine("Microsoft Azure - Blob Storage For .NET");
            Console.WriteLine();

            Execute();
            Console.WriteLine();

            Console.WriteLine("Press any key to exit Azur Application!");
            Console.ReadLine();
        }

        #endregion // end of Main Method


        #region Method

        private static void Execute()
        {
            //  Microsoft Azure Storage Account
            CloudStorageAccount oCloudStorageAccount = null;

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
                    Console.WriteLine("Azure - Storage Account is \"Connected\" !");
                }
                catch (Exception oException)
                {
                    Console.WriteLine("Error returned from the service : {0}.", oException.Message);
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