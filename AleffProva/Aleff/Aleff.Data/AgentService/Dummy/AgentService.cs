using Aleff.Domain.DTO;
using Aleff.Domain.Exceptions;
using Aleff.Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aleff.Data.AgentService.Dummy
{
  public class AgentService : IAgentService
  {
    private string _baseUrl;
    private HttpClient _httpClient;
    private static Lazy<JsonSerializerSettings> _settings;

    public AgentService(HttpClient httpClient)
    {
      BaseUrl = ConfigurationManager.AppSettings["DummyHost"].ToString();
      _httpClient = httpClient;
      _settings = new Lazy<JsonSerializerSettings>(CreateSerializerSettings, true);
    }

    private static JsonSerializerSettings CreateSerializerSettings()
    {
      var settings = new JsonSerializerSettings();
      UpdateJsonSerializerSettings(settings);
      return settings;
    }

    public string BaseUrl
    {
      get { return _baseUrl; }
      set
      {
        _baseUrl = value;
        if (!string.IsNullOrEmpty(_baseUrl) && !_baseUrl.EndsWith("/"))
          _baseUrl += '/';
      }
    }

    protected JsonSerializerSettings JsonSerializerSettings { get { return _settings.Value; } }

    protected void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
    {
      request.RequestUri = new Uri(url);
    }
    protected void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder)
    {
       request.RequestUri = new Uri(urlBuilder.ToString());
    }

    static void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
      settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    }

    public event EventHandler<HttpResponseMessage> RequestProcessed;

    protected void ProcessResponse(HttpClient client, HttpResponseMessage response)
    {
      RequestProcessed?.Invoke(client, response);
    }

    protected struct ObjectResponseResult<T>
    {
      public ObjectResponseResult(T responseObject, string responseText)
      {
        this.Object = responseObject;
        this.Text = responseText;
      }

      public T Object { get; }

      public string Text { get; }
    }

    public bool ReadResponseAsString { get; set; }

    protected virtual async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers, CancellationToken cancellationToken)
    {
      if (response == null || response.Content == null)
      {
        return new ObjectResponseResult<T>(default(T), string.Empty);
      }

      if (ReadResponseAsString)
      {
        var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        try
        {
          var typedBody = JsonConvert.DeserializeObject<T>(responseText, JsonSerializerSettings);
          return new ObjectResponseResult<T>(typedBody, responseText);
        }
        catch (JsonException exception)
        {
          var message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
          throw new ApiException(message, (int)response.StatusCode, responseText, headers, exception);
        }
      }
      else
      {
        try
        {
          using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
          using (var streamReader = new StreamReader(responseStream))
          using (var jsonTextReader = new JsonTextReader(streamReader))
          {
            var serializer = JsonSerializer.Create(JsonSerializerSettings);
            var typedBody = serializer.Deserialize<T>(jsonTextReader);
            return new ObjectResponseResult<T>(typedBody, string.Empty);
          }
        }
        catch (JsonException exception)
        {
          var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
          throw new ApiException(message, (int)response.StatusCode, string.Empty, headers, exception);
        }
      }
    }

    private string ConvertToString(object value, CultureInfo cultureInfo)
    {
      if (value is Enum)
      {
        string name = Enum.GetName(value.GetType(), value);
        if (name != null)
        {
          var field = IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
          if (field != null)
          {
            var attribute = CustomAttributeExtensions.GetCustomAttribute(field, typeof(EnumMemberAttribute))
                as EnumMemberAttribute;
            if (attribute != null)
            {
              return attribute.Value != null ? attribute.Value : name;
            }
          }

          return Convert.ToString(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()), cultureInfo));
        }
      }
      else if (value is bool)
      {
        return Convert.ToString(value, cultureInfo).ToLowerInvariant();
      }
      else if (value is byte[])
      {
        return Convert.ToBase64String((byte[])value);
      }
      else if (value != null && value.GetType().IsArray)
      {
        var array = Enumerable.OfType<object>((Array)value);
        return string.Join(",", Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
      }

      return Convert.ToString(value, cultureInfo);
    }

    public async Task<DTODummyEmployees> GetEmployeesAsync(CancellationToken cancellationToken)
    {
      var client_ = _httpClient;
      var disposeClient_ = false;
      try
      {
        using (var request_ = new HttpRequestMessage())
        {
          request_.Method = new HttpMethod("GET");
          request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));

          var urlBuilder_ = new StringBuilder();
          if (!string.IsNullOrEmpty(BaseUrl)) urlBuilder_.Append(BaseUrl);
          urlBuilder_.Append("api/v1/employees");

          PrepareRequest(client_, request_, urlBuilder_);

          var url_ = urlBuilder_.ToString();
          request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

          PrepareRequest(client_, request_, url_);

          var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
          var disposeResponse_ = true;
          try
          {
            var headers_ = new Dictionary<string, IEnumerable<string>>();
            foreach (var item_ in response_.Headers)
              headers_[item_.Key] = item_.Value;
            if (response_.Content != null && response_.Content.Headers != null)
            {
              foreach (var item_ in response_.Content.Headers)
                headers_[item_.Key] = item_.Value;
            }

            ProcessResponse(client_, response_);

            var status_ = (int)response_.StatusCode;
            if (status_ == 200)
            {
              var objectResponse_ = await ReadObjectResponseAsync<DTODummyEmployees>(response_, headers_, cancellationToken).ConfigureAwait(false);
              if (objectResponse_.Object == null)
              {
                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
              }
              return objectResponse_.Object;
            }
            else
            {
              var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
              throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
            }
          }
          finally
          {
            if (disposeResponse_)
              response_.Dispose();
          }
        }
      }
      finally
      {
        if (disposeClient_)
          client_.Dispose();
      }
    }


  }
}
