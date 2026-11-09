using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
namespace raceday_mvc.Services;
public class RaceDayApiClient
{
 private readonly IHttpClientFactory _factory; private readonly IHttpContextAccessor _accessor;
 private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web) { PropertyNameCaseInsensitive=true };
 public RaceDayApiClient(IHttpClientFactory factory, IHttpContextAccessor accessor){_factory=factory;_accessor=accessor;}
 private HttpClient Client()=>_factory.CreateClient("RaceDayApi");
 private HttpRequestMessage Request(HttpMethod method,string path,object? values=null){var req=new HttpRequestMessage(method,path); var cookie=_accessor.HttpContext?.Session.GetString("ApiCookie"); if(!string.IsNullOrWhiteSpace(cookie)) req.Headers.TryAddWithoutValidation("Cookie",cookie); if(values!=null) req.RequestUri=new Uri(QueryHelpers.AddQueryString(path, ToDictionary(values))); return req;}
 private static Dictionary<string,string?> ToDictionary(object obj)=>obj.GetType().GetProperties().ToDictionary(p=>JsonNamingPolicy.CamelCase.ConvertName(p.Name),p=>p.GetValue(obj)?.ToString());
 public async Task<(bool Ok,string Message,string? Json)> SendAsync(HttpMethod method,string path,object? values=null){try{using var req=Request(method,path,values); using var res=await Client().SendAsync(req); if(method==HttpMethod.Post && path=="api/auth/login" && res.Headers.TryGetValues("Set-Cookie",out var cookies)){var session=cookies.FirstOrDefault(x=>x.StartsWith(".RaceDay.Api.Session",StringComparison.OrdinalIgnoreCase)); if(session!=null)_accessor.HttpContext!.Session.SetString("ApiCookie",session.Split(';')[0]);} var body=await res.Content.ReadAsStringAsync(); return(res.IsSuccessStatusCode, string.IsNullOrWhiteSpace(body)?res.ReasonPhrase??"":body,body);}catch(Exception ex){return(false,"Could not contact the RaceDay API: "+ex.Message,null);}}
 public async Task<T?> GetAsync<T>(string path){var r=await SendAsync(HttpMethod.Get,path); if(!r.Ok||string.IsNullOrWhiteSpace(r.Json))return default; try{return JsonSerializer.Deserialize<T>(r.Json,JsonOptions);}catch{return default;}}
 public async Task<(bool Ok,string Message)> PostAsync(string path,object values)=>await Simple(HttpMethod.Post,path,values);
 public async Task<(bool Ok,string Message)> PutAsync(string path,object values)=>await Simple(HttpMethod.Put,path,values);
 public async Task<(bool Ok,string Message)> DeleteAsync(string path)=>await Simple(HttpMethod.Delete,path,null);
 private async Task<(bool Ok,string Message)> Simple(HttpMethod m,string p,object? v){var r=await SendAsync(m,p,v);return(r.Ok,r.Message);}
 public void ClearSession(){_accessor.HttpContext?.Session.Remove("ApiCookie");}
}
