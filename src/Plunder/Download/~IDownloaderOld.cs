﻿//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Plunder.Compoment;

//namespace Plunder.Download
//{
//    public interface IDownloaderOld
//    {
//        void ReInit();

//        PageType PageType { get; }

//        bool IsDefault { get; set; }

//        bool IsAllowDownload();

//        int DownloadingTaskCount { get;}


//        //Task DownloadAsync(Request requests, Action<Request, Response> onDownloaded);

//        Task<Tuple<Request, Response>> DownloadAsync(Request request);
//    }
//}