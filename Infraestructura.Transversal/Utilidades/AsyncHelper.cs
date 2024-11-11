using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Utilidades
{
    public static class AsyncHelper
    {
        private static readonly TaskFactory m_taskFactory = new TaskFactory(
            CancellationToken.None,
            TaskCreationOptions.None,
            TaskContinuationOptions.None,
            TaskScheduler.Default
        );

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            var cultureUi = CultureInfo.CurrentUICulture;
            var culture = CultureInfo.CurrentCulture;

            return m_taskFactory.StartNew(() =>
                {
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = cultureUi;

                    return func();
                })
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            var cultureUi = CultureInfo.CurrentUICulture;
            var culture = CultureInfo.CurrentCulture;

            m_taskFactory.StartNew(() =>
                {
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = cultureUi;

                    return func();
                })
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }
    }
}
