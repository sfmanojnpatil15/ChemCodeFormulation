# BPG Costing - Salesforce Migration

Migrated from VB.NET + MySQL desktop application to Salesforce Lightning platform.

## Architecture Overview

### VB.NET → Salesforce Mapping

| VB.NET Component | Salesforce Component |
|---|---|
| `LoginForm.vb` | Salesforce standard authentication |
| `Dashboard.vb` | `formulaDashboard` LWC |
| `Formulator2.vb` | `formulaEditor` LWC + `FormulaController.cls` |
| `NewMaterial.vb` / `EditMaterial.vb` | `materialManager` LWC + `MaterialController.cls` |
| `SpecificationsForm.vb` | Salesforce Reports / Quote PDF |
| `SalesRepDetails.vb` | `Sales_Rep__c` object |
| `PriceTags.vb` | Sales Pricing grid in `salesPricingGrid` LWC |
| `UpdateFreightCost.vb` | Settings tab in `formulaDashboard` LWC |
| `Helper.vb` cost calculations | `CostCalculationService.cls` |

### Database → Salesforce Objects

| MySQL Table | Salesforce Object |
|---|---|
| `UserList` | Salesforce User (standard) |
| `Formulas` | `Formula__c` |
| `Material` | `Material__c` |
| `FormulaDetails` | `Formula_Ingredient__c` |
| `FormulaVersions` | `Formula_Version__c` |
| `BlendingDetails` | `Blending_Detail__c` |
| `TabletingDetails` | `Tableting_Detail__c` |
| `EncapsulationDetails` | `Encapsulation_Detail__c` |
| `BottlePackagingDetails` | `Bottle_Packaging__c` |
| `StickPackDetails` | `Stick_Pack_Detail__c` |
| `SachetsDetails` | `Sachet_Detail__c` |
| `StandUpBagDetails` | `Stand_Up_Bag_Detail__c` |
| `blisterdetails` | `Blister_Detail__c` |
| `SalesDetails` | `Sales_Detail__c` |
| `salesreps` | `Sales_Rep__c` |
| `tblformualsettings` | `Formula_Settings__c` |

## Custom Objects

### `Material__c`
Catalog of all raw materials and packaging materials.
- Fields: Name (Material Name), Supplier, Price (per kg), Category, Component Code, Case Pack, Vendor Code, Is Active

### `Formula__c`
Main formula record with header information.
- Fields: Name (Formula Name), Formula Type (Powder/Tablet/Capsule), Customer Name, Sales Rep (lookup), Quote Number (auto), Serving Size, Servings Per Container, Total Unit Cost, Freight Cost, Entered Date, Terms & Conditions fields

### `Formula_Version__c`
Tracks multiple versions of the same formula (child of Formula__c).
- Fields: Version Number, Is Current, Version Notes

### `Formula_Ingredient__c`
Each raw material ingredient in a formula version (child of Formula__c).
- Fields: Material (lookup), Material Name, Component Code, Vendor Name, Actual mg, Potency %, Overage %, mg (calculated), Material Cost, Is Manual, Manual Cost, Latest Cost

### `Blending_Detail__c`
Blending/powder manufacturing process costs (child of Formula__c).
- Fields: Blending Cost, Wastage %, Wastage Value, Lab Cost, Flavor Profile Cost

### `Tableting_Detail__c`
Tablet compression and coating costs (child of Formula__c).
- Fields: Type of Coating, Coating Cost, Compression Cost, Wastage %, Lab Cost

### `Encapsulation_Detail__c`
Capsule filling process costs (child of Formula__c).
- Fields: Type of Capsule, Capsule Cost, Encapsulation Cost, Wastage %, Lab Cost, Color

### `Bottle_Packaging__c`
Bottle packaging component costs per size (child of Formula__c).
- Fields: Binding Index, Size Count, Category, Material Name, Vendor Name, Unit Cost, Component Code, Is Default

### `Stick_Pack_Detail__c`
Stick pack packaging costs and specifications (child of Formula__c).
- Fields: Size Count, Material, Quantity, Filling Cost, Printing Cost, Shrink Wrap, Wafer Seal, Art Preparation, R&D Bulk, R&D Box, Display Box details

### `Sachet_Detail__c`
Sachet packaging costs (child of Formula__c) — same structure as Stick Pack.

### `Stand_Up_Bag_Detail__c`
Stand-up bag packaging costs (child of Formula__c).
- Fields: Bag Material, Bag Size, Fill Weight, Bag Cost, Printing, Shipper details, R&D

### `Blister_Detail__c`
Blister pack costs (child of Formula__c).
- Fields: Blister Count, Blister Cost, Pack Out, Shrink Wrap, Wafer Seal, Printing, Tooling Cost, Shipper details, R&D

### `Sales_Detail__c`
Sales pricing by quantity and margin (child of Formula__c).
- Fields: Sale Type (BULK/BOX/BULKBAGS), Unit Size, Quantity, Margin %, Sales Price, Overridden Price

### `Sales_Rep__c`
Sales representative information.
- Fields: Name, Rep Code, Email, Phone, Company

### `Formula_Settings__c`
Global application settings (freight rate, price age thresholds).
- Fields: Freight Percentage, New Price Days (30), Mid Price Days (60), Old Price Days (61)

## Apex Classes

### `FormulaController.cls`
Main controller for all formula operations:
- `getFormulas()` — Dashboard formula list with filters
- `getFormulaDetail()` — Full formula with versions and ingredients
- `createFormula()` / `updateFormula()` / `deleteFormula()` — CRUD
- `getFormulaVersions()` / `createNewVersion()` — Version management
- `getFormulaIngredients()` / `saveFormulaIngredients()` — Ingredient CRUD
- `getBlendingDetail()` / `saveBlendingDetail()` — Blending costs
- `getTabletingDetail()` / `saveTabletingDetail()` — Tableting costs
- `getEncapsulationDetail()` / `saveEncapsulationDetail()` — Capsule costs
- `getBottlePackaging()` / `saveBottlePackaging()` — Bottle packaging
- `getStickPackDetails()` / `saveStickPackDetails()` — Stick pack details
- `getSachetDetails()` / `saveSachetDetails()` — Sachet details
- `getStandUpBagDetails()` / `saveStandUpBagDetails()` — Stand-up bag details
- `getBlisterDetails()` / `saveBlisterDetails()` — Blister details
- `getSalesDetails()` / `saveSalesDetails()` — Sales pricing
- `getSalesReps()` / `saveSalesRep()` — Sales rep management
- `getFormulaSettings()` / `saveFormulaSettings()` — Global settings

### `MaterialController.cls`
Material catalog management:
- `getMaterials()` — All materials with category filter
- `getRawMaterials()` — RM category only
- `searchMaterials()` — Live search for ingredient lookup
- `getBottleMaterials()` / `getStickPackMaterials()` / etc. — Category-specific lists
- `saveMaterial()` — Create/update material
- `deactivateMaterial()` — Soft delete

### `CostCalculationService.cls`
Core costing engine (mirrors VB.NET Formulator2.vb calculations):
- `calculateMgWithOverage()` — mg = (ActualMg / Potency) × (1 + Overage%)
- `calculateIngredientCost()` — Cost = (mg/1,000,000) × PricePerKg × (1 + Freight%)
- `calculateTotalMaterialCost()` — Sum all ingredient costs
- `calculateWastageValue()` — Wastage = MaterialCost × Wastage%
- `calculatePowderUnitCost()` — Powder: Material + Blending + Wastage + Lab + Flavor
- `calculateTabletUnitCost()` — Tablet: Material + Compression + Coating + Wastage + Lab
- `calculateCapsuleUnitCost()` — Capsule: Material + Capsule + Encapsulation + Wastage + Lab
- `calculateSalesPrice()` — SalesPrice = UnitCost / (1 - Margin%)
- `calculateMarginFromPrice()` — Margin = (1 - UnitCost/SalesPrice) × 100
- `calculateFullUnitCost()` — Full cost calculation dispatcher
- `getPriceAgeClass()` — Green/Yellow/Red price staleness indicator

## Lightning Web Components

### `formulaDashboard`
Main application entry point (replaces Dashboard.vb).
- Formulas tab: searchable/filterable grid with date range filter
- Materials tab: material list with add/edit/deactivate
- Sales Reps tab: sales rep management
- Settings tab: freight percentage and price age thresholds

### `formulaEditor`
Full formula editor (replaces Formulator2.vb).
- Ingredients tab: editable ingredient grid with material lookup, mg/cost auto-calculation, price age color coding
- Manufacturing tab: Blending / Tableting / Encapsulation based on formula type
- Packaging tab: delegates to `packagingEditor` component
- Sales Pricing tab: delegates to `salesPricingGrid` component
- Terms & Conditions tab: legal text fields

### `materialManager`
Material create/edit form (replaces NewMaterial.vb / EditMaterial.vb).

### `packagingEditor`
Packaging cost entry for all packaging types:
- Bottles: component grid per size
- Stick Packs / Sachets: datatable with all cost fields
- Stand-Up Bags: datatable with bag specs
- Blisters: form with all blister cost fields

### `salesPricingGrid`
Sales pricing grid with BULK / BOX / BULKBAGS tabs.
Auto-calculates sales price from unit cost + margin %.

## Deployment

```bash
# Install Salesforce CLI
npm install -g @salesforce/cli

# Authenticate to org
sf org login web -a MyOrg

# Deploy metadata
sf project deploy start --source-dir force-app --target-org MyOrg

# Assign permission set to users
sf org assign permset --name BPG_Costing_Users --target-org MyOrg
```

## Key Business Logic Preserved

1. **Cost Formula**: `MaterialCost = (mg ÷ 1,000,000) × PricePerKg × (1 + Freight%)`
2. **mg Calculation**: `mg = (ActualMg ÷ Potency%) × (1 + Overage%)`
3. **Unit Cost by Type**:
   - Powder: `Material + Blending + Wastage(Material+Blending) + LabCost + FlavorProfile`
   - Tablet: `Material + Compression + Coating + Wastage(Material+Compression+Coating) + LabCost`
   - Capsule: `Material + CapsuleCost + EncapsulationCost + Wastage(...) + LabCost`
4. **Sales Price**: `SalesPrice = UnitCost ÷ (1 - Margin%)`
5. **Price Age**: Green (< 30 days), Yellow (30-60 days), Red (> 60 days)
6. **Quote Number**: Auto-incremented starting at 1000 (e.g. QT-0001000)
