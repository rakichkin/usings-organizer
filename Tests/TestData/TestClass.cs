﻿/*
 * 
 * Тестовый класс, предназначенный для проверки работы расширения напрямую через Visual Studio
 * 
 */

using EyeCont.Infrastructure.Swagger;

using System.Reflection;
using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;
using EyeCont.Framework.LogHelpers;
using Microsoft.AspNetCore.Builder;
using EyeCont.Service.Contracts.Configuration;


using Microsoft.Extensions.Hosting;
using Prometheus;
using EyeCont.Service.Extensions;
using Mallenom.Newtonsoft.Json.Convertors;
using EyeCont.Service.Contracts.Parameters;
using EyeCont.Service.Validations;


using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EyeCont.Framework;
using EyeCont.Infrastructure.Swagger.Callback;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;

using System.Linq;
using Newtonsoft.Json.Converters;
using System;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;
using Mallenom.Configuration.Json;
using Mallenom.Swashbuckle.AspNetCore;
using System.IO;

using Mallenom.Swashbuckle.AspNetCore.MultiPart;
using EyeCont.Common.Contracts;






namespace Tests
{
	internal class TestClass
	{
	}
}
