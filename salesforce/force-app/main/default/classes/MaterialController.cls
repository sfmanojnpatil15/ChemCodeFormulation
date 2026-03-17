/**
 * MaterialController
 * Handles CRUD operations for the Material__c object.
 * Migrated from VB.NET NewMaterial.vb and EditMaterial.vb forms.
 */
public with sharing class MaterialController {

    /**
     * Retrieves all active materials, sorted by name.
     * Equivalent to: SELECT * FROM Material ORDER BY MaterialName
     */
    @AuraEnabled(cacheable=true)
    public static List<Material__c> getMaterials(String category) {
        String query = 'SELECT Id, Name, Supplier__c, Price__c, Category__c, '
                     + 'Component_Code__c, Case_Pack__c, Vendor_Code__c, Is_Active__c '
                     + 'FROM Material__c WHERE Is_Active__c = true';
        if (String.isNotBlank(category)) {
            query += ' AND Category__c = :category';
        }
        query += ' ORDER BY Name ASC';
        return Database.query(query);
    }

    /**
     * Retrieves raw materials only (Category = RM).
     * Equivalent to: SELECT * FROM Material WHERE Category = 'RM' ORDER BY MaterialName
     */
    @AuraEnabled(cacheable=true)
    public static List<Material__c> getRawMaterials() {
        return [
            SELECT Id, Name, Supplier__c, Price__c, Category__c,
                   Component_Code__c, Case_Pack__c, Vendor_Code__c
            FROM Material__c
            WHERE Category__c = 'RM' AND Is_Active__c = true
            ORDER BY Name ASC
        ];
    }

    /**
     * Retrieves non-raw materials (packaging, etc.)
     */
    @AuraEnabled(cacheable=true)
    public static List<Material__c> getNonRawMaterials() {
        return [
            SELECT Id, Name, Supplier__c, Price__c, Category__c,
                   Component_Code__c, Case_Pack__c, Vendor_Code__c
            FROM Material__c
            WHERE Category__c != 'RM' AND Is_Active__c = true
            ORDER BY Name ASC
        ];
    }

    /**
     * Searches materials by name (for live search/lookup).
     */
    @AuraEnabled(cacheable=true)
    public static List<Material__c> searchMaterials(String searchTerm, String category) {
        String likeClause = '%' + searchTerm + '%';
        if (String.isNotBlank(category)) {
            return [
                SELECT Id, Name, Supplier__c, Price__c, Category__c,
                       Component_Code__c, Case_Pack__c, Vendor_Code__c
                FROM Material__c
                WHERE Name LIKE :likeClause AND Category__c = :category AND Is_Active__c = true
                ORDER BY Name ASC
                LIMIT 50
            ];
        }
        return [
            SELECT Id, Name, Supplier__c, Price__c, Category__c,
                   Component_Code__c, Case_Pack__c, Vendor_Code__c
            FROM Material__c
            WHERE Name LIKE :likeClause AND Is_Active__c = true
            ORDER BY Name ASC
            LIMIT 50
        ];
    }

    /**
     * Returns a single material by Id.
     */
    @AuraEnabled(cacheable=true)
    public static Material__c getMaterialById(Id materialId) {
        return [
            SELECT Id, Name, Supplier__c, Price__c, Category__c,
                   Component_Code__c, Case_Pack__c, Vendor_Code__c, Is_Active__c
            FROM Material__c
            WHERE Id = :materialId
        ];
    }

    /**
     * Creates or updates a material.
     * Equivalent to INSERT/UPDATE Material table.
     */
    @AuraEnabled
    public static Id saveMaterial(Material__c material) {
        try {
            upsert material;
            return material.Id;
        } catch (DmlException e) {
            throw new AuraHandledException('Error saving material: ' + e.getMessage());
        }
    }

    /**
     * Soft-deletes a material by setting Is_Active__c = false.
     */
    @AuraEnabled
    public static void deactivateMaterial(Id materialId) {
        Material__c mat = new Material__c(Id = materialId, Is_Active__c = false);
        try {
            update mat;
        } catch (DmlException e) {
            throw new AuraHandledException('Error deactivating material: ' + e.getMessage());
        }
    }

    /**
     * Retrieves all bottle materials (from xls_bottlelist equivalent).
     */
    @AuraEnabled(cacheable=true)
    public static List<Material__c> getBottleMaterials() {
        return [
            SELECT Id, Name, Supplier__c, Price__c, Component_Code__c, Vendor_Code__c
            FROM Material__c
            WHERE Category__c = 'Bottle' AND Is_Active__c = true
            ORDER BY Name ASC
        ];
    }

    /**
     * Retrieves all stick pack materials.
     */
    @AuraEnabled(cacheable=true)
    public static List<Material__c> getStickPackMaterials() {
        return [
            SELECT Id, Name, Supplier__c, Price__c, Component_Code__c
            FROM Material__c
            WHERE Category__c = 'StickPack' AND Is_Active__c = true
            ORDER BY Name ASC
        ];
    }

    /**
     * Retrieves all sachet materials.
     */
    @AuraEnabled(cacheable=true)
    public static List<Material__c> getSachetMaterials() {
        return [
            SELECT Id, Name, Supplier__c, Price__c, Component_Code__c
            FROM Material__c
            WHERE Category__c = 'Sachet' AND Is_Active__c = true
            ORDER BY Name ASC
        ];
    }

    /**
     * Retrieves all lid materials.
     */
    @AuraEnabled(cacheable=true)
    public static List<Material__c> getLidMaterials() {
        return [
            SELECT Id, Name, Supplier__c, Price__c, Component_Code__c
            FROM Material__c
            WHERE Category__c = 'Lid' AND Is_Active__c = true
            ORDER BY Name ASC
        ];
    }

    /**
     * Retrieves all scoop materials.
     */
    @AuraEnabled(cacheable=true)
    public static List<Material__c> getScoopMaterials() {
        return [
            SELECT Id, Name, Supplier__c, Price__c, Component_Code__c
            FROM Material__c
            WHERE Category__c = 'Scoop' AND Is_Active__c = true
            ORDER BY Name ASC
        ];
    }

    /**
     * Retrieves all shipper materials.
     */
    @AuraEnabled(cacheable=true)
    public static List<Material__c> getShipperMaterials() {
        return [
            SELECT Id, Name, Supplier__c, Price__c, Component_Code__c
            FROM Material__c
            WHERE Category__c = 'Shipper' AND Is_Active__c = true
            ORDER BY Name ASC
        ];
    }
}
