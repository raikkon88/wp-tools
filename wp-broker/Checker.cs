using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace wpbroker
{
    public class Checker
    {
		/***********************************************************************************
         * ENUMERATIONS
         ***********************************************************************************/
		public enum CMS
		{
            WORDPRESS, 
            PRESTASHOP,
            DRUPAL
		}

		/***********************************************************************************
         * CONSTANTS 
         ***********************************************************************************
         * TEXT */
		private const string TEXT_INCLUDE_WP_NAME = "Include Name";
		private const string TEXT_INCLUDE_WP_FOLDER = "Include Content Folder (wp-content)";
		private const string TEXT_HAS_LICENSE_FILE = "Has License file";
        
		/* PATTERNS */
		private const string PATTERN_WP = "(W|w)(o|O)(r|R)(d|D)(p|P)(r|R)(e|E)(s|S)(s|S)";
		private const string PATTERN_CONTENT_FOLDER = "wp-content";

        /* OTHER */
		private const string LICENSE_FILE = "/license.txt";

		/***********************************************************************************
         * ATTRIBUTES
         ***********************************************************************************/        
		private string _webSite;
		private List<Conclusion> _conclusions;
        
		/***********************************************************************************
         * CONSTRUCTORS
         ***********************************************************************************/
        public Checker(string webSite)
        {
			_webSite = webSite;
			_conclusions = new List<Conclusion>();
        }

		/***********************************************************************************
         * PUBLIC METHODS 
         ***********************************************************************************/
		public IEnumerator<Conclusion> Check(CMS toValidate)
		{
			if(toValidate == CMS.WORDPRESS){
				return CheckIsWordpress();
			}
			else if(toValidate == CMS.DRUPAL){
				return CheckIsDrupal();
			}
			else{
				return CheckIsPrestashop();
			}
		}

        /***********************************************************************************
		* PRIVATE METHODS 
         ***********************************************************************************/
		private IEnumerator<Conclusion> CheckIsWordpress() {
			         
	        WebClient client = new WebClient();
            
			string downloadString = client.DownloadString(_webSite);
			Regex hasWpString = new Regex(PATTERN_WP);
			Regex hasContentFolder = new Regex(PATTERN_CONTENT_FOLDER);

			string licenseFile = client.DownloadString(_webSite + LICENSE_FILE);
         
			_conclusions.Add(new Conclusion(TEXT_INCLUDE_WP_NAME, hasWpString.IsMatch(downloadString)));
			_conclusions.Add(new Conclusion(TEXT_INCLUDE_WP_FOLDER, hasContentFolder.IsMatch(downloadString)));
			_conclusions.Add(new Conclusion(TEXT_HAS_LICENSE_FILE, hasWpString.IsMatch(licenseFile)));
         
			return _conclusions.GetEnumerator();
		}

		private IEnumerator<Conclusion> CheckIsDrupal() { return _conclusions.GetEnumerator(); }

		private IEnumerator<Conclusion> CheckIsPrestashop() { return _conclusions.GetEnumerator(); }


    }
}
