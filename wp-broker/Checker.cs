using System;
using System.Net;
using System.Text.RegularExpressions;

namespace wpbroker
{
    public class Checker
    {
		public enum CMS
		{
            WORDPRESS, 
            PRESTASHOP,
            DRUPAL
		}

		public class Conclusion
		{
            private bool _includeName;
			private bool _includeContentFolder;

			public Conclusion(bool includeName) {
				_includeName = includeName;
			}

			public Conclusion(bool includeName, bool includeContentFolder) : this (includeName){
				_includeContentFolder = includeContentFolder;
			}

			public override string ToString()
			{
				return  "Include Name                        : " + _includeName + "\n" +
					    "Include Content Folder (wp-content) : " + _includeContentFolder + "";
			}
		}
			

		private const string WP_PATTERN = "(W|w)ordpress";
		private const string CONTENT_FOLDER_PATTERN = "wp-content";

		private string _webSite;
        
        public Checker(string webSite)
        {
			_webSite = webSite;
        }


		public Conclusion Check(CMS toValidate)
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

		private Conclusion CheckIsWordpress() {
			         
	        WebClient client = new WebClient();
            
			Regex hasWpString = new Regex(WP_PATTERN);
			Regex hasContentFolder = new Regex(CONTENT_FOLDER_PATTERN);
                     
			string downloadString = client.DownloadString(_webSite);
                    
			return new Conclusion(
				hasWpString.IsMatch(downloadString), 
				hasContentFolder.IsMatch(downloadString)
			);
		}

		private Conclusion CheckIsDrupal() { return new Conclusion(false); }

		private Conclusion CheckIsPrestashop() { return new Conclusion(false); }


    }
}
