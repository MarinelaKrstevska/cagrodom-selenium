using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using Expect = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Final
{
    [TestFixture]
    public class Class1
    {
        public IWebDriver Driver;
        public WebDriverWait wait;
        public static Random random=new Random();  

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {

        }

        [SetUp]
        public void Setup()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(25);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(25);
            Driver.Manage().Window.Maximize();

            Driver.Navigate().GoToUrl("http://18.156.17.83:9095/");

            wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));

        }

        [Test]
        public void Test()
        {
            // Login as user
            login("krstevska.marinela@gmail.com", "pavel22@");
            

            // createRequest
            IWebElement createRequestButton = Driver.FindElement(By.CssSelector("a[ui-sref='client-create-request']"));
            createRequestButton.Click();

            IWebElement titleInputFiled = Driver.FindElement(By.Name("title"));
            titleInputFiled.Clear();
            string randomImeNaBaranje = RandomGenerateLetters(10);
            titleInputFiled.SendKeys(randomImeNaBaranje);

            IWebElement categoryFiled = Driver.FindElement(By.Id("field_y"));
            SelectElement requestCategory = new SelectElement(categoryFiled);
            requestCategory.SelectByIndex(3);

            IWebElement pickUp = Driver.FindElement(By.Name("pickUpAddress"));
            List<IWebElement> listOfRows = pickUp.FindElements(By.TagName("input")).ToList();

            IWebElement firstTextFiled = listOfRows[0];
            firstTextFiled.Clear();
            firstTextFiled.SendKeys("Strumica, North Macedonia");
            List<IWebElement> autoPickUpAddress = Driver.FindElements(By.CssSelector("span[class='pac-matched']")).ToList();
            autoPickUpAddress[0].Click();

            IWebElement deliveryAddress = Driver.FindElement(By.Name("deliveryAddress"));
            List<IWebElement> listOfRows1 = deliveryAddress.FindElements(By.TagName("input")).ToList();

            IWebElement firstTextFiled1 = listOfRows1[0];
            firstTextFiled1.Clear();
            firstTextFiled1.SendKeys("Greece");

            List<IWebElement> autoDeliveryAddress = Driver.FindElements(By.CssSelector("span[class='pac-matched']")).ToList();
            autoDeliveryAddress[0].Click();

            IWebElement removeItemButton = Driver.FindElement(By.CssSelector("a[title='Remove Item']"));

            removeItemButton.Click();

            IWebElement paymentMethodCachePickUp = Driver.FindElement(By.Id("cachePickup"));
            paymentMethodCachePickUp.Click();

            IWebElement paymentMethodCacheDelivery = Driver.FindElement(By.Id("cacheDelivery"));
            paymentMethodCacheDelivery.Click();

            IWebElement makeRequestButton = Driver.FindElement(By.CssSelector("input[ng-click='form.$valid && vm.pickUpAddressValid && vm.deliveryAddressValid && vm.validate()']"));
            makeRequestButton.Click();

            wait.Until(Expect.UrlMatches("http://18.156.17.83:9095/client/my-requests/active"));

            // confirm created request exists
            IWebElement tableBody = Driver.FindElement(By.ClassName("table-body"));

            List<IWebElement> ListofRows = tableBody.FindElements(By.ClassName("table-body__row")).ToList();

            IWebElement firstRow = ListofRows[0];
            IWebElement firstColumn = firstRow.FindElement(By.CssSelector("td[class='table-body__cell column1']"));
            IWebElement firstColumnTextElement = firstColumn.FindElement(By.CssSelector("a[ui-sref=\"client-request-details({id: request.id})\"]"));

            string text = firstColumnTextElement.Text;
            Assert.AreEqual(randomImeNaBaranje, text);

            // logout and confirm logout
            logout();

            //login as a transporter
            login("marinela.krstevska305@hotmail.com", "marinela2209");
            wait.Until(Expect.UrlMatches("http://18.156.17.83:9095/provider/home"));

            IWebElement tableBody1 = Driver.FindElement(By.ClassName("table-body"));

            List<IWebElement> ListofRows1 = tableBody1.FindElements(By.ClassName("table-body__row")).ToList();

            IWebElement firstRow1 = ListofRows1[0];
            IWebElement firstColumn1 = firstRow1.FindElement(By.CssSelector("td[class='table-body__cell column1']"));
            IWebElement firstColumnTextElement1 = firstColumn1.FindElement(By.CssSelector("a[ui-sref=\"provider-request-details({id: request.id})\"]"));

            string text1 = firstColumnTextElement1.Text;
            Assert.AreEqual(randomImeNaBaranje, text1);
            firstColumnTextElement1.Click();

            IWebElement makeOfferButton = Driver.FindElement(By.CssSelector("button[class='details-panel__make-offer-btn']"));
            makeOfferButton.Click();

            
            IWebElement priceField = Driver.FindElement(By.CssSelector("input[ng-model='paymentType.price']"));
            priceField.Clear();
            priceField.SendKeys("1");

            IWebElement expirationDate = Driver.FindElement(By.Name("expirationDateInput"));
            expirationDate.Clear();
            expirationDate.SendKeys("24.03.2023 00:00");

            IWebElement offerButtonCreate = Driver.FindElement(By.ClassName("make-offer__btn-create"));
            offerButtonCreate.Click();


            wait.Until(Expect.ElementToBeClickable(By.CssSelector("button[ng-click='vm.saveOffer()']")));
            IWebElement acceptOfferButton = Driver.FindElement(By.CssSelector("button[ng-click='vm.saveOffer()']"));
            acceptOfferButton.Click();

            Driver.Navigate().GoToUrl("http://18.156.17.83:9095/provider/home");


            wait.Until(Expect.ElementToBeClickable(By.CssSelector("a[ui-sref='provider-my-active-offers']")));
            IWebElement myOffersButton = Driver.FindElement(By.CssSelector("a[ui-sref='provider-my-active-offers']"));
            myOffersButton.Click();

            //logout and confirm logout
            logout();

            //Log in as a user and accept the offer

            login("krstevska.marinela@gmail.com", "pavel22@");
            wait.Until(Expect.UrlMatches("http://18.156.17.83:9095/client/home"));

            IWebElement makeRequest = Driver.FindElement(By.CssSelector("a[ui-sref='client-my-requests']"));
            makeRequest.Click();

            wait.Until(Expect.UrlMatches("http://18.156.17.83:9095/client/my-requests/active"));

            IWebElement tableBody3 = Driver.FindElement(By.ClassName("table-body"));

            List<IWebElement> ListofRows3 = tableBody3.FindElements(By.ClassName("table-body__row")).ToList();

            IWebElement firstRow3 = ListofRows3[0];
            IWebElement firstColumn3 = firstRow3.FindElement(By.CssSelector("td[class='table-body__cell column1']"));
            IWebElement firstColumnTextElement3 = firstColumn3.FindElement(By.CssSelector("a[ui-sref='client-request-details({id: request.id})']"));


            string text3 = firstColumnTextElement3.Text;
            Assert.AreEqual(randomImeNaBaranje, text3);

            IWebElement fruits = Driver.FindElement(By.CssSelector("a[ui-sref='client-request-details({id: request.id})']"));
            fruits.Click();

            wait.Until(Expect.ElementToBeClickable(By.CssSelector("a[translate='request.more']")));
            IWebElement moreButton = Driver.FindElement(By.CssSelector("a[translate='request.more']"));
            moreButton.Click();

            IWebElement radioButton = Driver.FindElement(By.Id("offer0"));
            radioButton.Click();

            IWebElement acceptOffer = Driver.FindElement(By.CssSelector("input[class='btn btn-default']"));
            acceptOffer.Click();


            IWebElement tableBody4 = Driver.FindElement(By.ClassName("table-body"));

            List<IWebElement> ListofRows4 = tableBody4.FindElements(By.ClassName("table-body__row")).ToList();

            IWebElement firstRow4 = ListofRows4[0];
            IWebElement firstColumn4 = firstRow4.FindElement(By.CssSelector("td[class='table-body__cell column1']"));
            IWebElement firstColumnTextElement4 = firstColumn4.FindElement(By.CssSelector("a[ui-sref='client-request-details({id: request.id})']"));


            string text4 = firstColumnTextElement4.Text;
            Assert.AreEqual(randomImeNaBaranje, text4);

            IWebElement myRiquests = Driver.FindElement(By.CssSelector("a[ui-sref='client-my-requests']"));
            myRiquests.Click();

            //logout and confirm logout

            logout();



        }
        [TearDown]
        public void TearDown()
        {
            Driver.Close();
            Driver.Dispose();

        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {

        }

        public void logout()
        {
            IWebElement logout = Driver.FindElement(By.Id("logout2"));
            logout.Click();
            wait.Until(Expect.ElementIsVisible(By.Id("login")));

            IWebElement loginButton1 = Driver.FindElement(By.Id("login"));

            Assert.IsTrue(loginButton1.Displayed);
        }
        
        public void login(string email, string password)
        {
            IWebElement loginButton = Driver.FindElement(By.Id("login"));
            loginButton.Click();

            IWebElement userNameFiled = Driver.FindElement(By.Id("username"));
            userNameFiled.Clear();
            userNameFiled.SendKeys(email);

            IWebElement passwordFiled = Driver.FindElement(By.Id("password"));
            passwordFiled.Clear();
            passwordFiled.SendKeys(password);

            IWebElement najavaButton = Driver.FindElement(By.CssSelector("button[translate='login.form.button']"));
            najavaButton.Click();
            wait.Until(Expect.ElementIsVisible(By.Id("logout2")));
            IWebElement logoutButton = Driver.FindElement(By.Id("logout2"));
            Assert.IsTrue(logoutButton.Displayed);

        }
        public static string RandomGenerateLetters(int lenght)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrsqtuvwxyz";
            return new string(Enumerable.Repeat(letters, lenght)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
       
        public static string RandomGenerateNumbers(int lenght)
        {
            const string numbers = "123456789";
            return new string(Enumerable.Repeat(numbers, lenght)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
        public static string RandomGenerateCharacters(int lenght)
        {
            const string numbers = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrsqtuvwxyz123456789";
            return new string(Enumerable.Repeat(numbers, lenght)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        string randomEmail = RandomGenerateLetters(10) + "@testdata.com";
    }   











}

