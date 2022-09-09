#if UIWIDGETS_DATABIND_SUPPORT
namespace UIWidgets.DataBind
{
	using Slash.Unity.DataBind.Foundation.Setters;
	using UnityEngine;

	/// <summary>
	/// Set the Value of a Spinner depending on the System.Int32 data value.
	/// </summary>
	[AddComponentMenu("Data Bind/New UI Widgets/Setters/[DB] Spinner Value Setter")]
	public class SpinnerValueSetter : ComponentSingleSetter<UIWidgets.Spinner, int>
	{
		/// <inheritdoc />
		protected override void UpdateTargetValue(UIWidgets.Spinner target, int value)
		{
			target.Value = value;
		}
	}
}
#endif