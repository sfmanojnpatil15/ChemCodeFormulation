/**
 * CostCalculationService
 * Handles all formula costing calculations.
 * Migrated from VB.NET Formulator2.vb cost calculation methods:
 *   - CalculateBlenderCost(), CalculateTabletingCost(), CalculateEncapsulationCost()
 *   - CalculateBottleCost(), CalculateStickPacksCost(), CalculateBlisterCost()
 *   - CalculateSachetsCost(), CalculateStandUpBagCost()
 *   - DisplayTotalUnitCost()
 */
public with sharing class CostCalculationService {

    /**
     * Wrapper holding all cost inputs for a formula calculation.
     */
    public class CostInput {
        @AuraEnabled public Id formulaId;
        @AuraEnabled public Integer versionNumber;
        @AuraEnabled public Decimal servingSize;         // mg per serving
        @AuraEnabled public Integer servingsPerContainer;
        @AuraEnabled public Decimal freightPercentage;   // e.g. 5.00 for 5%
        @AuraEnabled public List<IngredientCostInput> ingredients;
        @AuraEnabled public String formulaType;          // Powder, Tablet, Capsule
        @AuraEnabled public String packagingType;        // Bulk, Bottles, StickPacks, etc.

        // Manufacturing process costs
        @AuraEnabled public Decimal blendingCost;
        @AuraEnabled public Decimal blendingWastagePercent;
        @AuraEnabled public Decimal blendingFlavorProfile;
        @AuraEnabled public Decimal tabletingCoatingCost;
        @AuraEnabled public Decimal tabletingCompressionCost;
        @AuraEnabled public Decimal tabletingWastagePercent;
        @AuraEnabled public Decimal encapsulationCapsuleCost;
        @AuraEnabled public Decimal encapsulationCost;
        @AuraEnabled public Decimal encapsulationWastagePercent;
        @AuraEnabled public Decimal labCost;
    }

    public class IngredientCostInput {
        @AuraEnabled public Decimal actualMg;
        @AuraEnabled public Decimal potency;
        @AuraEnabled public Decimal overage;
        @AuraEnabled public Decimal pricePerKg;
        @AuraEnabled public Boolean isManual;
        @AuraEnabled public Decimal manualCost;
    }

    /**
     * Result wrapper with all calculated costs.
     */
    public class CostResult {
        @AuraEnabled public Decimal totalMaterialCostPerServing;
        @AuraEnabled public Decimal totalMaterialCostPerKg;
        @AuraEnabled public Decimal freightCost;
        @AuraEnabled public Decimal wastageValue;
        @AuraEnabled public Decimal manufacturingCost;
        @AuraEnabled public Decimal labCost;
        @AuraEnabled public Decimal totalUnitCost;
        @AuraEnabled public List<IngredientCostResult> ingredientCosts;
    }

    public class IngredientCostResult {
        @AuraEnabled public Decimal mgWithOverage;       // mg after applying overage %
        @AuraEnabled public Decimal materialCost;        // cost per serving for this ingredient
        @AuraEnabled public Decimal freightCost;
    }

    /**
     * Calculate the mg (with overage) for an ingredient.
     * Formula: mg = (ActualMg / Potency%) * (1 + Overage%)
     * Equivalent to the mg calculation in Formulator2.vb ingredient grid.
     */
    @AuraEnabled(cacheable=true)
    public static Decimal calculateMgWithOverage(Decimal actualMg, Decimal potency, Decimal overage) {
        if (actualMg == null || actualMg == 0) return 0;
        Decimal potencyFactor = (potency != null && potency > 0) ? (potency / 100) : 1;
        Decimal overageFactor = (overage != null && overage > 0) ? (1 + overage / 100) : 1;
        return (actualMg / potencyFactor) * overageFactor;
    }

    /**
     * Calculate the material cost per serving for one ingredient.
     * Formula: MaterialCost = (mg / 1,000,000) * PricePerKg * (1 + FreightPercent/100)
     * Equivalent to: MaterialCost calculation in grid cell update handlers.
     */
    @AuraEnabled(cacheable=true)
    public static Decimal calculateIngredientCost(Decimal mgWithOverage, Decimal pricePerKg,
                                                   Decimal freightPercent) {
        if (mgWithOverage == null || pricePerKg == null) return 0;
        Decimal mgToKg = mgWithOverage / 1000000;  // 1 kg = 1,000,000 mg
        Decimal freight = (freightPercent != null && freightPercent > 0)
                          ? (1 + freightPercent / 100) : 1;
        return mgToKg * pricePerKg * freight;
    }

    /**
     * Calculate total material cost per serving across all ingredients.
     */
    @AuraEnabled
    public static Decimal calculateTotalMaterialCost(CostInput costInput) {
        Decimal total = 0;
        if (costInput.ingredients == null) return 0;

        for (IngredientCostInput ing : costInput.ingredients) {
            if (ing == null) continue;
            Decimal mgWithOverage = calculateMgWithOverage(ing.actualMg, ing.potency, ing.overage);
            Decimal price = (ing.isManual == true) ? ing.manualCost : ing.pricePerKg;
            total += calculateIngredientCost(mgWithOverage, price, costInput.freightPercentage);
        }
        return total.setScale(6, RoundingMode.HALF_UP);
    }

    /**
     * Calculate wastage value from a wastage percentage.
     * Formula: WastageValue = TotalMaterialCost * WastagePercent / 100
     */
    @AuraEnabled(cacheable=true)
    public static Decimal calculateWastageValue(Decimal materialCost, Decimal wastagePercent) {
        if (materialCost == null || wastagePercent == null || wastagePercent == 0) return 0;
        return (materialCost * wastagePercent / 100).setScale(6, RoundingMode.HALF_UP);
    }

    /**
     * Calculate total unit cost for Powder (bulk blend) formula.
     * TotalUnitCost = MaterialCost + BlendingCost + WastageValue + LabCost + FlavorProfile
     */
    @AuraEnabled
    public static Decimal calculatePowderUnitCost(CostInput input) {
        Decimal materialCost = calculateTotalMaterialCost(input);
        Decimal blending = input.blendingCost != null ? input.blendingCost : 0;
        Decimal wastage = calculateWastageValue(
            materialCost + blending,
            input.blendingWastagePercent
        );
        Decimal flavor = input.blendingFlavorProfile != null ? input.blendingFlavorProfile : 0;
        Decimal lab = input.labCost != null ? input.labCost : 0;
        return (materialCost + blending + wastage + flavor + lab).setScale(6, RoundingMode.HALF_UP);
    }

    /**
     * Calculate total unit cost for Tablet formula.
     * TotalUnitCost = MaterialCost + CompressionCost + CoatingCost + WastageValue + LabCost
     */
    @AuraEnabled
    public static Decimal calculateTabletUnitCost(CostInput input) {
        Decimal materialCost = calculateTotalMaterialCost(input);
        Decimal compression = input.tabletingCompressionCost != null ? input.tabletingCompressionCost : 0;
        Decimal coating = input.tabletingCoatingCost != null ? input.tabletingCoatingCost : 0;
        Decimal wastage = calculateWastageValue(
            materialCost + compression + coating,
            input.tabletingWastagePercent
        );
        Decimal lab = input.labCost != null ? input.labCost : 0;
        return (materialCost + compression + coating + wastage + lab).setScale(6, RoundingMode.HALF_UP);
    }

    /**
     * Calculate total unit cost for Capsule formula.
     * TotalUnitCost = MaterialCost + CapsuleCost + EncapsulationCost + WastageValue + LabCost
     */
    @AuraEnabled
    public static Decimal calculateCapsuleUnitCost(CostInput input) {
        Decimal materialCost = calculateTotalMaterialCost(input);
        Decimal capsule = input.encapsulationCapsuleCost != null ? input.encapsulationCapsuleCost : 0;
        Decimal encapsulation = input.encapsulationCost != null ? input.encapsulationCost : 0;
        Decimal wastage = calculateWastageValue(
            materialCost + capsule + encapsulation,
            input.encapsulationWastagePercent
        );
        Decimal lab = input.labCost != null ? input.labCost : 0;
        return (materialCost + capsule + encapsulation + wastage + lab).setScale(6, RoundingMode.HALF_UP);
    }

    /**
     * Calculate the sales price from a unit cost and margin percentage.
     * Formula: SalesPrice = UnitCost / (1 - MarginPercent/100)
     * Equivalent to margin calculation in Formulator2.vb sales grid.
     */
    @AuraEnabled(cacheable=true)
    public static Decimal calculateSalesPrice(Decimal unitCost, Decimal marginPercent) {
        if (unitCost == null || unitCost == 0) return 0;
        Decimal margin = (marginPercent != null) ? marginPercent : 0;
        if (margin >= 100) {
            throw new AuraHandledException('Margin percentage cannot be 100% or higher.');
        }
        return (unitCost / (1 - margin / 100)).setScale(6, RoundingMode.HALF_UP);
    }

    /**
     * Calculate the margin percentage from unit cost and sales price.
     * Formula: Margin = (1 - UnitCost / SalesPrice) * 100
     */
    @AuraEnabled(cacheable=true)
    public static Decimal calculateMarginFromPrice(Decimal unitCost, Decimal salesPrice) {
        if (salesPrice == null || salesPrice == 0) return 0;
        return ((1 - unitCost / salesPrice) * 100).setScale(4, RoundingMode.HALF_UP);
    }

    /**
     * Calculate bottle packaging unit cost.
     * UnitCost = sum of all bottle component costs for a given size count.
     */
    @AuraEnabled(cacheable=true)
    public static Decimal calculateBottlePackagingUnitCost(List<Bottle_Packaging__c> components,
                                                            Decimal sizeCount) {
        Decimal total = 0;
        for (Bottle_Packaging__c comp : components) {
            if (comp.Size_Count__c == sizeCount && comp.Unit_Cost__c != null) {
                total += comp.Unit_Cost__c;
            }
        }
        return total.setScale(6, RoundingMode.HALF_UP);
    }

    /**
     * Full formula unit cost calculator — dispatches to the right method based on formula type.
     */
    @AuraEnabled
    public static CostResult calculateFullUnitCost(CostInput input) {
        CostResult result = new CostResult();
        result.ingredientCosts = new List<IngredientCostResult>();

        Decimal totalMatCost = 0;

        if (input.ingredients != null) {
            for (IngredientCostInput ing : input.ingredients) {
                IngredientCostResult icr = new IngredientCostResult();
                icr.mgWithOverage = calculateMgWithOverage(ing.actualMg, ing.potency, ing.overage);
                Decimal price = (ing.isManual == true) ? ing.manualCost : ing.pricePerKg;
                Decimal freightCostPerIng = 0;
                if (input.freightPercentage != null && input.freightPercentage > 0 && price != null) {
                    freightCostPerIng = (icr.mgWithOverage / 1000000) * price * (input.freightPercentage / 100);
                }
                icr.freightCost = freightCostPerIng;
                icr.materialCost = calculateIngredientCost(icr.mgWithOverage, price, input.freightPercentage);
                totalMatCost += icr.materialCost;
                result.ingredientCosts.add(icr);
            }
        }

        result.totalMaterialCostPerServing = totalMatCost.setScale(6, RoundingMode.HALF_UP);

        String fType = input.formulaType != null ? input.formulaType : 'Powder';
        if (fType == 'Tablet') {
            result.totalUnitCost = calculateTabletUnitCost(input);
        } else if (fType == 'Capsule') {
            result.totalUnitCost = calculateCapsuleUnitCost(input);
        } else {
            result.totalUnitCost = calculatePowderUnitCost(input);
        }

        return result;
    }

    /**
     * Determine material price "age" classification.
     * New  = updated within newPriceDays days (green)
     * Mid  = updated within midPriceDays days (yellow)
     * Old  = older than oldPriceDays days (red)
     * Equivalent to the price age coloring in Formulator2.vb grid.
     */
    @AuraEnabled(cacheable=true)
    public static String getPriceAgeClass(Date priceDate, Integer newPriceDays,
                                           Integer midPriceDays, Integer oldPriceDays) {
        if (priceDate == null) return 'old';
        Integer daysOld = priceDate.daysBetween(Date.today());
        if (daysOld <= newPriceDays) return 'new';
        if (daysOld <= midPriceDays) return 'mid';
        return 'old';
    }
}
