using Android.App;
using Android.Telecom;
using Android.Widget;
using System;
using Android.Media;
using Android.Content;
using Android.OS;

namespace App3
{
    internal class CallService : InCallService
    {
        private const string ChannelID = "xxxxyyyy09022022";
        static readonly int NOTIFICATION_ID = 1001;

        public CallService()
        {
        }

        override
        public void OnCallAdded(Call call)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }
             Context context = Android.App.Application.Context;
            // Toast.MakeText(this, "incomming call received", ToastLength.Long);
            NotificationChannel channel = new NotificationChannel(ChannelID, "Incommin call", NotificationImportance.Max);
            // other channel setup stuff goes here.

            // We'll use the default system ringtone for our incoming call notification channel.  You can
            // use your own audio resource here.
            var ringtoneUri = RingtoneManager.GetDefaultUri(RingtoneType.Ringtone);
            channel.SetSound(ringtoneUri, new AudioAttributes.Builder()
                     // Setting the AudioAttributes is important as it identifies the purpose of your
                     // notification sound.
                     .SetUsage(AudioUsageKind.NotificationRingtone)
                     .SetContentType(AudioContentType.Sonification)
                 .Build());

            NotificationManager mgr = (NotificationManager)GetSystemService(NotificationService);
            mgr.CreateNotificationChannel(channel);
         
            Intent intent = new Intent(Intent.ActionMain, null);
            intent.SetFlags(ActivityFlags.NoUserAction | ActivityFlags.NewTask);
            intent.SetClass(context, typeof(DialarActivity));

            PendingIntent pendingIntent = PendingIntent.GetActivity(context, 1, intent, PendingIntentFlags.Mutable);

            // Build the notification as an ongoing high priority item; this ensures it will show as
            // a heads up notification which slides down over top of the current content.

            var builder = new AndroidX.Core.App.NotificationCompat.Builder(this, ChannelID);
                     builder.SetOngoing(true);
                     builder.SetPriority(((int)NotificationPriority.Max));

                     // Set notification content intent to take user to the fullscreen UI if user taps on the
                     // notification body.
                     builder.SetContentIntent(pendingIntent);
                     // Set full screen intent to trigger display of the fullscreen UI when the notification
                     // manager deems it appropriate.
                     builder.SetFullScreenIntent(pendingIntent, true);

                     // Setup notification content.
                    // builder.SetSmallIcon(yourIconResourceId );
                     builder.SetContentTitle("Incomming call");
                     builder.SetContentText("you are receiving incommin call from xxx91");

            // Use builder.addAction(..) to add buttons to answer or reject the call.

            //NotificationManager notificationManager = context.GetSystemService(typeof(NotificationManager));
               mgr.Notify(NOTIFICATION_ID, builder.Build());
           

        }
        override
        public void OnCallRemoved(Call Call)
        {
           // Toast.MakeText(this, "incomming call ended", ToastLength.Long);
        }
    }
}