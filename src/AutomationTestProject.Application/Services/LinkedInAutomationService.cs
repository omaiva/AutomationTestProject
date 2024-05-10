using AutomationTestProject.Application.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationTestProject.Application.Services
{
    public class LinkedInAutomationService : ILinkedInAutomationService
    {
        private readonly IConfiguration _configuration;

        public LinkedInAutomationService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<string> GetProfileImage()
        {
            ChromeOptions options = new ChromeOptions();
            //options.AddArguments("--headless");

            using (var driver = new ChromeDriver(options))
            {
                try
                {
                    driver.Navigate().GoToUrl("https://www.linkedin.com/login");

                    driver.FindElement(By.Id("username")).SendKeys(_configuration["LinkedIn:User"]);
                    driver.FindElement(By.Id("password")).SendKeys(_configuration["LinkedIn:Password"]);
                    driver.FindElement(By.CssSelector("[type='submit']")).Click();

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(ExpectedConditions.UrlToBe("https://www.linkedin.com/feed/"));

                    driver.Navigate().GoToUrl("https://www.linkedin.com/in/");
                    IWebElement profilePicElement = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".profile-photo-edit__preview")));

                    if (profilePicElement != null)
                    {
                        var imgUrl = profilePicElement.GetAttribute("src");
                        return imgUrl;
                    }
                }
                finally
                {
                    driver.Quit();
                }
                return null;
            }
        }
    }
}
