# MVC4 CORS
## Support for enabling CORS in MVC4 via actionfilters.

MVC 4 based ActionFilter that checks list of allowable domains against the Origin property and reflects that back in the Access-Control-Allow-Origin header if it's in the list.
For complex requests (like PUTs and DELETEs), preflight behavior is supported.

**Usage:**
Can be used at action level or controller level to apply to all actions:
```csharp
[CorsEnabled("http://www.mysite.com,http://www.mysite2.com")]
```

To support 'Pre flight' behavior for complex requests like PUT, include the allowable methods as well:
```csharp
[CorsEnabled("http://www.mysite.com,http://www.mysite2.com", "PUT,DELETE")]
```

Custom url comparison method can be injected into filter using the IAllowableDomains interface and domains can also be injected dynamically by registering the filter during Startup. Check the FilterConffig in MMG.Public.MVC4Cors.Web project for examples.

**References**
* https://en.wikipedia.org/wiki/Cross-origin_resource_sharing
* https://www.w3.org/TR/cors/
* http://www.html5rocks.com/en/tutorials/cors/

