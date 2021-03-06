﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

using HydroNumerics.Geometry;
using HydroNumerics.Geometry.Net.VirtualEarthImage;


namespace HydroNumerics.Geometry.Net
{
  public class Map
  {

    public static BitmapImage GetImagery(XYPoint point, double dx, double dy)
    {
      
      try
      {
        string key = "ArfYQbvMgvi7NOpxSAuS3lkBlVc3NqzgBcVo-yxNyr_KPVJbhwR22c9cVfG7AnwY";
        MapUriRequest mapUriRequest = new MapUriRequest();

        // Set credentials using a valid Bing Maps key
        mapUriRequest.Credentials = new Credentials();
        mapUriRequest.Credentials.ApplicationId = key;

        // Set the location of the requested image
        mapUriRequest.Center = new Location();
        mapUriRequest.Center.Latitude = point.Latitude;
        mapUriRequest.Center.Longitude = point.Longitude;

        // Set the map style and zoom level
        MapUriOptions mapUriOptions = new MapUriOptions();
        mapUriOptions.Style = MapStyle.AerialWithLabels_v1;
        mapUriOptions.ZoomLevel = 13;

        // Set the size of the requested image in pixels
        mapUriOptions.ImageSize = new SizeOfint();
        mapUriOptions.ImageSize.Height = (int)(dy / 19.11); //19.11 to get meters
        mapUriOptions.ImageSize.Width = (int)(dx / 19.11);


        mapUriRequest.Options = mapUriOptions;
        //Make the request and return the URI
        ImageryServiceClient imageryService = new ImageryServiceClient("BasicHttpBinding_IImageryService");
        MapUriResponse mapUriResponse = imageryService.GetMapUri(mapUriRequest);
        return new BitmapImage(new Uri(mapUriResponse.Uri));      
      }
      catch (Exception e)
      {
        return null;
      }
    }

    static void b_DownloadCompleted(object sender, EventArgs e)
    {
      throw new NotImplementedException();
    }
  }
}
