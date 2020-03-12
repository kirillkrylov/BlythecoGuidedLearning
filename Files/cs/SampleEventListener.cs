using global::Common.Logging;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Entities.Events;

namespace GuidedLearningClio.Files.cs.el
{
    /// <summary>
    /// Listener for 'EntityName' entity events.
    /// </summary>
    /// <seealso cref="Terrasoft.Core.Entities.Events.BaseEntityEventListener" />
    [EntityEventListener(SchemaName = "Contact")]
    class SampleEventListener : BaseEntityEventListener
    {
        private static readonly ILog _log = LogManager.GetLogger("GuidedLearningLogger");
        public override void OnSaved(object sender, EntityAfterEventArgs e)
        {
            base.OnSaved(sender, e);
            Entity entity = (Entity)sender;

            UserConnection userConnection = entity.UserConnection;
            string message = $"Changing name for {entity.GetTypedColumnValue<string>("Name")}";
            _log.Info(message);
            
        }

        public override void OnSaving(object sender, EntityBeforeEventArgs e)
        {
            base.OnSaving(sender, e);
            Entity entity = (Entity)sender;
            UserConnection userConnection = entity.UserConnection;

            
            


            string oldValue = entity.GetTypedOldColumnValue<string>("Name");
            string newValue = entity.GetTypedColumnValue<string>("Name");

            if (!newValue.StartsWith("A"))
            {
                newValue = "A" + newValue;
                entity.SetColumnValue("Name", newValue);
                entity.Save();
                string msg = "Name did not start with an A, so I prefixed an A";


                //e.IsCanceled = true;
                //string msg = "Save canceled because name did not start with an A";
                _log.Info(msg);
            }


            string message = $"Changing name from {oldValue} to {newValue}";
            _log.Info(message);
        }





    }
}