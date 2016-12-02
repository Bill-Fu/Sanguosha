----------------------------------------------
         PCI: Procedural Creations
       Copyright © 2013 PCI Software
               Version 1.01
           paul.c.isaac@gmail.com
         http://pcisoft.boards.net
----------------------------------------------

Thank you for purchasing this package!
I am commited to making sure you get a great value.
If you have any questions, suggestions, comments or feature requests, please send me a message.

How-To-Use:

I suggest you start by loading the Examples\Blackjack scene a pressing (>) play button in the editor
to see what the cards look like in action. The game starts when you mouse-click on the RESET button
and then press HIT until you decide to STAY ala basic blackjack rules.

All the game scripts are included and documented for you to study.

How-To-Rebuild-The-Texture-Atlas:

Select the Cards\AtlasDesc1 asset in the Project tab.
The inspector window should show that the prefab contains an Atlas Desc script.

Atlas - reference to the texture atlas that will be populated with sub-texture locations
Target - reference to the texture image that will be generated and packed with sub-textures
Padding - this value (in pixels) affects optional spacing between sub-textures
List - contains the list of sub-texture descriptions
  Atlas Shape - name for sub-texture to be identified in the atlas
  Image - reference to an isolated texture that will be packed into the atlas
  Border Pixels - number of transparent pixels around sub-texture
     * advanced feature can be used to deal with edge flash caused by mip-maps
     * depends on how extreme your application is at render near/far cards
     * UV coordinates will be adjusted inward to isolate the sub-textures from adjacent images

  The [Generate] button at the bottom of the Atlas Desc inspector is used to rebuild the atlas.
  Pressing this will take a second or two to complete. It will repack the images and update the target
  texture and rebuild the list of items in the Atlas for use by the card generator.

How-To-Make-Specific-Cards

You can loads the Examples\Prefabs scene to see some premade cards.

To create a new card from scratch...
1) Create and empty game object in your scene
2) Add the Card component to the object
3) Set the Atlas property to the Cards\Atlas1 object
4) Set the Stock property to the Cards\Stock1 object
5) Set other properties as desired (ex. Text=2 Symbol=Heart Pattern=2)
6) The Image property can be left blank for most cards.
7) Press the [Generate] button

If the generated card appears scrambled, try rebuilding the Atlas (above) and then re-generate the card.

Enjoy.
