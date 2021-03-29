﻿using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.SessionState;

namespace Uow.Core.Fakes
{
    public class FakeHttpContext : HttpContextBase
    {
        private readonly HttpCookieCollection _cookies;
        private readonly NameValueCollection _formParams;
        private readonly string _method;
        private readonly NameValueCollection _queryStringParams;
        private readonly string _relativeUrl;
        private readonly NameValueCollection _serverVariables;
        private readonly SessionStateItemCollection _sessionItems;
        private IPrincipal _principal;
        private HttpRequestBase _request;
        private HttpResponseBase _response;

        public FakeHttpContext(string relativeUrl, string method)
            : this(relativeUrl, method, null, null, null, null, null, null)
        {
        }

        public FakeHttpContext(string relativeUrl)
            : this(relativeUrl, null, null, null, null, null, null)
        {
        }

        public FakeHttpContext(string relativeUrl,
            IPrincipal principal, NameValueCollection formParams,
            NameValueCollection queryStringParams, HttpCookieCollection cookies,
            SessionStateItemCollection sessionItems, NameValueCollection serverVariables)
            : this(relativeUrl, null, principal, formParams, queryStringParams, cookies, sessionItems, serverVariables)
        {
        }

        public FakeHttpContext(string relativeUrl, string method,
            IPrincipal principal, NameValueCollection formParams,
            NameValueCollection queryStringParams, HttpCookieCollection cookies,
            SessionStateItemCollection sessionItems, NameValueCollection serverVariables)
        {
            _relativeUrl = relativeUrl;
            _method = method;
            _principal = principal;
            _formParams = formParams;
            _queryStringParams = queryStringParams;
            _cookies = cookies;
            _sessionItems = sessionItems;
            _serverVariables = serverVariables;

            Items = new Hashtable();
        }

        public override HttpRequestBase Request =>
            _request ??
            new FakeHttpRequest(_relativeUrl, _method, _formParams, _queryStringParams, _cookies, _serverVariables);

        public override HttpResponseBase Response => _response ?? new FakeHttpResponse();

        public override IPrincipal User
        {
            get => _principal;
            set => _principal = value;
        }

        public override HttpSessionStateBase Session => new FakeHttpSessionState(_sessionItems);

        public override IDictionary Items { get; }


        public override bool SkipAuthorization { get; set; }

        public static FakeHttpContext Root()
        {
            return new FakeHttpContext("~/");
        }

        public void SetRequest(HttpRequestBase request)
        {
            _request = request;
        }

        public void SetResponse(HttpResponseBase response)
        {
            _response = response;
        }

        public override object GetService(Type serviceType)
        {
            return null;
        }
    }
}