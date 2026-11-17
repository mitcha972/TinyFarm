# Tiny Farm cleanup notes

All gameplay scripts compile against the installed Unity 2022.3.4f1 assemblies. An isolated Unity Editor run passed 53 behavior assertions; results are in `Temp/CleanupChecks/results.txt`. Checks covered money/inventory validation, read-only inventory, selection toggles, bag and market catch-up, planting and watering, stall reservations and returns, order completion and inventory colors, and page wrap-around. These checks did not exercise the original scene's Inspector wiring or pointer/UI raycasts.

Changes were compile-checked incrementally. Per-step commits were skipped following the follow-up instruction. Scenes and prefabs were left unchanged. Four renamed scripts retain their original metadata contents and GUIDs. Transparency sorting was already Custom Axis (0,1,0).

## Per-file changes

| File | Change |
|---|---|
| Assets/Scripts/FarmManager.cs | Added ToolType and checked spending, removal, and selling. Fixed selecting the same seed. Exposed Money/Level and a read-only inventory view. Kept the integer SelectTool callback for existing Buttons. Added serialized-name migration attributes. |
| Assets/Scripts/PlantItem.cs | Explicit Setup and SetSelected; renamed BuyPlant to OnClickSelect; removed manager search and made UI references private. |
| Assets/Scripts/PlantStoreManager.cs | Renamed PlantManager. Loads/sorts plants in Awake; creates only unlocked UI items in Start and explicitly activates them even when the source prefab is inactive. |
| Assets/Scripts/PlotManager.cs | Removed plant-food remnants, fixed recursive hover input, blocked actions over UI, used checked purchases and PlotPrice, added a serialized plant renderer, and renamed Plant to PlantSeed. |
| Assets/Scripts/BagManager.cs | Loads existing inventory in Start and safely unsubscribes on destruction. |
| Assets/Scripts/UI/BagItem.cs | Removed dead state and commented-out code. |
| Assets/Scripts/Market/MarketItem.cs | Explicit Setup; all sales use TrySellPlant; selection closes safely. |
| Assets/Scripts/Market/MarketManager.cs | Removed self-reference and unused farm reference; safe backwards removal; initializes and updates inventory items without duplicates. |
| Assets/Scripts/Stall/StallItem.cs | Safe quantity parsing, checked reservation once at startup, one SellOne sale path, return of unsold goods on close, and guards against repeated setup/close. percentBonus is clamped to -20..20, and the price label shows the rounded price and signed percentage. SellPlant remains as a compatible callback that delegates to SellOne. |
| Assets/Scripts/Stall/StallManager.cs | Initializes stall references without global searches; routes timed sales through SellOne; renamed CancleStall to CancelStall. |
| Assets/Scripts/Market/OrderManager.cs | Creates in Start, rolls line count once per order, chooses distinct unlocked plants, calls Setup, and stores each order once. |
| Assets/Scripts/Market/OrderItem.cs | Setup copies and combines order lines, caches slots even when the parent tab is inactive, recalculates bonus price, subscribes to inventory changes, and prevents repeated completion/payment. Renamed CheckOrder to CompleteOrder and migrated percent fields. |
| Assets/Scripts/Core/TabController.cs | Modulo wrap-around, ShowPage helper, correct tab indexing, and consistent visibility. |
| Assets/Scripts/Core/StallSelectionOverlay.cs | Renamed ClickImage; null-safe close that hides the overlay. |
| Assets/Scripts/Kitchen/IngredientSlot.cs | Renamed RecipeObj; preserved metadata GUID. |
| Assets/Scripts/Kitchen/RecipeData.cs | Renamed RecipeObject; preserved metadata GUID. Added an outputPlant reference for the existing inventory type. |
| Assets/Scripts/Kitchen/RecipeManager.cs | Updated renamed types; loads data in Awake and creates UI in Start. Passes output data through RecipeItem.Setup. |
| Assets/Scripts/Kitchen/RecipeItem.cs | Replaced manager search, removed commented code, combined repeated ingredients, and guarded ingredient consumption against repeated clicks. Migrated precent to percentBonus. Baking now grants one configured Plant output, and missing outputs cannot consume ingredients. |
| Assets/Scripts/PlantFood(remove)/PlantFoodItem.cs | Removed the unused global manager search and obsolete commented statements. |
| Assets/Scripts/PlantFood(remove)/PlantFoodObject.cs | Removed the runtime script's unnecessary UnityEditor import. |
| Assets/Scripts/PlantFood(remove)/PlantFoodStoreManager.cs | Builds UI in Start, after loading data in Awake. |
| Assets/Resources/Recipes/Soup.asset | Reconnected the old embedded Corn ingredient to the existing Corn asset, and assigned the Soup output. |
| Assets/Resources/Recipes/SoupOutput.asset | Added Soup using the existing Plant type, with its existing recipe icon and a base sell price of $30 (the ingredient total). Kept outside the Plants resource folder so it is not offered as a seed. |
| Assets/Scripts/Kitchen/RequestTab.cs | Deleted the empty script and its metadata. |
| Assets/Scripts/Core/CameraMovement.cs | Deleted movement, zoom, and rotation logic and its metadata. No RotateFarm method was added because rotation was explicitly requested to be removed. |
| Assets/Scripts/Core/IsometricZ.cs | Deleted the script and its metadata. |
| Assets/Scripts/Remove/Plant(old).cs | Deleted the unused legacy Plantt class and its metadata. |

## Re-hook in Inspector

### Button callbacks requiring reconnection

- PlantItem.BuyPlant → PlantItem.OnClickSelect (`Assets/Prefabs/PlantItem.prefab`).
- OrderItem.CheckOrder → OrderItem.CompleteOrder (`Assets/Prefabs/OrderItem.prefab`).
- StallManager.CancleStall → StallManager.CancelStall (two callbacks found in `Assets/Scenes/MainScene.unity`).

PlotManager.Plant → PlotManager.PlantSeed is a private method rename; it needs no Button reconnection.

### New reference and deleted components

- On `Assets/Prefabs/Plot.prefab`, assign PlotManager.plantSpriteRenderer to the SpriteRenderer on its existing Plant child. Its current local fileID is 2919739941886435230. Instances inherit the prefab reference.
- Remove missing CameraMovement, IsometricZ, and RequestTab components wherever attached. The Plot prefab contains an IsometricZ component.
- Remove the obsolete RotateCamera and ToggleCameraMove Button callbacks found in MainScene. Rotation and camera movement were removed entirely.

### Renamed serialized fields (automatically migrated)

These retain their old values through FormerlySerializedAs and normally require no manual reconnection:

- FarmManager.selectPlant → SelectedSeed.
- FarmManager.buttonsImg → toolButtonImages.
- OrderItem.precent → percentBonus.
- OrderItem.precentTxt → percentTxt.
- RecipeItem.precent → percentBonus.
- StallItem.percent → percentBonus.

The renamed classes PlantManager → PlantStoreManager, RecipeObj → IngredientSlot, RecipeObject → RecipeData, and ClickImage → StallSelectionOverlay keep the same metadata GUIDs, so existing references should remain connected.

## Follow-up behavior

- Stall bonuses adjust the plant's base selling price: Mathf.RoundToInt(plant.SellPrice * (1 + percentBonus / 100f)). The bonus is clamped to -20..20 and shown with a sign, for example "$33 (+10%)".
- The Soup recipe consumes one Corn and produces one Soup after five seconds. Soup is a Plant asset, so bag, market, and stall sales use the existing code. Its base sell price is $30, matching the recipe's ingredient-total display before this change.
- New recipes must assign an outputPlant on RecipeData. RecipeManager passes it to RecipeItem automatically; Soup is already assigned. No new Button callback needs reconnecting for this follow-up.
## Order startup regression

Orders instantiated under an inactive tab can receive Setup before Unity invokes Awake. OrderItem now calls the same idempotent CacheSlots helper from both methods. The isolated regression reproduced the original NullReferenceException at slots.Length before the fix; it checks setup under a hidden parent and subsequent Awake initialization.

## Shop visibility regression

PlantItem.prefab is saved with its root inactive. PlantStoreManager now activates each instantiated item after Setup. Locked plants are still excluded before instantiation. The regression checks an inactive item template under a hidden shop tab, correct level filtering, visibility when opened, and no duplicate items when reopened. No scene or prefab edits were needed.
