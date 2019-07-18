# WinUI Redline
An app that generates design values for WinUI.

## Intended target audiences
- Application creators who want to examine how WinUI controls are put together.
- WinUI documentation creators for control pages that may benefit from having a detailed redline.

## Initial scope
For initial project, we are targeting following controls:
- Button
- Checkbox
- MenuFlyout
- Slider

## What should one expect from each page/view?
- Controls in all the states
- Dark/light – side by side
- Redline
  - Color – SystemThemeBrush + Hex value
  - Size (includes thickness and corner radius)
  - Spacing
  - Text style

## Not in the initial scope, but interesting future ideas
- Having a single page with all the controls in view where the individual pages created here would be linked from
- We do not expect to be able to cover High Contrast view for now
- Redline to help developers identify the right light weight style value
