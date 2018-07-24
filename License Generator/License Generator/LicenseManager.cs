namespace Teraque.LicenseGenerator
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Windows.Data;
	using System.IO;
	using System.Linq;
	using System.Windows.Input;
	using System.Windows;
	using System.Windows.Controls;
	using System.Security.Cryptography;
	using Teraque;

	class LicenseManager
	{

		static Byte[] privateKeyD = {31, 216, 167, 30, 177, 31, 131, 182, 150, 153, 205, 229, 90, 206, 73, 225, 94, 60, 104, 171, 145, 132, 254, 160, 131, 149, 178,
					   239, 104, 206, 166, 18, 19, 192, 114, 87, 44, 7, 213, 64, 137, 168, 108, 131, 107, 158, 120, 138, 169, 12, 90, 117, 64, 62, 227, 236, 223,
					   193, 16, 245, 142, 75, 168, 75, 5, 2, 99, 203, 231, 198, 134, 98, 183, 68, 73, 241, 82, 248, 237, 0, 159, 114, 237, 16, 9, 110, 119, 169,
					   250, 161, 69, 47, 189, 217, 46, 50, 82, 212, 160, 116, 239, 28, 103, 87, 104, 134, 80, 235, 135, 197, 2, 208, 137, 39, 2, 140, 109, 76, 46,
					   70, 211, 183, 10, 233, 82, 123, 190, 113};
		static Byte[] privateKeyDp = {83, 32, 91, 73, 110, 85, 77, 43, 217, 111, 236, 27, 218, 165, 212, 199, 139, 97, 214, 155, 180, 245, 171, 251, 221, 250, 96,
					   196, 54, 100, 67, 198, 38, 247, 86, 88, 121, 183, 43, 96, 199, 253, 56, 83, 57, 35, 160, 254, 199, 231, 147, 78, 49, 142, 17, 100, 178, 178,
					   113, 168, 39, 70, 120, 57};
		static Byte[] privateKeyDq = {147, 206, 164, 204, 27, 159, 156, 44, 60, 106, 27, 4, 149, 195, 6, 141, 167, 18, 28, 91, 205, 202, 5, 59, 52, 113, 206, 182,
					   105, 23, 248, 194, 2, 231, 212, 229, 232, 49, 244, 74, 159, 166, 48, 37, 176, 140, 13, 108, 137, 168, 79, 202, 75, 140, 53, 131, 38, 200,
					   241, 36, 147, 178, 194, 183};
		static Byte[] publicKeyExponent = { 1, 0, 1 };
		static Byte[] privateKeyInverseQ = {74, 197, 45, 173, 179, 227, 156, 78, 100, 12, 229, 24, 233, 207, 243, 70, 42, 57, 226, 115, 132, 98, 159, 45, 126, 195,
					   21, 147, 160, 214, 226, 99, 79, 37, 165, 157, 22, 223, 90, 82, 169, 6, 190, 228, 129, 88, 10, 204, 49, 112, 222, 114, 117, 173, 91, 126, 31,
					   101, 255, 133, 197, 118, 135, 160};
		static Byte[] publicKeyModulus = {162, 172, 157, 96, 199, 45, 99, 237, 135, 145, 242, 167, 138, 150, 138, 119, 246, 15, 16, 20, 244, 102, 0, 252, 228, 58,
					   182, 121, 120, 91, 236, 15, 174, 0, 174, 36, 235, 191, 96, 138, 136, 148, 65, 133, 247, 250, 4, 105, 139, 95, 165, 93, 80, 187, 205, 162, 34,
					   50, 218, 230, 62, 214, 69, 4, 157, 2, 193, 183, 170, 118, 53, 190, 153, 73, 127, 241, 239, 140, 16, 209, 192, 102, 21, 121, 17, 119, 219,
					   102, 19, 105, 26, 59, 235, 139, 228, 163, 90, 99, 158, 40, 141, 65, 137, 114, 128, 159, 215, 33, 8, 46, 58, 112, 36, 10, 3, 221, 137, 127,
					   250, 12, 211, 13, 174, 136, 248, 113, 141, 255};
		static Byte[] privateKeyP = {205, 192, 14, 162, 202, 252, 77, 71, 240, 85, 193, 29, 82, 105, 18, 23, 220, 249, 158, 103, 25, 132, 193, 157, 112, 158, 180,
					   82, 74, 16, 136, 217, 194, 74, 147, 78, 85, 129, 1, 164, 165, 148, 194, 215, 138, 206, 206, 247, 63, 198, 183, 124, 23, 181, 158, 215, 6,
					   179, 154, 139, 168, 208, 252, 197};
		static Byte[] privateKeyQ = {202, 103, 91, 133, 86, 130, 52, 148, 142, 141, 204, 216, 109, 52, 60, 48, 235, 157, 15, 157, 8, 97, 138, 59, 51, 150, 21, 153,
					   193, 222, 22, 20, 173, 119, 254, 125, 123, 216, 129, 87, 208, 162, 24, 174, 10, 79, 195, 203, 86, 205, 179, 48, 225, 162, 219, 125, 69, 89,
					   211, 126, 77, 3, 19, 243};

		internal static void GenerateLicense(LicenseInfo licenseInfo, String fileName)
		{

			if (licenseInfo == null)
				throw new ArgumentNullException("licenseInfo");

			DataSet.ProductRow productRow = DataModel.Product.FindByProductId(licenseInfo.ProductId);
			DataSet.CustomerRow customerRow = DataModel.Customer.FindByCustomerId(licenseInfo.CustomerId);

			// Generate a new license.
			DataSet.LicenseRow licenseRow = DataModel.License.AddLicenseRow(
				licenseInfo.DateCreated,
				customerRow,
				Guid.NewGuid(),
				licenseInfo.LicenseType,
				productRow);
			DataModel.LicenseTableAdapter.Update(DataModel.License);

			// Generate the payload from the license type, the product id, the serial number and the current date.
			Byte[] licenseType = BitConverter.GetBytes(licenseRow.LicenseType);
			Byte[] productIdArray = licenseRow.ProductId.ToByteArray();
			Byte[] serialNumberArray = BitConverter.GetBytes(licenseRow.SerialNumber);
			Byte[] dateCreatedArray = BitConverter.GetBytes(licenseRow.DateCreated.ToBinary());
			Byte[] payload = new Byte[licenseType.Length + productIdArray.Length + serialNumberArray.Length + dateCreatedArray.Length];
			Array.Copy(licenseType, payload, licenseType.Length);
			Array.Copy(productIdArray, 0, payload, licenseType.Length, productIdArray.Length);
			Array.Copy(serialNumberArray, 0, payload, licenseType.Length + productIdArray.Length, serialNumberArray.Length);
			Array.Copy(dateCreatedArray, 0, payload, licenseType.Length + productIdArray.Length + serialNumberArray.Length, dateCreatedArray.Length);

			RSAParameters rsaParameters = new RSAParameters();
			rsaParameters.D = LicenseManager.privateKeyD;
			rsaParameters.DP = LicenseManager.privateKeyDp;
			rsaParameters.DQ = LicenseManager.privateKeyDq;
			rsaParameters.Exponent = LicenseManager.publicKeyExponent;
			rsaParameters.InverseQ = LicenseManager.privateKeyInverseQ;
			rsaParameters.Modulus = LicenseManager.publicKeyModulus;
			rsaParameters.P = LicenseManager.privateKeyP;
			rsaParameters.Q = LicenseManager.privateKeyQ;
			RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider();
			rsaCryptoServiceProvider.ImportParameters(rsaParameters);

			SHA1CryptoServiceProvider sha1CryptoServiceProvider = new SHA1CryptoServiceProvider();
			Byte[] signature = rsaCryptoServiceProvider.SignData(payload, sha1CryptoServiceProvider);
			Byte[] signedPayload = new Byte[payload.Length + signature.Length];
			Array.Copy(payload, signedPayload, payload.Length);
			Array.Copy(signature, 0, signedPayload, payload.Length, signature.Length);

			Byte[] publicKey = new Byte[LicenseManager.publicKeyExponent.Length + LicenseManager.publicKeyModulus.Length];
			Array.Copy(LicenseManager.publicKeyExponent, publicKey, LicenseManager.publicKeyExponent.Length);
			Array.Copy(LicenseManager.publicKeyModulus, 0, publicKey, LicenseManager.publicKeyExponent.Length, LicenseManager.publicKeyModulus.Length);

			Byte[] licenseKey = new Byte[publicKey.Length + signedPayload.Length];
			Array.Copy(publicKey, licenseKey, publicKey.Length);
			Array.Copy(signedPayload, 0, licenseKey, publicKey.Length, signedPayload.Length);

			using (StreamWriter streamWriter = new StreamWriter(fileName))
				streamWriter.WriteLine(Convert.ToBase64String(licenseKey));

		}

	}

}
