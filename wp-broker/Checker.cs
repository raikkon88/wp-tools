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
		private const string TEXT_CONTENT_FOLDER_ACCESSIBLE = "Response 200 for directory wp-content";
		private const string TEXT_LOGIN_PATH_ACCESSIBLE = "Response 200 for wp-login.php path";
		private const string TEXT_ADMIN_PATH_ACCESSIBLE = "Response 200 for wp-admin path";
		private const string TEXT_TRACEBACK_PATH_ACCESSIBLE = "Response 200 for wp-traceback.php path";
		private const string TEXT_FEED_PATH_ACCESSIBLE = "Response 200 for feed path";
        
		/* PATTERNS */
		private const string PATTERN_WP = "(W|w)(o|O)(r|R)(d|D)(p|P)(r|R)(e|E)(s|S)(s|S)";
		private const string PATTERN_CONTENT_FOLDER = "wp-content";

        /* OTHER */
		private const string PATH_LICENSE_FILE = "/license.txt";
		private const string PATH_WP_CONTENT_FOLDER = "/wp-content";
		private const string PATH_WP_LOGIN = "/wp-login.php";
		private const string PATH_WP_ADMIN = "/wp-admin";
		private const string PATH_WP_TRACEBACK = "/wp-trackback.php";
		private const string PATH_WP_FEED = "/feed";

        /* PARAMETERS */
		private const int HTTP_REQUEST_TIMEOUT = 5000; // 5 seconds

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
            
            // Verify the wordpress content.
			string downloadString = client.DownloadString(_webSite);
			Regex hasWpString = new Regex(PATTERN_WP);
			Regex hasContentFolder = new Regex(PATTERN_CONTENT_FOLDER);
            // Verify the license file
			string licenseFile = client.DownloadString(_webSite + PATH_LICENSE_FILE);

			GenerateConclusion(TEXT_INCLUDE_WP_NAME, hasWpString.IsMatch(downloadString));
			GenerateConclusion(TEXT_INCLUDE_WP_FOLDER, hasContentFolder.IsMatch(downloadString));
			GenerateConclusion(TEXT_HAS_LICENSE_FILE, hasWpString.IsMatch(licenseFile));
			GenerateConclusion(TEXT_CONTENT_FOLDER_ACCESSIBLE, IsHttpRequest200(_webSite + PATH_WP_CONTENT_FOLDER, HTTP_REQUEST_TIMEOUT));
			GenerateConclusion(TEXT_ADMIN_PATH_ACCESSIBLE, IsHttpRequest200(_webSite + PATH_WP_ADMIN,HTTP_REQUEST_TIMEOUT));
			GenerateConclusion(TEXT_TRACEBACK_PATH_ACCESSIBLE, IsHttpRequest200(_webSite + PATH_WP_TRACEBACK, HTTP_REQUEST_TIMEOUT));
			GenerateConclusion(TEXT_FEED_PATH_ACCESSIBLE, IsHttpRequest200(_webSite + PATH_WP_FEED, HTTP_REQUEST_TIMEOUT));

			return _conclusions.GetEnumerator();
		}

		private void GenerateConclusion(string text, bool result){
			Conclusion conclusion = new Conclusion(text, result);
            Console.WriteLine(conclusion);
            _conclusions.Add(conclusion);
		}

		private bool IsHttpRequest200(string website, int timeout)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(website);
				request.Timeout = timeout;
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				return response.StatusCode == HttpStatusCode.OK;
			}
			catch (Exception e)
			{
				Console.WriteLine("EXCEPTION \n" + e);
				return false;
			}
		}

		private IEnumerator<Conclusion> CheckIsDrupal() { return _conclusions.GetEnumerator(); }

		private IEnumerator<Conclusion> CheckIsPrestashop() { return _conclusions.GetEnumerator(); }


    }
}
