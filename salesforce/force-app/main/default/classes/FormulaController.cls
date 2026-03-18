/**
 * FormulaController
 * Core controller for Formula__c CRUD, ingredient management, and formula versioning.
 * Migrated from VB.NET Formulator2.vb and Dashboard.vb.
 */
public with sharing class FormulaController {

    // ─── Formula CRUD ─────────────────────────────────────────────────────────

    /**
     * Retrieves all active formulas for the dashboard list.
     * Equivalent to: SELECT FormulaID, FormulaName, FormulaType, EnteredDate, SalesRepId
     *                FROM Formulas WHERE IsInactive = 0
     */
    @AuraEnabled(cacheable=true)
    public static List<Formula__c> getFormulas(String filterType, String filterValue, Integer daysBack) {
        String query = 'SELECT Id, Name, Formula_Type__c, Entered_Date__c, Is_Inactive__c, '
                     + 'Customer_Name__c, Quote_Number__c, Current_Version__c, '
                     + 'Sales_Rep__r.Name, Sales_Rep__c, Total_Unit_Cost__c '
                     + 'FROM Formula__c WHERE Is_Inactive__c = false';

        if (daysBack != null && daysBack > 0) {
            Date filterDate = Date.today().addDays(-daysBack);
            query += ' AND Entered_Date__c >= :filterDate';
        }

        if (String.isNotBlank(filterType) && String.isNotBlank(filterValue)) {
            if (filterType == 'Name') {
                String nameLike = '%' + filterValue + '%';
                query += ' AND Name LIKE :nameLike';
            } else if (filterType == 'SalesRep') {
                query += ' AND Sales_Rep__r.Name = :filterValue';
            }
        }

        query += ' ORDER BY Entered_Date__c DESC NULLS LAST';
        return Database.query(query);
    }

    /**
     * Returns a single formula with all related data for the formula editor.
     */
    @AuraEnabled(cacheable=true)
    public static FormulaWrapper getFormulaDetail(Id formulaId) {
        Formula__c formula = [
            SELECT Id, Name, Formula_Type__c, Entered_Date__c, Is_Inactive__c,
                   Customer_Name__c, Contact_Info__c, Quote_Number__c,
                   Sales_Rep__c, Sales_Rep__r.Name,
                   Other_Ingredients__c, Lab_Testing_Cost_Policy__c,
                   Quote_Expiry_Disclaimer__c, Message__c,
                   Serving_Size__c, Servings_Per_Container__c,
                   Total_Material_Cost__c, Total_Unit_Cost__c,
                   Freight_Cost__c, Lab_Cost__c, Current_Version__c
            FROM Formula__c
            WHERE Id = :formulaId
        ];

        FormulaWrapper wrapper = new FormulaWrapper();
        wrapper.formula = formula;
        wrapper.versions = getFormulaVersions(formulaId);
        wrapper.ingredients = getFormulaIngredients(formulaId, formula.Current_Version__c != null ? formula.Current_Version__c.intValue() : 1);
        return wrapper;
    }

    /**
     * Creates a new formula record.
     * Equivalent to INSERT INTO Formulas (FormulaName, FormulaType, EnteredDate, IsInactive)
     */
    @AuraEnabled
    public static Id createFormula(Formula__c formula) {
        try {
            if (formula.Entered_Date__c == null) {
                formula.Entered_Date__c = Date.today();
            }
            formula.Is_Inactive__c = false;
            formula.Current_Version__c = 1;
            insert formula;

            // Auto-create version 1
            Formula_Version__c v = new Formula_Version__c(
                Formula__c = formula.Id,
                Version_Number__c = 1,
                Name = formula.Name + ' - v1',
                Is_Current__c = true
            );
            insert v;

            return formula.Id;
        } catch (DmlException e) {
            throw new AuraHandledException('Error creating formula: ' + e.getMessage());
        }
    }

    /**
     * Updates an existing formula's header information.
     */
    @AuraEnabled
    public static void updateFormula(Formula__c formula) {
        try {
            update formula;
        } catch (DmlException e) {
            throw new AuraHandledException('Error updating formula: ' + e.getMessage());
        }
    }

    /**
     * Soft-deletes a formula and all related records.
     * Equivalent to setting IsInactive = 1 (or full DELETE cascade in VB.NET).
     */
    @AuraEnabled
    public static void deleteFormula(Id formulaId) {
        try {
            Formula__c f = new Formula__c(Id = formulaId, Is_Inactive__c = true);
            update f;
        } catch (DmlException e) {
            throw new AuraHandledException('Error deleting formula: ' + e.getMessage());
        }
    }

    // ─── Formula Versions ─────────────────────────────────────────────────────

    /**
     * Returns all versions for a formula.
     * Equivalent to: SELECT * FROM FormulaVersions WHERE FormulaID = x ORDER BY VersionNumber ASC
     */
    @AuraEnabled(cacheable=true)
    public static List<Formula_Version__c> getFormulaVersions(Id formulaId) {
        return [
            SELECT Id, Name, Version_Number__c, Is_Current__c, Version_Notes__c, CreatedDate
            FROM Formula_Version__c
            WHERE Formula__c = :formulaId
            ORDER BY Version_Number__c ASC
        ];
    }

    /**
     * Creates a new version for a formula (copy from current version).
     */
    @AuraEnabled
    public static Id createNewVersion(Id formulaId, String versionNotes) {
        List<Formula_Version__c> versions = [
            SELECT Version_Number__c FROM Formula_Version__c
            WHERE Formula__c = :formulaId
            ORDER BY Version_Number__c DESC
            LIMIT 1
        ];

        Decimal nextVersionNum = versions.isEmpty() ? 1 : versions[0].Version_Number__c + 1;

        // Get the parent formula
        Formula__c formula = [SELECT Name FROM Formula__c WHERE Id = :formulaId];

        Formula_Version__c newVersion = new Formula_Version__c(
            Formula__c = formulaId,
            Version_Number__c = nextVersionNum,
            Name = formula.Name + ' - v' + nextVersionNum.intValue(),
            Is_Current__c = true,
            Version_Notes__c = versionNotes
        );

        // Mark all other versions as not current
        List<Formula_Version__c> allVersions = [
            SELECT Id FROM Formula_Version__c WHERE Formula__c = :formulaId
        ];
        for (Formula_Version__c v : allVersions) {
            v.Is_Current__c = false;
        }

        insert newVersion;
        update allVersions;

        // Update parent formula current version
        update new Formula__c(Id = formulaId, Current_Version__c = nextVersionNum);

        // Copy ingredients from previous version
        List<Formula_Ingredient__c> existingIngredients = getFormulaIngredients(
            formulaId, (nextVersionNum - 1).intValue()
        );
        List<Formula_Ingredient__c> newIngredients = new List<Formula_Ingredient__c>();
        for (Formula_Ingredient__c ing : existingIngredients) {
            Formula_Ingredient__c copy = ing.clone(false, true, false, false);
            copy.Formula_Version__c = newVersion.Id;
            copy.Version_Number__c = nextVersionNum;
            newIngredients.add(copy);
        }
        if (!newIngredients.isEmpty()) {
            insert newIngredients;
        }

        return newVersion.Id;
    }

    // ─── Formula Ingredients ──────────────────────────────────────────────────

    /**
     * Returns all ingredients for a specific formula version.
     * Equivalent to: SELECT * FROM FormulaDetails WHERE FormulaID = x ORDER BY IngredientIndex ASC
     */
    @AuraEnabled(cacheable=true)
    public static List<Formula_Ingredient__c> getFormulaIngredients(Id formulaId, Integer versionNumber) {
        Decimal vNum = versionNumber != null ? versionNumber : 1;
        return [
            SELECT Id, Name, Formula__c, Material__c, Material__r.Name,
                   Material_Name__c, Component_Code__c, Vendor_Name__c,
                   Ingredient_Index__c, Actual_Mg__c, Potency__c,
                   Overage__c, Mg__c, Freight__c, Material_Cost__c,
                   Is_Manual__c, Manual_Cost__c, Latest_Cost__c, Version_Number__c
            FROM Formula_Ingredient__c
            WHERE Formula__c = :formulaId AND Version_Number__c = :vNum
            ORDER BY Ingredient_Index__c ASC NULLS LAST
        ];
    }

    /**
     * Saves (upserts) a list of formula ingredients.
     * Equivalent to the DataAdapter insert/update/delete commands in daF_commands().
     */
    @AuraEnabled
    public static void saveFormulaIngredients(List<Formula_Ingredient__c> ingredients,
                                               List<Id> deletedIngredientIds) {
        try {
            if (deletedIngredientIds != null && !deletedIngredientIds.isEmpty()) {
                List<Formula_Ingredient__c> toDelete = [
                    SELECT Id FROM Formula_Ingredient__c WHERE Id IN :deletedIngredientIds
                ];
                delete toDelete;
            }
            if (ingredients != null && !ingredients.isEmpty()) {
                upsert ingredients;
            }
        } catch (DmlException e) {
            throw new AuraHandledException('Error saving ingredients: ' + e.getMessage());
        }
    }

    // ─── Manufacturing Details ─────────────────────────────────────────────────

    /**
     * Returns blending details for a formula version.
     * Equivalent to: SELECT * FROM BlendingDetails WHERE FormulaID = x
     */
    @AuraEnabled(cacheable=true)
    public static Blending_Detail__c getBlendingDetail(Id formulaId, Integer versionNumber) {
        Decimal vNum = versionNumber != null ? versionNumber : 1;
        List<Blending_Detail__c> results = [
            SELECT Id, Formula__c, Blending_Cost__c, Wastage_Percentage__c,
                   Wastage_Value__c, Lab_Cost__c, Flavor_Profile__c, Version_Number__c
            FROM Blending_Detail__c
            WHERE Formula__c = :formulaId AND Version_Number__c = :vNum
            ORDER BY CreatedDate DESC
            LIMIT 1
        ];
        return results.isEmpty() ? null : results[0];
    }

    @AuraEnabled
    public static void saveBlendingDetail(Blending_Detail__c detail) {
        try {
            upsert detail;
        } catch (DmlException e) {
            throw new AuraHandledException('Error saving blending detail: ' + e.getMessage());
        }
    }

    /**
     * Returns tableting details for a formula version.
     * Equivalent to: SELECT * FROM TabletingDetails WHERE FormulaID = x
     */
    @AuraEnabled(cacheable=true)
    public static Tableting_Detail__c getTabletingDetail(Id formulaId, Integer versionNumber) {
        Decimal vNum = versionNumber != null ? versionNumber : 1;
        List<Tableting_Detail__c> results = [
            SELECT Id, Formula__c, Type_of_Coating__c, Coating_Cost__c,
                   Compression_Cost__c, Wastage_Percentage__c, Wastage_Value__c,
                   Lab_Cost__c, Version_Number__c
            FROM Tableting_Detail__c
            WHERE Formula__c = :formulaId AND Version_Number__c = :vNum
            ORDER BY CreatedDate DESC
            LIMIT 1
        ];
        return results.isEmpty() ? null : results[0];
    }

    @AuraEnabled
    public static void saveTabletingDetail(Tableting_Detail__c detail) {
        try {
            upsert detail;
        } catch (DmlException e) {
            throw new AuraHandledException('Error saving tableting detail: ' + e.getMessage());
        }
    }

    /**
     * Returns encapsulation details for a formula version.
     * Equivalent to: SELECT * FROM EncapsulationDetails WHERE FormulaID = x
     */
    @AuraEnabled(cacheable=true)
    public static Encapsulation_Detail__c getEncapsulationDetail(Id formulaId, Integer versionNumber) {
        Decimal vNum = versionNumber != null ? versionNumber : 1;
        List<Encapsulation_Detail__c> results = [
            SELECT Id, Formula__c, Type_of_Capsule__c, Capsule_Cost__c,
                   Encapsulation_Cost__c, Wastage_Percentage__c, Wastage_Value__c,
                   Lab_Cost__c, Color__c, Version_Number__c
            FROM Encapsulation_Detail__c
            WHERE Formula__c = :formulaId AND Version_Number__c = :vNum
            ORDER BY CreatedDate DESC
            LIMIT 1
        ];
        return results.isEmpty() ? null : results[0];
    }

    @AuraEnabled
    public static void saveEncapsulationDetail(Encapsulation_Detail__c detail) {
        try {
            upsert detail;
        } catch (DmlException e) {
            throw new AuraHandledException('Error saving encapsulation detail: ' + e.getMessage());
        }
    }

    // ─── Packaging Details ─────────────────────────────────────────────────────

    /**
     * Returns bottle packaging details for a formula version.
     * Equivalent to: SELECT * FROM BottlePackagingDetails WHERE FormulaID = x
     */
    @AuraEnabled(cacheable=true)
    public static List<Bottle_Packaging__c> getBottlePackaging(Id formulaId, Integer versionNumber) {
        Decimal vNum = versionNumber != null ? versionNumber : 1;
        return [
            SELECT Id, Formula__c, Binding_Index__c, Size_Count__c, Category__c,
                   Material_Name__c, Vendor_Name__c, Unit_Cost__c, Component_Code__c,
                   Is_Default__c, Version_Number__c
            FROM Bottle_Packaging__c
            WHERE Formula__c = :formulaId AND Version_Number__c = :vNum
            ORDER BY Binding_Index__c ASC NULLS LAST
        ];
    }

    @AuraEnabled
    public static void saveBottlePackaging(List<Bottle_Packaging__c> records, List<Id> deletedIds) {
        try {
            if (deletedIds != null && !deletedIds.isEmpty()) {
                delete [SELECT Id FROM Bottle_Packaging__c WHERE Id IN :deletedIds];
            }
            if (records != null && !records.isEmpty()) {
                upsert records;
            }
        } catch (DmlException e) {
            throw new AuraHandledException('Error saving bottle packaging: ' + e.getMessage());
        }
    }

    /**
     * Returns stick pack details for a formula version.
     * Equivalent to: SELECT * FROM StickPackDetails WHERE FormulaID = x
     */
    @AuraEnabled(cacheable=true)
    public static List<Stick_Pack_Detail__c> getStickPackDetails(Id formulaId, Integer versionNumber) {
        Decimal vNum = versionNumber != null ? versionNumber : 1;
        return [
            SELECT Id, Formula__c, Size_Count__c, Material__c, Quantity__c,
                   Packet_Contents__c, Cost_Content__c, Filling_Cost__c,
                   Printing_Cost__c, Display_Cost__c, Printing_Plates__c,
                   Art_Preparation__c, Pack_Out__c, Other_Cost_Desc__c,
                   Other_Cost_Value__c, Shrink_Wrap__c, Wafer_Seal__c,
                   Colors__c, Stick_Packs__c, Shipper_Case_Count_Value__c,
                   Shipper_Case_Count_Desc__c, Display_Quantity__c,
                   Display_Description__c, Display_Spec__c, Display_Board_Grade__c,
                   Display_Dimension__c, Display_Product_Style__c,
                   Printing_Plates_Bulk__c, Art_Preparation_Bulk__c,
                   Other_Cost_Bulk__c, Other_Cost_Desc_Bulk__c,
                   Shipper_Case_Count_Bulk__c, Shipper_Case_Cost_Bulk__c,
                   Research_Development_Bulk__c, Research_Development_Box__c,
                   Version_Number__c
            FROM Stick_Pack_Detail__c
            WHERE Formula__c = :formulaId AND Version_Number__c = :vNum
        ];
    }

    @AuraEnabled
    public static void saveStickPackDetails(List<Stick_Pack_Detail__c> records, List<Id> deletedIds) {
        try {
            if (deletedIds != null && !deletedIds.isEmpty()) {
                delete [SELECT Id FROM Stick_Pack_Detail__c WHERE Id IN :deletedIds];
            }
            if (records != null && !records.isEmpty()) {
                upsert records;
            }
        } catch (DmlException e) {
            throw new AuraHandledException('Error saving stick pack details: ' + e.getMessage());
        }
    }

    /**
     * Returns sachet details for a formula version.
     * Equivalent to: SELECT * FROM SachetsDetails WHERE FormulaID = x
     */
    @AuraEnabled(cacheable=true)
    public static List<Sachet_Detail__c> getSachetDetails(Id formulaId, Integer versionNumber) {
        Decimal vNum = versionNumber != null ? versionNumber : 1;
        return [
            SELECT Id, Formula__c, Size_Count__c, Material__c, Quantity__c,
                   Packet_Contents__c, Cost_Content__c, Filling_Cost__c,
                   Printing_Cost__c, Display_Cost__c, Printing_Plates__c,
                   Art_Preparation__c, Pack_Out__c, Other_Cost_Desc__c,
                   Other_Cost_Value__c, Shrink_Wrap__c, Wafer_Seal__c,
                   Stick_Packs__c, Shipper_Case_Count_Value__c,
                   Shipper_Case_Count_Desc__c, Version_Number__c
            FROM Sachet_Detail__c
            WHERE Formula__c = :formulaId AND Version_Number__c = :vNum
        ];
    }

    @AuraEnabled
    public static void saveSachetDetails(List<Sachet_Detail__c> records, List<Id> deletedIds) {
        try {
            if (deletedIds != null && !deletedIds.isEmpty()) {
                delete [SELECT Id FROM Sachet_Detail__c WHERE Id IN :deletedIds];
            }
            if (records != null && !records.isEmpty()) {
                upsert records;
            }
        } catch (DmlException e) {
            throw new AuraHandledException('Error saving sachet details: ' + e.getMessage());
        }
    }

    /**
     * Returns stand-up bag details for a formula version.
     * Equivalent to: SELECT * FROM StandUpBagDetails WHERE FormulaID = x
     */
    @AuraEnabled(cacheable=true)
    public static List<Stand_Up_Bag_Detail__c> getStandUpBagDetails(Id formulaId, Integer versionNumber) {
        Decimal vNum = versionNumber != null ? versionNumber : 1;
        return [
            SELECT Id, Formula__c, Bag_Material__c, Bag_Size__c, Fill_Weight__c,
                   Bag_Cost__c, Printing_Cost__c, Printing_Plates__c,
                   Art_Preparation__c, Secondary_Packout__c, Other_Cost__c,
                   Shipper_Case_Count__c, Shipper_Case_Cost__c,
                   Research_Development__c, Total_Unit_Cost__c, Version_Number__c
            FROM Stand_Up_Bag_Detail__c
            WHERE Formula__c = :formulaId AND Version_Number__c = :vNum
        ];
    }

    @AuraEnabled
    public static void saveStandUpBagDetails(List<Stand_Up_Bag_Detail__c> records, List<Id> deletedIds) {
        try {
            if (deletedIds != null && !deletedIds.isEmpty()) {
                delete [SELECT Id FROM Stand_Up_Bag_Detail__c WHERE Id IN :deletedIds];
            }
            if (records != null && !records.isEmpty()) {
                upsert records;
            }
        } catch (DmlException e) {
            throw new AuraHandledException('Error saving stand-up bag details: ' + e.getMessage());
        }
    }

    /**
     * Returns blister details for a formula version.
     * Equivalent to: SELECT * FROM blisterdetails WHERE FormulaID = x
     */
    @AuraEnabled(cacheable=true)
    public static List<Blister_Detail__c> getBlisterDetails(Id formulaId, Integer versionNumber) {
        Decimal vNum = versionNumber != null ? versionNumber : 1;
        return [
            SELECT Id, Formula__c, Blister_Count__c, Blister_Count_Amount__c,
                   Blister_Cost__c, Pack_Out__c, Shrink_Wrap__c, Wafer_Seal__c,
                   Other_Cost__c, Printing_Cost__c, Printing_Plates__c,
                   Art_Preparation__c, Tooling_Cost__c, Display_Box_Cost__c,
                   Shipper_Case_Count__c, Shipper_Case_Cost__c,
                   Other_Cost_Bulk__c, Research_Development_Bulk__c,
                   Research_Development_Box__c, Total_Unit_Cost__c, Version_Number__c
            FROM Blister_Detail__c
            WHERE Formula__c = :formulaId AND Version_Number__c = :vNum
        ];
    }

    @AuraEnabled
    public static void saveBlisterDetails(List<Blister_Detail__c> records, List<Id> deletedIds) {
        try {
            if (deletedIds != null && !deletedIds.isEmpty()) {
                delete [SELECT Id FROM Blister_Detail__c WHERE Id IN :deletedIds];
            }
            if (records != null && !records.isEmpty()) {
                upsert records;
            }
        } catch (DmlException e) {
            throw new AuraHandledException('Error saving blister details: ' + e.getMessage());
        }
    }

    // ─── Sales Details ─────────────────────────────────────────────────────────

    /**
     * Returns sales pricing details for a formula.
     * Equivalent to: SELECT * FROM SalesDetails WHERE Type='BULK/BOX/BULKBAGS' AND FormulaID = x
     */
    @AuraEnabled(cacheable=true)
    public static List<Sales_Detail__c> getSalesDetails(Id formulaId, String saleType, Integer versionNumber) {
        Decimal vNum = versionNumber != null ? versionNumber : 1;
        if (String.isNotBlank(saleType)) {
            return [
                SELECT Id, Formula__c, Packaging_Format__c, Unit_Size__c,
                       Quantity__c, Margin_Percentage__c, Sales_Price__c,
                       Overridden_Sales_Price__c, Is_Overridden__c,
                       Sale_Type__c, Version_Number__c
                FROM Sales_Detail__c
                WHERE Formula__c = :formulaId AND Sale_Type__c = :saleType
                  AND Version_Number__c = :vNum
                ORDER BY Quantity__c ASC
            ];
        }
        return [
            SELECT Id, Formula__c, Packaging_Format__c, Unit_Size__c,
                   Quantity__c, Margin_Percentage__c, Sales_Price__c,
                   Overridden_Sales_Price__c, Is_Overridden__c,
                   Sale_Type__c, Version_Number__c
            FROM Sales_Detail__c
            WHERE Formula__c = :formulaId AND Version_Number__c = :vNum
            ORDER BY Sale_Type__c, Quantity__c ASC
        ];
    }

    @AuraEnabled
    public static void saveSalesDetails(List<Sales_Detail__c> records, List<Id> deletedIds) {
        try {
            if (deletedIds != null && !deletedIds.isEmpty()) {
                delete [SELECT Id FROM Sales_Detail__c WHERE Id IN :deletedIds];
            }
            if (records != null && !records.isEmpty()) {
                upsert records;
            }
        } catch (DmlException e) {
            throw new AuraHandledException('Error saving sales details: ' + e.getMessage());
        }
    }

    // ─── Sales Reps ────────────────────────────────────────────────────────────

    /**
     * Returns all sales reps.
     * Equivalent to: SELECT * FROM salesreps
     */
    @AuraEnabled(cacheable=true)
    public static List<Sales_Rep__c> getSalesReps() {
        return [
            SELECT Id, Name, Rep_Code__c, Email__c, Phone__c, Company__c
            FROM Sales_Rep__c
            ORDER BY Name ASC
        ];
    }

    @AuraEnabled(cacheable=true)
    public static Sales_Rep__c getSalesRepById(Id repId) {
        return [
            SELECT Id, Name, Rep_Code__c, Email__c, Phone__c, Company__c
            FROM Sales_Rep__c
            WHERE Id = :repId
        ];
    }

    @AuraEnabled
    public static Id saveSalesRep(Sales_Rep__c rep) {
        try {
            upsert rep;
            return rep.Id;
        } catch (DmlException e) {
            throw new AuraHandledException('Error saving sales rep: ' + e.getMessage());
        }
    }

    // ─── Formula Settings ──────────────────────────────────────────────────────

    /**
     * Returns the global formula settings (freight rate, price age buckets).
     * Equivalent to: SELECT * FROM tblformualsettings
     */
    @AuraEnabled(cacheable=true)
    public static Formula_Settings__c getFormulaSettings() {
        List<Formula_Settings__c> settings = [
            SELECT Id, Freight_Percentage__c, New_Price_Days__c,
                   Mid_Price_Days__c, Old_Price_Days__c
            FROM Formula_Settings__c
            LIMIT 1
        ];
        if (settings.isEmpty()) {
            return new Formula_Settings__c(
                Freight_Percentage__c = 0,
                New_Price_Days__c = 30,
                Mid_Price_Days__c = 60,
                Old_Price_Days__c = 61
            );
        }
        return settings[0];
    }

    @AuraEnabled
    public static void saveFormulaSettings(Formula_Settings__c settings) {
        try {
            upsert settings;
        } catch (DmlException e) {
            throw new AuraHandledException('Error saving formula settings: ' + e.getMessage());
        }
    }

    // ─── Inner Wrapper Classes ─────────────────────────────────────────────────

    public class FormulaWrapper {
        @AuraEnabled public Formula__c formula;
        @AuraEnabled public List<Formula_Version__c> versions;
        @AuraEnabled public List<Formula_Ingredient__c> ingredients;
    }
}
