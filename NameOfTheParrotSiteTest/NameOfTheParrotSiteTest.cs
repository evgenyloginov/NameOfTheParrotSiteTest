using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NameOfTheParrotSiteTest
{
    [TestFixture]
    public class NameOfTheParrotSiteTest
    {
        private IWebDriver driver;
        private const string Email = "test@email.ru";
        private const string SiteUrl = "https://qa-course.kontur.host/selenium-practice/";
        private const string InvalidEmailAlert = "Некорректный email";
        private const string EmailInputIsEmptyAlert = "Введите email";
        private const string BoyResultText = "Хорошо, мы пришлём имя для вашего мальчика на e-mail:";
        private const string GirlResultText = "Хорошо, мы пришлём имя для вашей девочки на e-mail:";
        // Содержимое страницы
        private const string HeadText = "Тестирование программного обеспечения";
        private const string HeadTitle = "Не знаешь как назвать?";
        private const string FormTitle = "Мы подберём имя для твоего попугайчика!";
        private const string RadioButtonBoyText = "мальчик";
        private const string RadioButtonGirlText = "девочка";
        private const string ButtonName = "ПОДОБРАТЬ ИМЯ";
        
        [SetUp]
        public void SetUpTest()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(SiteUrl);
        }
        [TearDown]
        public void TearDownTest()
        {
            driver.Quit();
        }
        
        private readonly By _emailInputLocator = By.Name("email");
        private readonly By _buttonLocator = By.Id("sendMe");
        private readonly By _radioBoyLocator = By.XPath("//input[@name='parrotGender' and @value='male']");
        private readonly By _radioGirlLocator = By.XPath("//input[@name='parrotGender' and @value='female']");
        private readonly By _emailDuplicateLocator = By.ClassName("your-email");
        private readonly By _resultTextLocator = By.ClassName("result-text");
        private readonly By _formErrorLocator = By.ClassName("form-error");
        private readonly By _anotherEmailLinkLocator = By.LinkText("указать другой e-mail");
        // Содержимое страницы
        private readonly By _headTextLocator = By.ClassName("text-1");
        private readonly By _headTitleLocator = By.ClassName("title");
        private readonly By _formTitleLocator = By.ClassName("subtitle-bold");
        private readonly By _radioButtonBoyTextLocator = By.XPath("//label[@for='choiceFirst']");
        private readonly By _radioButtonGirlTextLocator = By.XPath("//label[@for='choiceSecond']");
        
        /**********                            Проверка начальных состояний: начало                             **********/
        [Test]
        public void ParrotSite_Default_RadioButtonBoy()
        {
            Assert.IsTrue(driver.FindElement(_radioBoyLocator).Selected, "RadioButton по умолчанию не мальчик");
        }
        [Test]
        public void ParrotSite_Default_EmailInputIsEmpty()
        {
            Assert.AreEqual(string.Empty, driver.FindElement(_emailInputLocator).Text, "Поле E-mail по умолчанию не пустое");
        }
        [Test]
        public void ParrotSite_Default_ButtonDisplayed()
        {
            Assert.IsTrue(driver.FindElement(_buttonLocator).Displayed, "По умолчанию кнопка \"Подобрать имя\" скрыта");
        }
        /**********                            Проверка начальных состояний: конец                             **********/

        /**********                         Проверка полей после отправки формы: начало                         **********/
        [Test]
        public void ParrotSite_SendForm_HideButton()
        {
            driver.FindElement(_radioGirlLocator).Click();
            driver.FindElement(_emailInputLocator).SendKeys(Email);
            driver.FindElement(_buttonLocator).Click();
            Assert.IsFalse(driver.FindElement(_buttonLocator).Displayed, "После отправки формы не скрылась кнопка \"Подобрать имя\"");
        }
        [Test]
        public void ParrotSite_SendForm_AnotherEmailLinkDisplayed()
        {
            driver.FindElement(_emailInputLocator).SendKeys(Email);
            driver.FindElement(_buttonLocator).Click();
            Assert.AreNotEqual(0, driver.FindElements(_anotherEmailLinkLocator).Count, "После отправки формы не появилась ссылка \"указать другой e - mail\"");
        }
        [Test]
        public void ParrotSite_SendForm_CorrectDuplicateEmail()
        {
            driver.FindElement(_emailInputLocator).SendKeys(Email);
            driver.FindElement(_buttonLocator).Click();
            Assert.AreEqual(Email, driver.FindElement(_emailDuplicateLocator).Text, "После отправки формы продублировался в заявке не верный E-mail");
        }
        [Test]
        public void ParrotSite_SendFormRadioBoy_ResultTextBoy()
        {
            driver.FindElement(_radioBoyLocator).Click();
            driver.FindElement(_emailInputLocator).SendKeys(Email);
            driver.FindElement(_buttonLocator).Click();
            Assert.AreEqual(BoyResultText, driver.FindElement(_resultTextLocator).Text, "После отправки формы приняли заявку на другой пол");
        }
        [Test]
        public void ParrotSite_SendFormRadioGirl_ResultTextGirl()
        {
            driver.FindElement(_radioGirlLocator).Click();
            driver.FindElement(_emailInputLocator).SendKeys(Email);
            driver.FindElement(_buttonLocator).Click();
            Assert.AreEqual(GirlResultText, driver.FindElement(_resultTextLocator).Text, "После отправки формы приняли заявку на другой пол");
        }
        [Test]
        public void ParrotSite_SendForm_EmailInputNotModify()
        {
            driver.FindElement(_emailInputLocator).SendKeys(Email);
            driver.FindElement(_buttonLocator).Click();
            driver.FindElement(_emailInputLocator).Clear();
            Assert.AreEqual(Email, driver.FindElement(_emailInputLocator).Text, "После отправки формы осталась возможность редактировать поле E-mail");
        }
        [Test]
        public void ParrotSite_SendForm_RadioButtonNotModify()
        {
            driver.FindElement(_radioGirlLocator).Click();
            driver.FindElement(_emailInputLocator).SendKeys(Email);
            driver.FindElement(_buttonLocator).Click();
            driver.FindElement(_radioBoyLocator).Click();
            Assert.IsFalse(driver.FindElement(_radioBoyLocator).Selected, "После отправки формы осталась возможность переключать RadioButton");
        }
        /**********                         Проверка полей после отправки формы: конец                         **********/

        /**********           Проверка полей после клика по ссылке "указать другой e-mail": начало            **********/
        [Test]
        public void ParrotSite_ClickAnotherEmail_HideAnotherEmail()
        {
            driver.FindElement(_radioGirlLocator).Click();
            driver.FindElement(_emailInputLocator).SendKeys(Email);
            driver.FindElement(_buttonLocator).Click();
            driver.FindElement(_anotherEmailLinkLocator).Click();
            Assert.AreEqual(0, driver.FindElements(_anotherEmailLinkLocator).Count, "После клика по ссылке \"указать другой e-mail\" не скрылась ссылка \"указать другой e-mail\"");
        }
        [Test]
        public void ParrotSite_ClickAnotherEmail_HideResultText()
        {
            driver.FindElement(_emailInputLocator).SendKeys(Email);
            driver.FindElement(_buttonLocator).Click();
            driver.FindElement(_anotherEmailLinkLocator).Click();
            Assert.AreEqual(0, driver.FindElements(_resultTextLocator).Count, "После клика по ссылке \"указать другой e-mail\" не скрылось текстовое поле с результатом заявки");
        }
        [Test]
        public void ParrotSite_ClickAnotherEmail_HideDuplicateEmail()
        {
            driver.FindElement(_radioGirlLocator).Click();
            driver.FindElement(_emailInputLocator).SendKeys(Email);
            driver.FindElement(_buttonLocator).Click();
            driver.FindElement(_anotherEmailLinkLocator).Click();
            Assert.AreEqual(0, driver.FindElements(_emailDuplicateLocator).Count, "После клика по ссылке \"указать другой e-mail\" не скрылся дубликат e-mail");
        }
        [Test]
        public void ParrotSite_ClickAnotherEmail_EmailInputIsEmpty()
        {
            driver.FindElement(_emailInputLocator).SendKeys(Email);
            driver.FindElement(_buttonLocator).Click();
            driver.FindElement(_anotherEmailLinkLocator).Click();
            Assert.AreEqual(string.Empty, driver.FindElement(_emailInputLocator).Text, "После клика по ссылке \"указать другой e-mail\" поле E-mail не очистилось");
        }
        [Test]
        public void ParrotSite_ClickAnotherEmail_ButtonDisplayed()
        {
            driver.FindElement(_emailInputLocator).SendKeys(Email);
            driver.FindElement(_buttonLocator).Click();
            driver.FindElement(_anotherEmailLinkLocator).Click();
            Assert.IsTrue(driver.FindElement(_buttonLocator).Displayed, "После клика по ссылке \"указать другой e-mail\" не отображается кнопка \"Подобрать имя\"");
        }
        /**********           Проверка полей после клика по ссылке "указать другой e-mail": конец            **********/
        
        /**********         Проверка поля E-mail (пустое, корректный адрес, некорректный адрес): начало        **********/
        [Test]
        public void ParrotSite_SendFormEmailInputIsEmpty_Alert()
        {
            driver.FindElement(_buttonLocator).Click();
            Assert.AreEqual(EmailInputIsEmptyAlert, driver.FindElement(_formErrorLocator).Text, "Оставили поле E-mail пустым");
        }
        // Valid Email Addresses (при необходимости пополнить список)
        [TestCase("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijkl@email.ru")]
        [TestCase("test@mxbzavwtozwrjybaajocfdhywklkbtgjnwcnundumsrefkwoygptvzvmnvtidfpxzawqujebfteofpcrqoxwzsickcborwdnoubfjrpyxpoolvlzyihxxrjnwiabvfxhgmoosdizjojkymvyscnebqrbucoghhmsiibemrehrpsvgqfatbcrjhqlwkanqzjwinfwdannxmixdsbejbdafgxriotkdqpsjobxgzqfljzbwzhhhkzfkuzslssl.ru")]
        [TestCase("1234567890@email.ru")]
        [TestCase("TEST@email.ru")]
        [TestCase("test@email777.ru")]
        [TestCase("test@тест.рф")]
        [TestCase("simple@example.com")]
        [TestCase("simple@subdomain.example.com")]
        [TestCase("very.common@example.com")]
        [TestCase("disposable.style.email.with+symbol@example.com")]
        [TestCase("other.email-with-hyphen@example.com")]
        [TestCase("fully-qualified-domain@example.com")]
        [TestCase("user.name+tag+sorting@example.com")]
        [TestCase("x@example.com")]
        [TestCase("example-indeed@strange-example.com")]
        [TestCase("test/test@test.com")]
        [TestCase("example@s.example")]
        [TestCase("\" \"@example.org")]
        [TestCase("\"john..doe\"@example.org")]
        [TestCase("mailhost!username@example.org")]
        [TestCase("user%example.com@example.org")]
        [TestCase("user-@example.org")]
        [TestCase("jsmith@[192.168.2.1]")]
        [TestCase("jsmith@[IPv6:2001:db8::1]")]
        [TestCase("a!b#c$d%e&f'a*S+wow-dot/h=a?q^s_a`d{d|d}a~a@example.com")]
        public void ParrotSite_SendFormValidEmail_Success(string validEmail)
        {
            driver.FindElement(_emailInputLocator).SendKeys(validEmail);
            driver.FindElement(_buttonLocator).Click();
            Assert.AreNotEqual(InvalidEmailAlert, driver.FindElement(_formErrorLocator).Text, "Определил как некорректный e-mail");
        }

        // Invalid Email Addresses (при необходимости пополнить список)
        [TestCase("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklm@email.ru")]
        [TestCase("test@rmxbzavwtozwrjybaajocfdhywklkbtgjnwcnundumsrefkwoygptvzvmnvtidfpxzawqujebfteofpcrqoxwzsickcborwdnoubfjrpyxpoolvlzyihxxrjnwiabvfxhgmoosdizjojkymvyscnebqrbucoghhmsiibemrehrpsvgqfatbcrjhqlwkanqzjwinfwdannxmixdsbejbdafgxriotkdqpsjobxgzqfljzbwzhhhkzfkuzslssl.ru")]
        [TestCase("test")]
        [TestCase("Abc.example.com")]
        [TestCase("A@b@c@example.com")]
        [TestCase("a\"b(c)d, e: f;g < h > i[j\\k]l@example.com")]
        [TestCase("just\"not\"right@example.com")]
        [TestCase("this is\"not\\allowed@example.com")]
        [TestCase("i_like_underscore@but_its_not_allowed_in_this_part.example.com")]
        [TestCase("@example.com")]
        [TestCase(".test@example.com")]
        [TestCase("test.@example.com")]
        [TestCase("test@example..com")]
        [TestCase("test..kontur@example.com")]
        [TestCase("test@example")]
        [TestCase("test@-example.com")]
        [TestCase("test@example-.com")]
        [TestCase("test@example.com-")]
        [TestCase("example@a!b#c$d%e&f'a*S+wow-dot/h=a?q^s_a`d{d|d}a~a.com")]
        public void ParrotSite_SendFormInvalidEmail_Alert(string invalidEmail)
        {
            driver.FindElement(_emailInputLocator).SendKeys(invalidEmail);
            driver.FindElement(_buttonLocator).Click();
            Assert.AreEqual(InvalidEmailAlert, driver.FindElement(_formErrorLocator).Text, "Определил как корректный e-mail");
        }
        /**********         Проверка поля E-mail (пустое, корректный адрес, некорректный адрес): конец         **********/

        /**********                            Проверка содержимого страницы: начало                            **********/
        [Test]
        public void ParrotSite_DiffContent_TitlePageSuccess()
        {
            Assert.IsTrue(driver.Title.Contains("Тестирование программного обеспечения"), "Неверный заголовок страницы");
        }
        [Test]
        public void ParrotSite_DiffContent_HeadTextSuccess()
        {
            Assert.AreEqual(HeadText, driver.FindElement(_headTextLocator).Text, "Неверный текст в шапке");
        }
        [Test]
        public void ParrotSite_DiffContent_HeadTitleSuccess()
        {
            Assert.AreEqual(HeadTitle, driver.FindElement(_headTitleLocator).Text, "Неверный заголовок");
        }
        [Test]
        public void ParrotSite_DiffContent_FormTitleSuccess()
        {
            Assert.AreEqual(FormTitle, driver.FindElement(_formTitleLocator).Text, "Неверный заголовок формы");
        }
        [Test]
        public void ParrotSite_DiffContent_RadioButtonBoyTextSuccess()
        {
            Assert.AreEqual(RadioButtonBoyText, driver.FindElement(_radioButtonBoyTextLocator).Text, "Неверная подпись у radiobutton мальчик");
        }
        [Test]
        public void ParrotSite_DiffContent_RadioButtonGirlTextSuccess()
        {
            Assert.AreEqual(RadioButtonGirlText, driver.FindElement(_radioButtonGirlTextLocator).Text, "Неверная подпись у radiobutton девочка");
        }
        [Test]
        public void ParrotSite_DiffContent_ButtonNameSuccess()
        {
            Assert.AreEqual(ButtonName, driver.FindElement(_buttonLocator).Text, "Неверное название кнопки \" ПОДОБРАТЬ ИМЯ\"");
        }
        /**********                            Проверка содержимого страницы: конец                             **********/
    }
}
