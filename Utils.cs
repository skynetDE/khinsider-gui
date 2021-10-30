using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace khi
{
    internal class Utils
    {
        public static void ShowWaitDialog(Form source, Action codeToRun, string title)
        {
            var dialogLoadedFlag = new ManualResetEvent(false);

            // open the dialog on a new thread so that the dialog window gets
            // drawn. otherwise our long running code will run and the dialog
            // window never renders.
            new Thread(() =>
            {
                var waitDialog = new Form()
                {
                    Name = "WaitForm",
                    Text = title,
                    ControlBox = false,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterParent,
                    Width = 240,
                    Height = 50,
                    Enabled = true
                };

                var unused = new ProgressBar()
                {
                    Style = ProgressBarStyle.Marquee,
                    Parent = waitDialog,
                    Dock = DockStyle.Fill,
                    Enabled = true
                };

                waitDialog.Load += (x, y) => { dialogLoadedFlag.Set(); };

                waitDialog.Shown += (x, y) =>
                {
                    // note: if codeToRun function takes a while it will 
                    // block this dialog thread and the loading indicator won't draw
                    // so launch it too in a different thread
                    new Thread(() =>
                    {
                        codeToRun();

                        // after that code completes, kill the wait dialog which will unblock 
                        // the main thread
                        source.Invoke((MethodInvoker) (() => waitDialog.Close()));
                    }).Start();
                };

                source.Invoke((MethodInvoker) (() => waitDialog.ShowDialog(source)));
            }).Start();

            while (dialogLoadedFlag.WaitOne(100, true) == false)
                Application.DoEvents(); // note: this will block the main thread once the wait dialog shows
        }

        public static async Task DownloadFileAsync(HttpClient client, Uri uri, IProgress<DownloadProgress> progress,
            CancellationToken cancellationToken, string fileName)
        {
            using var response = await client
                .GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"The request returned with HTTP status code {response.StatusCode}");

            var total = response.Content.Headers.ContentLength ?? -1L;
            var canReportProgress = total != -1 && progress != null;

            using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var totalRead = 0L;
            var buffer = new byte[4096];

            using var fs = new FileStream(fileName, FileMode.CreateNew);
            var read = 0;

            while ((read = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)
                .ConfigureAwait(false)) > 0)
            {
                await fs.WriteAsync(buffer, 0, read, cancellationToken).ConfigureAwait(false);

                totalRead += read;

                if (canReportProgress)
                    progress.Report(new DownloadProgress
                    {
                        TotalBytesToReceive = total,
                        BytesReceived = totalRead,
                        Pct = 100 * (totalRead * 1d) / (total * 1d)
                    });
            }
        }

        internal class DownloadProgress
        {
            public long TotalBytesToReceive { get; set; }
            public long BytesReceived { get; set; }
            public double Pct { get; set; }
        }
    }
}