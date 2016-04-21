# CompositeHomeScreen
__CompositeHomeScreen__ is a mockup of the iPhone home screen created using <a href="https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.composition.aspx">__Windows.UI.Composition__ </a> API

<img src="https://cloud.githubusercontent.com/assets/7021835/14696195/e990ce0e-072d-11e6-87d0-eaff3a90080a.gif" />

# CompositeHomeScreen Demystified
This project mainly contains a HomeScreen page which hosts the Menu and the subsequent child pages. The main constituents of the HomeScreen page can be see in the image below.

<img src="https://cloud.githubusercontent.com/assets/7021835/14698562/f095e54c-0744-11e6-9294-ed27d66be61a.png" />

In the layout, 
- __MenuGrid__ hosts the icons
- __CompositionGrid__ is where the visuals are created and animated
- __ContentFrame__ is where the Page, corresponding to the icon clicked by the user, is loaded.

Animation is triggered by two events -
- When the user clicks on any of the icons displayed in the __MenuGrid__
  * The __MenuGrid__ scales up and fades out.
  * A white rectangular visual (having the same size as the icon) is created and placed in the __CompositionGrid__ at the selected icon's location. It is then scaled up and faded in to fit the size of the __MenuGrid__.
  * The __ContentGrid__ also scales up and fades in to fit the size of the __MenuGrid__.
  * Once the animation completes, the __ContentFrame__ navigates to the appropriate content page. (__Note__: _There is a flag_ ***_loadContentDuringAnimation*** *which when set to __true__ will navigate to the content page as soon as the animation starts. If set to __false__, it will navigate to the content page only after the animation batch completes*)
- When the user presses the Home button (at the bottom)
  * The __ContentGrid__ scales down and fades out.
  * The white rectangular visual also scales down and fades out.
  * The __MenuGrid__ scales down and fades in
  * Once the animation completes, the white rectangular visual is removed from the CompositionGrid
  
## ScopedBatchHelper
In this example, I have used the __ScopedBatchHelper__ class of the <a href="https://github.com/ratishphilip/CompositionExpressionToolkit">__CompositionExpressionToolkit__ </a> (another project of mine). __ScopedBatchHelper__ helps you to batch together & begin a set of animations and also define action(s) to be performed once the animation batch completes. It internally subscribes to the Completed event of the __CompositionScopedBatch__ before beginnning the animation and once the batch completes, it performs the post-action (if defined) and unsubscribes from the Completed event.  
This way, the user needs to focus on defining just the action and post actions while being spared from the trouble of subscribing (and unsubscribing) from the __CompositionScopedBatch.Completed__ event.
