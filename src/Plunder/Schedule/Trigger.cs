﻿using Plunder.Download;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Plunder.Schedule
{
    public class Trigger
    {
        private SchedulerContext _context;
        private readonly List<IDownloaderOld> _downloaders;
        
        private readonly ConcurrentDictionary<string, IDownloader> _downloaderCollection;
        private readonly int _maxDownloadThreadNumber;
        private AutoResetEvent _messagePullAutoResetEvent;
        private bool _pulling;

        public Trigger(SchedulerContext schedulerContext, int maxDownLoadThreadNumber)
        {
            if (schedulerContext == null)
                throw new ArgumentNullException(nameof(schedulerContext));
            _context = schedulerContext;
            _maxDownloadThreadNumber = maxDownLoadThreadNumber;
            _messagePullAutoResetEvent = new AutoResetEvent(false);
        }

        public int DownloadingTaskCount()
        {
            return _downloaders.Count();
        }

        public void Start()
        {
            PullMessage();
        }

        public void Stop()
        {
            _stopPull = true;
        }
        private bool _stopPull;

        private bool _first = true;

        private void PullMessage()
        {
            while (true)
            {
                if (_stopPull)
                    break;
                if (!_first)
                {
                    _messagePullAutoResetEvent.WaitOne();
                    _first = false;
                }

                var downloadingNumber = DownloadingTaskCount();

                if (_pulling || downloadingNumber >= _maxDownloadThreadNumber)
                {
                    _messagePullAutoResetEvent.Reset();
                    continue;
                }

                _pulling = true;
                var message = _context.Scheduler.Poll();
                _pulling = false;

                if (message == null)
                    message = _context.Scheduler.WaitUntillPoll();
                _messagePullAutoResetEvent.Reset();

                Task.Run(async () => {
                    await WorkingAsync(message);
                });
                
            }
        }

        private async Task WorkingAsync(params RequestMessage[] messages)
        {
            if (messages == null || !messages.Any())
                return;

            foreach (var item in messages)
            {
                var downloader = _context.DownloaderFactory.Create(item.Request.PageType);
                _downloaderCollection.TryAdd(item.Request.Id, downloader);
                await downloader.DownloadAsync()
                    .ContinueWith(t =>
                    {
                        Downloaded();
                        return t.Result;
                    }).ContinueWith(t => {

#if DEBUG
                        Console.WriteLine("Downloaded:" + t.Result.Request.Url);
#endif

                        var pageAnalyzer = _context.PageAnalyzerFactory.Create(t.Result.Request.SiteId, t.Result.Request.Channel);
                        var pageResult = pageAnalyzer.Analyze(t.Result);
                        _context.ResultPipeline.Inject(pageResult);
                    });
            }
        }

        private void Downloaded()
        {
            //ConsumeTotal++;
            _messagePullAutoResetEvent.Set();
        }
    }
}
