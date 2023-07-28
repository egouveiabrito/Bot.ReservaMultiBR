namespace AutomationTest.Core
{
    public abstract class PageObjectModel
    {
        protected readonly SeleniumHelper _helper;
        protected PageObjectModel(SeleniumHelper helper)
        {
            _helper = helper;
        }
        public string GetUrl()
        {
            return _helper.GetUrl();
        }
        public void NavegateToUrl(string url)
        {
            _helper.GoToUrl(url);
        }
    }
}