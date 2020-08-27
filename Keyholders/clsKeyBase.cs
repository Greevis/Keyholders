using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Data.Odbc;
using Resources;
using System.IO;
//using BCL.easyPDF6.Interop.EasyPDFPrinter;
//using BCL.easyPDF6.Interop.EasyPDFLoader;
//using BCL.easyPDF6.Interop.EasyPDFProcessor;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Diagnostics;

namespace Keyholders
{
	/// <summary>
	/// clsKeyBase is the Base Class for all Keyholder System Classes. 
	/// Inherits from <see cref="clsGeneralBase">clsGeneralBase"</see>. 
	/// </summary>
	
	[GuidAttribute("F0F7B364-DC15-48de-8320-8569AE2FADC1")]
	public class clsKeyBase : clsGeneralBase
	{

		#region Initialisation

		/// <summary>
		/// Constructor for clsKeyBase
		/// </summary>
		public clsKeyBase() : base()
		{
			thisClientTimeZoneOsIndex = 290;
			thisClientTimeZoneRegKey = "New Zealand Standard Time";
		}

		/// <summary>
		/// Overloaded Constructor for clsKeyBase
		/// </summary>
		/// <param name="Name">Name of Table for Class</param>
		public clsKeyBase(string Name) : base(Name, true)
		{
			thisClientTimeZoneOsIndex = 290;
			thisClientTimeZoneRegKey = "New Zealand Standard Time";
		}

		#endregion

		#region Enumerations and Methods from Obtaining them
		
		#region addressPart

		/// <summary>
		/// This enumeration is used for members that are Addresses 
		/// </summary>
		public enum addressPart : int
		{
			/// <summary>
			/// Id of this Address.
			/// Obtainable through <see cref="addressPart_id">
			/// addressPart_id</see>  
			/// </summary>
			id = 0,			
			/// <summary>
			/// The Whole Address with carriage returns between constituent parts.
			/// Obtainable through <see cref="addressPart_fullAddress">
			/// addressPart_fullAddress</see>  
			/// </summary>
			fullAddress = 1,
			/// <summary>
			/// The Whole Address with all carriage returns taken out, and just 
			/// commas and spaces between constituent Parts.
			/// Obtainable through <see cref="addressPart_fullAddressNoReturns">
			/// addressPart_fullAddressNoReturns</see>
			/// </summary>
			fullAddressNoReturns = 2,
			/// <summary>
			/// The Street Address e.g. Either 1/20 Egmont St, Te Aro or PO Box 6594, Te Aro.
			/// Obtainable through <see cref="addressPart_streetAddress">
			/// addressPart_streetAddress</see>  
			/// </summary>
			streetAddress = 3,
			/// <summary>
			/// The City, e.g. Wellington.
			/// Obtainable through <see cref="addressPart_city">
			/// addressPart_city</see>
			/// </summary>
			city = 4,
			/// <summary>
			/// The Post Code, e.g. Wellington.
			/// Obtainable through <see cref="addressPart_postCode">
			/// addressPart_postCode</see>
			/// </summary>
			postCode = 5,
			/// <summary>
			/// The State or County, e.g. California or Suffolk
			/// Obtainable through <see cref="addressPart_country">
			/// addressPart_state</see>
			/// </summary>
			state = 6,
			/// <summary>
			/// The Country, e.g. New Zealand.
			/// Obtainable through <see cref="addressPart_country">
			/// addressPart_country</see>
			/// </summary>
			country = 7,
			/// <summary>
			/// Type of Address; refers to 
			/// <see cref="addressType">addressType</see>.
			/// Obtainable through 
			/// <see cref="addressPart_type">addressPart_type</see> 
			/// </summary>
			type = 8,
			/// <summary>
			/// Description of the Address. This is only appropriate for
			/// Addresss of
			/// <see cref="addressPart_type">addressPart_type</see> 
			/// <see cref="addressType_other">addressType_other</see> 
			/// Obtainable through <see cref="addressPart_description">
			/// addressPart_description</see> 
			/// </summary>
			description = 9
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="addressPart">addressPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="addressPart.id">id</see> 
		/// </returns>
		public int addressPart_id()
		{
			return Convert.ToInt32(addressPart.id);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="addressPart">addressPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="addressPart.fullAddress">fullAddress</see> 
		/// </returns>
		public int addressPart_fullAddress()
		{
			return Convert.ToInt32(addressPart.fullAddress);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="addressPart">addressPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="addressPart.fullAddressNoReturns">fullAddressNoReturns</see> 
		/// </returns>
		public int addressPart_fullAddressNoReturns()
		{
			return Convert.ToInt32(addressPart.fullAddressNoReturns);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="addressPart">addressPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="addressPart.streetAddress">streetAddress</see> 
		/// </returns>
		public int addressPart_streetAddress()
		{
			return Convert.ToInt32(addressPart.streetAddress);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="addressPart">addressPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="addressPart.city">city</see> 
		/// </returns>
		public int addressPart_city()
		{
			return Convert.ToInt32(addressPart.city);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="addressPart">addressPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="addressPart.postCode">postCode</see> 
		/// </returns>
		public int addressPart_postCode()
		{
			return Convert.ToInt32(addressPart.postCode);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="addressPart">addressPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="addressPart.state">state</see> 
		/// </returns>
		public int addressPart_state()
		{
			return Convert.ToInt32(addressPart.state);
		}


		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="addressPart">addressPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="addressPart.country">country</see> 
		/// </returns>
		public int addressPart_country()
		{
			return Convert.ToInt32(addressPart.country);
		}


		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="addressPart">addressPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="addressPart.type">type</see> 
		/// </returns>
		public int addressPart_type()
		{
			return Convert.ToInt32(addressPart.type);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="addressPart">addressPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="addressPart.description">description</see> 
		/// </returns>
		public int addressPart_description()
		{
			return Convert.ToInt32(addressPart.description);
		}
		#endregion

		#region addressType

		/// <summary>
		/// This enumeration is used for members that are Addresses 
		/// </summary>
		public enum addressType : int
		{
			/// <summary>
			/// Physical Address.
			/// Obtainable through <see cref="addressType_physical">
			/// addressType_physical</see>  
			/// </summary>
			physical = 0,			
			/// <summary>
			/// Postal Address.
			/// Obtainable through <see cref="addressType_postal">
			/// addressType_postal</see>  
			/// </summary>
			postal = 1,
			/// <summary>
			/// An Address that doesn't fit with the other descriptions.
			/// See User added description for this
			/// Obtainable through <see cref="addressType_other">
			/// addressType_other</see>
			/// </summary>
			other = 2
		}		

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="addressType">addressType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="addressType.physical">physical</see> 
		/// </returns>
		public int addressType_physical()
		{
			return Convert.ToInt32(addressType.physical);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="addressType">addressType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="addressType.postal">postal</see> 
		/// </returns>
		public int addressType_postal()
		{
			return Convert.ToInt32(addressType.postal);
		}


		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="addressType">addressType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="addressType.other">other</see> 
		/// </returns>
		public int addressType_other()
		{
			return Convert.ToInt32(addressType.other);
		}

		#endregion

		#region correspondenceFieldType

		/// <summary>
		/// This enumeration is used for email CorrespondenceFieldId types
		/// </summary>
		public enum correspondenceFieldType : int
		{
			/// <summary>
			/// To CorrespondenceFieldId
			/// Obtainable through <see cref="correspondenceFieldType_to">
			/// correspondenceFieldType_to</see> 
			/// </summary>
			to = 1,
			/// <summary>
			/// From CorrespondenceFieldId
			/// Obtainable through <see cref="correspondenceFieldType_from">
			/// correspondenceFieldType_from</see> 
			/// </summary>
			from = 2,
			/// <summary>
			/// CC CorrespondenceFieldId
			/// Obtainable through <see cref="correspondenceFieldType_cc">
			/// correspondenceFieldType_cc</see> 
			/// </summary>
			cc = 3,
			/// <summary>
			/// BCC CorrespondenceFieldId
			/// Obtainable through <see cref="correspondenceFieldType_bcc">
			/// correspondenceFieldType_bcc</see> 
			/// </summary>
			bcc = 4
		}
		
		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceFieldType">correspondenceFieldType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="correspondenceFieldType.to">to</see> 
		/// </returns>
		public int correspondenceFieldType_to()
		{
			return Convert.ToInt32(correspondenceFieldType.to);
		}			


		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceFieldType">correspondenceFieldType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="correspondenceFieldType.from">from</see> 
		/// </returns>
		public int correspondenceFieldType_from()
		{
			return Convert.ToInt32(correspondenceFieldType.from);
		}		

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceFieldType">correspondenceFieldType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="correspondenceFieldType.cc">cc</see> 
		/// </returns>
		public int correspondenceFieldType_cc()
		{
			return Convert.ToInt32(correspondenceFieldType.cc);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceFieldType">correspondenceFieldType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="correspondenceFieldType.bcc">bcc</see> 
		/// </returns>
		public int correspondenceFieldType_bcc()
		{
			return Convert.ToInt32(correspondenceFieldType.bcc);
		}

		#endregion

		#region correspondenceSendToCustomerStatus


		/// <summary>
		/// This enumeration is Used for credit card identification
		/// </summary>
		public enum correspondenceSendToCustomerStatus : int
		{
			/// <summary>
			/// Correspondence Required
			/// Obtainable through <see cref="correspondenceSendToCustomerStatus_required">
			/// correspondenceSendToCustomerStatus_required</see> 
			/// </summary>
			required = 1,
			/// <summary>
			/// Correspondence Generated
			/// Obtainable through <see cref="correspondenceSendToCustomerStatus_generated">
			/// correspondenceSendToCustomerStatus_generated</see> 
			/// </summary>
			generated = 2,
			/// <summary>
			/// Correspondence Sent
			/// Obtainable through <see cref="correspondenceSendToCustomerStatus_sent">
			/// correspondenceSendToCustomerStatus_sent</see> 
			/// </summary>
			sent = 3
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceSendToCustomerStatus">correspondenceSendToCustomerStatus</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="correspondenceSendToCustomerStatus.required">required</see> 
		/// </returns>
		public int correspondenceSendToCustomerStatus_required()
		{
			return Convert.ToInt32(correspondenceSendToCustomerStatus.required);
		}			


		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceSendToCustomerStatus">correspondenceSendToCustomerStatus</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="correspondenceSendToCustomerStatus.generated">generated</see> 
		/// </returns>
		public int correspondenceSendToCustomerStatus_generated()
		{
			return Convert.ToInt32(correspondenceSendToCustomerStatus.generated);
		}		

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceSendToCustomerStatus">correspondenceSendToCustomerStatus</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="correspondenceSendToCustomerStatus.sent">sent</see> 
		/// </returns>
		public int correspondenceSendToCustomerStatus_sent()
		{
			return Convert.ToInt32(correspondenceSendToCustomerStatus.sent);
		}

		#endregion

		#region correspondenceType


		/// <summary>
		/// This enumeration is Used for credit card identification
		/// </summary>
		public enum correspondenceType : int
		{
			/// <summary>
			/// Cover Page for other Correspondence
			/// Obtainable through <see cref="correspondenceType_coverPage">
			/// correspondenceType_coverPage</see> 
			/// </summary>
			coverPage = 1,
			/// <summary>
			/// Welcome Message
			/// Obtainable through <see cref="correspondenceType_welcome">
			/// correspondenceType_welcome</see> 
			/// </summary>
			welcome = 2,
			/// <summary>
			/// Invoice
			/// Obtainable through <see cref="correspondenceType_invoice">
			/// correspondenceType_invoice</see> 
			/// </summary>
			invoice = 3,
			/// <summary>
			/// Statement
			/// Obtainable through <see cref="correspondenceType_statement">
			/// correspondenceType_statement</see> 
			/// </summary>
			statement = 4,
			/// <summary>
			/// Details Update
			/// Obtainable through <see cref="correspondenceType_detailsUpdate">
			/// correspondenceType_detailsUpdate</see> 
			/// </summary>
			detailsUpdate = 5,
			/// <summary>
			/// Sticker Required
			/// Obtainable through <see cref="correspondenceType_welcome">
			/// correspondenceType_welcome</see> 
			/// </summary>
			stickerRequired = 6,
			/// <summary>
			/// Lost Password
			/// Obtainable through <see cref="correspondenceType_lostPassword">
			/// correspondenceType_lostPassword</see> 
			/// </summary>
			lostPassword = 7,
			/// <summary>
			/// Bulk email type
			/// Obtainable through <see cref="correspondenceType_bulk">
			/// correspondenceType_bulk</see> 
			/// </summary>
			bulk = 8,
			/// <summary>
			/// copyOfInvoice
			/// Obtainable through <see cref="correspondenceType_copyOfInvoice">
			/// correspondenceType_copyOfInvoice</see> 
			/// </summary>
			copyOfInvoice = 9
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceType">correspondenceType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="correspondenceType.lostPassword">lostPassword</see> 
		/// </returns>
		public int correspondenceType_lostPassword()
		{
			return Convert.ToInt32(correspondenceType.lostPassword);
		}			


		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceType">correspondenceType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="correspondenceType.detailsUpdate">detailsUpdate</see> 
		/// </returns>
		public int correspondenceType_detailsUpdate()
		{
			return Convert.ToInt32(correspondenceType.detailsUpdate);
		}		

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceType">correspondenceType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="correspondenceType.invoice">invoice</see> 
		/// </returns>
		public int correspondenceType_invoice()
		{
			return Convert.ToInt32(correspondenceType.invoice);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceType">correspondenceType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="correspondenceType.copyOfInvoice">copyOfInvoice</see> 
		/// </returns>
		public int correspondenceType_copyOfInvoice()
		{
			return Convert.ToInt32(correspondenceType.copyOfInvoice);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceType">correspondenceType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="correspondenceType.bulk">bulk</see> 
		/// </returns>
		public int correspondenceType_bulk()
		{
			return Convert.ToInt32(correspondenceType.bulk);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceType">correspondenceType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="correspondenceType.statement">statement</see> 
		/// </returns>
		public int correspondenceType_statement()
		{
			return Convert.ToInt32(correspondenceType.statement);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceType">correspondenceType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="correspondenceType.stickerRequired">stickerRequired</see> 
		/// </returns>
		public int correspondenceType_stickerRequired()
		{
			return Convert.ToInt32(correspondenceType.stickerRequired);
		}		

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceType">correspondenceType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="correspondenceType.welcome">welcome</see> 
		/// </returns>
		public int correspondenceType_welcome()
		{
			return Convert.ToInt32(correspondenceType.welcome);
		}		

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceType">correspondenceType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="correspondenceType.coverPage">coverPage</see> 
		/// </returns>
		public int correspondenceType_coverPage()
		{
			return Convert.ToInt32(correspondenceType.coverPage);
		}	

		#endregion

		#region creditCardType	

		/// <summary>
		/// This enumeration is Used for credit card identification
		/// </summary>
		public enum creditCardType : int
		{

			/// <summary>
			/// MasterCard.
			/// Obtainable through <see cref="creditCardType_mastercard">
			/// creditCardType_mastercard</see> 
			/// </summary>
			mastercard = 1,
			/// <summary>
			/// Visa.
			/// Obtainable through <see cref="creditCardType_visa">
			/// creditCardType_visa</see> 
			/// </summary>
			visa = 2,
			/// <summary>
			/// JCB
			/// Obtainable through <see cref="creditCardType_jcb">
			/// creditCardType_jcb</see> 
			/// </summary>
			jcb = 3,
			/// <summary>
			/// Switch/Maestro
			/// Obtainable through <see cref="creditCardType_switchMaestro">
			/// creditCardType_switchMaestro</see> 
			/// </summary>
			switchMaestro = 4,
			/// <summary>
			/// American Express Card
			/// Obtainable through <see cref="creditCardType_amex">
			/// creditCardType_amex</see> 
			/// </summary>
			amex = 5,
			/// <summary>
			/// Diners Card
			/// Obtainable through <see cref="creditCardType_diners">
			/// creditCardType_diners</see> 
			/// </summary>
			diners = 6,
			/// <summary>
			/// Discover Card
			/// Obtainable through <see cref="creditCardType_discover">
			/// creditCardType_diners</see> 
			/// </summary>
			discover = 7,
			/// <summary>
			/// Solo Card
			/// Obtainable through <see cref="creditCardType_solo">
			/// creditCardType_solo</see> 
			/// </summary>
			solo = 8,
			/// <summary>
			/// Visa Electron Card
			/// Obtainable through <see cref="creditCardType_visaelectron">
			/// creditCardType_visaelectron</see> 
			/// </summary>
			visaelectron = 9
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="creditCardType">creditCardType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="creditCardType.mastercard">mastercard</see> 
		/// </returns>
		public int creditCardType_mastercard()
		{
			return Convert.ToInt32(creditCardType.mastercard);
		}		

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="creditCardType">creditCardType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="creditCardType.visa">visa</see> 
		/// </returns>
		public int creditCardType_visa()
		{
			return Convert.ToInt32(creditCardType.visa);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="creditCardType">creditCardType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="creditCardType.jcb">jcb</see> 
		/// </returns>
		public int creditCardType_jcb()
		{
			return Convert.ToInt32(creditCardType.jcb);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="creditCardType">creditCardType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="creditCardType.switchMaestro">switchMaestro</see> 
		/// </returns>
		public int creditCardType_switchMaestro()
		{
			return Convert.ToInt32(creditCardType.switchMaestro);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="creditCardType">creditCardType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="creditCardType.amex">amex</see> 
		/// </returns>
		public int creditCardType_amex()
		{
			return Convert.ToInt32(creditCardType.amex);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="creditCardType">creditCardType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="creditCardType.diners">diners</see> 
		/// </returns>
		public int creditCardType_diners()
		{
			return Convert.ToInt32(creditCardType.diners);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="creditCardType">creditCardType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="creditCardType.discover">discover</see> 
		/// </returns>
		public int creditCardType_discover()
		{
			return Convert.ToInt32(creditCardType.discover);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="creditCardType">creditCardType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="creditCardType.solo">solo</see> 
		/// </returns>
		public int creditCardType_solo()
		{
			return Convert.ToInt32(creditCardType.solo);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="creditCardType">creditCardType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="creditCardType.visaelectron">visaelectron</see> 
		/// </returns>
		public int creditCardType_visaelectron()
		{
			return Convert.ToInt32(creditCardType.visaelectron);
		}


		#endregion

		#region customerType

		/// <summary>
		/// Enumeration that determines the type of customer; business or residential
		/// </summary>
		public enum customerType : int
		{
			/// <summary>
			/// Business Customer
			/// </summary>
			business = 1,
			/// <summary>
			/// Residential Customer
			/// </summary>
			residential = 2
		}
		
		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="customerType">customerType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="customerType.business">business</see> 
		/// </returns>
		public int customerType_business()
		{
			return Convert.ToInt32(customerType.business);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="customerType">customerType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="customerType.residential">residential</see> 
		/// </returns>
		public int customerType_residential()
		{
			return Convert.ToInt32(customerType.residential);
		}
		#endregion

		#region displayType

		/// <summary>
		/// This enumeration is used for determing how products are displayed and/or allowed to be purchased
		/// to use when searching
		/// </summary>
		public enum displayType : int
		{
			/// <summary>
			/// Do Not Advertise this Product
			/// Obtainable through <see cref="displayType_doNotAdvertise">
			/// displayType_doNotAdvertise</see>
			/// </summary>
			doNotAdvertise = 1,
			/// <summary>
			/// Advertise the Product But Disallow Purchase
			/// Obtainable through <see cref="displayType_advertiseButDisallowPurchase">
			/// displayType_advertiseButDisallowPurchase</see>
			/// </summary>
			advertiseButDisallowPurchase = 2,
			/// <summary>
			/// Advertise the Product And Allow Purchase
			/// Obtainable through <see cref="displayType_advertiseAllowPurchase">
			/// displayType_advertiseAllowPurchase</see>
			/// </summary>
			advertiseAllowPurchase = 3
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="displayType">displayType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="displayType.doNotAdvertise">doNotAdvertise</see> 
		/// </returns>
		public int displayType_doNotAdvertise()
		{
			return Convert.ToInt32(displayType.doNotAdvertise);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="displayType">displayType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="displayType.advertiseButDisallowPurchase">advertiseButDisallowPurchase</see> 
		/// </returns>
		public int displayType_advertiseButDisallowPurchase()
		{
			return Convert.ToInt32(displayType.advertiseButDisallowPurchase);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="displayType">displayType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="displayType.advertiseAllowPurchase">advertiseAllowPurchase</see> 
		/// </returns>
		public int displayType_advertiseAllowPurchase()
		{
			return Convert.ToInt32(displayType.advertiseAllowPurchase);
		}
		#endregion

		#region durationUnitType

		/// <summary>
		/// This enumeration is Used for members that are Phone number 
		/// </summary>
		public enum durationUnitType : int
		{
			/// <summary>
			/// Duration Unit Type of second.
			/// Obtainable through <see cref="durationUnitType_second">
			/// durationUnitType_second</see> 
			/// </summary>
			second = 0,
			/// <summary>
			/// Duration Unit Type of minute.
			/// Obtainable through <see cref="durationUnitType_minute">
			/// durationUnitType_minute</see> 
			/// </summary>
			minute = 1,
			/// <summary>
			/// Duration Unit Type of hour.
			/// Obtainable through <see cref="durationUnitType_hour">
			/// durationUnitType_hour</see> 
			/// </summary>
			hour = 2,
			/// <summary>
			/// Duration Unit Type of day.
			/// Obtainable through <see cref="durationUnitType_day">
			/// durationUnitType_day</see> 
			/// </summary>
			day = 3,
			/// <summary>
			/// Duration Unit Type of week.
			/// 21 for Vodafone Mobile.
			/// Obtainable through <see cref="durationUnitType_week">
			/// durationUnitType_week</see> 
			/// </summary>
			week = 4,
			/// <summary>
			/// Duration Unit Type of month.
			/// Obtainable through <see cref="durationUnitType_month">
			/// durationUnitType_month</see> 
			/// </summary>
			month = 5,
			/// <summary>
			/// Duration Unit Type of year.
			/// Obtainable through <see cref="durationUnitType_year">
			/// durationUnitType_year</see> 
			/// </summary>
			year = 6
		}

		/// <summary>
		/// Return Name of <see cref="durationUnitType">durationUnitType</see>
		/// </summary>
		/// <param name="durationUnitTypeId"><see cref="durationUnitType">durationUnitType</see>
		/// </param>
		/// <returns>Name of <see cref="durationUnitType">durationUnitType</see></returns>
		public string durationUnitTypeName(int durationUnitTypeId)
		{
			string retVal = "";
			
			switch  ((durationUnitType) durationUnitTypeId)
			{
				case durationUnitType.second:
					retVal = "Second";
					break;
				case durationUnitType.minute:
					retVal = "Minute";
					break;
				case durationUnitType.hour:
					retVal = "Hour";
					break;
				case durationUnitType.day:
					retVal = "Day";
					break;
				case durationUnitType.week:
					retVal = "Week";
					break;
				case durationUnitType.month:
					retVal = "Month";
					break;
				case durationUnitType.year:
					retVal = "Year";
					break;
				default:
					break;
			}
			return retVal;
		}


		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="durationUnitType">durationUnitType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="durationUnitType.second">second</see> 
		/// </returns>
		public int durationUnitType_second()
		{
			return Convert.ToInt32(durationUnitType.second);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="durationUnitType">durationUnitType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="durationUnitType.minute">minute</see> 
		/// </returns>
		public int durationUnitType_minute()
		{
			return Convert.ToInt32(durationUnitType.minute);
		}		

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="durationUnitType">durationUnitType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="durationUnitType.hour">hour</see> 
		/// </returns>
		public int durationUnitType_hour()
		{
			return Convert.ToInt32(durationUnitType.hour);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="durationUnitType">durationUnitType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="durationUnitType.day">day</see> 
		/// </returns>
		public int durationUnitType_day()
		{
			return Convert.ToInt32(durationUnitType.day);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="durationUnitType">durationUnitType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="durationUnitType.week">week</see> 
		/// </returns>
		public int durationUnitType_week()
		{
			return Convert.ToInt32(durationUnitType.week);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="durationUnitType">durationUnitType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="durationUnitType.month">month</see> 
		/// </returns>
		public int durationUnitType_month()
		{
			return Convert.ToInt32(durationUnitType.month);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="durationUnitType">durationUnitType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="durationUnitType.year">year</see> 
		/// </returns>
		public int durationUnitType_year()
		{
			return Convert.ToInt32(durationUnitType.year);
		}

		#endregion
		
		#region fieldsToMatch

		/// <summary>
		/// Enumeration that determines which fields to check
		/// </summary>
		public enum fieldsToMatch : int
		{
			/// <summary>
			/// Any of FirstName, LastName or CompanyName
			/// Obtainable through <see cref="fieldsToMatch_allNames">
			/// fieldsToMatch_allNames</see>
			/// </summary>
			allNames = 0,
			/// <summary>
			/// Just FirstName
			/// Obtainable through <see cref="fieldsToMatch_firstNameOnly">
			/// fieldsToMatch_firstNameOnly</see>
			/// </summary>
			firstNameOnly = 1,
			/// <summary>
			/// Just LastName
			/// Obtainable through <see cref="fieldsToMatch_lastNameOnly">
			/// fieldsToMatch_lastNameOnly</see>
			/// </summary>
			lastNameOnly = 2,		
			/// <summary>
			/// Just CompanyName
			/// Obtainable through <see cref="fieldsToMatch_companyNameOnly">
			/// fieldsToMatch_companyNameOnly</see>
			/// </summary>
			companyNameOnly = 3,
			/// <summary>
			/// FirstName and LastName only
			/// Obtainable through <see cref="fieldsToMatch_firstAndLastNamesOnly">
			/// fieldsToMatch_firstAndLastNamesOnly</see>
			/// </summary>
			firstAndLastNamesOnly = 4
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="fieldsToMatch">fieldsToMatch</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="fieldsToMatch.allNames">allNames</see> 
		/// </returns>
		public int fieldsToMatch_allNames()
		{
			return Convert.ToInt32(fieldsToMatch.allNames);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="fieldsToMatch">fieldsToMatch</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="fieldsToMatch.companyNameOnly">companyNameOnly</see> 
		/// </returns>
		public int fieldsToMatch_companyNameOnly()
		{
			return Convert.ToInt32(fieldsToMatch.companyNameOnly);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="fieldsToMatch">fieldsToMatch</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="fieldsToMatch.firstAndLastNamesOnly">firstAndLastNamesOnly</see> 
		/// </returns>
		public int fieldsToMatch_firstAndLastNamesOnly()
		{
			return Convert.ToInt32(fieldsToMatch.firstAndLastNamesOnly);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="fieldsToMatch">fieldsToMatch</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="fieldsToMatch.firstNameOnly">firstNameOnly</see> 
		/// </returns>
		public int fieldsToMatch_firstNameOnly()
		{
			return Convert.ToInt32(fieldsToMatch.firstNameOnly);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="fieldsToMatch">fieldsToMatch</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="fieldsToMatch.lastNameOnly">lastNameOnly</see> 
		/// </returns>
		public int fieldsToMatch_lastNameOnly()
		{
			return Convert.ToInt32(fieldsToMatch.lastNameOnly);
		}

		#endregion

		#region freightChargeType
		
		/// <summary>
		/// This enumeration is used for determing how freight is charged
		/// to use when searching
		/// </summary>
		public enum freightChargeType : int
		{
			/// <summary>
			/// A Singal Freight Charge Per Item in Order
			/// Obtainable through <see cref="freightChargeType_singleChargePerItem">
			/// freightChargeType_singleChargePerItem</see>
			/// </summary>
			singleChargePerItem = 1,
			/// <summary>
			/// A Singal Freight Charge Per Order
			/// Obtainable through <see cref="freightChargeType_singleChargePerValueRange">
			/// freightChargeType_singleChargePerValueRange</see>
			/// </summary>
			singleChargePerValueRange = 2,
			/// <summary>
			/// A Singal Freight Charge for a Total Weight Range of an Order
			/// Obtainable through <see cref="freightChargeType_singleChargePerWeightRange">
			/// freightChargeType_singleChargePerWeightRange</see>
			/// </summary>
			singleChargePerWeightRange = 3
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="freightChargeType">freightChargeType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="freightChargeType.singleChargePerValueRange">singleChargePerValueRange</see> 
		/// </returns>
		public int freightChargeType_singleChargePerValueRange()
		{
			return Convert.ToInt32(freightChargeType.singleChargePerValueRange);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="freightChargeType">freightChargeType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="freightChargeType.singleChargePerItem">singleChargePerItem</see> 
		/// </returns>
		public int freightChargeType_singleChargePerItem()
		{
			return Convert.ToInt32(freightChargeType.singleChargePerItem);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="freightChargeType">freightChargeType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="freightChargeType.singleChargePerWeightRange">singleChargePerWeightRange</see> 
		/// </returns>
		public int freightChargeType_singleChargePerWeightRange()
		{
			return Convert.ToInt32(freightChargeType.singleChargePerWeightRange);
		}

		#endregion

		#region orderCreatedMechanism
		
		/// <summary>
		/// This enumeration is used for determing how freight is charged
		/// to use when searching
		/// </summary>
		public enum orderCreatedMechanism : int
		{
			/// <summary>
			/// Customer Creates a Cart from the Web Front End
			/// Obtainable through <see cref="orderCreatedMechanism_byCustomerCreatingCart">
			/// orderCreatedMechanism_byCustomerCreatingCart</see>
			/// </summary>
			byCustomerCreatingCart = 1,
			/// <summary>
			/// Vendor creates the order automatically from the back end
			/// Obtainable through <see cref="orderCreatedMechanism_byVendorAutomatically">
			/// orderCreatedMechanism_byVendorAutomatically</see>
			/// </summary>
			byVendorAutomatically = 2,
			/// <summary>
			/// Some other mechanism
			/// Obtainable through <see cref="orderCreatedMechanism_other">
			/// orderCreatedMechanism_other</see>
			/// </summary>
			other = 3
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="orderCreatedMechanism">orderCreatedMechanism</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="orderCreatedMechanism.byVendorAutomatically">byVendorAutomatically</see> 
		/// </returns>
		public int orderCreatedMechanism_byVendorAutomatically()
		{
			return Convert.ToInt32(orderCreatedMechanism.byVendorAutomatically);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="orderCreatedMechanism">orderCreatedMechanism</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="orderCreatedMechanism.byCustomerCreatingCart">byCustomerCreatingCart</see> 
		/// </returns>
		public int orderCreatedMechanism_byCustomerCreatingCart()
		{
			return Convert.ToInt32(orderCreatedMechanism.byCustomerCreatingCart);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="orderCreatedMechanism">orderCreatedMechanism</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="orderCreatedMechanism.other">other</see> 
		/// </returns>
		public int orderCreatedMechanism_other()
		{
			return Convert.ToInt32(orderCreatedMechanism.other);
		}

		#endregion

		#region paymentMethodType
        
        /// <summary>
		/// This enumeration is Used for members that are payment types
		/// </summary>
		public enum paymentMethodType : int
		{
			/// <summary>
			/// As yet undetermined
			/// Obtainable through <see cref="paymentMethodType_asYetUndetermined">
			/// paymentMethodType_asYetUndetermined</see> 
			/// </summary>
			asYetUndetermined = 1,
			/// <summary>
			/// Transaction is a Reversal of a previous transaction
			/// Obtainable through <see cref="paymentMethodType_reversal">
			/// paymentMethodType_reversal</see> 
			/// </summary>
			reversal = 2,
			/// <summary>
			/// vendorCredit.
			/// Obtainable through <see cref="paymentMethodType_vendorCredit">
			/// paymentMethodType_vendorCredit</see> 
			/// </summary>
			vendorCredit = 3,
			/// <summary>
			/// vendorCredit.
			/// Obtainable through <see cref="paymentMethodType_vendorDebit">
			/// paymentMethodType_vendorDebit</see> 
			/// </summary>
			vendorDebit = 4,
			/// <summary>
			/// orderDebit
			/// Obtainable through <see cref="paymentMethodType_orderDebit">
			/// paymentMethodType_orderDebit</see> 
			/// </summary>
			orderDebit = 5,
			/// <summary>
			/// Credit Card, processed automatically by the Ecommerce Engine
			/// Obtainable through <see cref="paymentMethodType_creditCardAuto">
			/// paymentMethodType_creditCardAuto</see> 
			/// </summary>
			creditCardAuto = 6,
			/// <summary>
			/// Credit Card, processed manually by the vendor sometime after purchase
			/// Obtainable through <see cref="paymentMethodType_creditCardManual">
			/// paymentMethodType_creditCardManual</see> 
			/// </summary>
			creditCardManual = 7,
			/// <summary>
			/// Customer Adds order to account with vendor
			/// Obtainable through <see cref="paymentMethodType_chargeAccount">
			/// paymentMethodType_chargeAccount</see> 
			/// </summary>
			chargeAccount = 8,
			/// <summary>
			/// Customer will pay by cash for this order 
			/// Obtainable through <see cref="paymentMethodType_cash">
			/// paymentMethodType_chargeAccount</see> 
			/// </summary>
			cash = 9,
			/// <summary>
			/// Customer will pay by cheque for this order 
			/// Obtainable through <see cref="paymentMethodType_cheque">
			/// paymentMethodType_cheque</see> 
			/// </summary>
			cheque = 10,
			/// <summary>
			/// Customer will pay by Bank Desposit for this order 
			/// Obtainable through <see cref="paymentMethodType_bankDeposit">
			/// paymentMethodType_bankDeposit</see> 
			/// </summary>
			bankDeposit = 11,
			/// <summary>
			/// other Card
			/// Obtainable through <see cref="paymentMethodType_other">
			/// paymentMethodType_other</see> 
			/// </summary>
			other = 12,
			/// <summary>
			/// directDebit
			/// Obtainable through <see cref="paymentMethodType_directDebit">
			/// paymentMethodType_directDebitr</see> 
			/// </summary>
			directDebit = 13
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="paymentMethodType">paymentMethodType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="paymentMethodType.directDebit">directDebit</see> 
		/// </returns>
		public int paymentMethodType_directDebit()
		{
			return Convert.ToInt32(paymentMethodType.directDebit);
		}		

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="paymentMethodType">paymentMethodType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="paymentMethodType.asYetUndetermined">asYetUndetermined</see> 
		/// </returns>
		public int paymentMethodType_asYetUndetermined()
		{
			return Convert.ToInt32(paymentMethodType.asYetUndetermined);
		}		

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="paymentMethodType">paymentMethodType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="paymentMethodType.reversal">reversal</see> 
		/// </returns>
		public int paymentMethodType_reversal()
		{
			return Convert.ToInt32(paymentMethodType.reversal);
		}		

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="paymentMethodType">paymentMethodType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="paymentMethodType.vendorCredit">vendorCredit</see> 
		/// </returns>
		public int paymentMethodType_vendorCredit()
		{
			return Convert.ToInt32(paymentMethodType.vendorCredit);
		}		

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="paymentMethodType">paymentMethodType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="paymentMethodType.vendorDebit">vendorDebit</see> 
		/// </returns>
		public int paymentMethodType_vendorDebit()
		{
			return Convert.ToInt32(paymentMethodType.vendorDebit);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="paymentMethodType">paymentMethodType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="paymentMethodType.orderDebit">orderDebit</see> 
		/// </returns>
		public int paymentMethodType_orderDebit()
		{
			return Convert.ToInt32(paymentMethodType.orderDebit);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="paymentMethodType">paymentMethodType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="paymentMethodType.creditCardAuto">creditCardAuto</see> 
		/// </returns>
		public int paymentMethodType_creditCardAuto()
		{
			return Convert.ToInt32(paymentMethodType.creditCardAuto);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="paymentMethodType">paymentMethodType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="paymentMethodType.creditCardManual">creditCardManual</see> 
		/// </returns>
		public int paymentMethodType_creditCardManual()
		{
			return Convert.ToInt32(paymentMethodType.creditCardManual);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="paymentMethodType">paymentMethodType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="paymentMethodType.chargeAccount">chargeAccount</see> 
		/// </returns>
		public int paymentMethodType_chargeAccount()
		{
			return Convert.ToInt32(paymentMethodType.chargeAccount);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="paymentMethodType">paymentMethodType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="paymentMethodType.cash">cash</see> 
		/// </returns>
		public int paymentMethodType_cash()
		{
			return Convert.ToInt32(paymentMethodType.cash);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="paymentMethodType">paymentMethodType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="paymentMethodType.cheque">cheque</see> 
		/// </returns>
		public int paymentMethodType_cheque()
		{
			return Convert.ToInt32(paymentMethodType.cheque);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="paymentMethodType">paymentMethodType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="paymentMethodType.bankDeposit">bankDeposit</see> 
		/// </returns>
		public int paymentMethodType_bankDeposit()
		{
			return Convert.ToInt32(paymentMethodType.bankDeposit);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="paymentMethodType">paymentMethodType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="paymentMethodType.other">other</see> 
		/// </returns>
		public int paymentMethodType_other()
		{
			return Convert.ToInt32(paymentMethodType.other);
		}

		#endregion

		#region personPremiseRoleType
		
		#region type name

		/// <summary>
		/// Returns the name of the Person Premise Role Type
		/// </summary>
		/// <param name="roleType">Type of Role, from the enumeration
		/// <see cref="personPremiseRoleType">personPremiseRoleType</see> 
		/// </param>
		/// <returns>Name of the Person Premise Role Type</returns>
		public string personPremiseRoleTypeName(personPremiseRoleType roleType)
		{
			string result = "";
			switch (roleType)
			{
				case personPremiseRoleType.billingContact :
					result = "Billing Contact";
					break;
				case personPremiseRoleType.daytimeContact:
					result = "Daytime Contact";
					break;
				case personPremiseRoleType.detailsManager :
					result = "Details Manager";
					break;
				case personPremiseRoleType.keyHolder:
					result = "Keyholder";
					break;
				default:
					break;
			}
			return result;
		}


		/// <summary>
		/// Returns the name of the Person Premise Role Type
		/// </summary>
		/// <param name="roleType">Type of Role, from the enumeration
		/// <see cref="personPremiseRoleType">personPremiseRoleType</see> 
		/// </param>
		/// <returns>Name of the Person Premise Role Type</returns>
		public string personPremiseRoleTypeName(int roleType)
		{
			return personPremiseRoleTypeName(
				(personPremiseRoleType) roleType);
		}

		#endregion


		/// <summary>
		/// Enumeration that supplies the type of Role performed by a Person for a Premise
		/// </summary>
		public enum personPremiseRoleType : int
		{
			/// <summary>
			/// This Person is the Daytime Contact for this Premise
			/// </summary>
			daytimeContact = 1,
			/// <summary>
			/// This Person is the person who is Billed Person for this Premise
			/// </summary>
			billingContact = 2,
			/// <summary>
			/// This Person manages the details of this Premise
			/// </summary>
			detailsManager = 3,
			/// <summary>
			/// This Person is a keyholder to this Premise
			/// </summary>
			keyHolder = 4
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="personPremiseRoleType">personPremiseRoleType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="personPremiseRoleType.daytimeContact">daytimeContact</see> 
		/// </returns>
		public int personPremiseRoleType_daytimeContact()
		{
			return Convert.ToInt32(personPremiseRoleType.daytimeContact);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="personPremiseRoleType">personPremiseRoleType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="personPremiseRoleType.billingContact">billingContact</see> 
		/// </returns>
		public int personPremiseRoleType_billingContact()
		{
			return Convert.ToInt32(personPremiseRoleType.billingContact);
		}


		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="personPremiseRoleType">personPremiseRoleType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="personPremiseRoleType.detailsManager">detailsManager</see> 
		/// </returns>
		public int personPremiseRoleType_detailsManager()
		{
			return Convert.ToInt32(personPremiseRoleType.detailsManager);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="personPremiseRoleType">personPremiseRoleType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="personPremiseRoleType.keyHolder">keyHolder</see> 
		/// </returns>
		public int personPremiseRoleType_keyHolder()
		{
			return Convert.ToInt32(personPremiseRoleType.keyHolder);
		}

		#endregion

		#region phoneNumberPart

		/// <summary>
		/// This enumeration is Used for members that are Phone number 
		/// </summary>
		public enum phoneNumberPart : int
		{
			/// <summary>
			/// Id of this Phone Number.
			/// Obtainable through <see cref="phoneNumberPart_id">
			/// phoneNumberPart_id</see> 
			/// </summary>
			id = 0,
			/// <summary>
			/// Whole number, with spaces between constituent parts and in main number.
			/// Obtainable through <see cref="phoneNumberPart_fullNumber">
			/// phoneNumberPart_fullNumber</see> 
			/// </summary>
			fullNumber = 1,
			/// <summary>
			/// Whole number, without any spaces (primarily used for searching on).
			/// Obtainable through <see cref="phoneNumberPart_fullNumberNoSpaces">
			/// phoneNumberPart_fullNumberNoSpaces</see> 
			/// </summary>
			fullNumberNoSpaces = 2,
			/// <summary>
			/// International Prefix e.g. + 61 for Australia only.
			/// Obtainable through <see cref="phoneNumberPart_internationalPrefix">
			/// phoneNumberPart_internationalPrefix</see> 
			/// </summary>
			internationalPrefix = 3,
			/// <summary>
			/// The national or mobilePhone prefix, e.g. 4 for Wellington or 
			/// 21 for Vodafone Mobile.
			/// Obtainable through <see cref="phoneNumberPart_nationalOrMobilePrefix">
			/// phoneNumberPart_nationalOrMobilePrefix</see> 
			/// </summary>
			nationalOrMobilePrefix = 4,
			/// <summary>
			/// Main Body of the number, e.g. 669 638.
			/// Obtainable through <see cref="phoneNumberPart_mainNumber">
			/// phoneNumberPart_mainNumber</see> 
			/// </summary>
			mainNumber = 5,
			/// <summary>
			/// Extension if the main phone connects to a switchboard.
			/// Obtainable through <see cref="phoneNumberPart_extension">
			/// phoneNumberPart_extension</see> 
			/// </summary>
			extension = 6,
			/// <summary>
			/// Type of Phone Number; refers to 
			/// <see cref="phoneNumberType">phoneNumberType</see>.
			/// Obtainable through 
			/// <see cref="phoneNumberPart_type">phoneNumberPart_type</see> 
			/// </summary>
			type = 7,
			/// <summary>
			/// Description of the Phone Number. This is only appropriate for
			/// Phone Numbers of
			/// <see cref="phoneNumberPart_type">phoneNumberPart_type</see> 
			/// <see cref="phoneNumberType_other">phoneNumberType_other</see> 
			/// Obtainable through <see cref="phoneNumberPart_description">
			/// phoneNumberPart_description</see> 
			/// </summary>
			description = 8
		}


		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="phoneNumberPart">phoneNumberPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="phoneNumberPart.id">id</see> 
		/// </returns>
		public int phoneNumberPart_id()
		{
			return Convert.ToInt32(phoneNumberPart.id);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="phoneNumberPart">phoneNumberPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="phoneNumberPart.fullNumber">fullNumber</see> 
		/// </returns>
		public int phoneNumberPart_fullNumber()
		{
			return Convert.ToInt32(phoneNumberPart.fullNumber);
		}		

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="phoneNumberPart">phoneNumberPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="phoneNumberPart.fullNumberNoSpaces">fullNumberNoSpaces</see> 
		/// </returns>
		public int phoneNumberPart_fullNumberNoSpaces()
		{
			return Convert.ToInt32(phoneNumberPart.fullNumberNoSpaces);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="phoneNumberPart">phoneNumberPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="phoneNumberPart.internationalPrefix">internationalPrefix</see> 
		/// </returns>
		public int phoneNumberPart_internationalPrefix()
		{
			return Convert.ToInt32(phoneNumberPart.internationalPrefix);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="phoneNumberPart">phoneNumberPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="phoneNumberPart.nationalOrMobilePrefix">nationalOrMobilePrefix</see> 
		/// </returns>
		public int phoneNumberPart_nationalOrMobilePrefix()
		{
			return Convert.ToInt32(phoneNumberPart.nationalOrMobilePrefix);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="phoneNumberPart">phoneNumberPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="phoneNumberPart.mainNumber">mainNumber</see> 
		/// </returns>
		public int phoneNumberPart_mainNumber()
		{
			return Convert.ToInt32(phoneNumberPart.mainNumber);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="phoneNumberPart">phoneNumberPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="phoneNumberPart.extension">extension</see> 
		/// </returns>
		public int phoneNumberPart_extension()
		{
			return Convert.ToInt32(phoneNumberPart.extension);
		}


		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="phoneNumberPart">phoneNumberPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="phoneNumberPart.type">type</see> 
		/// </returns>
		public int phoneNumberPart_type()
		{
			return Convert.ToInt32(phoneNumberPart.type);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="phoneNumberPart">phoneNumberPart</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="phoneNumberPart.description">description</see> 
		/// </returns>
		public int phoneNumberPart_description()
		{
			return Convert.ToInt32(phoneNumberPart.description);
		}

		#endregion

		#region phoneNumberType

		/// <summary>
		/// This enumeration is Used for members that are Phone number 
		/// </summary>
		public enum phoneNumberType : int
		{
			/// <summary>
			/// This is a Daytime Phone Number.
			/// Obtainable through <see cref="phoneNumberType_daytimePhone">
			/// phoneNumberType_daytimePhone</see> 
			/// </summary>
			daytimePhone = 1,
			/// <summary>
			/// This is an After Hours Phone Number.
			/// Obtainable through <see cref="phoneNumberType_afterHoursPhone">
			/// phoneNumberType_afterHoursPhone</see> 
			/// </summary>
			afterHoursPhone = 2,
			/// <summary>
			/// This is a Mobile Phone Number.
			/// Obtainable through <see cref="phoneNumberType_mobilePhone">
			/// phoneNumberType_mobilePhone</see> 
			/// </summary>
			mobilePhone = 3,
			/// <summary>
			/// This is a Daytime Fax Number.
			/// Obtainable through <see cref="phoneNumberType_daytimeFax">
			/// phoneNumberType_daytimeFax</see> 
			/// </summary>
			daytimeFax = 4,
			/// <summary>
			/// This is an After Hours Fax Number.
			/// Obtainable through <see cref="phoneNumberType_afterHoursFax">
			/// phoneNumberType_afterHoursFax</see> 
			/// </summary>
			afterHoursFax = 5,
			/// <summary>
			/// This a phone number that doesn't fit with the other descriptions.
			/// See User added description for this
			/// Obtainable through <see cref="phoneNumberType_other">
			/// phoneNumberType_other</see> 
			/// </summary>
			other = 6
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="phoneNumberType">phoneNumberType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="phoneNumberType.daytimePhone">daytimePhone</see> 
		/// </returns>
		public int phoneNumberType_daytimePhone()
		{
			return Convert.ToInt32(phoneNumberType.daytimePhone);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="phoneNumberType">phoneNumberType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="phoneNumberType.daytimeFax">daytimeFax</see> 
		/// </returns>
		public int phoneNumberType_daytimeFax()
		{
			return Convert.ToInt32(phoneNumberType.daytimeFax);
		}		
		

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="phoneNumberType">phoneNumberType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="phoneNumberType.afterHoursPhone">afterHoursPhone</see> 
		/// </returns>
		public int phoneNumberType_afterHoursPhone()
		{
			return Convert.ToInt32(phoneNumberType.afterHoursPhone);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="phoneNumberType">phoneNumberType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="phoneNumberType.afterHoursFax">afterHoursFax</see> 
		/// </returns>
		public int phoneNumberType_afterHoursFax()
		{
			return Convert.ToInt32(phoneNumberType.afterHoursFax);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="phoneNumberType">phoneNumberType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="phoneNumberType.mobilePhone">mobilePhone</see> 
		/// </returns>
		public int phoneNumberType_mobilePhone()
		{
			return Convert.ToInt32(phoneNumberType.mobilePhone);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="phoneNumberType">phoneNumberType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="phoneNumberType.other">other</see> 
		/// </returns>
		public int phoneNumberType_other()
		{
			return Convert.ToInt32(phoneNumberType.other);
		}

		#endregion

		#region poBoxType

		#region GetTypeName

		/// <summary>
		/// Returns poBoxTypeName 
		/// </summary>
		/// <param name="Status">Status</param>
		/// <returns>poBoxTypeName</returns>
		public string GetpoBoxTypeName(int Status)
		{
			string Name = "";
			switch ((poBoxType) Status)
			{
				case poBoxType.poBox:
					Name = "PO Box";
					break;
				case poBoxType.privateBag:
					Name = "Private Bag";
					break;
				case poBoxType.unknown:
					Name = "Unknown";
					break;
				case poBoxType.normal:
				default:
					Name = "Normal";
					break;
			}
			return Name;
		}



		#endregion

		/// <summary>
		/// This enumeration is used for poBoxType
		/// </summary>
		public enum poBoxType : int
		{
			/// <summary>
			/// Unknown 
			/// </summary>
			unknown = -1,
			/// <summary>
			/// Normal 
			/// </summary>
			normal = 0,
			/// <summary>
			/// PO Box
			/// </summary>
			poBox = 1,
			/// <summary>
			/// Private Bag 
			/// </summary>
			privateBag = 2

		}


		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="poBoxType">poBoxType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="poBoxType.poBox">poBox</see> 
		/// </returns>
		public int poBoxType_poBox()
		{
			return Convert.ToInt32(poBoxType.poBox);
		}				
	
		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="poBoxType">poBoxType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="poBoxType.privateBag">privateBag</see> 
		/// </returns>
		public int poBoxType_privateBag()
		{
			return Convert.ToInt32(poBoxType.privateBag);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="poBoxType">poBoxType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="poBoxType.normal">normal</see> 
		/// </returns>
		public int poBoxType_normal()
		{
			return Convert.ToInt32(poBoxType.normal);
		}	
		
		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="poBoxType">poBoxType</see> 
		/// </summary>
		/// <returns>value of 
		/// <see cref="poBoxType.unknown">unknown</see> 
		/// </returns>
		public int poBoxType_unknown()
		{
			return Convert.ToInt32(poBoxType.unknown);
		}	

		#endregion

		#region correspondenceMedium


		/// <summary>
		/// Enumeration that determines a person's preferred contact method; mail or fax
		/// </summary>
		public enum correspondenceMedium : int
		{
			/// <summary>
			/// Customer prefers to be contacted by mail 
			/// </summary>
			mail = 1,
			/// <summary>
			/// Customer prefers to be contacted by email
			/// </summary>
			email = 2,
			/// <summary>
			/// Customer prefers to be contacted by fax
			/// </summary>
			fax = 3
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceMedium">correspondenceMedium</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="correspondenceMedium.mail">mail</see> 
		/// </returns>
		public int correspondenceMedium_mail()
		{
			return Convert.ToInt32(correspondenceMedium.mail);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceMedium">correspondenceMedium</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="correspondenceMedium.email">email</see> 
		/// </returns>
		public int correspondenceMedium_email()
		{
			return Convert.ToInt32(correspondenceMedium.email);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="correspondenceMedium">correspondenceMedium</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="correspondenceMedium.fax">fax</see> 
		/// </returns>
		public int correspondenceMedium_fax()
		{
			return Convert.ToInt32(correspondenceMedium.fax);
		}

		#endregion
	
		#region productNumberType

		/// <summary>
		/// Enumeration that determines a products purchase type; 
		/// is it contiuous i.e. you could order 3.2KG of Mince, 
		/// discrete as in 3 bottles of Water, 
		/// or single instance per Item only e.g. 1 annual subscription
		/// </summary>
		public enum productNumberType : int
		{
			/// <summary>
			/// Customer prefers to be contacted by mail 
			/// </summary>
			continuous = 0,
			/// <summary>
			/// Customer prefers to be contacted by email
			/// </summary>
			discrete = 1,
			/// <summary>
			/// Customer prefers to be contacted by email
			/// </summary>
			oneOnly = 2
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="productNumberType">productNumberType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="productNumberType.continuous">continuous</see> 
		/// </returns>
		public int productNumberType_continuous()
		{
			return Convert.ToInt32(productNumberType.continuous);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="productNumberType">productNumberType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="productNumberType.discrete">discrete</see> 
		/// </returns>
		public int productNumberType_discrete()
		{
			return Convert.ToInt32(productNumberType.discrete);
		}
		

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="productNumberType">productNumberType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="productNumberType.oneOnly">oneOnly</see> 
		/// </returns>
		public int productNumberType_oneOnly()
		{
			return Convert.ToInt32(productNumberType.oneOnly);
		}
		
	
		#endregion

		#region recordType

		/// <summary>
		/// Enumeration that supplies the type of Role performed by a Person 
		/// for a Premise
		/// </summary>
		public enum recordType : int
		{
			/// <summary>
			/// This Service Provider monitors the alarm at this Premise
			/// </summary>
			premise = 1,
			/// <summary>
			/// This Service Provider responds when the alarm goes at this Premise
			/// </summary>
			person = 2,
			/// <summary>
			/// This Service Provider customers this Premise
			/// </summary>
			customer = 3,
			/// <summary>
			/// This Service Provider provides serviceProvider services at this Premise
			/// </summary>
			serviceProvider = 4
		}

		#region Type Name

		/// <summary>
		/// Returns recordTypeName from recordType
		/// </summary>
		/// <param name="thisRecordType">thisRecordType</param>
		/// <returns>recordTypeName</returns>
		public string getrecordTypeName(int thisRecordType)
		{
			string Name = "";
			switch ((recordType) thisRecordType)
			{
				case recordType.premise:
					Name = "Premise";
					break;
				case recordType.person:
					Name = "Person";
					break;
				case recordType.customer:
					Name = "Customer";
					break;
				case recordType.serviceProvider:
				default:
					Name = "Service Provider";
					break;
			}
			return Name;
		}


		#endregion		


	


		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="recordType">recordType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="recordType.premise">premise</see> 
		/// </returns>
		public int recordType_premise()
		{
			return Convert.ToInt32(recordType.premise);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="recordType">recordType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="recordType.person">person</see> 
		/// </returns>
		public int recordType_person()
		{
			return Convert.ToInt32(recordType.person);
		}


		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="recordType">recordType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="recordType.customer">customer</see> 
		/// </returns>
		public int recordType_customer()
		{
			return Convert.ToInt32(recordType.customer);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="recordType">recordType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="recordType.serviceProvider">serviceProvider</see> 
		/// </returns>
		public int recordType_serviceProvider()
		{
			return Convert.ToInt32(recordType.serviceProvider);
		}


		#endregion

		#region sPPremiseRoleType

		#region Type Name

		/// <summary>
		/// Returns SPPremiseRoleTypeName from SPPremiseRoleType
		/// </summary>
		/// <param name="SPPremiseRoleType">SPPremiseRoleType</param>
		/// <returns>SPPremiseRoleTypeName</returns>
		public string getSPPremiseRoleTypeName(int SPPremiseRoleType)
		{
			string Name = "";
			switch ((sPPremiseRoleType) SPPremiseRoleType)
			{
				case sPPremiseRoleType.alarmMonitor:
					Name = "Alarm Monitor";
					break;
				case sPPremiseRoleType.alarmResponse:
					Name = "Alarm Response";
					break;
				case sPPremiseRoleType.patrol:
					Name = "Patrol";
					break;
				case sPPremiseRoleType.other:
				default:
					Name = "Other";
					break;
			}
			return Name;
		}


		#endregion		


		/// <summary>
		/// Enumeration that supplies the type of Role performed by a Person 
		/// for a Premise
		/// </summary>
		public enum sPPremiseRoleType : int
		{
			/// <summary>
			/// This Service Provider monitors the alarm at this Premise
			/// </summary>
			alarmMonitor = 1,
			/// <summary>
			/// This Service Provider responds when the alarm goes at this Premise
			/// </summary>
			alarmResponse = 2,
			/// <summary>
			/// This Service Provider patrols this Premise
			/// </summary>
			patrol = 3,
			/// <summary>
			/// This Service Provider provides other services at this Premise
			/// </summary>
			other = 4
		}

		/// <summary>
		/// Return Name of <see cref="sPPremiseRoleType">sPPremiseRoleType</see>
		/// </summary>
		/// <param name="sPPremiseRoleTypeId"><see cref="sPPremiseRoleType">sPPremiseRoleType</see>
		/// </param>
		/// <returns>Name of <see cref="sPPremiseRoleType">sPPremiseRoleType</see></returns>
		public string sPPremiseRoleTypeName(int sPPremiseRoleTypeId)
		{
			string retVal = "";
			
			switch  ((sPPremiseRoleType) sPPremiseRoleTypeId)
			{
				case sPPremiseRoleType.alarmMonitor:
					retVal = "Alarm Monitoring";
					break;
				case sPPremiseRoleType.alarmResponse:
					retVal = "Alarm Response";
					break;
				case sPPremiseRoleType.patrol:
					retVal = "Patrol";
					break;
				case sPPremiseRoleType.other:
					retVal = "Other";
					break;
				default:
					break;
			}
			return retVal;
		}


		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="sPPremiseRoleType">sPPremiseRoleType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="sPPremiseRoleType.alarmMonitor">alarmMonitor</see> 
		/// </returns>
		public int sPPremiseRoleType_alarmMonitor()
		{
			return Convert.ToInt32(sPPremiseRoleType.alarmMonitor);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="sPPremiseRoleType">sPPremiseRoleType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="sPPremiseRoleType.alarmResponse">alarmResponse</see> 
		/// </returns>
		public int sPPremiseRoleType_alarmResponse()
		{
			return Convert.ToInt32(sPPremiseRoleType.alarmResponse);
		}


		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="sPPremiseRoleType">sPPremiseRoleType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="sPPremiseRoleType.patrol">patrol</see> 
		/// </returns>
		public int sPPremiseRoleType_patrol()
		{
			return Convert.ToInt32(sPPremiseRoleType.patrol);
		}

		/// <summary>
		/// Externally available version of the enumeration
		/// <see cref="sPPremiseRoleType">sPPremiseRoleType</see> 
		/// </summary>
		/// <returns>Value of 
		/// <see cref="sPPremiseRoleType.other">other</see> 
		/// </returns>
		public int sPPremiseRoleType_other()
		{
			return Convert.ToInt32(sPPremiseRoleType.other);
		}


		#endregion

		#endregion 

		# region Global Variables

        #region Logging

        /// <summary>
        /// Log file for logging (Used in many places)
        /// </summary>
        public string logFileStem = @"W:\logs\OBSTPU\";

        #endregion

        /// <summary>
        /// Email Address Delimiter
        /// </summary>
        public string EmailAddressDelimiter = @",";

		/// <summary>
		/// Base Queries for the 'GetBy' Methods
		/// </summary>
		public clsQueryPart[] baseQueries;

		/// <summary>
		/// Part of the Query that Pertains to ChangeData Information
		/// </summary>
		public clsQueryPart ChangeDataQ = new clsQueryPart();


		/// <summary>
		/// Local Representation of the class <see cref="clsSetting">clsSetting</see>
		/// </summary>
		public clsSetting Setting;

		/// <summary>
		/// UserId of System User
		/// </summary>
		public int systemUserId = 0; 

		/// <summary>
		/// Tax Rate for the system
		/// </summary>
		public decimal localTaxRate = 1.15M; 

		/// <summary>
		/// Whether the Price shown includes the local tax rate or not
		/// </summary>
		public bool priceShownIncludesLocalTaxRate = true; 

		/// <summary>
		/// Tax Rate for the system
		/// </summary>
		public clsKeyBase.freightChargeType freightChargeBasis; 

		/// <summary>
		/// Minimum Allowable Freight Charge
		/// </summary>
		public decimal minimumFreightCharge;
		
		/// <summary>
		/// Maximum Allowable Freight Charge
		/// </summary>
		public decimal maximumFreightCharge;

		/// <summary>
		/// CustomerGroupId that represents the 'Public'
		/// </summary>
		public int publicCustomerGroupId; 

		/// <summary>
		/// "Assumed Country that represents the 'Public'
		/// </summary>
		public int assumedCountryId; 

		/// <summary>
		/// "Assumed ShippingZone that represents the 'Public'
		/// </summary>
		public int assumedShippingZoneId; 

		/// <summary>
		/// CityId of City that is 'no city'
		/// </summary>
		public int nonCityCityId = 0; 

		/// <summary>
		/// Root Path for all folders
		/// </summary>
		public string thisRootPath = "";

		/// <summary>
		/// Path for all reports
		/// </summary>
		public string thisReportPath = "";

		/// <summary>
		/// Path for Output
		/// </summary>
		public string thisOutputPath = "";

		/// <summary>
		/// minimumBalanceRequiringStatement
		/// </summary>
		public decimal minimumBalanceRequiringStatement = 4.95M; 

		/// <summary>
		/// minimumStatementFrequency
		/// </summary>
		public int minimumStatementFrequency = 25; 

		/// <summary>
		/// minimumDetailsUpdateFrequency
		/// </summary>
		public int minimumDetailsUpdateFrequency = 6; 

		/// <summary>
		/// thisSmtpServer
		/// </summary>
		public string thisSmtpServer = "Localhost";



		/// <summary>
		/// MaxAllowable Number of a product
		/// </summary>
		public string MaxAllowable = "case when tblProduct.DisplayTypeId = 3 and tblProduct.IsPublic = 0 then" + crLf
			+ "	case when tblProduct.UseStockControl = 1 then" + crLf
			+ "		case when tblProduct.MaxQuantity > 0 then" + crLf
			+ "			Least(tblProduct.MaxQuantity, tblProduct.QuantityAvailable)" + crLf
			+ "		else tblProduct.QuantityAvailable end" + crLf
			+ "	else " + crLf
			+ "		case when tblProduct.MaxQuantity > 0 then" + crLf
			+ "			tblProduct.MaxQuantity" + crLf
			+ "		else -1 end" + crLf
			+ "	end" + crLf
			+ "else 0" + crLf
			+ "end";

		#endregion

		#region Method that gets Settings for Global Variables

		
		/// <summary>
		/// Obtains Settings from the Setting Table
		/// </summary>
		public virtual void GetGeneralSettings()
		{
			//Need to get the tax rate so that we can return the price with tax
			Setting = new clsSetting(thisDbType, localRecords.dbConnection);

			Setting.GetGeneralSettings();

			assumedCountryId = Setting.assumedCountryId;
			assumedShippingZoneId = Setting.assumedShippingZoneId;
			freightChargeBasis = Setting.freightChargeBasis;
			localTaxRate = Setting.localTaxRate;
			minimumBalanceRequiringStatement = Setting.minimumBalanceRequiringStatement;
			minimumDetailsUpdateFrequency = Setting.minimumDetailsUpdateFrequency;
			minimumFreightCharge = Setting.minimumFreightCharge;
			minimumStatementFrequency = Setting.minimumStatementFrequency;
			maximumFreightCharge = Setting.maximumFreightCharge;
			priceShownIncludesLocalTaxRate = Setting.priceShownIncludesLocalTaxRate;
			publicCustomerGroupId = Setting.publicCustomerGroupId;
			nonCityCityId = Setting.nonCityCityId;
			systemUserId = Setting.systemUserId;
			thisClientTimeZoneOsIndex = Setting.thisClientTimeZoneOsIndex;
			thisClientTimeZoneRegKey = Setting.thisClientTimeZoneRegKey;
			thisReportPath = Setting.thisReportPath;
			thisOutputPath = Setting.thisOutputPath;
			thisRootPath = Setting.thisRootPath;
		}

		/// <summary>
		/// Obfuscates a card number
		/// </summary>
		/// <param name="cardNum">Card Number to Obfuscate</param>
		/// <param name="dontNeedAgain">Whether we need the card in future</param>
		/// <returns>Encrypted Obfuscated card number</returns>
		public string ObfuscateCardNumber(string cardNum, bool dontNeedAgain)
		{
			clsStringEncryption thisEncrypt;
			thisEncrypt = new clsStringEncryption();
			
			string newCardNum = "";
			int CardNumLength = cardNum.Length;
			int numStartingDigitsToKeep = 1;
			int numTrailingDigitsToKeep = 5;
			char obfuscationReplacement = Convert.ToChar("x");

			if (dontNeedAgain && CardNumLength > numStartingDigitsToKeep + numTrailingDigitsToKeep)
			{
				newCardNum = cardNum.Substring(0,numStartingDigitsToKeep).PadRight(CardNumLength - numTrailingDigitsToKeep,obfuscationReplacement) 
					+ cardNum.Substring(CardNumLength - numTrailingDigitsToKeep);
			}
			else
				newCardNum = cardNum;

			newCardNum = thisEncrypt.Encrypt(newCardNum);
				
			return newCardNum;
		}

		
		/// <summary>
		/// De-Obfuscates a Card Number that has been retrieved from the Database
		/// </summary>
		/// <param name="cardNum">cardNum to UnObfuscate</param>
		/// <returns>Decrypted card number (note this may still be obfuscated)</returns>
		public string UnObfuscateCardNumber(string cardNum)
		{
			clsStringEncryption thisEncrypt;
			thisEncrypt = new clsStringEncryption();
		
			return thisEncrypt.Decrypt(cardNum);
		}


		#endregion

		# region SQL Helper Methods

		/// <summary>
		/// Returns the Query Part allowing the information about the Address to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart AddressQueryPart()
		{
			return AddressQueryPart(false);
		}		
		/// <summary>
		/// Returns the Query Part allowing the information about the Address to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart AddressQueryPart(bool forDistinctQueries)
		{
			clsQueryPart thisQ = new clsQueryPart();
			
			string stem = "";
			string table = "tblAddress";
			string pk = "AddressId";

			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				if (forDistinctQueries)
					thisQ.AddSelectColumn("Distinct " + AddStem(table, stem, pk));
				else
					thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				if (stem == "")
					stem = "Address_";

				if (forDistinctQueries)
					thisQ.AddSelectColumn("Distinct " + AddStem(table, stem, pk));
				
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "POBoxType"));
			thisQ.AddSelectColumn(AddStem(table, stem, "BuildingName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "UnitNumber"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Number"));
			thisQ.AddSelectColumn(AddStem(table, stem, "StreetAddress"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Suburb"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PostCode"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CityId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CityName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "StateName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CountryId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CountryName"));

			if (!forDistinctQueries)
				thisQ.AddSelectColumn(AddStem(table, stem, "QuickAddress"));
			else
				thisQ.AddSelectColumn(AddStem(table, stem, "QuickAddress", true));
			
			thisQ.AddSelectColumn(AddStem(table, stem, "AddressType"));
			thisQ.AddSelectColumn(AddStem(table, stem, "AddressTypeDescription"));
			thisQ.AddSelectColumn(AddStem(table, stem, "AssocTableName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "AssocRowId"));

			thisQ.AddSelectColumn(AddStem(table, stem, "ChangeDataId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Archive"));
									
			thisQ.AddFromTable("");
			
			return thisQ;
		}


		/// <summary>
		/// Returns the Query Part allowing the information about the CCAttempt to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart CCAttemptQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();
			
			string stem = "";
			string table = "tblCCAttempt";
			string pk = "CCAttemptId";

			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				if (stem == "")
					stem = "CCAttempt_";
				
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "OrderId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PersonId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "UserId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CardTypeId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "NameOnCard"));
			thisQ.AddSelectColumn(AddStem(table, stem, "StartDate"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ExpiryDate"));
			thisQ.AddSelectColumn(AddStem(table, stem, "IssueNumber"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CardNumber"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Amount"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateSubmitted"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateSubmittedUtc"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateAttempted"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateAttemptedUtc"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Success"));
			thisQ.AddSelectColumn(AddStem(table, stem, "IsManual"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DeclineReason"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Receipt"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerIPAddress"));
			thisQ.AddSelectColumn(AddStem(table, stem, "UserIPAddress"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PersonName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "UserName"));
			
			return thisQ;
		}	


		/// <summary>
		/// Returns the Query Part allowing the information about the ChangeData to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart ChangeDataQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();
			
			string stem = "";
			string table = "tblChangeData";
			string pk = "ChangeDataId";

			
			thisQ.AddFromTable(table);
			

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				if (stem == "")
					stem = "ChangeData_";
				
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "CreatedByUserId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CreatedByFirstNameLastName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateCreated"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ChangedByUserId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ChangedByFirstNameLastName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateChanged"));
									
			thisQ.AddFromTable("");
			
			return thisQ;
		}

		/// <summary>
		/// Returns the Query Part allowing the information about the City to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart CityQueryPart()
		{
			return CityQueryPart("");
		}

		/// <summary>
		/// Returns the Query Part allowing the information about the City to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart CityQueryPart(string alias)
		{
			clsQueryPart thisQ = new clsQueryPart();
			
			string stem = "";
			string table = "tblCity";
			string pk = "CityId";

			if (alias != "")
			{
				table = alias;
				stem = alias + "_";
				thisQ.AddFromTable(thisTable + " " + alias);
			}
			else
			{
				thisQ.AddFromTable(table);
			}

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				if (stem == "")
					stem = "City_";
				
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "CountryId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CityName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "StateName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "IncludeAsPublic"));
			
			return thisQ;
		}


		/// <summary>
		/// Returns the Query Part allowing the information about the CompanyType  to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart CompanyTypeQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();

			string stem = "";
			string table = "tblCompanyType"; 
			string pk = "CompanyTypeId";
			
			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				stem = "CompanyType_";
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}
			thisQ.AddSelectColumn(AddStem(table, stem, "CompanyTypeName"));
	
			return thisQ;
		}



		/// <summary>
		/// Returns the Query Part allowing the information about the CorrespondenceSendToCustomer to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart CorrespondenceSendToCustomerQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();
			
			string stem = "";
			string table = "tblCorrespondenceSendToCustomer";
			string pk = "CorrespondenceSendToCustomerId";

			
			thisQ.AddFromTable(table);
			

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				if (stem == "")
					stem = "CorrespondenceSendToCustomer_";
				
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PersonId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "IncludesAtLeastOneInvoice"));
			thisQ.AddSelectColumn(AddStem(table, stem, "IncludesStatement"));
			thisQ.AddSelectColumn(AddStem(table, stem, "IncludesAtLeastOneSticker"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CorrespondenceMedium"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CorrespondenceSendToCustomerStatus"));
			thisQ.AddSelectColumn(AddStem(table, stem, "StickerIncludedList"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CorrespondenceFile"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateGenerated"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateGeneratedUtc"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateSent"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateSentUtc"));									
			thisQ.AddFromTable("");
			
			return thisQ;
		}



		/// <summary>
		/// Returns the Query Part allowing the information about the Country  to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart CountryQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();

			string stem = "";
			string table = "tblCountry"; 
			string pk = "CountryId";
			
			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				stem = "Country_";
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "CountryName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "LocalTaxApplies"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ShippingZoneId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CanShipToCountry"));
	
	
			return thisQ;
		}



		/// <summary>
		/// Returns the Query Part allowing the information about the Customer  to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart CustomerQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();

			string stem = "";
			string table = "tblCustomer"; 
			string pk = "CustomerId";
			
			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				stem = "Customer_";
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerGroupId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerType"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Title"));
			thisQ.AddSelectColumn(AddStem(table, stem, "FirstName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "LastName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CompanyName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "AccountNumber"));
			thisQ.AddSelectColumn(AddStem(table, stem, "OpeningBalance"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CreditLimit"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CountryId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "KdlComments"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerComments"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateStart"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateLastLoggedIn"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateLastLoggedInUtc"));
			thisQ.AddSelectColumn(AddStem(table, stem, "StartDateForStatement"));
			thisQ.AddSelectColumn(AddStem(table, stem, "StartDateForInvoices"));

			thisQ.AddSelectColumn(AddStem(table, stem, "DateFirstOrder"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateFirstOrderUtc"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateLastOrder"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateLastOrderUtc"));
			thisQ.AddSelectColumn(AddStem(table, stem, "NumOrders"));
			thisQ.AddSelectColumn(AddStem(table, stem, "TotalItemCost"));
			thisQ.AddSelectColumn(AddStem(table, stem, "TotalTaxCost"));
			thisQ.AddSelectColumn(AddStem(table, stem, "TotalFreightCost"));
			thisQ.AddSelectColumn(AddStem(table, stem, "BaseBalance"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CurrentBalance"));
			thisQ.AddSelectColumn(AddStem(table, stem, "AvailableCredit"));
			thisQ.AddSelectColumn(AddStem(table, stem, "TotalPurchases"));
			thisQ.AddSelectColumn(AddStem(table, stem, "TotalPaid"));
			thisQ.AddSelectColumn(AddStem(table, stem, "TotalUncleared"));
			thisQ.AddSelectColumn(AddStem(table, stem, "InvoiceBaseBalance"));
			thisQ.AddSelectColumn(AddStem(table, stem, "InvoiceCurrentBalance"));
			thisQ.AddSelectColumn(AddStem(table, stem, "InvoiceTotalPurchases"));
			thisQ.AddSelectColumn(AddStem(table, stem, "InvoiceTotalPaid"));
			thisQ.AddSelectColumn(AddStem(table, stem, "InvoiceTotalUncleared"));
			thisQ.AddSelectColumn(AddStem(table, stem, "FullName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "IsDirectDebitCustomer"));
	
			thisQ.AddSelectColumn(AddStem(table, stem, "ChangeDataId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Archive"));
	



			return thisQ;
		}

		/// <summary>
		/// Returns the Query Part allowing the information about the CustomerGroup  to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart CustomerGroupQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();

			string stem = "";
			string table = "tblCustomerGroup"; 
			string pk = "CustomerGroupId";
			
			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				stem = "CustomerGroup_";
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerGroupName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerGroupDescription"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ChangeDataId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Archive"));
	
	
			return thisQ;
		}


		/// <summary>
		/// Returns the Query Part allowing the information about Data Changes to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart DefinedContentQueryPart()
		{
			clsQueryPart DefinedContentQ = new clsQueryPart();

			DefinedContentQ.AddSelectColumn("tblDefinedContent.DefinedContentTitle as DefinedContent_DefinedContentTitle");
			DefinedContentQ.AddSelectColumn("tblDefinedContent.PopUpWidth as DefinedContent_PopUpWidth");
			DefinedContentQ.AddSelectColumn("tblDefinedContent.PopUpHeight as DefinedContent_PopUpHeight");
			DefinedContentQ.AddSelectColumn("tblDefinedContent.UsesExternalUrl as DefinedContent_UsesExternalUrl");
			DefinedContentQ.AddSelectColumn("tblDefinedContent.ExternalUrl as DefinedContent_ExternalUrl");
			DefinedContentQ.AddSelectColumn("tblDefinedContent.Description as DefinedContent_Description");

			DefinedContentQ.AddJoin(thisTable + ".DefinedContentId = tblDefinedContent.DefinedContentId");
			
			DefinedContentQ.AddFromTable("tblDefinedContent");

			return DefinedContentQ;
		}


		/// <summary>
		/// Returns the Query Part allowing the information about Data Changes to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart DefinedContentImageQueryPart()
		{
			clsQueryPart DefinedContentImageQ = new clsQueryPart();

			DefinedContentImageQ.AddSelectColumn("AuxDefinedContentImage.DefinedContentImageId as DefinedContentImage_DefinedContentImageId");
			DefinedContentImageQ.AddSelectColumn("tblDefinedContentImage.ImageRef as DefinedContentImage_ImageRef");
			DefinedContentImageQ.AddSelectColumn("tblDefinedContentImage.ThumbNailRef as DefinedContentImage_ThumbNailRef");
			DefinedContentImageQ.AddSelectColumn("tblDefinedContentImage.Caption as DefinedContentImage_Caption");

			DefinedContentImageQ.AddFromTable("tblDefinedContentImage");
			DefinedContentImageQ.AddFromTable("(Select tblDefinedContent.DefinedContentId, " + crLf
				+ "Max(tblDefinedContentImage.DefinedContentImageId) as DefinedContentImageId" + crLf
				+ "From tblDefinedContent left outer join tblDefinedContentImage " + crLf
				+ "on tblDefinedContent.DefinedContentId = tblDefinedContentImage.DefinedContentId" + crLf
				+ "Group by tblDefinedContent.DefinedContentId) AuxDefinedContentImage");

			DefinedContentImageQ.AddJoin("tblProduct.DefinedContentId = AuxDefinedContentImage.DefinedContentId");
			DefinedContentImageQ.AddJoin("tblDefinedContentImage.DefinedContentImageId = AuxDefinedContentImage.DefinedContentImageId");

			return DefinedContentImageQ;
		}

		/// <summary>
		/// Returns the Query Part allowing the information about the FreightRule to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart FreightRuleQueryPart()
		{
			clsQueryPart FreightRuleQ = new clsQueryPart();

			FreightRuleQ.AddSelectColumn("tblFreightRule.FreightRuleName as FreightRule_FreightRuleName");
			FreightRuleQ.AddSelectColumn("tblFreightRule.FreightRuleDescription as FreightRule_FreightRuleDescription");
			FreightRuleQ.AddSelectColumn("tblFreightRule.MinTotalWeight as FreightRule_MinTotalWeight");
			FreightRuleQ.AddSelectColumn("tblFreightRule.MaxTotalWeight as FreightRule_MaxTotalWeight");
			FreightRuleQ.AddSelectColumn("tblFreightRule.MinTotalValue as FreightRule_MinTotalValue");
			FreightRuleQ.AddSelectColumn("tblFreightRule.MaxTotalValue as FreightRule_MaxTotalValue");
			FreightRuleQ.AddSelectColumn("tblFreightRule.FRCost as FreightRule_FRCost");

			FreightRuleQ.AddFromTable("tblFreightRule");

			FreightRuleQ.AddJoin(thisTable + ".FreightRuleId = tblFreightRule.FreightRuleId");	
			
			return FreightRuleQ;
		}

		/// <summary>
		/// Returns the Query Part allowing the information about the HighRiskGood to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart HighRiskGoodQueryPart()
		{
			return HighRiskGoodQueryPart(false);
		}

		/// <summary>
		/// Returns the Query Part allowing the information about the HighRiskGood to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart HighRiskGoodQueryPart(bool forDistinctQueries)
		{
			clsQueryPart thisQ = new clsQueryPart();
			
			string stem = "";
			string table = "tblHighRiskGood";
			string pk = "HighRiskGoodId";

			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				if (forDistinctQueries)
					thisQ.AddSelectColumn("Distinct " + AddStem(table, stem, pk));
				else
					thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				if (stem == "")
					stem = "HighRiskGood_";

				if (forDistinctQueries)
					thisQ.AddSelectColumn("Distinct " + AddStem(table, stem, pk));
				else
					thisQ.AddSelectColumn(AddStem(table, stem, pk));
				
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "PremiseId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "HighRiskGoodName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "HighRiskGoodDescription"));
			thisQ.AddSelectColumn(AddStem(table, stem, "SerialNumber"));
			thisQ.AddSelectColumn(AddStem(table, stem, "InformationType"));
			thisQ.AddSelectColumn(AddStem(table, stem, "MakeBrand"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Model"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PurchasePlace"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PurchaseDate"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PricePaid"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Status"));
			thisQ.AddSelectColumn(AddStem(table, stem, "StatusDate"));
			thisQ.AddSelectColumn(AddStem(table, stem, "InsuranceCompany"));
			thisQ.AddSelectColumn(AddStem(table, stem, "InsuranceClaimNumber"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PoliceFileNumber"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ChangeDataId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Archive"));

			return thisQ;

		}

		/// <summary>
		/// Returns the Query Part allowing the information about History to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart HistoryQueryPart(string DateForHistory, string SelectionCriteria)
		{
			clsQueryPart thisQ = new clsQueryPart();
			
			string ThisRecordExpression =  "(case when " + thisTable + ".Archive < 1 then " + thisTable + "." + thisPk + " else " + thisTable + ".Archive end)" ;

			string tTable = "(Select " + ThisRecordExpression + " as ThisRecord," + crLf
				+ "	Max(tblChangeData.DateChanged) as LastChangedDateThisRecord" + crLf
				+ "From " + thisTable + ", tblChangeData" + crLf
				+ "Where " + thisTable + ".ChangeDataId = tblChangeData.ChangeDataId" + crLf
				+ "	And " + SelectionCriteria + crLf;
			
			//			if (DateForHistory != "")
			//				tTable += "	And tblChangeData.DateChanged < " + MatchCondition(DateForHistory, matchCriteria.exactMatch) + crLf;

			if (DateForHistory != "")
				tTable += "	And DateDiff(s, Convert(datetime, '" + DateForHistory + "'), DateChanged) < 61  " + crLf;

			tTable += "Group by " + ThisRecordExpression
				+ ") tblHistory ";

			thisQ.AddFromTable(tTable);
				
			thisQ.AddJoin(ThisRecordExpression + " = tblHistory.ThisRecord");
			thisQ.AddJoin("tblChangeData.DateChanged = tblHistory.LastChangedDateThisRecord");
			
			return thisQ;
		}



		/// <summary>
		/// Returns the Query Part allowing the information about the Image to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart ImageQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();

			string stem = "";
			string table = "tblImage"; 
			string pk = "ImageId";
			
			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				stem = "Image_";
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "HighRiskGoodId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Caption"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ThumbFilename"));
			thisQ.AddSelectColumn(AddStem(table, stem, "FeatureFilename"));
			thisQ.AddSelectColumn(AddStem(table, stem, "FullFullname"));
			thisQ.AddSelectColumn(AddStem(table, stem, "SortOrder"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Archive"));
			
			thisQ.AddFromTable("");

			return thisQ;
		}

		/// <summary>
		/// Returns the Query Part allowing the information about the Item to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart ItemQueryPart()
		{
			clsQueryPart ItemQ = new clsQueryPart();

			ItemQ.AddSelectColumn("tblItem.OrderId as Item_OrderId");
			ItemQ.AddSelectColumn("tblItem.ProductId as Item_ProductId");
			ItemQ.AddSelectColumn("tblItem.PremiseId as Item_PremiseId");
			ItemQ.AddSelectColumn("tblItem.Quantity as Item_Quantity");
			ItemQ.AddSelectColumn("tblItem.ItemName as Item_ItemName");
			ItemQ.AddSelectColumn("tblItem.ItemCode as Item_ItemCode");
			ItemQ.AddSelectColumn("tblItem.ShortDescription as Item_ShortDescription");
			ItemQ.AddSelectColumn("tblItem.LongDescription as Item_LongDescription");
			ItemQ.AddSelectColumn("tblItem.Cost as Item_Cost");
			ItemQ.AddSelectColumn("tblItem.Weight as Item_Weight");
			ItemQ.AddSelectColumn("tblItem.MaxKeyholdersPerPremise as Item_MaxKeyholdersPerPremise");
			ItemQ.AddSelectColumn("tblItem.MaxAssetRegisterAssets as Item_MaxAssetRegisterAssets");
			ItemQ.AddSelectColumn("tblItem.MaxAssetRegisterStorage as Item_MaxAssetRegisterStorage");
			ItemQ.AddSelectColumn("tblItem.MaxDocumentSafeDocuments as Item_MaxDocumentSafeDocuments");
			ItemQ.AddSelectColumn("tblItem.MaxDocumentSafeStorage as Item_MaxDocumentSafeStorage");
			ItemQ.AddSelectColumn("tblItem.RequiresPremiseForActivation as Item_RequiresPremiseForActivation");
			ItemQ.AddSelectColumn("tblItem.DateActivation as Item_DateActivation");
			ItemQ.AddSelectColumn("tblItem.DateActivationUtc as Item_DateActivationUtc");
			ItemQ.AddSelectColumn("tblItem.DateExpiry as Item_DateExpiry");
			ItemQ.AddSelectColumn("tblItem.DateExpiryUtc as Item_DateExpiryUtc");
			ItemQ.AddSelectColumn("tblItem.DurationNumUnits as Item_DurationNumUnits");
			ItemQ.AddSelectColumn("tblItem.DurationUnitId as Item_DurationUnitId");
			ItemQ.AddSelectColumn("tblItem.FreightCost as Item_FreightCost");
			
			return ItemQ;
		}


		/// <summary>
		/// Returns the Query Part allowing the information about the Order to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart OrderQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();
			
			string stem = "";
			string table = "tblOrder";
			string pk = "OrderId";

			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				if (stem == "")
					stem = "Order_";
				
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "OrderId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PersonId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PaymentMethodTypeId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerGroupId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "OrderNum"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerType"));
			thisQ.AddSelectColumn(AddStem(table, stem, "FullName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Title"));
			thisQ.AddSelectColumn(AddStem(table, stem, "FirstName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "LastName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickPostalAddress"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickDaytimePhone"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickDaytimeFax"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickAfterHoursPhone"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickAfterHoursFax"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickMobilePhone"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CountryId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Email"));
			thisQ.AddSelectColumn(AddStem(table, stem, "OrderSubmitted"));
			thisQ.AddSelectColumn(AddStem(table, stem, "OrderPaid"));
			thisQ.AddSelectColumn(AddStem(table, stem, "OrderCreatedMechanism"));
			thisQ.AddSelectColumn(AddStem(table, stem, "OrderStatusId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "SupplierComment"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateCreated"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateCreatedUtc"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateSubmitted"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateSubmittedUtc"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateProcessed"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateProcessedUtc"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateShipped"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateShippedUtc"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateDue"));
			thisQ.AddSelectColumn(AddStem(table, stem, "InvoiceRequested"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateInvoiceLastPrinted"));
			thisQ.AddSelectColumn(AddStem(table, stem, "TaxAppliedToOrder"));
			thisQ.AddSelectColumn(AddStem(table, stem, "TaxRateAtTimeOfOrder"));
			thisQ.AddSelectColumn(AddStem(table, stem, "TaxCost"));
			thisQ.AddSelectColumn(AddStem(table, stem, "FreightCost"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Total"));
			thisQ.AddSelectColumn(AddStem(table, stem, "TotalItemWeight"));
			thisQ.AddSelectColumn(AddStem(table, stem, "TotalItemCost"));
			thisQ.AddSelectColumn(AddStem(table, stem, "TotalItemFreightCost"));
			thisQ.AddSelectColumn(AddStem(table, stem, "IsInvoiceOrder"));
			thisQ.AddSelectColumn(AddStem(table, stem, "NumItems"));
			
			return thisQ;
		}	



		/// <summary>
		/// Returns the Query Part allowing the information about the OrderStatus  to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart OrderStatusQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();

			string stem = "";
			string table = "tblOrderStatus"; 
			string pk = "OrderStatusId";
			
			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				stem = "OrderStatus_";
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerFacingName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "SupplierFacingName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerFacingDescription"));
			thisQ.AddSelectColumn(AddStem(table, stem, "SupplierFacingDescription"));
			thisQ.AddSelectColumn(AddStem(table, stem, "IsPublic"));
	
			return thisQ;
		}

		/// <summary>
		/// Returns the Query Part allowing the information about the PaymentMethodType to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart PaymentMethodTypeQueryPart()
		{
			clsQueryPart PaymentMethodTypeQ = new clsQueryPart();

			PaymentMethodTypeQ.AddSelectColumn("tblPaymentMethodType.PaymentMethodTypeName as PaymentMethodType_PaymentMethodTypeName");
			PaymentMethodTypeQ.AddSelectColumn("tblPaymentMethodType.PaymentMethodTypeDescription as PaymentMethodType_PaymentMethodTypeDescription");
			PaymentMethodTypeQ.AddSelectColumn("tblPaymentMethodType.CustomerNotePreSale as PaymentMethodType_CustomerNotePreSale");
			PaymentMethodTypeQ.AddSelectColumn("tblPaymentMethodType.CustomerNotePostSale as PaymentMethodType_CustomerNotePostSale");
			PaymentMethodTypeQ.AddSelectColumn("tblPaymentMethodType.CustomerNoteEmailHtml as PaymentMethodType_CustomerNoteEmailHtml");
			PaymentMethodTypeQ.AddSelectColumn("tblPaymentMethodType.CustomerNoteEmailPlainText as PaymentMethodType_CustomerNoteEmailPlainText");
			PaymentMethodTypeQ.AddSelectColumn("tblPaymentMethodType.CustomerCanNotChoose as PaymentMethodType_CustomerCanNotChoose");
			PaymentMethodTypeQ.AddSelectColumn("tblPaymentMethodType.AllowCustomerToUseByDefault as PaymentMethodType_AllowCustomerToUseByDefault");

			PaymentMethodTypeQ.AddFromTable("tblPaymentMethodType");

			PaymentMethodTypeQ.AddJoin(thisTable + ".PaymentMethodTypeId = tblPaymentMethodType.PaymentMethodTypeId");	
			
			return PaymentMethodTypeQ;
		}


		/// <summary>
		/// Returns the Query Part allowing the information about the PersonPremiseRole  to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart PersonPremiseRoleQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();

			string stem = "";
			string table = "tblPersonPremiseRole"; 
			string pk = "PersonPremiseRoleId";
			
			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				stem = "PersonPremiseRole_";
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "PersonId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PremiseId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PersonPremiseRoleType"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Priority"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ChangeDataId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Archive"));
	
			return thisQ;
		}

		/// <summary>
		/// Returns the Query Part allowing the information about the PhoneNumber to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart PhoneNumberQueryPart()
		{
			return PhoneNumberQueryPart(false);
		}		
		/// <summary>
		/// Returns the Query Part allowing the information about the PhoneNumber to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart PhoneNumberQueryPart(bool forDistinctQueries)
		{
			clsQueryPart thisQ = new clsQueryPart();
			
			string stem = "";
			string table = "tblPhoneNumber";
			string pk = "PhoneNumberId";

			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				if (forDistinctQueries)
					thisQ.AddSelectColumn("Distinct " + AddStem(table, stem, pk));
				else
					thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				if (stem == "")
					stem = "PhoneNumber_";

				if (forDistinctQueries)
					thisQ.AddSelectColumn("Distinct " + AddStem(table, stem, pk));
				
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "InternationalPrefix"));
			thisQ.AddSelectColumn(AddStem(table, stem, "NationalOrMobilePrefix"));
			thisQ.AddSelectColumn(AddStem(table, stem, "MainNumber"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Extension"));

			if (!forDistinctQueries)
				thisQ.AddSelectColumn(AddStem(table, stem, "QuickPhoneNumber"));
			else
				thisQ.AddSelectColumn(AddStem(table, stem, "QuickPhoneNumber", true));
			
			thisQ.AddSelectColumn(AddStem(table, stem, "PhoneNumberType"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PhoneNumberTypeDescription"));
			thisQ.AddSelectColumn(AddStem(table, stem, "AssocTableName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "AssocRowId"));

			thisQ.AddSelectColumn(AddStem(table, stem, "ChangeDataId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Archive"));
									
			thisQ.AddFromTable("");
			
			return thisQ;
		}


		/// <summary>
		/// Returns the Query Part allowing the information about the Person to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart PersonQueryPart()
		{
			return PersonQueryPart(false);

		}

		/// <summary>
		/// Returns the Query Part allowing the information about the Person to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart PersonQueryPart(bool forDistinctQueries)
		{
			clsQueryPart thisQ = new clsQueryPart();
			
			string stem = "";
			string table = "tblPerson";
			string pk = "PersonId";

			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				if (forDistinctQueries)
					thisQ.AddSelectColumn("Distinct " + AddStem(table, stem, pk));
				else
					thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				if (stem == "")
					stem = "Person_";

				if (forDistinctQueries)
					thisQ.AddSelectColumn("Distinct " + AddStem(table, stem, pk));
				else
					thisQ.AddSelectColumn(AddStem(table, stem, pk));
				
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Title"));
			thisQ.AddSelectColumn(AddStem(table, stem, "FirstName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "LastName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "UserName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Password"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PositionInCompany"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Email"));
			thisQ.AddSelectColumn(AddStem(table, stem, "IsCustomerAdmin"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PreferredContactMethod"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickPostalAddress"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickDaytimePhone"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickDaytimeFax"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickAfterHoursPhone"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickAfterHoursFax"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickMobilePhone"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerComments"));
			thisQ.AddSelectColumn(AddStem(table, stem, "KdlComments"));
			thisQ.AddSelectColumn(AddStem(table, stem, "FullName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateLastLoggedIn"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ChangeDataId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Archive"));
			
			return thisQ;
		}

		/// <summary>
		/// Returns the Query Part allowing the information about the Premise to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart PremiseQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();
			
			string stem = "";
			string table = "tblPremise";
			string pk = "PremiseId";

			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				if (stem == "")
					stem = "Premise_";
				
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}


			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ItemId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ProductId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PremiseNumber"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CompanyAtPremiseName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickPhysicalAddress"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CompanyTypeId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CompanyTypeName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Url"));
			thisQ.AddSelectColumn(AddStem(table, stem, "AlarmDetails"));
			thisQ.AddSelectColumn(AddStem(table, stem, "KdlComments"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerComments"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateStart"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateNextSubscriptionDueToBeInvoiced"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateSubscriptionExpires"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateLastDetailsUpdate"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateLastInvoice"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateLastCopyOfInvoice"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateLastStatement"));
			thisQ.AddSelectColumn(AddStem(table, stem, "StickerRequired"));
			thisQ.AddSelectColumn(AddStem(table, stem, "InvoiceRequired"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CopyOfInvoiceRequired"));
			thisQ.AddSelectColumn(AddStem(table, stem, "StatementRequired"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DetailsUpdateRequired"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ChangeDataId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Archive"));

			return thisQ;

		}


		/// <summary>
		/// Returns the Query Part allowing the information about the Product to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart ProductQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();
			
			string stem = "";
			string table = "tblProduct";
			string pk = "ProductId";

			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				if (stem == "")
					stem = "Product_";
				
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}


			thisQ.AddSelectColumn(AddStem(table, stem, "ProductName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ProductCode"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ShortDescription"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DefinedContentId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DisplayTypeId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuantityDescription"));
			thisQ.AddSelectColumn(AddStem(table, stem, "WholeNumbersOnly"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ProductOnSpecial"));
			thisQ.AddSelectColumn(AddStem(table, stem, "UseStockControl"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuantityAvailable"));
			thisQ.AddSelectColumn(AddStem(table, stem, "MaxQuantity"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Weight"));
			thisQ.AddSelectColumn(AddStem(table, stem, "MaxKeyholdersPerPremise"));
			thisQ.AddSelectColumn(AddStem(table, stem, "MaxAssetRegisterAssets"));
			thisQ.AddSelectColumn(AddStem(table, stem, "MaxAssetRegisterStorage"));
			thisQ.AddSelectColumn(AddStem(table, stem, "MaxDocumentSafeDocuments"));
			thisQ.AddSelectColumn(AddStem(table, stem, "MaxDocumentSafeStorage"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DurationNumUnits"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DurationUnitId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "RequiresPremiseForActivation"));
			thisQ.AddSelectColumn(AddStem(table, stem, "IsPublic"));
			
			thisQ.AddSelectColumn(MaxAllowable + " as " + stem + "MaxAllowable");

			return thisQ;

		}

		/// <summary>
		/// Returns the Query Part allowing the information about the ProductCategory to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart ProductCategoryQueryPart()
		{
			clsQueryPart ProductCategoryQ = new clsQueryPart();

			ProductCategoryQ.AddSelectColumn("tblProductCategory.ProductCategoryName as ProductCategory_ProductCategoryName");
			ProductCategoryQ.AddSelectColumn("tblProductCategory.ProductCategoryShortDescription as ProductCategory_ProductCategoryShortDescription");
			ProductCategoryQ.AddSelectColumn("tblProductCategory.DefinedContentId as ProductCategory_DefinedContentId");
			ProductCategoryQ.AddSelectColumn("tblProductCategory.ShowInCategoryBar as ProductCategory_ShowInCategoryBar");
			ProductCategoryQ.AddSelectColumn("tblProductCategory.ShowOnHomePage as ProductCategory_ShowOnHomePage");
			ProductCategoryQ.AddSelectColumn("tblProductCategory.CategoryDisplayOrder as ProductCategory_CategoryDisplayOrder");
			ProductCategoryQ.AddSelectColumn("tblProductCategory.CategoryDisplayContent as ProductCategory_CategoryDisplayContent");
			ProductCategoryQ.AddSelectColumn("tblProductCategory.IsSubCategoryOf as ProductCategory_IsSubCategoryOf");
			ProductCategoryQ.AddSelectColumn("tblProductCategory.DisplayStyle as ProductCategory_DisplayStyle");

			ProductCategoryQ.AddFromTable("tblProductCategory");

			ProductCategoryQ.AddJoin(thisTable + ".ProductCategoryId = tblProductCategory.ProductCategoryId");
			
			return ProductCategoryQ;
		}

		/// <summary>
		/// Returns the Query Part allowing the information about the ProductCustomerGroupPrice to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart ProductCustomerGroupPriceQueryPart()
		{
			clsQueryPart ProductCustomerGroupPriceQ = new clsQueryPart();

			ProductCustomerGroupPriceQ.AddSelectColumn("tblProductCustomerGroupPrice.ProductId as ProductCustomerGroupPrice_ProductId");
			ProductCustomerGroupPriceQ.AddSelectColumn("tblProductCustomerGroupPrice.CustomerGroupId as ProductCustomerGroupPrice_CustomerGroupId");
			ProductCustomerGroupPriceQ.AddSelectColumn("tblProductCustomerGroupPrice.Price as ProductCustomerGroupPrice_Price");

			ProductCustomerGroupPriceQ.AddFromTable("tblProductCustomerGroupPrice");

			ProductCustomerGroupPriceQ.AddJoin(thisTable + ".ProductId = tblProductCustomerGroupPrice.ProductId");
			
			return ProductCustomerGroupPriceQ;
		}

		/// <summary>
		/// Returns the Query Part allowing the information about the Search  to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart SearchQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();

			string stem = "";
			string table = "tblSearch"; 
			string pk = "SearchId";
			
			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				stem = "Search_";
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "UserId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "SearchType"));
			thisQ.AddSelectColumn(AddStem(table, stem, "SearchQuery"));
			thisQ.AddSelectColumn(AddStem(table, stem, "SearchReason"));
			thisQ.AddSelectColumn(AddStem(table, stem, "IPAddress"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateSearch"));
	
			return thisQ;
		}


		/// <summary>
		/// Returns the Query Part allowing the information about the SearchRecord  to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart SearchRecordQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();

			string stem = "";
			string table = "tblSearchRecord"; 
			string pk = "SearchRecordId";
			
			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				stem = "SearchRecord_";
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "SearchId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "RecordType"));
			thisQ.AddSelectColumn(AddStem(table, stem, "IdRecord"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateSearchRecord"));
	
			return thisQ;
		}


		/// <summary>
		/// Returns the Query Part allowing the information about the Child to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart ServiceProviderQueryPart()
		{
			return ServiceProviderQueryPart(false);
		}		

		/// <summary>
		/// Returns the Query Part allowing the information about the ServiceProvider  to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart ServiceProviderQueryPart(bool forDistinctQueries)
		{
			clsQueryPart thisQ = new clsQueryPart();

			string stem = "";
			string table = "tblServiceProvider"; 
			string pk = "ServiceProviderId";
			
			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				if (forDistinctQueries)
					thisQ.AddSelectColumn("Distinct " + AddStem(table, stem, pk));
				else
					thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				if (stem == "")
					stem = "ServiceProvider_";

				//				if (forDistinctQueries)
				//					thisQ.AddSelectColumn("Distinct " + AddStem(table, stem, pk));
				//				else
				thisQ.AddSelectColumn(AddStem(table, stem, pk));

				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);		
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Title"));
			thisQ.AddSelectColumn(AddStem(table, stem, "FirstName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "LastName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CompanyName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickDaytimePhone"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickDaytimeFax"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickAfterHoursPhone"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickAfterHoursFax"));
			thisQ.AddSelectColumn(AddStem(table, stem, "QuickMobilePhone"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Email"));
			thisQ.AddSelectColumn(AddStem(table, stem, "IsSecurityCompany"));
			thisQ.AddSelectColumn(AddStem(table, stem, "KdlComments"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerComments"));
			thisQ.AddSelectColumn(AddStem(table, stem, "FullName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ChangeDataId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Archive"));

			return thisQ;
		}

		/// <summary>
		/// Returns the Query Part allowing the information about the SPPremiseRole  to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart SPPremiseRoleQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();

			string stem = "";
			string table = "tblSPPremiseRole"; 
			string pk = "SPPremiseRoleId";
			
			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				stem = "SPPremiseRole_";
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "ServiceProviderId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PremiseId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "SPPremiseRoleType"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ChangeDataId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Archive"));
	
			return thisQ;
		}

		/// <summary>
		/// Returns the Query Part allowing the information about the ShippingZone  to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart ShippingZoneQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();

			string stem = "";
			string table = "tblShippingZone"; 
			string pk = "ShippingZoneId";
			
			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				stem = "ShippingZone_";
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "ShippingZoneCode"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ShippingZoneDescription"));
			thisQ.AddSelectColumn(AddStem(table, stem, "IsPublic"));
	
			return thisQ;
		}

		/// <summary>
		/// Returns the Query Part allowing the information about the Transaction to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart TransactionQueryPart()
		{
			clsQueryPart thisQ = new clsQueryPart();
			
			string stem = "";
			string table = "tblTransaction";
			string pk = "TransactionId";

			thisQ.AddFromTable(table);

			if (thisTable == table)
			{
				thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				if (stem == "")
					stem = "Transaction_";
				
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "OrderId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PersonId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "UserId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PaymentMethodTypeId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CCAttemptId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Amount"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Pending"));
			thisQ.AddSelectColumn(AddStem(table, stem, "IsInvoicePayment"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateSubmitted"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateSubmittedUtc"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateCompleted"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateCompletedUtc"));
			thisQ.AddSelectColumn(AddStem(table, stem, "VendorMemo"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerIPAddress"));
			thisQ.AddSelectColumn(AddStem(table, stem, "UserIPAddress"));
			thisQ.AddSelectColumn(AddStem(table, stem, "InvoiceBaseBalance"));
			thisQ.AddSelectColumn(AddStem(table, stem, "InvoicePriorBalance"));
			thisQ.AddSelectColumn(AddStem(table, stem, "InvoicePostBalance"));
			thisQ.AddSelectColumn(AddStem(table, stem, "BaseBalance"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PriorBalance"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PostBalance"));
			thisQ.AddSelectColumn(AddStem(table, stem, "CustomerName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PersonName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "UserName"));
			
			return thisQ;
		}	


		/// <summary>
		/// Returns the Query Part allowing the information about the User to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart UserQueryPart()
		{
			return UserQueryPart(false);
		}		
		/// <summary>
		/// Returns the Query Part allowing the information about the User to be retrieved
		/// as part of a query
		/// </summary>
		/// <returns>clsQueryPart</returns>
		public clsQueryPart UserQueryPart(bool forDistinctQueries)
		{
			clsQueryPart thisQ = new clsQueryPart();
			
			string stem = "";
			string table = "tblUser";
			string pk = "UserId";

			
			thisQ.AddFromTable(table);
			

			if (thisTable == table)
			{
				if (forDistinctQueries)
					thisQ.AddSelectColumn("Distinct " + AddStem(table, stem, pk));
				else
					thisQ.AddSelectColumn(AddStem(table, stem, pk));
			}
			else
			{
				stem = "User_";
				thisQ.AddJoin(thisTable + "." + pk +" = " + table + "." + pk);
			}

			thisQ.AddSelectColumn(AddStem(table, stem, "UserId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "PersonId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "FirstName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "LastName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "UserName"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Password"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Email"));
			thisQ.AddSelectColumn(AddStem(table, stem, "AccessLevel"));
			thisQ.AddSelectColumn(AddStem(table, stem, "DateLastLoggedIn"));
			thisQ.AddSelectColumn(AddStem(table, stem, "ChangeDataId"));
			thisQ.AddSelectColumn(AddStem(table, stem, "Archive"));
			
			thisQ.AddFromTable("");

			return thisQ;
		}

		# endregion

		# region Name Look Ups


		/// <summary>
		/// Function that returns field names for phone part fields
		/// </summary>
		/// <param name="PartRequired">Part of the Phone Number required e.g. internationalPrefix </param>
		/// <returns>Full Name of Phone Field</returns>
		public string PhonePartFieldName(phoneNumberPart PartRequired)
		{
			string result = "";

			switch (PartRequired)
			{
				case phoneNumberPart.fullNumber:
					result += "FullNumber";
					break;
				case phoneNumberPart.fullNumberNoSpaces:
					result += "FullNumberNoSpaces";
					break;
				case phoneNumberPart.internationalPrefix:
					result += "InternationalPrefix";
					break;
				case phoneNumberPart.nationalOrMobilePrefix:
					result += "NationalOrMobilePrefix";
					break;
				case phoneNumberPart.mainNumber:
					result += "MainNumber";
					break;
				case phoneNumberPart.extension:
					result += "Extension";
					break;
				case phoneNumberPart.id:
					result += "PhoneNumberId";
					break;
				case phoneNumberPart.description:
					result += "PhoneNumberTypeDescription";
					break;
				default:
					result += "You rangi the part of the number is wrong";
					break;
			}
			return result;

		}

		/// <summary>
		/// Function that returns field names for phone part fields
		/// </summary>
		/// <param name="TypeRequired">Part of the Phone Number required e.g. Daytime Phone</param>
		/// <returns>Full Name of Phone Field</returns>
		public string PhoneTypeFieldName(phoneNumberType TypeRequired)
		{
			string result = "";

			switch (TypeRequired)
			{
				case phoneNumberType.daytimePhone:
					result += "Daytime Phone";
					break;
				case phoneNumberType.daytimeFax:
					result += "Daytime Fax";
					break;
				case phoneNumberType.afterHoursPhone:
					result += "After Hours Phone";
					break;
				case phoneNumberType.afterHoursFax:
					result += "After Hours Fax";
					break;
				case phoneNumberType.mobilePhone:
					result += "Mobile";
					break;
				case phoneNumberType.other:
				default:
					result += "Other";
					break;
			}
			return result;

		}

		/// <summary>
		/// Function that returns field names for address part fields
		/// </summary>
		/// <param name="PartRequired">Part of the Address required e.g. city </param>
		/// <returns>Full Name of Phone Field</returns>
		public string AddressFieldName(addressPart PartRequired)
		{
			string result = "";

			switch (PartRequired)
			{
				case addressPart.fullAddress:
					result = "FullAddress";
					break;
				case addressPart.fullAddressNoReturns:
					result = "FullAddressNoReturns";
					break;
				case addressPart.streetAddress:
					result = "StreetAddress";
					break;
				case addressPart.city:
					result = "CityName";
					break;
				case addressPart.country:
					result = "CountryName";
					break;
				case addressPart.id:
					result = "AddressId";
					break;
				default:
					result += "You rangi the part of the number is wrong";
					break;
			}

			return result;
		}



		# endregion

		# region SQL Helper Methods

		/// <summary>
		/// Configures a Select Entry
		/// </summary>
		/// <param name="table">Table for this field</param>
		/// <param name="stem">Stem to use as prefix for this field</param>
		/// <param name="field">Name of the field</param>
		/// <returns>Configured Entry</returns>
		public string AddStem(string table, string stem, string field)
		{
			return AddStem(table, stem, field, false);
		}
		/// <summary>
		/// Configures a Select Entry
		/// </summary>
		/// <param name="table">Table for this field</param>
		/// <param name="stem">Stem to use as prefix for this field</param>
		/// <param name="field">Name of the field</param>
		/// <param name="ConvertToNvarchar">Whether this is a 'ntext' field and needs to be converted 
		/// e.g. for use in a distinct query</param>
		/// <returns>Configured Entry</returns>
		public string AddStem(string table, string stem, string field, bool ConvertToNvarchar)
		{
			string retVal = table + "." + field;
			
			if (ConvertToNvarchar)
				retVal = "Convert(nvarchar(2000), " + retVal + ")";

			if (stem != "" || ConvertToNvarchar)
				retVal += " as " + stem + field;

			return retVal;
		}

		#endregion

		#region 'Quick' Phone Numbers and Addresses

		/// <summary>
		/// Gets the 'Quick version' of a phone number
		/// </summary>
		/// <param name="InternationalPrefix">International Prefix</param>
		/// <param name="NationalOrMobilePrefix">National/Mobile Prefix</param>
		/// <param name="MainNumber">Main Number</param>
		/// <param name="Extension">Extension</param>
		/// <returns>'Quick version' of a phone number</returns>
		public string GetQuickNumber(string InternationalPrefix,		
			string NationalOrMobilePrefix, 
			string MainNumber, 
			string Extension)
		{
			string retVal = "";
			string seperator = " ";

			if (InternationalPrefix.Trim().Length != 0)
				retVal += InternationalPrefix.Trim() + seperator;

			if (NationalOrMobilePrefix.Trim().Length != 0)
				retVal += NationalOrMobilePrefix.Trim() + seperator;
			
			if (MainNumber.Trim().Length != 0)
				retVal += MainNumber.Trim() + seperator;
			
			if (Extension.Trim().Length != 0)
				retVal += "x" + Extension.Trim();

			return retVal.Trim();
		}



	
		/// <summary>
		/// Gets the 'Quick version' of an address
		/// </summary>
		/// <param name="POBoxType">Whether this Address includes a PO Box or not</param>
		/// <param name="BuildingName">BuildingName</param>
		/// <param name="UnitNumber">UnitNumber e.g Flat 1</param>
		/// <param name="Number">Number</param>
		/// <param name="StreetAddress">Street Address</param>
		/// <param name="Suburb">Suburb</param>
		/// <param name="PostCode">Post Code</param>
		/// <param name="CityName">CityName</param>
		/// <param name="StateName">StateName</param>
		/// <param name="CountryName">CountryName</param>
		/// <returns>'Quick version' of an address</returns>
		public string GetQuickAddress(int POBoxType,
			string BuildingName,
			string UnitNumber,
			string Number,
			string StreetAddress,		
			string Suburb,		
			string PostCode, 
			string CityName, 
			string StateName, 
			string CountryName)
		{

			string retVal = "";
			string seperator = crLf;

			BuildingName = BuildingName.Trim();
			UnitNumber = UnitNumber.Trim();
			Number = Number.Trim();
			StreetAddress = StreetAddress.Trim();
			Suburb = Suburb.Trim();
			PostCode = PostCode.Trim();
			CityName = CityName.Trim();
			StateName = StateName.Trim();
			CountryName = CountryName.Trim();

			switch ((poBoxType) POBoxType)
			{
				case poBoxType.poBox:
					retVal += "PO Box " + Number.Trim() + seperator;
					break;
				case poBoxType.privateBag:
					retVal += "Private Bag " + Number.Trim() + seperator;
					break;
				case poBoxType.normal:
				default:
					if (BuildingName != "")
						retVal += BuildingName + seperator;

					if (UnitNumber != "") 
					{
						if (Number != "") 
						{
							//							retVal += (UnitNumber + ", " + Number + " " + StreetAddress).Trim() + seperator;
							retVal += ("Unit " + UnitNumber + ", " + Number + " " + StreetAddress).Trim() + seperator;
						}
						else
						{
							retVal += (UnitNumber + " " + StreetAddress).Trim() + seperator;
						}
					}
					else
						retVal += (Number + " " + StreetAddress).Trim() + seperator;
					break;
			}

			if (Suburb.Length != 0)
				retVal += Suburb + seperator;

			if (StateName.Length != 0)
			{
				if (CityName.Length != 0)
					retVal += CityName + seperator;

				retVal += (StateName + " " + PostCode).Trim() + seperator;

			}
			else
			{
				retVal += (CityName + " " + PostCode).Trim() + seperator;
			}

			if (CountryName.Length != 0)
				retVal += CountryName + seperator;

			//If there is just a country, get rid of it
			if (retVal.Trim() == CountryName.Trim())
				retVal = "";

			return retVal.Trim();
		}


		#endregion

		#region Make PDFs

		/// <summary>
		/// Returns whether Background colour will be printed
		/// </summary>
		/// <returns></returns>
		public int BGColour()
		{
			//Type type = Type.GetTypeFromProgID("easyPDF.Printer.5");
			//Printer oPrinter = (Printer)Activator.CreateInstance(type);

			//IEPrintJob oPrintJob = oPrinter.IEPrintJob;
			//IESetting oIESetting = oPrintJob.IESetting;

			//if (oIESetting.PrintBGColor == true)
			//	return 1;
			//else 
			//	return 0;

			return 0;
		}

		/// <summary>
		/// Attempts to make a PDF given the supplied data
		/// </summary>
		/// <param name="Html">Html Data to be converted to PDF, as a string</param>
		/// <param name="HeaderImage">Path and File of Header Image, if there is one</param>
		/// <param name="FooterImage">Path and File of Footer Image, if there is one</param>
		/// <param name="OutputPdfPathFile">Path and File of Output PDF</param>
		/// <param name="TopMargin">Top Margin of the page</param>
		/// <param name="BottomMargin">Bottom Margin of the page</param>
		/// <param name="LeftMargin">Left Margin of the page</param>
		/// <param name="RightMargin">Right Margin of the page</param>
		/// <returns>Success or otherwise of the operation</returns>
		public bool MakePDF(string Html, 
			string HeaderImage, 
			string FooterImage, 
			string OutputPdfPathFile, 
			double TopMargin, 
			double BottomMargin, 
			double LeftMargin, 
			double RightMargin)
		{
			//Turn Printer off while doing this

			//Start of Convert Function
			bool success = false;

//			Type loaderType = Type.GetTypeFromProgID("easyPDF.Loader.5");

//			Loader oLoader = (Loader) Activator.CreateInstance(loaderType);

//			Printer oPrinter = (Printer) oLoader.LoadObject("easyPDF.Printer.5");

//			IEPrintJob oPrintJob;
//			PDFSetting oPDFSetting;
//			IESetting oIESetting;

//			oPrintJob = null;
//			oPDFSetting = null;
//			oIESetting = null;

//			try
//			{
//				oPrintJob = oPrinter.IEPrintJob;

//				oIESetting = oPrintJob.IESetting;
//				oPDFSetting = oPrintJob.PDFSetting;			

//				//Add Header and Footer
//				if (HeaderImage != "")
//				{
//					oPDFSetting.set_Stamp(0, true);
//					oPDFSetting.set_StampFileName(0, HeaderImage);
//					oPDFSetting.set_StampFirstPageOnly(0, false);
//					oPDFSetting.set_StampHPosition(0, 0);
//					oPDFSetting.set_StampVPosition(0, 0);
//				}

//				if (FooterImage != "")
//				{
//					oPDFSetting.set_Stamp(1, true);
//					oPDFSetting.set_StampFileName(1, FooterImage);
//					oPDFSetting.set_StampFirstPageOnly(1, false);
//					oPDFSetting.set_StampHPosition(1, 0);
//					oPDFSetting.set_StampVPosition(1, BCL.easyPDF6.Interop.EasyPDFPrinter.prnStampVPosition.PRN_STAMP_VPOS_BOTTOM);
//				}

////				double BottomMargin = 1.40;
////				double TopMargin = 0.5;
////				oIESetting.MarginLeft = 0;
////				oIESetting.MarginRight = 0;

//				//IE Print Settings
//				oIESetting.Footer = "";
//				oIESetting.Header = "";
//				oIESetting.MarginBottom = BottomMargin;
//				oIESetting.MarginLeft = LeftMargin;
//				oIESetting.MarginRight = RightMargin;
//				oIESetting.MarginTop = TopMargin;
//				oIESetting.PrintBGColor = true;
//				oIESetting.Save();


//				//PDF Meta Data
//				oPDFSetting.MetaData = true;
//				oPDFSetting.MetaDataAuthor = "Graham Mann";
//				oPDFSetting.MetaDataCreator = "Welman Tachnologies Ltd";
//				oPDFSetting.MetaDataKeywords = "PDF";
//				oPDFSetting.MetaDataSubject = "KDL Correspondence";
//				oPDFSetting.MetaDataTitle = "Test";

//				Encoding ascii = Encoding.ASCII;
				
//				byte[] theseBytes = ascii.GetBytes(Html.TrimEnd());

//				byte[] oPDF = (byte[]) oPrintJob.PrintOut3(theseBytes, "HTML");
				
//				FileStream dataFileStream = new FileStream(OutputPdfPathFile, FileMode.Create, FileAccess.Write);
//				BinaryWriter binWriter = new BinaryWriter(dataFileStream, ascii);

//				binWriter.Write(oPDF);
//				binWriter.Close();
//				dataFileStream.Close();
//				success = true;
//			}
//			catch(System.Runtime.InteropServices.COMException err)
//			{
////				MessageBox.Show(err.Message);

//				if(err.ErrorCode == (int)prnResult.PRN_R_CONVERSION_FAILED && oPrintJob != null)
//				{
////					MessageBox.Show(oPrintJob.ConversionResultMessage);

//					prnConversionResult result = oPrintJob.ConversionResult;
//					if(result == prnConversionResult.PRN_CR_CONVERSION || result == prnConversionResult.PRN_CR_CONVERSION_INIT ||result == prnConversionResult.PRN_CR_CONVERSION_PRINT)
//					{
////						MessageBox.Show(oPrintJob.PrinterResultMessage);
//					}
//				}
//			}
			return success;

		}


		#endregion 

		#region MergePDFs

		/// <summary>
		/// MergePDFs
		/// </summary>
		public void MergePDFs()
		{
			//Type loaderType = Type.GetTypeFromProgID("easyPDF.Loader.6");

			//Loader oLoader = (Loader) Activator.CreateInstance(loaderType);

			//Printer oPrinter = (Printer) oLoader.LoadObject("easyPDF.Printer.6");
		}

		/// <summary>
		/// Arraylist used for the purposes of merging files
		/// </summary>
		public ArrayList filesToMerge = new ArrayList();

		/// <summary>
		/// Add a file to the internal list of filesToMerge
		/// </summary>
		/// <param name="fileName">fileName</param>
		public void AddFileToMerge(string fileName)
		{
			filesToMerge.Add(fileName);
		}


		/// <summary>
		/// Merge Files using internal Arraylist
		/// </summary>
		/// <param name="outfileName">outfilename</param>
		/// <param name="DeleteComponentFiles">DeleteComponentFiles</param>
		/// <returns>Success (empty string) or error string (failure)</returns>
		public string MergeFiles(string outfileName, int DeleteComponentFiles)
		{
			return Merge_File(filesToMerge, outfileName, DeleteComponentFiles);
		}

		/// <summary>
		/// Merge Files from an Arraylist
		/// </summary>
		/// <param name="files">Arraylist</param>
		/// <param name="DeleteComponentFiles">DeleteComponentFiles</param>
		/// <param name="outFileName">Final file Name</param>
		/// <returns>Success (empty string) or error string (failure)</returns>
		public string Merge_File(ArrayList files, string outFileName, int DeleteComponentFiles)
		{
			try
			{
				Type type = Type.GetTypeFromProgID("easyPDF.PDFProcessor.6");

				//PDFProcessor oPDFProcessor = (PDFProcessor)Activator.CreateInstance(type);

				//// set the first select file as the first file to merge, then loop the rest
				//// of the files which are concatenated one by one


				//string firstFile = files[0].ToString();
				//for(int counter = 1; counter < files.Count ; counter++)
				//{
				//	oPDFProcessor.Merge(firstFile, files[counter].ToString(), outFileName);				
				//	firstFile = outFileName;
				//}
				
					
//				MessageBox.Show(files.Length.ToString() + " files merged!");
			}
			catch(System.Runtime.InteropServices.COMException e)
			{
				return e.Message + " (" + e.ErrorCode.ToString() + ")";
//				MessageBox.Show(e.Message + " (" + e.ErrorCode.ToString() + ")");
			}

			if (DeleteComponentFiles > 0)
			{

				foreach(object thisFile in files)
				{
					try
					{
						File.Delete(thisFile.ToString());
					}
					catch 
					{
					}
				}
			}

			return "";

		}


		#endregion

		#region WelmanDateTime

		/// <summary>
		/// Returns the Welman Date from DateTime
		/// </summary>
		/// <param name="thisDate">thisDate</param>
		/// <returns>Welman Date</returns>
		public string WelmanDateFromIndex(DateTime thisDate)
		{


			string month = "01";
			string day = "01";

			switch (thisDate.Month)
			{
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
					month = "0" + thisDate.Month.ToString().Trim();
					break;
				default:
					month = thisDate.Month.ToString().Trim();
					break;
			}

			switch (thisDate.Day)
			{
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
					day = "0" + thisDate.Day.ToString().Trim();
					break;
				default:
					day = thisDate.Day.ToString().Trim();
					break;
			}
				
			return thisDate.Year.ToString().Trim() + month + day;

		}

		/// <summary>
		/// Returns the Welman Date from DateTime
		/// </summary>
		/// <param name="thisDate">thisDate</param>
		/// <returns>Welman Date</returns>
		public string WelmanDateTime(DateTime thisDate)
		{

			string WelmanDay = WelmanDateFromIndex(thisDate);

			string hour = "01";
			string minute = "01";
			string second = "01";

			switch (thisDate.Hour)
			{
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
					hour = "0" + thisDate.Hour.ToString().Trim();
					break;
				default:
					hour = thisDate.Hour.ToString().Trim();
					break;
			}

			switch (thisDate.Minute)
			{
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
					minute = "0" + thisDate.Minute.ToString().Trim();
					break;
				default:
					minute = thisDate.Minute.ToString().Trim();
					break;
			}

			switch (thisDate.Second)
			{
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
					second = "0" + thisDate.Second.ToString().Trim();
					break;
				default:
					second = thisDate.Second.ToString().Trim();
					break;
			}
				
			return WelmanDay + "-" + hour + minute + second;

		}

		#endregion

        #region ArrayList <--> DelimitedList

        /// <summary>
        /// DelimitedListToArrayList
        /// </summary>
        /// <param name="DelimitedList">DelimitedList</param>
        /// <returns>System.Collections.ArrayList</returns>
        public System.Collections.ArrayList DelimitedListToArrayList(string DelimitedList)
        {
            return DelimitedListToArrayList(DelimitedList, ";");

        }

        /// <summary>
        /// DelimitedListToArrayList
        /// </summary>
        /// <param name="DelimitedList">DelimitedList</param>
        /// <param name="Delimiter">Delimiter</param>
        /// <returns>System.Collections.ArrayList</returns>
        public System.Collections.ArrayList DelimitedListToArrayList(string DelimitedList, string Delimiter)
        {
            System.Collections.ArrayList retVal = new System.Collections.ArrayList();
            int delimiterIndex = DelimitedList.IndexOf(Delimiter);
            int lenDelimiter = Delimiter.Length;

            while (delimiterIndex > -1)
            {
                string thisValue = DelimitedList.Substring(0, delimiterIndex);

                retVal.Add(thisValue);
                DelimitedList = DelimitedList.Substring(delimiterIndex + lenDelimiter);
                delimiterIndex = DelimitedList.IndexOf(Delimiter);
            }

            return retVal;
        }

        /// <summary>
        /// Takes an ArrayList and returns a Delimited List
        /// </summary>
        /// <param name="thisList">ArrayList</param>
        /// <returns>Delimited List</returns>
        public string ArrayListToDelimitedList(System.Collections.ArrayList thisList)
        {
            return ArrayListToDelimitedList(thisList, ";");

        }

        /// <summary>
        /// Takes an ArrayList and returns a Delimited List
        /// </summary>
        /// <param name="thisList">ArrayList</param>
        /// <param name="Delimiter">Delimiter</param>
        /// <returns>Delimited List</returns>
        public string ArrayListToDelimitedList(System.Collections.ArrayList thisList, string Delimiter)
        {
            string thisRetVal = "";
            if (thisList == null)
                return thisRetVal;

            for (int counter = 0; counter < thisList.Count; counter++)
                thisRetVal += (thisList[counter]).ToString() + Delimiter;

            return thisRetVal;
        }

        #endregion

        #region Method for Sending Email

        #region Main Method

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="delimitedListOfToAddress">Destination Email Address(s)</param>
        /// <param name="FromAddress">Who the email appears from</param>
        /// <param name="delimitedListOfReplyToAddress">Who gets the email if the recipient presses reply</param>
        /// <param name="Subject">Subject Line</param>
        /// <param name="Priority">Priority: 0 = Normal; 1 = High; 2 = Low</param>
        /// <param name="Format">Formal: 0 = Text; 1 = HTML</param>
        /// <param name="Message">Message</param>
        /// <param name="SmtpServer">SmtpServer</param>
        /// <param name="Attachments">Attachments</param>
        /// <param name="thisLogFileStem">thisLogFileStem</param>
        public void SendEmail(
            string delimitedListOfToAddress,
            string FromAddress,
            string delimitedListOfReplyToAddress,
            string Subject,
            int Priority,
            int Format,
            string Message,
            string SmtpServer,
            System.Collections.ArrayList Attachments,
            string thisLogFileStem)
        {


            #region Initialise

            bool success = true;
            string Error = "";

            DateTime startDateTime = DateTime.Now;

            System.Net.Mail.SmtpClient thisClient = new SmtpClient(SmtpServer);

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

            string delimiter = ",";

            #endregion

            #region Prepare and Send email

            try
            {

                #region Prepare email

                if (!delimitedListOfToAddress.EndsWith(delimiter))
                    delimitedListOfToAddress += delimiter;

                System.Collections.ArrayList thisArrayListOfToAddress = DelimitedListToArrayList(delimitedListOfToAddress, delimiter);

                for (int counter = 0; counter < thisArrayListOfToAddress.Count; counter++)
                    mail.To.Add(thisArrayListOfToAddress[counter].ToString());

                int FromAddressLength = FromAddress.Length;
                if (FromAddress.EndsWith(EmailAddressDelimiter) && FromAddressLength > 1)
                    FromAddress = FromAddress.Substring(0, FromAddressLength - 1).Trim();

                mail.From = new System.Net.Mail.MailAddress(FromAddress);

                if (!delimitedListOfReplyToAddress.EndsWith(delimiter))
                    delimitedListOfReplyToAddress += delimiter;

                System.Collections.ArrayList thisArrayListOfReplyToAddress = DelimitedListToArrayList(delimitedListOfReplyToAddress, delimiter);

                for (int counter = 0; counter < thisArrayListOfReplyToAddress.Count; counter++)
                    mail.ReplyToList.Add(thisArrayListOfReplyToAddress[counter].ToString());

                mail.Subject = Subject;
                mail.Priority = (System.Net.Mail.MailPriority)Priority;

                if (Format == 1)
                    mail.IsBodyHtml = true;
                else
                    mail.IsBodyHtml = false;

                //mail.BodyFormat = (System.Net.Mail.MailFormat) Format;
                mail.Body = Message;

                if (Attachments != null && Attachments.Count > 0)
                {
                    for (int counter = 0; counter < Attachments.Count; counter++)
                    {
                        string thisFile = Attachments[counter].ToString();
                        if (System.IO.File.Exists(thisFile))
                        {
                            System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(thisFile);
                            mail.Attachments.Add(attach);
                        }
                    }
                }

                #endregion

                #region Send email

                thisClient.Send(mail);
                success = true;

                #endregion

            }
            catch (System.SystemException e)
            {
                #region Deal with errors

                string Response = e.ToString();
                int RaisedError = 901;

                Error = "Error Message: " + e.Message + crLf
                    + "Stack Trace: " + e.StackTrace + crLf
                    + "ThisSmtpServer: " + thisSmtpServer + crLf
                    + "Mail: " + mail.ToString() + crLf
                    + "Whole exception: " + e.ToString() + crLf
                    + "delimitedListOfToAddress: " + delimitedListOfToAddress.ToString() + crLf
                    + "FromAddress: " + FromAddress.ToString() + crLf
                    + "delimitedListOfReplyToAddress: " + delimitedListOfReplyToAddress.ToString() + crLf
                    + "Subject: " + Subject.ToString() + crLf
                    + "Priority: " + Priority.ToString() + crLf
                    + "Format: " + Format.ToString() + crLf
                    + "Message: " + Message.ToString() + crLf
                    + "SmtpServer: " + SmtpServer.ToString() + crLf
                    + "thisLogFileStem: " + thisLogFileStem.ToString()
                    ;

                success = false;

                LogError(SmtpServer, Error, RaisedError);

                #endregion
            }

            #endregion

            #region Log Email

            DateTime thisDateTime = DateTime.Now;

            string thisLogFile = thisLogFileStem;

            if (success)
                thisLogFile += "Success";
            else
                thisLogFile += "Failure";

            thisLogFile += "Email"
                + GetLogFileDate(DateTime.Now)
                + GetComputerNameAndProcessId()
                + ".csv";

            clsCsvWriter fileWriter = new clsCsvWriter(thisLogFile, true);

            string theseAttachments = ArrayListToDelimitedList(Attachments);

            string[] logFileData = new string[] { 
                "SendEmail",
                thisDateTime.ToString(), 
                thisDateTime.Subtract(startDateTime).TotalMilliseconds.ToString(), 
                delimitedListOfToAddress,
                FromAddress,
                delimitedListOfReplyToAddress,
                Subject,
                Priority.ToString(),
                Format.ToString(),
                Message,
                SmtpServer,
                theseAttachments,
                Error
			};

            fileWriter.WriteFields(logFileData);
            fileWriter.Close();


            #endregion

        }

        #endregion

        #region Overrides

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="delimitedListOfToAddress">Destination Email Address(s)</param>
        /// <param name="FromAddress">Who the email appears from</param>
        /// <param name="delimitedListOfReplyToAddress">Who gets the email if the recipient presses reply</param>
        /// <param name="Subject">Subject Line</param>
        /// <param name="Priority">Priority: 0 = Normal; 1 = High; 2 = Low</param>
        /// <param name="Format">Formal: 0 = Text; 1 = HTML</param>
        /// <param name="Message">Message</param>
        /// <param name="SmtpServer">SmtpServer</param>
        /// <param name="Attachments">Attachments</param>
        public void SendEmail(
            string delimitedListOfToAddress,
            string FromAddress,
            string delimitedListOfReplyToAddress,
            string Subject,
            int Priority,
            int Format,
            string Message,
            string SmtpServer,
            System.Collections.ArrayList Attachments)
        {

            if (logFileStem == "")
                GetGeneralSettings();

            string thisLogFileStem = logFileStem.Replace("OBSTPU", "Email");

            if (!Directory.Exists(thisLogFileStem))
                Directory.CreateDirectory(thisLogFileStem);

            SendEmail(delimitedListOfToAddress,
                FromAddress,
                delimitedListOfReplyToAddress,
                Subject,
                Priority,
                Format,
                Message,
                thisSmtpServer,
                Attachments,
                thisLogFileStem);

        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="ToAddress">Destination Email Address</param>
        /// <param name="FromAddress">Who the email appears from</param>
        /// <param name="ReplyToAddress">Who gets the email if the recipient presses reply</param>
        /// <param name="Subject">Subject Line</param>
        /// <param name="Priority">Priority: 0 = Normal; 1 = High; 2 = Low</param>
        /// <param name="Format">Formal: 0 = Text; 1 = HTML</param>
        /// <param name="Message">Message</param>
        /// <param name="Attachments">Attachments</param>
        public void SendEmail(
            string ToAddress,
            string FromAddress,
            string ReplyToAddress,
            string Subject,
            int Priority,
            int Format,
            string Message,
            System.Collections.ArrayList Attachments)
        {
            GetGeneralSettings();

            if (thisSmtpServer == "")
            {
                Setting.Ensure("thisSmtpServer", "localHost");
                GetGeneralSettings();

            }

            SendEmail(
                ToAddress,
                FromAddress,
                ReplyToAddress,
                Subject,
                Priority,
                Format,
                Message,
                thisSmtpServer,
                Attachments);

        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="ToAddress">Destination Email Address</param>
        /// <param name="FromAddress">Who the email appears from</param>
        /// <param name="ReplyToAddress">Who gets the email if the recipient presses reply</param>
        /// <param name="Subject">Subject Line</param>
        /// <param name="Priority">Priority: 0 = Normal; 1 = High; 2 = Low</param>
        /// <param name="Format">Formal: 0 = Text; 1 = HTML</param>
        /// <param name="Message">Message</param>
        public void SendEmail(
            string ToAddress,
            string FromAddress,
            string ReplyToAddress,
            string Subject,
            int Priority,
            int Format,
            string Message)
        {
            SendEmail(ToAddress,
                FromAddress,
                ReplyToAddress,
                Subject,
                Priority,
                Format,
                Message,
                null);

        }

        #endregion

        #endregion

        #region Error Logging
        /// <summary>
        /// Log an Error to the Event Log
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Event">Error</param>
        /// <param name="ErrorId">ErrorId</param>
        public void LogError(string Source, string Event, int ErrorId)
        {
            try
            {
                string Log = "Application";
                EventLogEntryType errorType = EventLogEntryType.Warning;

                int MaxSourceLength = 212;
                string thisSource = Source;

                if (Source.Length > MaxSourceLength)
                {
                    Source = Source.Substring(0, MaxSourceLength);

                    Event = Source + crLf + Event;

                }

                if (!System.Diagnostics.EventLog.SourceExists(Source))
                    System.Diagnostics.EventLog.CreateEventSource(Source, Log);

                System.Diagnostics.EventLog.WriteEntry(Source, Event, errorType, ErrorId);
            }
            catch
            {
            }

        }
        #endregion

        #region GetLogFileDate

        /// <summary>
        /// Returns the date for the purposes of log files
        /// </summary>
        /// <returns>The date for the purposes of log files</returns>
        public string GetLogFileDate(DateTime ThisProcessStart)
        {
            string thisDate = ThisProcessStart.Year.ToString().Trim();

            thisDate += ".";

            if (ThisProcessStart.Month < 10)
                thisDate += "0";

            thisDate += ThisProcessStart.Month.ToString().Trim();
            thisDate += ".";

            if (ThisProcessStart.Day < 10)
                thisDate += "0";
            thisDate += ThisProcessStart.Day.ToString().Trim();

            thisDate += "-";

            if (ThisProcessStart.Hour < 10)
                thisDate += "0";
            thisDate += ThisProcessStart.Hour.ToString().Trim();
            thisDate += ".";

            if (ThisProcessStart.Minute < 10)
                thisDate += "0";
            thisDate += ThisProcessStart.Minute.ToString().Trim();
            thisDate += ".";

            if (ThisProcessStart.Second < 10)
                thisDate += "0";
            thisDate += ThisProcessStart.Second.ToString().Trim();
            thisDate += ".";

            if (ThisProcessStart.Millisecond < 100)
            {
                thisDate += "0";
                if (ThisProcessStart.Millisecond < 10)
                    thisDate += "0";
            }

            thisDate += ThisProcessStart.Millisecond.ToString().Trim();

            return thisDate;
        }

        #endregion

        #region Make Safe for file Names

        /// <summary>
        /// MakeSafeForFileName
        /// </summary>
        /// <param name="original">original</param>
        /// <returns>Safe File Name</returns>
        public string MakeSafeForFileName(string original)
        {
            string result = original;
            result = result.Replace(@"?", "");
            result = result.Replace(@"[", "");
            result = result.Replace(@"]", "");
            result = result.Replace(@"/", "");
            result = result.Replace(@"\", "");
            result = result.Replace(@"=", "");
            result = result.Replace(@"+", "");
            result = result.Replace(@"<", "");
            result = result.Replace(@">", "");
            result = result.Replace(@":", "");
            result = result.Replace(@";", "");
            result = result.Replace(((char)34).ToString(), "");
            result = result.Replace(@",", "");
            result = result.Replace(@"*", "");

            return result;

        }


        #endregion

        #region Computer Name and Process

        /// <summary>
        /// GetComputerNameAndProcessId
        /// </summary>
        /// <returns>ComputerNameAndProcessId</returns>
        public string GetComputerNameAndProcessId()
        {

            int thisProcessId = Process.GetCurrentProcess().Id;
            string thisComputerName = System.Environment.MachineName;

            string thisComputerName_ProcessId = thisComputerName + "_" + thisProcessId.ToString();

            return thisComputerName_ProcessId;
        }

        #endregion

		#region Universal Get Bys
		/// <summary>
		/// Gets all changes to this record
		/// </summary>
		/// <param name="pkId">Id of record to retrieve changes for</param>
		/// <returns>Number of resulting records</returns>
		public int GetByPrimaryKey(int pkId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = " + pkId.ToString();

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(baseQueries, 
				OrderByColumns,
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += "Order By " + thisTable + "." + thisPk;

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets all changes to this record
		/// </summary>
		/// <param name="pkId">Id of record to retrieve changes for</param>
		/// <returns>Number of resulting records</returns>
		public int GetAllChanges(int pkId)
		{
			clsQueryBuilder QB = new clsQueryBuilder();

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + thisPk + " = " + pkId.ToString() + " or "
				+ thisTable + ".Archive = " + pkId.ToString();

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(baseQueries, 
				OrderByColumns,
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += "Order By " + thisTable + ".Archive, " + thisTable + "." + thisPk + " desc";

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets the record in this table with this Primary Key at this time
		/// </summary>
		/// <param name="pkId">Primary Key of Record</param>
		/// <param name="AtDateTime">DateTime of interest</param>
		/// <param name="ShowRemovedRecords">Whether to show the record if it is removed</param>
		/// <returns>Number of Results returned</returns>
		public virtual int GetHistoryByPrimaryKey(int pkId, string AtDateTime, int ShowRemovedRecords)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			
			string thisDateTime = localRecords.DBDateTime(Convert.ToDateTime(AtDateTime));

			string thisCriteria = thisTable + "." + thisPk + " = " + pkId.ToString() + crLf;

			int numQueryParts = baseQueries.GetUpperBound(0)+ 2;
			
			clsQueryPart[] theseQueries = new clsQueryPart[numQueryParts];
			for(int counter = 0; counter < numQueryParts - 1; counter ++)
				theseQueries[counter] = baseQueries[counter];

			theseQueries[numQueryParts-1] = HistoryQueryPart(thisDateTime, thisCriteria);

			string condition = "(Select * from " + thisTable 
				+ " Where " +  thisTable + "." + thisPk + " = " 
				+ pkId.ToString()  + crLf;

			if (ShowRemovedRecords == 0)
				condition += " And " + thisTable + ".Archive != -1" + crLf;


			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(theseQueries, 
				OrderByColumns,
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += "Order By tblChangeData.DateChanged desc";

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets all changes to all records in this table with this Foreign Key
		/// </summary>
		/// <param name="fkId">Foreign Key of Record</param>
		/// <param name="foreignKeyName">Name of Foreign Key of Interest</param>
		/// <returns>Number of resulting records</returns>
		public virtual int GetAllChangesByForeignKey(int fkId, string foreignKeyName)
		{
			return GetAllChangesByForeignKeyWithConstraint(fkId, foreignKeyName, "", "");
		}


		/// <summary>
		/// Gets all changes to all records in this table with this Foreign Key
		/// </summary>
		/// <param name="fkId">Foreign Key of Record</param>
		/// <param name="foreignKeyName">Name of Foreign Key of Interest</param>
		/// <param name="ConstraintField">If there is an additional Constraint, the name of the field that is constrained</param>
		/// <param name="ConstraintFieldValue">If there is an additional Constraint, the value that the constrainted field that is constrained to</param>
		/// <returns>Number of resulting records</returns>
		public virtual int GetAllChangesByForeignKeyWithConstraint(int fkId, string foreignKeyName, string ConstraintField, string ConstraintFieldValue)
		{
			clsQueryBuilder QB = new clsQueryBuilder();

			string condition = "(Select * from " + thisTable 
				+ " Where " + thisTable + "." + foreignKeyName 
				+ " = " + fkId.ToString() + crLf;

			int numQueryParts = baseQueries.GetUpperBound(0);

			if (ConstraintField != "")
				baseQueries[numQueryParts-1].AddJoin(ConstraintField + MatchCondition(ConstraintFieldValue, matchCriteria.exactMatch));
			//				condition += "And " + ConstraintField + " " + MatchCondition(ConstraintFieldValue, matchCriteria.exactMatch) + crLf;

			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(baseQueries, 
				OrderByColumns,
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += "Order By tblChangeData.DateChanged desc";

			return localRecords.GetRecords(thisSqlQuery);
		}

		/// <summary>
		/// Gets the records in this table with this Foreign Key at this time
		/// </summary>
		/// <param name="fkId">Foreign Key of Record</param>
		/// <param name="AtDateTime">DateTime of interest</param>
		/// <param name="foreignKeyName">Name of Foreign Key of Interest</param>
		/// <param name="ShowRemovedRecords">Whether to show records that have been removed</param>
		/// <param name="ConstraintField">If there is an additional Constraint, the name of the field that is constrained</param>
		/// <param name="ConstraintFieldValue">If there is an additional Constraint, the value that the constrainted field that is constrained to</param>
		/// <returns>Number of Results returned</returns>
		public virtual int GetHistoryByForeignKeyWithConstraint(int fkId, string foreignKeyName, string AtDateTime, int ShowRemovedRecords, string ConstraintField, string ConstraintFieldValue)
		{
			clsQueryBuilder QB = new clsQueryBuilder();
			
			string thisDateTime = localRecords.DBDateTime(Convert.ToDateTime(AtDateTime));

			string thisCriteria = thisTable + "." + foreignKeyName + " = " + fkId.ToString() + crLf;

			int numQueryParts = baseQueries.GetUpperBound(0)+2;
			
			clsQueryPart[] theseQueries = new clsQueryPart[numQueryParts];
			for(int counter = 0; counter < numQueryParts - 1; counter ++)
				theseQueries[counter] = baseQueries[counter];

			theseQueries[numQueryParts-1] = HistoryQueryPart(thisDateTime, thisCriteria);

			string condition = "(Select * from " + thisTable 
				+ " Where " +  thisTable + "." + foreignKeyName + " = " 
				+ fkId.ToString() + crLf;

			if (ConstraintField != "")
				theseQueries[numQueryParts-1].AddJoin(ConstraintField + MatchCondition(ConstraintFieldValue, matchCriteria.exactMatch));
			//				condition += " And " +  + crLf;


			if (ShowRemovedRecords == 0)
				condition += " And " + thisTable + ".Archive != -1" + crLf;


			condition += ") " + thisTable;

			thisSqlQuery = QB.BuildSqlStatement(theseQueries, 
				OrderByColumns,
				condition,
				thisTable
				);

			//Ordering
			if (OrderByColumns.Count == 0)
				thisSqlQuery += "Order By tblChangeData.DateChanged desc";

			return localRecords.GetRecords(thisSqlQuery);
		}


		#endregion

		#region Universal My_ Values

		/// <summary>
		/// Whether this record is Archived or not
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>If this record is Archived</returns>
		public int my_Archive(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "Archive")); 
		}

		/// <summary>
		/// Id of Change Data
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>ChangeDataId</returns>
		public int my_ChangeDataId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ChangeDataId"));
		}

		/// <summary>
		/// Whether this record is ChangeDataId or not
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns>ChangeDataId</returns>
		public int my_PrimaryKeyId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, thisPk));
		}


	
		# region My_ Values ChangeData

		/// <summary>
		/// <see cref="clsKeyBase.my_ChangeDataId">Id</see> of
		/// <see cref="clsChangeData">ChangeData</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsKeyBase.my_ChangeDataId">Id</see> of 
		/// <see cref="clsChangeData">ChangeData</see> 
		/// </returns>
		public int my_ChangeData_ChangeDataId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ChangeData_ChangeDataId"));
		}

		/// <summary>
		/// <see cref="clsChangeData.my_CreatedByUserId">CreatedByUserId</see> of
		/// <see cref="clsChangeData">ChangeData</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsChangeData.my_CreatedByUserId">CreatedByUserId</see> of 
		/// <see cref="clsChangeData">ChangeData</see> 
		/// </returns>
		public int my_ChangeData_CreatedByUserId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ChangeData_CreatedByUserId"));
		}

		/// <summary>
		/// <see cref="clsChangeData.my_CreatedByFirstNameLastName">CreatedByFirstNameLastName</see> of
		/// <see cref="clsChangeData">ChangeData</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsChangeData.my_CreatedByFirstNameLastName">CreatedByFirstNameLastName</see> of 
		/// <see cref="clsChangeData">ChangeData</see> 
		/// </returns>
		public string my_ChangeData_CreatedByFirstNameLastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ChangeData_CreatedByFirstNameLastName");
		}

		/// <summary>
		/// <see cref="clsChangeData.my_DateCreated">DateCreated</see> of
		/// <see cref="clsChangeData">ChangeData</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsChangeData.my_DateCreated">DateCreated</see> of 
		/// <see cref="clsChangeData">ChangeData</see> 
		/// </returns>
		public string my_ChangeData_DateCreated(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ChangeData_DateCreated");
		}

		/// <summary>
		/// <see cref="clsChangeData.my_ChangedByUserId">ChangedByUserId</see> of
		/// <see cref="clsChangeData">ChangeData</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsChangeData.my_ChangedByUserId">ChangedByUserId</see> of 
		/// <see cref="clsChangeData">ChangeData</see> 
		/// </returns>
		public int my_ChangeData_ChangedByUserId(int rowNum)
		{
			return Convert.ToInt32(localRecords.FieldByName(rowNum, "ChangeData_ChangedByUserId"));
		}

		/// <summary>
		/// <see cref="clsChangeData.my_ChangedByFirstNameLastName">ChangedByFirstNameLastName</see> of
		/// <see cref="clsChangeData">ChangeData</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsChangeData.my_ChangedByFirstNameLastName">ChangedByFirstNameLastName</see> of 
		/// <see cref="clsChangeData">ChangeData</see> 
		/// </returns>
		public string my_ChangeData_ChangedByFirstNameLastName(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ChangeData_ChangedByFirstNameLastName");
		}

		/// <summary>
		/// <see cref="clsChangeData.my_DateChanged">DateChanged</see> of
		/// <see cref="clsChangeData">ChangeData</see>
		/// </summary>
		/// <param name="rowNum">Record Index</param>
		/// <returns><see cref="clsChangeData.my_DateChanged">DateChanged</see> of 
		/// <see cref="clsChangeData">ChangeData</see> 
		/// </returns>
		public string my_ChangeData_DateChanged(int rowNum)
		{
			return localRecords.FieldByName(rowNum, "ChangeData_DateChanged");
		}
		#endregion


		#endregion

		#region Universal AddArchiveConditionIfNecessary Method

		/// <summary>
		/// Adds a archive condition to a SQL Query if it is required.
		/// </summary>
		/// <returns>Archive Condition</returns>
		public string ArchiveConditionIfNecessary(bool AddAnd)
		{
			return ArchiveConditionIfNecessary(AddAnd, thisTable);
		}

		/// <summary>
		/// Adds a archive condition to a SQL Query if it is required.
		/// </summary>
		/// <returns>Archive Condition</returns>
		public string ArchiveConditionIfNecessary(bool AddAnd, string TableToCheck)
		{
			string condition = "";
			
			switch (thisArchivedRecordBehaviour)
			{
				case archivedRecordBehaivour.archivedOnly:
					condition += " " + TableToCheck + ".Archive != 0" + crLf;
					break;
				case archivedRecordBehaivour.unarchivedOnly:
					condition += " " + TableToCheck + ".Archive = 0" + crLf;
					break;
				case archivedRecordBehaivour.unarchivedAndArchived:
				default:
					break;
			}
			
			if (condition != "" && AddAnd)
				condition = " And " + condition;
			
			return condition;
		}

		#endregion

		#region Universal SanitiseDate Function

		/// <summary>
		/// Takes a Date and 'Sanitises it'
		/// </summary>
		/// <param name="DateProvided">Date to Be Sanitised</param>
		/// <returns>Sanitised Date</returns>
		public object SanitiseDate(string DateProvided)
		{
			object retVal;
			if (DateProvided == "")
				retVal = DBNull.Value;
			else
			{
				DateProvided = DateProvided.Trim();
				DateTime thisDate = SanitiseDateAsDate(DateProvided);
				
				retVal = localRecords.DBDateTime(thisDate);
			}

			return retVal;
	
		}

		/// <summary>
		/// Takes a Date and 'Sanitises it'
		/// </summary>
		/// <param name="DateProvided">Date to Be Sanitised</param>
		/// <returns>Sanitised Date</returns>
		public DateTime SanitiseDateAsDate(string DateProvided)
		{
			DateTime thisDate = Convert.ToDateTime(DateProvided);
			if (thisDate.Year < 1000)
				thisDate = thisDate.AddYears(1000);
			if (thisDate.Year < 1100)
				thisDate = thisDate.AddYears(1000);
			while ( thisDate.Year < 1900)
				thisDate = thisDate.AddYears(100);
			while ( thisDate.Year > 2100)
				thisDate = thisDate.AddYears(-100);

			return thisDate;
	
		}

		#endregion

		#region Global Remove Methods

		/// <summary>
		/// Method that Archives an old record
		/// </summary>
		/// <param name="CurrentUser">CurrentUser</param>
		/// <param name="thisPkId">thisPkId</param>
		public virtual int AddArchive(int CurrentUser, int thisPkId)
		{
			return 0;
		}

		/// <summary>
		/// Remove a Record
		/// </summary>
		/// <param name="thisPkId">PK of Record to Remove</param>
		/// <param name="CurrentUser">Id of User doing the Removing</param>
		public virtual void Remove(int thisPkId, int CurrentUser)
		{
			int newChangeDataId = AddArchive(CurrentUser, thisPkId);


			System.Data.DataRow attributeChangeRowToAdd = 
				attributeChangeData.NewRow();
							
			attributeChangeRowToAdd["AttributeChangeId"] = 
				attributeChangeData.Rows.Count + 1;
			attributeChangeRowToAdd["RowChangeId"] = 
				thisPkId;
			attributeChangeRowToAdd["PrimaryTableName"] =
				thisTable;
			attributeChangeRowToAdd["PrimaryRowId"] = 
				thisPkId;
			attributeChangeRowToAdd["SecondaryTableName"] = 
				DBNull.Value;
			attributeChangeRowToAdd["SecondaryRowId"] = 
				DBNull.Value;
			attributeChangeRowToAdd["HasSecondary"] = 
				0;
			attributeChangeRowToAdd["AttributeName"] = 
				"ChangeDataId";

			attributeChangeRowToAdd["OldValue"] = 
				"0";
			attributeChangeRowToAdd["NewValue"] = 
				newChangeDataId.ToString();

			attributeChangeData.Rows.Add(attributeChangeRowToAdd);

			attributeChangeRowToAdd = 
				attributeChangeData.NewRow();

			attributeChangeRowToAdd["AttributeChangeId"] = 
				attributeChangeData.Rows.Count + 1;
			attributeChangeRowToAdd["RowChangeId"] = 
				thisPkId;
			attributeChangeRowToAdd["PrimaryTableName"] =
				thisTable;
			attributeChangeRowToAdd["PrimaryRowId"] = 
				thisPkId;
			attributeChangeRowToAdd["SecondaryTableName"] = 
				DBNull.Value;
			attributeChangeRowToAdd["SecondaryRowId"] = 
				DBNull.Value;
			attributeChangeRowToAdd["HasSecondary"] = 
				0;
			attributeChangeRowToAdd["AttributeName"] = 
				"Archive";
			attributeChangeRowToAdd["OldValue"] = 
				"0";
			attributeChangeRowToAdd["NewValue"] = 
				(-1).ToString();

			attributeChangeData.Rows.Add(attributeChangeRowToAdd);
			
			System.Data.DataRow rowToAdd = dataToBeModified.NewRow();
			rowToAdd[thisPk] = thisPkId;

			dataToBeModified.Rows.Add(rowToAdd);

		}


		/// <summary>
		/// Make a record non-public
		/// </summary>
		/// <param name="thisPkId">Primary Key Id of Record</param>
		public virtual void MakeNotIsPublic(int thisPkId)
		{
			SetAttribute(thisPkId,
				"IsPublic",
				"0");

		}
		#endregion

		#region Random character and number Functions

		Random randomNum = new Random();

		/// <summary>
		/// GetRandomChar (lower case; not I or O)
		/// </summary>
		/// <returns>A Random Char (lower case; not I or O)</returns>
		public string GetRandomChar()
		{

			char thisA = Convert.ToChar("A");
			string thisChar = Convert.ToChar(Convert.ToInt32(thisA) + randomNum.Next(0, 25)).ToString();
			while (thisChar == "I" || thisChar == "O")
				thisChar = Convert.ToChar(Convert.ToInt32(thisA) + randomNum.Next(0, 25)).ToString();

			return thisChar;
		}


		/// <summary>
		/// GetRandomNum
		/// </summary>
		/// <returns>A Random Number (not 1 or 0)</returns>
		public string GetRandomNum()
		{

			int thisInt = randomNum.Next(2, 9);

			return thisInt.ToString().Trim();
		}

		#endregion


	}
}
