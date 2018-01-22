﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Unity;
using Unity.Exceptions;

namespace TPGPServices.App_Start
{
    public class UnityResolv: IDependencyResolver
    {
       
   
       protected IUnityContainer container;
       public UnityResolv(IUnityContainer container) {
           if (container == null) {
               throw new ArgumentNullException("container");
           }
           this.container = container;
       }
       public IDependencyScope BeginScope()
       {
           var child = container.CreateChildContainer();
           return new UnityResolv(child);
       }

       public void Dispose()
       {
           Dispose(true);
       }

       public object GetService(Type serviceType)
       {
           try
           {
               return container.Resolve(serviceType);
           }
           catch (ResolutionFailedException)
           {
               return null;
           }
       }

       public IEnumerable<object> GetServices(Type serviceType)
       {
           try {
               return container.ResolveAll(serviceType);
           } catch (ResolutionFailedException) {
               return new List<object>();
           }
       }

       protected virtual void Dispose(bool disposing) {
           container.Dispose();
       }
   }
   }
