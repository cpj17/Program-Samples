using System;

namespace ASPControlUtils
{
    public class ClsASPUtils
    {
        public static void DisableASPControls(params object[] objControls)
        {
            SetControlProperty("Enabled", false, objControls);
        }

        public static void EnableASPControls(params object[] objControls)
        {
            SetControlProperty("Enabled", true, objControls);
        }

        public static void ShowASPControls(params object[] objControls)
        {
            SetControlProperty("Visible", true, objControls);
        }

        public static void HideASPControls(params object[] objControls)
        {
            SetControlProperty("Visible", false, objControls);
        }

		//public static void SetControlProperty(string propertyName, bool value, params object[] objControls)  //this also working
        public static void SetControlProperty(string propertyName, bool value, object[] objControls)
        {
            foreach (object objControl in objControls)
            {
                Type controlType = objControl.GetType();
                var property = controlType.GetProperty(propertyName);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(objControl, value);
                }
            }
        }

        private static void ShowAlertMessage(string message)
        {
            // Implement your alert message logic here
            Console.WriteLine(message); // Placeholder implementation
        }
    }
}