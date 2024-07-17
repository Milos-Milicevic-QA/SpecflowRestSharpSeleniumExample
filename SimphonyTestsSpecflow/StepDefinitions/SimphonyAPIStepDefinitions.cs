using NUnit.Framework;
using RestSharp;
using TechTalk.SpecFlow;
using Newtonsoft.Json.Linq;
using System.Net;
using Newtonsoft.Json;

namespace SimphonyTestsSpecflow.StepDefinitions
{
    [Binding]
    public sealed class SimphonyAPIStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;
        private RestClient _client;
        private RestRequest _request;
        private RestResponse _response;
        private string _token;
        private int _postId;

        public SimphonyAPIStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
            _client = new RestClient("https://randomlyapi.symphony.is");
            _featureContext = featureContext;
        }

        [When(@"I register a new user with username ""(.*)"" email ""(.*)"" password ""(.*)"" first name ""(.*)"" last name ""(.*)"" and date of birth ""(.*)""")]
        public void WhenIRegisterANewUser(string username, string email, string password, string firstName, string lastName, string dateOfBirth)
        {
            var body = new
            {
                username,
                email,
                password,
                firstName,
                lastName,
                dateOfBirth
            };

            _request = new RestRequest("/api/auth/signup/", Method.Post);
            _request.AddHeader("accept", "application/json");
            _request.AddHeader("Content-Type", "application/json");
            _request.AddJsonBody(body);

            // Log the request details
            Console.WriteLine("Request URL: " + _client.BuildUri(_request));
            Console.WriteLine("Request Body: " + JsonConvert.SerializeObject(body, Formatting.Indented));

            _response = _client.Execute(_request);
            _scenarioContext["Response"] = _response;

            // Log the response details
            Console.WriteLine("Response Status: " + _response.StatusCode);
            Console.WriteLine("Response Content: " + _response.Content);
        }

        [When(@"I register a new user with dynamic username and email")]
        public void WhenIRegisterANewUser()
        {
            string username = "testuser" + DateTime.Now.Ticks;
            string email = $"testuser{DateTime.Now.Ticks}@example.com";
            string password = "TestPassword123";
            string firstName = "John";
            string lastName = "Doe";
            string dateOfBirth = "01/01/1991"; // Adjust the format as needed

            var body = new
            {
                username,
                email,
                password,
                firstName,
                lastName,
                dateOfBirth
            };

            _request = new RestRequest("/api/auth/signup/", Method.Post);
            _request.AddHeader("accept", "application/json");
            _request.AddHeader("Content-Type", "application/json");
            _request.AddJsonBody(body);

            // Log the request details
            Console.WriteLine("Request URL: " + _client.BuildUri(_request));
            Console.WriteLine("Request Body: " + JsonConvert.SerializeObject(body, Formatting.Indented));

            _response = _client.Execute(_request);
            _scenarioContext["Response"] = _response;

            // Log the response details
            Console.WriteLine("Response Status: " + _response.StatusCode);
            Console.WriteLine("Response Content: " + _response.Content);
        }

        [Then(@"the response status should be (.*)")]
        public void ThenTheResponseStatusShouldBe(int statusCode)
        {
            _response = (RestResponse)_scenarioContext["Response"];
            Assert.AreEqual((HttpStatusCode)statusCode, _response.StatusCode);
        }

        [When(@"I login with username ""(.*)"" and password ""(.*)""")]
        public void WhenILogin(string username, string password)
        {
            var body = new { username, password };

            _request = new RestRequest("/api/auth/login/", Method.Post);
            _request.AddHeader("accept", "application/json");
            _request.AddHeader("Content-Type", "application/json");
            _request.AddJsonBody(body);

            _response = _client.Execute(_request);
            _scenarioContext["Response"] = _response;

            // Log the request details
            Console.WriteLine("Request URL: " + _client.BuildUri(_request));
            Console.WriteLine("Request Body: " + JsonConvert.SerializeObject(body, Formatting.Indented));
            Console.WriteLine("Response Status: " + _response.StatusCode);
            Console.WriteLine("Response Content: " + _response.Content);

            if (_response.StatusCode == HttpStatusCode.OK)
            {
                JObject jsonResponse = JObject.Parse(_response.Content);
                _token = jsonResponse["token"].ToString();
                _featureContext["Token"] = _token;
            }
        }

        [Given(@"I am logged in")]
        public void GivenIAmLoggedIn()
        {
            _token = (string)_featureContext["Token"];
            Assert.IsNotNull(_token);
        }

        [When(@"I create a post with title ""(.*)"" and content ""(.*)""")]
        public void WhenICreateAPost(string title, string content)
        {
            var body = new { title, content };

            _request = new RestRequest("/api/posts/", Method.Post);
            _request.AddHeader("Authorization", $"token {_token}");
            _request.AddHeader("accept", "application/json");
            _request.AddHeader("Content-Type", "application/json");
            _request.AddJsonBody(body);

            _response = _client.Execute(_request);
            _scenarioContext["Response"] = _response;

            if (_response.StatusCode == HttpStatusCode.Created)
            {
                JObject jsonResponse = JObject.Parse(_response.Content);
                _postId = (int)jsonResponse["id"];
                _featureContext["PostId"] = _postId;
            }

            // Log the request details
            Console.WriteLine("Request URL: " + _client.BuildUri(_request));
            Console.WriteLine("Request Body: " + JsonConvert.SerializeObject(body, Formatting.Indented));
            Console.WriteLine("Response Status: " + _response.StatusCode);
            Console.WriteLine("Response Content: " + _response.Content);
        }

        [Then(@"I save the token from the response")]
        public void ThenISaveTheTokenFromTheResponse()
        {
            _response = (RestResponse)_scenarioContext["Response"];
            JObject jsonResponse = JObject.Parse(_response.Content);
            _token = jsonResponse["token"].ToString();
            _featureContext["Token"] = _token;
        }

        [Then(@"I save the post ID from the response")]
        public void ThenISaveThePostIdFromTheResponse()
        {
            _response = (RestResponse)_scenarioContext["Response"];
            JObject jsonResponse = JObject.Parse(_response.Content);
            _postId = (int)jsonResponse["id"];
            _featureContext["PostId"] = _postId;
        }

        [Given(@"I have a post ID")]
        public void GivenIHaveAPostId()
        {
            _postId = (int)_featureContext["PostId"];
            Assert.IsNotNull(_postId);
        }

        [When(@"I add a comment ""(.*)""")]
        public void WhenIAddAComment(string comment)
        {
            var body = new { post = _postId, text = comment };

            _request = new RestRequest("/api/post-comments/", Method.Post);
            _request.AddHeader("Authorization", $"token {_token}");
            _request.AddHeader("accept", "application/json");
            _request.AddHeader("Content-Type", "application/json");
            _request.AddJsonBody(body);

            _response = _client.Execute(_request);
            _scenarioContext["Response"] = _response;

            // Log the request details
            Console.WriteLine("Request URL: " + _client.BuildUri(_request));
            Console.WriteLine("Request Body: " + JsonConvert.SerializeObject(body, Formatting.Indented));
            Console.WriteLine("Response Status: " + _response.StatusCode);
            Console.WriteLine("Response Content: " + _response.Content);
        }

        [When(@"I get comments for the post")]
        public void WhenIGetCommentsForThePost()
        {
            _request = new RestRequest($"/api/posts/{_postId}/comments/", Method.Get);
            _request.AddHeader("Authorization", $"token {_token}");
            _request.AddHeader("accept", "application/json");

            _response = _client.Execute(_request);
            _scenarioContext["Response"] = _response;

            // Log the request details
            Console.WriteLine("Request URL: " + _client.BuildUri(_request));
            Console.WriteLine("Response Status: " + _response.StatusCode);
            Console.WriteLine("Response Content: " + _response.Content);
        }

        [Then(@"the response should contain the comment ""(.*)""")]
        public void ThenTheResponseShouldContainTheComment(string comment)
        {
            _response = (RestResponse)_scenarioContext["Response"];
            JObject jsonResponse = JObject.Parse(_response.Content);

            bool commentFound = false;
            JArray resultsArray = (JArray)jsonResponse["results"];
            foreach (var item in resultsArray)
            {
                if (item["text"].ToString() == comment)
                {
                    commentFound = true;
                    break;
                }
            }
            Assert.IsTrue(commentFound, $"Comment '{comment}' was not found in the response.");
        }

        [When(@"I register a new user with missing fields")]
        public void WhenIRegisterANewUserWithMissingFields()
        {
            var body = new { username = "testuser", email = "testuser@example.com" };

            _request = new RestRequest("/api/auth/signup/", Method.Post);
            _request.AddHeader("accept", "application/json");
            _request.AddHeader("Content-Type", "application/json");
            _request.AddJsonBody(body);

            _response = _client.Execute(_request);
            _scenarioContext["Response"] = _response;

            // Log the request details
            Console.WriteLine("Request URL: " + _client.BuildUri(_request));
            Console.WriteLine("Request Body: " + JsonConvert.SerializeObject(body, Formatting.Indented));
            Console.WriteLine("Response Status: " + _response.StatusCode);
            Console.WriteLine("Response Content: " + _response.Content);
        }

        [When(@"I login with invalid username ""(.*)"" and password ""(.*)""")]
        public void WhenILoginWithInvalidCredentials(string username, string password)
        {
            var body = new { username, password };

            _request = new RestRequest("/api/auth/login/", Method.Post);
            _request.AddHeader("accept", "application/json");
            _request.AddHeader("Content-Type", "application/json");
            _request.AddJsonBody(body);

            _response = _client.Execute(_request);
            _scenarioContext["Response"] = _response;

            // Log the request details
            Console.WriteLine("Request URL: " + _client.BuildUri(_request));
            Console.WriteLine("Request Body: " + JsonConvert.SerializeObject(body, Formatting.Indented));
            Console.WriteLine("Response Status: " + _response.StatusCode);
            Console.WriteLine("Response Content: " + _response.Content);
        }

        [When(@"I create a post with title ""(.*)"" and content ""(.*)"" without login")]
        public void WhenICreateAPostWithoutLogin(string title, string content)
        {
            var body = new { title, content };

            _request = new RestRequest("/api/posts/", Method.Post);
            _request.AddHeader("accept", "application/json");
            _request.AddHeader("Content-Type", "application/json");
            _request.AddJsonBody(body);

            _response = _client.Execute(_request);
            _scenarioContext["Response"] = _response;

            // Log the request details
            Console.WriteLine("Request URL: " + _client.BuildUri(_request));
            Console.WriteLine("Request Body: " + JsonConvert.SerializeObject(body, Formatting.Indented));
            Console.WriteLine("Response Status: " + _response.StatusCode);
            Console.WriteLine("Response Content: " + _response.Content);
        }

        [When(@"I add a comment ""(.*)"" without post ID")]
        public void WhenIAddACommentWithoutPostID(string comment)
        {
            var body = new { content = comment };

            _request = new RestRequest("/api/post-comments/", Method.Post);
            _request.AddHeader("Authorization", $"token {_token}");
            _request.AddHeader("accept", "application/json");
            _request.AddHeader("Content-Type", "application/json");
            _request.AddJsonBody(body);

            _response = _client.Execute(_request);
            _scenarioContext["Response"] = _response;

            // Log the request details
            Console.WriteLine("Request URL: " + _client.BuildUri(_request));
            Console.WriteLine("Request Body: " + JsonConvert.SerializeObject(body, Formatting.Indented));
            Console.WriteLine("Response Status: " + _response.StatusCode);
            Console.WriteLine("Response Content: " + _response.Content);
        }

        [When(@"I get comments for a non-existing post")]
        public void WhenIGetCommentsForANonExistingPost()
        {
            _request = new RestRequest("/api/posts/99999/comments/", Method.Get); // Assuming 99999 is a non-existing post ID
            _request.AddHeader("Authorization", $"token {_token}");
            _request.AddHeader("accept", "application/json");

            _response = _client.Execute(_request);
            _scenarioContext["Response"] = _response;

            // Log the request details
            Console.WriteLine("Request URL: " + _client.BuildUri(_request));
            Console.WriteLine("Response Status: " + _response.StatusCode);
            Console.WriteLine("Response Content: " + _response.Content);
        }
    }
}
