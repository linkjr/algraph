using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace Algraph.Infrastructure.Web.Http.Dispatcher
{
    public class NamespaceHttpControllerSelector : DefaultHttpControllerSelector
    {
        private const string NamespaceRouteVariableName = "Namespace";

        private readonly HttpConfiguration _configuration;
        private readonly Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>> _controllerInfoCache;
        private readonly Lazy<Dictionary<string, HttpControllerDescriptor>> _controllersDicts;

        public NamespaceHttpControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            this._configuration = configuration;
            this._controllerInfoCache = new Lazy<ConcurrentDictionary<string, HttpControllerDescriptor>>(new Func<ConcurrentDictionary<string, HttpControllerDescriptor>>(this.InitializeControllerInfoCache));
            this._controllersDicts = new Lazy<Dictionary<string, HttpControllerDescriptor>>(this.InitializeControllerDictionary);
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            string controllerName = this.GetControllerName(request);
            if (string.IsNullOrEmpty(controllerName))
                throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.NotFound,
                    string.Format("No route providing a controller name was found to match request URI '{0}'", new object[] { request.RequestUri })));

            HttpControllerDescriptor value;
            if (!this._controllersDicts.Value.TryGetValue(controllerName, out value))
                throw new HttpResponseException(request.CreateErrorResponse(HttpStatusCode.NotFound,
                    string.Format("No route providing a controller name was found to match request URI '{0}'", new object[] { request.RequestUri })));

            return value;
        }

        public override IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            //return base.GetControllerMapping();
            var cacheList = this._controllerInfoCache.Value.ToDictionary((KeyValuePair<string, HttpControllerDescriptor> c) => c.Key, (KeyValuePair<string, HttpControllerDescriptor> c) => c.Value, StringComparer.OrdinalIgnoreCase);
            var dictList = this._controllersDicts.Value;
            return dictList;
        }

        private Dictionary<string, HttpControllerDescriptor> InitializeControllerDictionary()
        {
            IAssembliesResolver assembliesResolver = this._configuration.Services.GetAssembliesResolver();
            IHttpControllerTypeResolver httpControllerTypeResolver = this._configuration.Services.GetHttpControllerTypeResolver();
            ICollection<Type> controllerTypes = httpControllerTypeResolver.GetControllerTypes(assembliesResolver);
            //var types = controllerTypes.ToDictionary(m => m.Name.Substring(0, m.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length), m => new HttpControllerDescriptor(this._configuration, m.Name.Substring(0, m.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length), m));


            var dictionary = new Dictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);
            var hashSet = new HashSet<string>();
            foreach (var type in controllerTypes)
            {
                var assemblyName = type.Assembly.GetName().Name;
                var controllerName = type.Name.Remove(type.Name.Length - ControllerSuffix.Length);
                var key = string.Format("{0}.{1}", assemblyName, controllerName);

                if (dictionary.Keys.Contains(key))
                {
                    hashSet.Add(key);
                    continue;
                }
                dictionary.Add(key, new HttpControllerDescriptor(this._configuration, controllerName, type));
            }

            foreach (var item in hashSet)
            {
                dictionary.Remove(item);
            }
            return dictionary;


            //return types;
        }

        private ConcurrentDictionary<string, HttpControllerDescriptor> InitializeControllerInfoCache()
        {
            IAssembliesResolver assembliesResolver = this._configuration.Services.GetAssembliesResolver();
            IHttpControllerTypeResolver httpControllerTypeResolver = this._configuration.Services.GetHttpControllerTypeResolver();
            ICollection<Type> controllerTypes = httpControllerTypeResolver.GetControllerTypes(assembliesResolver);
            IEnumerable<IGrouping<string, Type>> source = controllerTypes.GroupBy((Type t) => t.Name.Substring(0, t.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length), StringComparer.OrdinalIgnoreCase);


            ConcurrentDictionary<string, HttpControllerDescriptor> concurrentDictionary = new ConcurrentDictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> hashSet = new HashSet<string>();
            Dictionary<string, ILookup<string, Type>> cache = source.ToDictionary((IGrouping<string, Type> g) => g.Key, (IGrouping<string, Type> g) => g.ToLookup((Type t) => t.Namespace ?? string.Empty, StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase);

            foreach (KeyValuePair<string, ILookup<string, Type>> current in cache)
            {
                string key = current.Key;
                foreach (IGrouping<string, Type> current2 in current.Value)
                {
                    foreach (Type current3 in current2)
                    {
                        if (concurrentDictionary.Keys.Contains(key))
                        {
                            hashSet.Add(key);
                            break;
                        }
                        concurrentDictionary.TryAdd(key, new HttpControllerDescriptor(this._configuration, key, current3));
                    }
                }
            }

            foreach (string current4 in hashSet)
            {
                HttpControllerDescriptor httpControllerDescriptor;
                concurrentDictionary.TryRemove(current4, out httpControllerDescriptor);
            }
            return concurrentDictionary;
        }

        public override string GetControllerName(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            IHttpRouteData routeData = request.GetRouteData();
            if (routeData == null)
            {
                return null;
            }

            var assemblyName = this.GetRouteVariable<string>(request, "namespace");
            if (assemblyName == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var controllerName = this.GetRouteVariable<string>(request, "controller");
            if (controllerName == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return string.Format("{0}.{1}", assemblyName, controllerName);
        }

        private T GetRouteVariable<T>(HttpRequestMessage request, string key)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var routeData = request.GetRouteData();
            if (routeData == null)
            {
                return default(T);
            }

            object value;
            if (!routeData.Values.TryGetValue(key, out value))
                return default(T);

            return (T)value;
        }
    }
}
