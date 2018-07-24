namespace Receipt_Generator_AddIn
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Globalization;
	using System.IO;
	using System.Reflection;
	using System.Windows;
	using Microsoft.Office.Core;
	using Microsoft.Office.Interop.Outlook;

	/// <summary>
	/// An Outlook Add-In for automatically generating a license file and a receipt for a new purchase.
	/// </summary>
	/// <copyright>Copyright © 2011 - Teraque, Inc.  All Rights Reserved.</copyright>
	public partial class AddIn
    {

		/// <summary>
		/// The name of the source used for event logging.
		/// </summary>
		const String eventSource = "Receipt Generator";
	
		/// <summary>
		/// The 'Inbox' folder.
		/// </summary>
		MAPIFolder inboxFolder;

		/// <summary>
		/// The name of the license file generated from the process.
		/// </summary>
		const String licenseFileName = "license.lic";

		/// <summary>
		/// The header of a purchase report.
		/// </summary>
		const String purchaseReportSubject = "Purchase Report";

		/// <summary>
		/// The sender of a purchase report.
		/// </summary>
		const String purchaseReportSender = "sales@teraque.com";

		/// <summary>
		/// The subject line of the receipt
		/// </summary>
		const String receiptSubject = "Your Explorer Chrome Suite purchase";

		/// <summary>
		/// The address from which the receipt is issued.
		/// </summary>
		const String sendFromAddress = "sales@teraque.com";

		/// <summary>
		/// The URL for the 32 bit download.
		/// </summary>
		const String x86DownloadUrl = "www.teraque.com/wp-content/plugins/download-monitor/download.php?id=6";

		/// <summary>
		/// The URL for the 64 bit download.
		/// </summary>
		const String x64DownloadUrl = "www.teraque.com/wp-content/plugins/download-monitor/download.php?id=7";

		/// <summary>
		/// Used to map the process id back to the purchase report.
		/// </summary>
		Dictionary<Int32, PurchaseReport> purchaseReports;

		/// <summary>
		/// Gets the Outlook Account for the given emal address.
		/// </summary>
		/// <param name="smtpAddress">The email address.</param>
		/// <returns>The Outlook account associated with the given address.</returns>
		public Account GetAccountForEmailAddress(String smtpAddress)
		{

			// Loop over the Accounts collection looking for an address that matches the one desired.
			Accounts accounts = this.Application.Session.Accounts;
			foreach (Account account in accounts)
				if (account.SmtpAddress == smtpAddress)
					return account;

			// Not having the right address for an account is bad and will prevent a message from being sent.
			throw new ArgumentOutOfRangeException(smtpAddress);

		}

		/// <summary>
		/// Called when the AddIn is created.
		/// </summary>
		void InternalStartup()
        {

			// Initialize the object.
			this.purchaseReports = new Dictionary<Int32, PurchaseReport>();

			// Install the event handlers that will take care of starting up and shutting down this add-in.
            this.Startup += this.OnAddInStartup;
            this.Shutdown += this.OnAddInShutdown;

        }

		/// <summary>
		/// Invoked when the AddIn starts up.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="eventArgs">The event arguments</param>
		void OnAddInStartup(Object sender, EventArgs eventArgs)
		{

			// Log the progress of the Add In.
			AddIn.WriteEventLogInformation("Installing Incoming Mail event handler.");

			try
			{

				// Install the event handler that watch for an incoming email.
				this.inboxFolder = this.Application.GetNamespace("MAPI").GetDefaultFolder(OlDefaultFolders.olFolderInbox);
				this.inboxFolder.Items.ItemAdd += this.OnItemAdded;

			}
			catch (System.Exception exception)
			{

				// Make sure any problems on start up are logged.
				AddIn.WriteEventLogError(exception.Message);

			}

		}


		/// <summary>
		/// Invoked when the AddIn is shut down.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="eventArgs">The event arguments</param>
		void OnAddInShutdown(Object sender, EventArgs eventArgs)
		{

			// Log the progress of the Add In.
			AddIn.WriteEventLogInformation("Removing Incoming Mail event handler.");

			try
			{

				// This will uninstall the handler for new items in the 'Inbox' and de-reference the folder so it can be garbage collected.
				this.inboxFolder.Items.ItemAdd -= new ItemsEvents_ItemAddEventHandler(this.OnItemAdded);
				this.inboxFolder = null;

			}
			catch (System.Exception exception)
			{

				// Make sure any problems shutting down are logged.
				AddIn.WriteEventLogError(exception.Message);

			}

		}

		/// <summary>
		/// Handles a new item added to the folder.
		/// </summary>
		/// <param name="item">The item added to the folder.</param>
		void OnItemAdded(Object item)
		{

			// Write about the progress.
			AddIn.WriteEventLogInformation("OnItemAdded called");

			// Extract the specific argument from the generic event argument.
			MailItem mailItem = item as MailItem;

			// Only handle the item if it is a purchase report from sales at Teraque.
			if (mailItem != null && mailItem.Subject == AddIn.purchaseReportSubject && mailItem.SenderEmailAddress == AddIn.purchaseReportSender)
			{

				try
				{

					AddIn.WriteEventLogInformation("Generating purchase report.");

					// This will parse out the fields from the body of the message.
					PurchaseReport purchaseReport = new PurchaseReport(mailItem.Body);

					// A new license file is required for the new purchase.  This will call out to the license generator to create one using all the parameters
					// extracted from the purchase report message.  Note that the process is handled asynchronously and that when the license generator is
					// finished a handler will be called to complete the processing of the receipt.
					Process process = new Process();
					process.EnableRaisingEvents = true;
#if DEBUG
					process.StartInfo.FileName = @"C:\Users\Donald Roy Airey\Documents\Visual Studio 2010\Projects\Teraque\Main\License Generator\License Generator\bin\Debug\Teraque.LicenseGenerator.exe";
#else
					process.StartInfo.FileName = @"C:\Program Files\Teraque\License Generator\Teraque.LicenseGenerator.exe";
#endif
					process.StartInfo.Arguments += String.Format(" -Address \"{0}\"", purchaseReport.Address);
					process.StartInfo.Arguments += String.Format(" -City \"{0}\"", purchaseReport.City);
					process.StartInfo.Arguments += String.Format(" -Command \"{0}\"", "GenerateLicense");
					process.StartInfo.Arguments += String.Format(" -Country \"{0}\"", purchaseReport.Country);
					process.StartInfo.Arguments += String.Format(" -Email \"{0}\"", purchaseReport.Email);
					process.StartInfo.Arguments += String.Format(" -FirstName \"{0}\"", purchaseReport.FirstName);
					process.StartInfo.Arguments += String.Format(" -LastName \"{0}\"", purchaseReport.LastName);
					process.StartInfo.Arguments += String.Format(" -Output \"{0}\"", purchaseReport.FileName);
					process.StartInfo.Arguments += String.Format(" -Phone \"{0}\"", purchaseReport.Phone);
					process.StartInfo.Arguments += String.Format(" -PostalCode \"{0}\"", purchaseReport.PostalCode);
					process.StartInfo.Arguments += String.Format(" -Product \"{0}\"", "{9FD4FF74-CCAC-463E-922E-79613F6A7CD9}");
					process.StartInfo.Arguments += String.Format(" -Province \"{0}\"", purchaseReport.State);
					process.Start();
					this.purchaseReports.Add(process.Id, purchaseReport);
					process.Exited += new EventHandler(this.OnLicenseGeneratorExited);

					AddIn.WriteEventLogInformation("Waiting for License Generation.");

				}
				catch (System.Exception exception)
				{

					// Make sure any errors trying to handle the incoming message are logged.
					AddIn.WriteEventLogError(exception.Message);

				}

			}
			else
			{
				AddIn.WriteEventLogInformation("Got an item but it wasn't a PurchaseReport.");
			}

		}

		/// <summary>
		/// Handles the completion of a license generation.
		/// </summary>
		/// <param name="sender">The object that originated the event.</param>
		/// <param name="eventArgs">The event arguments</param>
		void OnLicenseGeneratorExited(Object sender, EventArgs eventArgs)
		{

			// Extract the process from the generic event arguments.
			Process process = sender as Process;

			// If the license was generated properly then construct a receipt.
			if (process.ExitCode == 0)
			{

				// This is the information associated with the recently created license file.
				PurchaseReport purchaseReport = this.purchaseReports[process.Id];

				// Create the receipt in HTML and fill in the header.
				MailItem receipt = this.Application.CreateItem(OlItemType.olMailItem) as MailItem;
				receipt.BodyFormat = OlBodyFormat.olFormatHTML;
				receipt.Subject = AddIn.receiptSubject;
				receipt.To = purchaseReport.Email;
				receipt.SendUsingAccount = this.GetAccountForEmailAddress(AddIn.sendFromAddress);
				receipt.Attachments.Add(purchaseReport.FileName);

				// This will load the template of the receipt from the assembly resources.
				String messageBody = String.Empty;
				using (Stream stream = typeof(AddIn).Assembly.GetManifestResourceStream("Receipt_Generator_AddIn.Resources.ReceiptTemplate.htm"))
				{
					StreamReader streamReader = new StreamReader(stream);
					messageBody = streamReader.ReadToEnd();
				}

				// This will replace the first name in the salutation (or with "Customer" if they want to remain anonymous).
				messageBody = messageBody.Replace("%FirstName%", purchaseReport.FirstName == String.Empty ? "Customer" : purchaseReport.FirstName);
	
				// This will replace the hyperlink in the message body with the proper URL depending on the CPU type ordered.
				if (purchaseReport.CpuType == "x86")
					messageBody = messageBody.Replace("%CpuDownload%", AddIn.x86DownloadUrl);
				if (purchaseReport.CpuType == "x64")
					messageBody = messageBody.Replace("%CpuDownload%", AddIn.x64DownloadUrl);

				// The body of the receipt is now complete.
				receipt.HTMLBody = messageBody;

				// Everything is now ready and the receipt can be sent.
				((_MailItem)receipt).Send();

				// Log the fact that we sent the receipt.
				AddIn.WriteEventLogInformation("Sending a message to {0}", purchaseReport.Email);

			}
			else
			{

				// Write out the reason the generator failed.
				AddIn.WriteEventLogError("License Generator failed with error code {0}", process.ExitCode);

			}

		}

		/// <summary>
		/// Write an error message to the event log.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="parameters">An object array that contains zero or more objects to format.</param>
		static void WriteEventLogError(String format, params Object[] parameters)
		{

			// Write the error to the event log.
			EventLog.WriteEntry(AddIn.eventSource, String.Format(CultureInfo.InvariantCulture, format, parameters), EventLogEntryType.Error);

		}

		/// <summary>
		/// Write an information message to the event log.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="parameters">An object array that contains zero or more objects to format.</param>
		static void WriteEventLogInformation(String format, params Object[] parameters)
		{

			// Write the message to the event log.
			EventLog.WriteEntry(AddIn.eventSource, String.Format(CultureInfo.InvariantCulture, format, parameters), EventLogEntryType.Information);

		}

		/// <summary>
		/// Information extracted from a purchase report.
		/// </summary>
		struct PurchaseReport
		{

			/// <summary>
			/// Address
			/// </summary>
			public String Address;

			/// <summary>
			/// City
			/// </summary>
			public String City;

			/// <summary>
			/// Country
			/// </summary>
			public String Country;

			/// <summary>
			/// Cpu Type
			/// </summary>
			public String CpuType;

			/// <summary>
			/// Email
			/// </summary>
			public String Email;

			/// <summary>
			/// File Name
			/// </summary>
			public String FileName;

			/// <summary>
			/// First Name
			/// </summary>
			public String FirstName;

			/// <summary>
			/// Last Name
			/// </summary>
			public String LastName;

			/// <summary>
			/// Phone
			/// </summary>
			public String Phone;

			/// <summary>
			/// Postal Code
			/// </summary>
			public String PostalCode;

			/// <summary>
			/// State
			/// </summary>
			public String State;

			/// <summary>
			/// Creates a new instance of the PurchaseReport class from a message body.
			/// </summary>
			/// <param name="message">The body of a message containing the parameters of a purchase report.</param>
			public PurchaseReport(String message)
			{

				// Parse the fields out of the message.
				this.Address = PurchaseReport.GetField("Address: ", message);
				this.City = PurchaseReport.GetField("City: ", message);
				this.Country = PurchaseReport.GetField("Country: ", message);
				this.Email = PurchaseReport.GetField("Email: ", message);
				this.FirstName = PurchaseReport.GetField("First Name: ", message);
				this.LastName = PurchaseReport.GetField("Last Name: ", message);
				this.Phone = PurchaseReport.GetField("Phone: ", message);
				this.PostalCode = PurchaseReport.GetField("Postal Code: ", message);
				this.State = PurchaseReport.GetField("State: ", message);

				// Create a temporary directory where we can place the license file when it is generated.				
				DirectoryInfo directoryInfo = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
				this.FileName = Path.Combine(directoryInfo.FullName, AddIn.licenseFileName);

				// Parse the CPU type out of the product description.
				this.CpuType = String.Empty;
				if (message.IndexOf("Explorer Chrome Suite (x86)") != -1)
					this.CpuType = "x86";
				if (message.IndexOf("Explorer Chrome Suite (x64)") != -1)
					this.CpuType = "x64";

			}

			/// <summary>
			/// Extracts a field from the body of a message.
			/// </summary>
			/// <param name="field">The field tag.</param>
			/// <param name="message">The message body.</param>
			/// <returns>The contents of the field.</returns>
			static String GetField(String field, String message)
			{

				// Extract the field from the message.
				Int32 startIndex = message.IndexOf(field) + field.Length;
				Int32 endIndex = message.IndexOf("\r", startIndex);
				return message.Substring(startIndex, endIndex - startIndex);

			}

		}

	}

}
