using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;



string filePath = @"C:\Users\msinp\Desktop\odos.txt";

try
{
    File.WriteAllText(filePath, string.Empty);
    Console.WriteLine("Nội dung của tệp tin đã được xóa.");
}
catch (Exception ex)
{
    Console.WriteLine("Lỗi: " + ex.Message);
}
int threadCount = 1;
int keyCount = 0;
string[] keys = "".Split("\r\n");

while (true)
{
    if (keys.Length - keyCount < threadCount)
        threadCount = keys.Length - keyCount;
    var tasks = new Task[threadCount];

    for (int i = 0; i < threadCount; i++)
    {
        int currentKeyCount = keyCount;

        if (currentKeyCount != keys.Length)
        {

            tasks[i] = Task.Run(async () =>
            {
                Console.WriteLine($"Task {Task.CurrentId} started.");
                Console.WriteLine(await Kyber(keys[currentKeyCount]));
                Console.WriteLine($"Task {Task.CurrentId} completed.");
            });
            keyCount++;
        }
        else
        {
            break;
        }
    }

    await Task.WhenAll(tasks);


    if (keyCount >= keys.Length)
    {
        break;
    }
}


Console.WriteLine("All tasks completed.");
static string ChromeDriverPath()
{
    string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
    var chromeDriverPath = Path.Combine(rootDirectory, "chromedriver.exe");
    return chromeDriverPath;
}
static string address(string privateKey)
{
    return new Account(privateKey).Address;
}

static async Task<string> QueryBalance(string privateKey)
{
    var web3 = new Web3("https://optimism.llamarpc.com");
    var balance = await web3.Eth.GetBalance.SendRequestAsync(address(privateKey));
    var balanceeth = Web3.Convert.FromWei(balance.Value);
    /* var balanceOfMessage = new BalanceOfFunction() { Owner = address(privateKey) };
     var queryHandler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();
     var balanceusdc = await queryHandler
         .QueryAsync<BigInteger>("0x06eFdBFf2a14a7c8E15944D1F4A48F9F95F663A4", balanceOfMessage)
         .ConfigureAwait(false);
     var balanceusdc1 = (float)balanceusdc / 1000000;
     Console.WriteLine((balanceusdc1).ToString());*/
    return balanceeth.ToString();
}
static void MetamaskConnect(ChromeDriver driver, string privateKey)
{

    Thread.Sleep(5000);
    driver.Navigate().GoToUrl("chrome-extension://nkbihfbeogaeaoehlefnkodbefgpgknn/home.html#onboarding/welcome");
    Thread.Sleep(5000);
    try
    {
        driver.SwitchTo().Window(driver.WindowHandles[1]);
        driver.Close();
        driver.SwitchTo().Window(driver.WindowHandles[0]);
    }
    catch
    {
        driver.SwitchTo().Window(driver.WindowHandles[0]);

    }
    Thread.Sleep(5000);

    IWebElement checkbox = driver.FindElement(By.XPath("//input[@type='checkbox']"));
    Console.WriteLine(driver.Title);

    if (!checkbox.Selected)
    {

        checkbox.Click();
    }
    driver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/div/div/div/ul/li[2]/button")).Click();
    Thread.Sleep(2000);
    driver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/div/div/div/div/button[1]")).Click();
    Thread.Sleep(2000);
    driver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/div/div/div/div[2]/form/div[1]/label/input")).SendKeys("12345678");
    driver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/div/div/div/div[2]/form/div[2]/label/input")).SendKeys("12345678");
    driver.FindElement(By.XPath("//input[@type='checkbox']")).Click();
    driver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/div/div/div/div[2]/form/button")).Click();
    Thread.Sleep(2000);
    driver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/div/div/div/div[2]/button[1]")).Click();
    driver.FindElement(By.XPath("//input[@type='checkbox']")).Click();
    driver.FindElement(By.XPath("/html/body/div[2]/div/div/section/div[2]/div/button[2]")).Click();
    Thread.Sleep(2000);
    driver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/div/div/div/div[2]/button")).Click();
    Thread.Sleep(1000);
    driver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/div/div/div/div[2]/button")).Click();
    driver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/div/div/div/div[2]/button")).Click();
    Thread.Sleep(3000);
    driver.FindElement(By.XPath("/html/body/div[2]/div/div/section/div[1]/div/button/span")).Click();
    driver.FindElement(By.XPath("/html/body/div[1]/div/div[2]/div/button")).Click();

    driver.FindElement(By.XPath("/html/body/div[3]/div[3]/div/section/div[2]/div[2]/div[2]/button")).Click();
    driver.FindElement(By.XPath("/html/body/div[3]/div[3]/div/section/div[2]/div/div[1]/div/input")).SendKeys(privateKey);
    driver.FindElement(By.XPath("/html/body/div[3]/div[3]/div/section/div[2]/div/div[2]/button[2]")).Click();

}
async Task<string> Kyber(string privateKey)
{
    string chromeDriverPath = ChromeDriverPath();// Đường dẫn đến chromedriver
    ChromeOptions options = new ChromeOptions();
    // Thêm tiện ích mở rộng vào trình duyệt
    options.AddExtension(@"C:\Users\msinp\Desktop\11.0.0_0.crx");
    options.AddUserProfilePreference("profile.managed_default_content_settings.images", 2);
    options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36");
    // Tắt chế độ tự động chơi phương tiện
    options.AddArgument("--headless");

    options.AddArgument("--window-size=945,1012");
    options.AddArgument("--autoplay-policy=no-user-gesture-required");
    ChromeDriver driver = new ChromeDriver(chromeDriverPath, options);
    try
    {


        MetamaskConnect(driver, privateKey);
        driver.Navigate().GoToUrl("https://stargate.finance/transfer");
        Thread.Sleep(3000);
        driver.FindElement(By.XPath("/html/body/div/header[1]/div/div[3]/button[2]")).Click();
        Console.WriteLine(driver.Title);
        Thread.Sleep(1000);
        driver.FindElement(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div/a[1]")).Click();



        Thread.Sleep(3000);

        var windowHandles = driver.WindowHandles.ToList();
        foreach (var windowHandle in windowHandles)
        {
            driver.SwitchTo().Window(windowHandle);
            if (driver.Url.Contains("chrome-extension"))
            {
                Console.WriteLine("This is metamask notification");
                break;
            }

        }
        //click 2 nut cua metamask
        driver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div[3]/div[2]/footer/button[2]")).Click();
        driver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div[3]/div[2]/footer/button[2]")).Click();

        driver.SwitchTo().Window(windowHandles[0]);
        Thread.Sleep(2000);
        driver.FindElement(By.Id("From-input-network")).Click();
        Thread.Sleep(3000);
        string searchText = "Optimism";
        driver.FindElement(By.XPath("//div[contains(@class,'jss246') and contains(@class,'label') and text()='" + searchText + "']")).FindElement(By.XPath("..")).Click();
        Thread.Sleep(1000);
        driver.FindElement(By.Id("To-input-network")).Click();
        Thread.Sleep(3000);
        searchText = "Base";
        driver.FindElement(By.XPath("/html/body/div/main/div[2]/section/div[1]/div[2]/div[1]/div[3]/div/div[2]/div/div/div/div[2]/div[2]/ul/button[11]"))/*.FindElement(By.XPath(".."))*/.Click();
        Thread.Sleep(1000);
        driver.FindElement(By.XPath("/html/body/div/main/div[2]/section/div[1]/div[2]/div[1]/div[4]/div[2]/div/input")).Clear();

        Random random = new Random();
        int randomint = random.Next(1, 9999);
        string randomstring = randomint.ToString();
        randomstring = "0.0000" + randomstring;

        // '''random từ 0.0000x (x = 1-9999)'


        driver.FindElement(By.XPath("/html/body/div/main/div[2]/section/div[1]/div[2]/div[1]/div[4]/div[2]/div/input")).SendKeys(randomstring);
        Thread.Sleep(10000);
        driver.FindElement(By.XPath("/html/body/div/main/div[2]/section/div[1]/div[2]/div[3]/div[2]/div/button")).Click();
        Thread.Sleep(5000);

        windowHandles = driver.WindowHandles.ToList();
        foreach (var windowHandle in windowHandles)
        {
            driver.SwitchTo().Window(windowHandle);
            if (driver.Url.Contains("chrome-extension"))
            {
                Console.WriteLine("This is metamask notification");
                break;
            }

        }
        driver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div[2]/div/button[2]")).Click();
        Thread.Sleep(5000);
        driver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div[2]/div/button[2]")).Click();
        driver.SwitchTo().Window(windowHandles[0]);
        Thread.Sleep(3000);
        /* driver.FindElement(By.XPath("/html/body/div/header[1]/div/div[3]/button[2]")).Click();
         Thread.Sleep(1000);
         driver.FindElement(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div/a[1]")).Click();
         Thread.Sleep(5000);
         driver.FindElement(By.XPath("/html/body/div/main/div[2]/section/div[1]/div[2]/div[3]/div[2]/div/button")).Click();
         */
        Thread.Sleep(10000);
        windowHandles = driver.WindowHandles.ToList();
        foreach (var windowHandle in windowHandles)
        {
            driver.SwitchTo().Window(windowHandle);
            if (driver.Url.Contains("chrome-extension"))
            {
                Console.WriteLine("This is metamask notification");
                break;
            }

        }
        for (int i = 0; i < 100; i++)
        {
            Thread.Sleep(500);
            bool isEnabled = driver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div[3]/div[3]/footer/button[2]")).Enabled;
            if (isEnabled)
            {
                driver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div[3]/div[3]/footer/button[2]")).Click();
                break;
            }

        }
        driver.SwitchTo().Window(windowHandles[0]);

        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1000));
        Thread.Sleep(3000);
        IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div/header[1]/div/div[3]/button[1]")));
        element.Click();

        // driver.FindElement(By.XPath("/html/body/div/header[1]/div/div[3]/button[1]")).Click();
        var txhash = driver.FindElement(By.XPath("//a[contains(@href,'https://layerzeroscan.com/tx/')]")).GetAttribute("href");
        // string filePath = @"C:\Users\msinp\Desktop\odos.txt";
        /*string contentToAppend = privateKey + "\t" + txhash + "\t" + await QueryBalance(privateKey);


        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            // Ghi nội dung vào tệp
            writer.WriteLine(contentToAppend);
        }
        driver.Close();
        driver.Quit();*/
        return privateKey + "\t" + txhash + "\t" + await QueryBalance(privateKey);

    }


    catch (Exception ex)
    {
        //string filePath = @"C:\Users\msinp\Desktop\odos.txt";
        string contentToAppend = privateKey + "\t" + "Lỗi" + "\t" + await QueryBalance(privateKey) + "\t" + ex.Message;


        using (StreamWriter writer = new StreamWriter(filePath, true))
        {

            writer.WriteLine(contentToAppend);
        }
        driver.Close();
        driver.Quit();
        return privateKey + "\t" + "Lỗi" + "\t" + await QueryBalance(privateKey) + "\t" + ex.Message;

    }

}
